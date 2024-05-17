using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class animationFunctions : MonoBehaviour
{
    public void noMoreAirAttack()
    {
        PlayerController.instance.canAirAttack = false;
    }

    public void MovingWhileAttackAir()
    {
        if (!PlayerController.instance.canMove)
        {
            PlayerController.instance.canMove = true;
            PlayerController.instance.canFlip = true;
        }
        else
        {
            PlayerController.instance.canMove = false;
            PlayerController.instance.canFlip = false;
            PlayerController.instance.rb.velocity = new Vector2(PlayerController.instance.movementDirection * 15, 0f);
        }
    }
}
