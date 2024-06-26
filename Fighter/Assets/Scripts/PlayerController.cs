using System;
using System.Collections;
using System.Collections.Generic;
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
    public float attack3Cooldown = 0.5f; // Cooldown time for Attack 3
    public float attack1Radius, attack2Radius, airAttackRadius, attack3Radius;

    // private variables
    public int direction = 1;
    public bool isFacingRight = true;
    public bool isGround;
    private bool canJump;
    private int jumpLeft;
    private bool canDash = false;
    private float dashTimeLeft;
    private float lastDash = -100f;
    private int airAttack = 1;
    private float lastAttack3Time = -100f; // Tracks the last time Attack 3 was performed
    private float attackRadius;
    private int attackDamage;
    private bool isKnockedBack;
    private int life;

    // animation variables
    private bool isRunning = false;

    // references
    public Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask ground;
    public TrailRenderer trailRenderer;
    public static PlayerController instance;
    public Transform A1Hitbox, A2Hitbox, AAHitbox, A3hitbox;
    public LayerMask damageable;
    private Transform box;
    public ParticleSystem dust;
    public ParticleSystem landDust;
    public Slider slider;
    public HealthBar healthBar;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ground = LayerMask.GetMask("Ground");
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
        if (canFlip)
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

        //show damage
        healthBar.setDamage(life);
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
        movementDirection = Input.GetAxisRaw("Horizontal_1");

        // jumping
        if ((Input.GetButtonDown("Jump_1") && canMove && canJump))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpLeft--;
        }

        // dashing
        if ((Input.GetButtonDown("Dash_1")) && Time.time >= (lastDash + dashCooldown) && movementDirection != 0)
        {
            AttemptToDash();
        }

        // check attacking
        if (Input.GetButtonDown("Fire1_1") && !isAttacking1 && isGround)
        {
            Debug.Log("attack1");
            box = A1Hitbox;
            attackRadius = attack1Radius;
            isAttacking1 = true;
            isAttacking2 = false;
        }
        if ((Input.GetButtonDown("Fire1_1") || Input.GetButtonDown("Fire2_1")) && !isAttacking1 && !isGround && airAttack == 1)
        {
            airAttack--;
            canAirAttack = true;
            attackRadius = airAttackRadius;
            box = AAHitbox;
        }
        if (Input.GetButtonDown("Fire2_1") && !isAttacking2 && isGround && Time.time >= (lastAttack3Time + attack3Cooldown))
        {
            box = A3hitbox;
            attackRadius = attack3Radius;
            isAttacking2 = true;
            isAttacking1 = false;
            lastAttack3Time = Time.time; // Update the last attack time
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
        if (!isKnockedBack)
        {
            // moving left and right
            if (canMove)
            {
                rb.velocity = new Vector2(movementSpeed * movementDirection, rb.velocity.y);
            }
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
        int me = 0;

        foreach (Collider2D obj in detectedObjects)
        {
            if (obj.gameObject != this.gameObject)
            {
                obj.transform.SendMessage("Damaged", dg);

                if (!isGround)
                {
                    jumpLeft++;
                }
            }
            else
            {
                me++;
            }
        }

        if (detectedObjects.Length > 0+me)
        {
            CameraShake.instance.Shake(dg / 2.0f, 0.25f);
        }
    }

    public void chainedHitbox()
    {
        box = A2Hitbox;
        attackRadius = attack2Radius;
        isAttacking1 = true;
        isAttacking2 = false;
    }

    private void Animate()
    {
        animator.SetBool("AirAttack", canAirAttack);
        animator.SetBool("Running", isRunning);
        animator.SetBool("isGrounded", isGround);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("Hit", isKnockedBack);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(A2Hitbox.position, attack2Radius);
        Gizmos.DrawWireSphere(A1Hitbox.position, attack1Radius);
        Gizmos.DrawWireSphere(AAHitbox.position, airAttackRadius);
        Gizmos.DrawWireSphere(A3hitbox.position, attack3Radius);
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

    public void Damaged(int damage)
    {
        life += damage;
        // Apply knockback force
        rb.velocity = new Vector2(life * PlayerController2.instance.direction, life);

        // Set isKnockedBack flag to true
        isKnockedBack = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
        if (PlayerController2.instance.direction < 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            if (isGround)
            {
                dust.Play();
            }
        }
        else if (PlayerController2.instance.direction> 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            if (isGround)
            {
                dust.Play();
            }
        }

        // Reset isKnockedBack flag after a certain duration
        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        // Wait for a duration before resetting isKnockedBack flag
        yield return new WaitForSeconds(life * 0.02f); // Adjust the duration as needed
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        // Reset isKnockedBack flag
        isKnockedBack = false;
        canMove = true;
        canFlip = true;
    }

    public void ResetLife()
    {
        life = 0;
        rb.velocity = new Vector2(0, 0);
    }



    public void EnableMove()
    {
        if (!canMove)
        {
            canMove = true;
            canFlip = true;
        }
    }
    public void DisableMove()
    {
        if (canMove)
        {
            canMove = false;
            canFlip = false;
            rb.velocity = new Vector2(movementDirection * 0, 0f);
        }
    }
}