using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public int jumpLeft;


    //animation variables
    private bool isRunning = false;

    //refrences
    private Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask ground;

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
        if (rb.velocity.x != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
    }

    //checks for all player inputs
    private void CheckInput()
    {
        //direction movement
        movementDirection = Input.GetAxisRaw("Horizontal");

        //jumping
        if ((Input.GetButtonDown("Jump") && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpLeft--;
            Debug.Log(jumpLeft);
        }
    }    

    private void ApplyMovement()
    {
        //moving left and right
        rb.velocity = new Vector2(movementSpeed*movementDirection, rb.velocity.y);
    }

    private void Animate()
    {
        animator.SetBool("Running", isRunning);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
