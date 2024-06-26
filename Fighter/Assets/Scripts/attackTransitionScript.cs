using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTransitionScript : StateMachineBehaviour
{
    public string nextAttack;
    public string attack2;
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
                    PlayerController.instance.animator.Play(nextAttack);
                    PlayerController.instance.isAttacking1 = false;

                }
                if (PlayerController.instance.isAttacking2)
                {
                    PlayerController.instance.animator.Play(attack2);
                    PlayerController.instance.isAttacking2 = false;
                }
                break;
            case 2:
                if (PlayerController2.instance.isAttacking1)
                {
                    PlayerController2.instance.animator.Play(nextAttack);
                    PlayerController2.instance.isAttacking1 = false;

                }
                if (PlayerController2.instance.isAttacking2)
                {
                    PlayerController2.instance.animator.Play(attack2);
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