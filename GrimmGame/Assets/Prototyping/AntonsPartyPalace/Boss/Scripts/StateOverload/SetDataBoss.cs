using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDataBoss : StateMachineBehaviour
{
    public BossDataEnums m_data;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (m_data)
        {
            case BossDataEnums.AI_ISMOVEMENT:
                {
                    animator.SetBool("Ai/IsMoving", true);
                    break;
                }
            case BossDataEnums.AI_ISDASHING:
                {
                    animator.SetBool("Ai/IsDashing", true);
                    break;
                }
            case BossDataEnums.AI_ISATTACKING:
                {
                    animator.SetBool("Ai/IsAttacking", true);
                    break;
                }
            case BossDataEnums.AI_ISROTATING:
                {
                    animator.SetBool("Ai/CanRotate", true);
                    break;
                }
            case BossDataEnums.AI_ISINACTION:
                {
                    animator.SetBool("Ai/PlayingAction", true);
                    break;
                }
            default:
                {
                    Debug.Log("VALUE " + m_data.ToString() + " NOT SET TO BE SET AS OF YET. IT'S ANTON'S FAULT.");
                    break;
                }
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (m_data)
        {
            case BossDataEnums.AI_ISMOVEMENT:
                {
                    animator.SetBool("Ai/IsMoving", false);
                    break;
                }
            case BossDataEnums.AI_ISDASHING:
                {
                    animator.SetBool("Ai/IsDashing", false);
                    break;
                }
            case BossDataEnums.AI_ISATTACKING:
                {
                    animator.SetBool("Ai/IsAttacking", false);
                    break;
                }
            case BossDataEnums.AI_ISROTATING:
                {
                    animator.SetBool("Ai/CanRotate", false);
                    break;
                }
            case BossDataEnums.AI_ISINACTION:
                {
                    animator.SetBool("Ai/PlayingAction", false);
                    break;
                }
            default:
                { 
                    Debug.Log("VALUE " + m_data.ToString() + " NOT SET TO BE SET AS OF YET. IT'S ANTON'S FAULT.");
                    break;
                }
         
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
