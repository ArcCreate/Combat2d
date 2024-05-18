using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // public variables
    public float movementSpeed = 10;
    public float jumpHeight = 16.0f;
    public float groundCheckRadius;
    public int jumpCount;
    public float dashTime;
    public float dashSpeed;
    public float dashCooldown;
    public bool isAttacking1;
    public bool isAttacking2;
    public bool canFlip;
    public bool canMove;
    public bool canAirAttack;
    public float movementDirection;
    public float attack2Cooldown = 0.5f; // Cooldown time for Attack 2
    public float attack1Radius, attack2Radius, airAttackRadius;

    // private variables
    private int direction = 1;
    public bool isFacingRight = true;
    public bool isGround;
    private bool canJump;
    private int jumpLeft;
    private bool canDash = false;
    private float dashTimeLeft;
    private float lastDash = -100f;
    private int airAttack = 1;
    private float lastAttack2Time = -100f; // Tracks the last time Attack 2 was performed
    private float attackRadius;
    private int attackDamage;

    // animation variables
    private bool isRunning = false;

    // references
    public Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask ground;
    public TrailRenderer trailRenderer;
    public static PlayerController instance;
    public Transform A1Hitbox, A2Hitbox, AAHitbox;
    public LayerMask damageable;
    private Transform box;
    public ParticleSystem dust;
    public ParticleSystem landDust;
    public Slider slider;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpLeft = jumpCount;

        // Initialize the slider
        if (slider != null)
        {
            slider.maxValue = 1;
            slider.value = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Animate();

        // checking parameters
        // jump
        if (isGround)
        {
            jumpLeft = jumpCount;
            airAttack = 1;
        }

        // continuous jump
        if (jumpLeft > 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        // direction
        if (canFlip && canMove)
        {
            if (isFacingRight && movementDirection < 0)
            {
                direction = -1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
                if (isGround)
                {
                    dust.Play();
                }
            }
            else if (!isFacingRight && movementDirection > 0)
            {
                direction = 1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
                if (isGround)
                {
                    dust.Play();
                }
            }
        }

        // checking if running
        isRunning = Mathf.Abs(rb.velocity.x) > 0.01f;

        // Update the slider
        UpdateDashSlider();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckDash();
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
    }

    private void CheckDash()
    {
        // checking dashing
        if (canDash && canMove)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(dashSpeed * direction, 0f);
                dashTimeLeft -= Time.deltaTime;
                trailRenderer.emitting = true;
            }

            if (dashTimeLeft <= 0)
            {
                canDash = false;
                trailRenderer.emitting = false;
            }
        }
    }

    // checks for all player inputs
    private void CheckInput()
    {
        // direction movement
        movementDirection = Input.GetAxisRaw("Horizontal");

        // jumping
        if ((Input.GetButtonDown("Jump") && canMove && canJump))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpLeft--;
        }

        // dashing
        if ((Input.GetButtonDown("Dash")) && Time.time >= (lastDash + dashCooldown) && movementDirection != 0)
        {
            AttemptToDash();
        }

        // check attacking
        if (Input.GetButtonDown("Fire1") && !isAttacking1 && isGround)
        {
            box = A1Hitbox;
            attackRadius = attack1Radius;
            isAttacking1 = true;
            isAttacking2 = false;
        }
        else if (Input.GetButtonDown("Fire1") && !isAttacking1 && !isGround && airAttack == 1)
        {
            airAttack--;
            canAirAttack = true;
            attackRadius = airAttackRadius;
            box = AAHitbox;
        }
        if (Input.GetButtonDown("Fire2") && !isAttacking2 && isGround && Time.time >= (lastAttack2Time + attack2Cooldown))
        {
            box = A2Hitbox;
            attackRadius = attack2Radius;
            isAttacking2 = true;
            isAttacking1 = false;
            lastAttack2Time = Time.time; // Update the last attack time
        }
    }

    private void AttemptToDash()
    {
        canDash = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
    }

    private void ApplyMovement()
    {
        // moving left and right
        if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementDirection, rb.velocity.y);
        }
    }

    public void MovingWhileAttack()
    {
        if (!canMove)
        {
            canMove = true;
            canFlip = true;
        }
        else
        {
            canMove = false;
            canFlip = false;
            rb.velocity = new Vector2(movementDirection * 0, 0f);
        }
    }

    public void CheckAttackHitbox(int dg)
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(box.position, attackRadius, damageable);

        foreach (Collider2D obj in detectedObjects)
        {
            obj.transform.SendMessage("Damaged", dg);
        }

        if(detectedObjects.Length > 0)
        {
            CameraShake.instance.Shake(1f, 0.25f);
        }
    }

    private void Animate()
    {
        animator.SetBool("Running", isRunning);
        animator.SetBool("isGrounded", isGround);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("AirAttack", canAirAttack);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(A2Hitbox.position, attack2Radius);
        Gizmos.DrawWireSphere(A1Hitbox.position, attack1Radius);
        Gizmos.DrawWireSphere(AAHitbox.position, airAttackRadius);
    }

    private void UpdateDashSlider()
    {
        float timeSinceLastDash = Time.time - lastDash;
        float cooldownProgress = Mathf.Clamp(dashCooldown - timeSinceLastDash, 0, dashCooldown);
        slider.value = 1 - (cooldownProgress / dashCooldown);

        // Show the slider only when dash is on cooldown
        if (cooldownProgress != 0)
        {
            slider.gameObject.SetActive(true);
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }
}
