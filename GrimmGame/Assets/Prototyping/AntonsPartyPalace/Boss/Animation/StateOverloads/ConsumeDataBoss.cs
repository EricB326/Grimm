using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeDataBoss : StateMachineBehaviour
{
    public List<BossDataEnums> m_dataToClear;



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (BossDataEnums clear in m_dataToClear)
        {
            switch (clear)
            {
                case BossDataEnums.AI_ATTACK:
                    {
                        animator.SetInteger("Ai/Attack", 0);
                        break;
                    }
                case BossDataEnums.AI_PHASE:
                    {
                        animator.SetInteger("Ai/AttackPhase", 0);
                        break;
                    }
                case BossDataEnums.AI_ISDASHING:
                    {
                        animator.SetBool("Ai/IsDashing", false);
                        break;
                    }
                case BossDataEnums.MOVEMENT_X:
                    {
                        //animator.SetFloat("Movement/X", 0);
                        break;
                    }
                case BossDataEnums.MOVEMENT_Z:
                    {
                        //animator.SetFloat("Movement/Z", 0);
                        break;
                    }
                default:
                    {
                        Debug.Log("VALUE " + m_dataToClear.ToString() + " NOT SET TO BE CONSUMED AS OF YET. IT'S ANTON'S FAULT.");
                        break;
                    }
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // Data clearing goes here. Any data in the list gets set back to 0.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (BossDataEnums clear in m_dataToClear)
        {
            switch (clear)
            {
                case BossDataEnums.AI_ATTACK:
                    {
                        //animator.SetInteger("Ai/Attack", 0);
                        break;
                    }
                case BossDataEnums.AI_PHASE:
                    {
                        //animator.SetInteger("Ai/AttackPhase", 0);
                        break;
                    }
                case BossDataEnums.AI_ISDASHING:
                    {
                        //animator.SetBool("Ai/IsDashing", false);
                        break;
                    }
                case BossDataEnums.MOVEMENT_X:
                    {
                        animator.SetFloat("Movement/X", 0);
                        break;
                    }
                case BossDataEnums.MOVEMENT_Z:
                    {
                        animator.SetFloat("Movement/Z", 0);
                        break;
                    }
                default:
                    {
                        Debug.Log("VALUE " + m_dataToClear.ToString() + " NOT SET TO BE CONSUMED AS OF YET. IT'S ANTON'S FAULT.");
                        break;
                    }
            }
        }
    }
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}