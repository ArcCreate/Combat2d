using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleScript : StateMachineBehaviour
{
    public int playerNumber;
    public string attack1Name;
    public string attack2Name;
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
            //player 1
            case 1:
                if (PlayerController.instance.isAttacking1)
                {
                    PlayerController.instance.animator.Play(attack1Name);
                    PlayerController.instance.isAttacking1 = false;
                }
                if (PlayerController.instance.isAttacking2)
                {
                    PlayerController.instance.animator.Play(attack2Name);
                    PlayerController.instance.isAttacking2 = false;
                }
                break;

            //player 2
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