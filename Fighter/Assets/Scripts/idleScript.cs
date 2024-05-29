using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleScript : StateMachineBehaviour
{
    public int playerNumber;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (playerNumber)
        {
            case 1:
                if (PlayerController.instance.isAttacking1)
                {
                    switch (PlayerController.instance.characterNumber)
                    {
                        case 1:
                            PlayerController.instance.animator.Play("Sand_A1");
                            break;
                        case 2:
                            PlayerController.instance.animator.Play("FireA1");
                            break;
                        default:
                            Debug.Log("Set character number in player controller script");
                            break;
                    }
                    PlayerController.instance.isAttacking1 = false;
                }
                if (PlayerController.instance.isAttacking2)
                {
                    switch (PlayerController.instance.characterNumber)
                    {
                        case 1:
                            PlayerController.instance.animator.Play("Sand_A3");
                            break;
                        case 2:
                            PlayerController.instance.animator.Play("Fire_A3");
                            break;
                        default:
                            Debug.Log("Set character number in player controller script");
                            break;
                    }
                    PlayerController.instance.isAttacking2 = false;
                }
                break;
            case 2:
                if (PlayerController2.instance.isAttacking1)
                {
                    PlayerController2.instance.animator.Play("FireA1");
                    PlayerController2.instance.isAttacking1 = false;
                }
                if (PlayerController2.instance.isAttacking2)
                {
                    PlayerController2.instance.animator.Play("Fire_A3");
                    PlayerController2.instance.isAttacking2 = false;
                }
                break;
            default:
                Debug.Log("Player number not set");
                break;
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}