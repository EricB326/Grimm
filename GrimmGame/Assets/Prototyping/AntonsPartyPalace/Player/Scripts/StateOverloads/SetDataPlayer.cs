using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Used to turn Outputs types on when entering and off when exiting.

public class SetDataPlayer : StateMachineBehaviour
{
    public PlayerDataEnums m_data;



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (m_data)
        {
            case PlayerDataEnums.OUTPUT_ISROLLING:
                {
                    animator.SetBool("Output/IsRolling", true);
                    break;
                }
            case PlayerDataEnums.OUTPUT_CANMOVE:
                {
                    animator.SetBool("Output/CanMove", true);
                    break;
                }
            default:
                {
                    Debug.Log("VALUE " + m_data.ToString() + " NOT SET TO BE SET AS OF YET. IT'S ANTON'S FAULT.");
                    break;
                }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (m_data)
        {
            case PlayerDataEnums.OUTPUT_ISROLLING:
                {
                    animator.SetBool("Output/IsRolling", false);
                    break;
                }
            case PlayerDataEnums.OUTPUT_CANMOVE:
                {
                    animator.SetBool("Output/CanMove", false);
                    break;
                }
            default:
                {
                    Debug.Log("VALUE " + m_data.ToString() + " NOT SET TO BE SET AS OF YET. IT'S ANTON'S FAULT.");
                    break;
                }
        }
    }

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
