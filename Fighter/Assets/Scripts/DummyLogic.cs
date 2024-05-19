using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLogic : MonoBehaviour
{
    //refrences
    public PlayerController pc;
    public PlayerController2 pc2;

    public Animator animator;

    private bool isFacingRight = false;

    public void Damaged(int damage)
    {
        animator.SetInteger("Damage", damage);
        // Determine the direction the dummy should face
        Vector3 directionToPlayer = pc.transform.position - transform.position;

        if (directionToPlayer.x > 0 && !isFacingRight)
        {
            // Player is to the right of the dummy and dummy is not facing right, rotate it
            transform.Rotate(0.0f, 180.0f, 0.0f);
            isFacingRight = !isFacingRight;
        }
        else if (directionToPlayer.x < 0 && isFacingRight)
        {
            // Player is to the left of the dummy and dummy is facing right, rotate it
            transform.Rotate(0.0f, 180.0f, 0.0f);
            isFacingRight = !isFacingRight;
        }
    }

    public void resetDamage()
    {
        animator.SetInteger("Damage", 0);
    }
}