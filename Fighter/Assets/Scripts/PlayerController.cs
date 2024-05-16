using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerController : MonoBehaviour
{
    //variables
    private float movementDirection; 
    public float movementSpeed = 10;
    private bool isFacingRight = true;
    public float jumpHeight = 16.0f;
    private bool isGround;
    public float groundCheckRadius;
    private bool canJump;
    public int jumpCount = 0;
    private int jumpLeft;
    private bool isDashing = false;
    public float dashTime;
    public float dashSpeed;
    public float dashCooldown;
    private float dashTimeLeft;
    private float lastDash = -100f;


    //animation variables
    private bool isRunning = false;

    //refrences
    private Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask ground;
    public TrailRenderer trailRenderer;

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
        }

        if(jumpLeft == 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

        //direction
        if (isFacingRight && movementDirection < 0)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if (!isFacingRight && movementDirection > 0)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
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
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(dashSpeed * movementDirection, 0f);
                dashTimeLeft -= Time.deltaTime;
                trailRenderer.emitting = true;
            }

            if (dashTimeLeft <= 0)
            {
                isDashing = false;
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
        if ((Input.GetButtonDown("Jump") && canJump))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpLeft--;
        }

        if ((Input.GetButtonDown("Dash")) && Time.time >= (lastDash + dashCooldown) && movementDirection != 0)
        {
            AttemptToDash();
        }
    }   
    
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
    }

    private void ApplyMovement()
    {
        //moving left and right
        rb.velocity = new Vector2(movementSpeed * movementDirection, rb.velocity.y);
    }

    private void Animate()
    {
        animator.SetBool("Running", isRunning);
        animator.SetBool("isGrounded", isGround);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isDashing", isDashing);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
