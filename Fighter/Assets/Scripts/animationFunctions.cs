using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class animationFunctions : MonoBehaviour
{
    public int playerNumber;
    public void noMoreAirAttack()
    {
        switch (playerNumber)
        {
            case 1:
                PlayerController.instance.canAirAttack = false;
                break;
            case 2:
                PlayerController2.instance.canAirAttack = false;
                break;
            default:
                Debug.Log("Player number not set");
                break;
        }
    }

    public void MovingWhileAttackAir()
    {
        switch (playerNumber)
        {
            case 1:

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
                break;
            case 2:

                if (!PlayerController2.instance.canMove)
                {
                    PlayerController2.instance.canMove = true;
                    PlayerController2.instance.canFlip = true;
                }
                else
                {
                    PlayerController2.instance.canMove = false;
                    PlayerController2.instance.canFlip = false;
                    PlayerController2.instance.rb.velocity = new Vector2(PlayerController2.instance.movementDirection * 15, 0f);
                }
                break;
            default:
                Debug.Log("Player number not set");
                break;
        }
    }
}