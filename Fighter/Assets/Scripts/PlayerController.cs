using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerController : MonoBehaviour
{
    //public variables
    public float movementSpeed = 10;
    public float jumpHeight = 16.0f;
    public float groundCheckRadius;
    public int jumpCount = 0;
    public float dashTime;
    public float dashSpeed;
    public float dashCooldown;
    public bool isAttacking1;
    public bool isAttacking2;
    public bool canFlip;
    public bool canMove;
    public bool canAirAttack;
    public float movementDirection;

    //private variables
    private int direction = 1;
    private bool isFacingRight = true;
    private bool isGround;
    private bool canJump;
    private int jumpLeft;
    private bool canDash = false;
    private float dashTimeLeft;
    private float lastDash = -100f;
    private int airAttack = 1;


    //animation variables
    private bool isRunning = false;

    //refrences
    public Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask ground;
    public TrailRenderer trailRenderer;
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpLeft = jumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Animate();

        //checking parameters
        //jump
        if (isGround)
        {
            jumpLeft = jumpCount;
            airAttack = 1;
        }
        //continuos jump
        if(jumpLeft == 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

        //direction
        if (canFlip && canMove)
        {
            if (isFacingRight && movementDirection < 0)
            {
                direction = -1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
            else if (!isFacingRight && movementDirection > 0)
            {
                direction = 1;
                isFacingRight = !isFacingRight;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }

        //checking if running
        isRunning = Mathf.Abs(rb.velocity.x) > 0.01f;
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

    //checks for all player inputs
    private void CheckInput()
    {
        //direction movement
        movementDirection = Input.GetAxisRaw("Horizontal");


        //jumping
        if ((Input.GetButtonDown("Jump") && canJump && canMove))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpLeft--;
        }

        //dashing
        if ((Input.GetButtonDown("Dash")) && Time.time >= (lastDash + dashCooldown) && movementDirection != 0)
        {
            AttemptToDash();
        }

        //check attacking
        if(Input.GetButtonDown("Fire1") && !isAttacking1 && isGround)
        {
            isAttacking1 = true;
            isAttacking2 = false;
        }
        else if (Input.GetButtonDown("Fire1") && !isAttacking1 && !isGround && airAttack == 1)
        {
            airAttack--;
            canAirAttack = true;
        }
        if (Input.GetButtonDown("Fire2") && !isAttacking2 && isGround)
        {
            isAttacking2 = true;
            isAttacking1 = false;
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
        //moving left and right
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
            rb.velocity = new Vector2(movementDirection * 5, 0f);
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
    }
}
