using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeDataBoss : StateMachineBehaviour
{
     public List<AiDataEnums> m_dataToClear;



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (AiDataEnums clear in m_dataToClear)
        {
            switch (clear)
            {
                case AiDataEnums.MoveSet:
                    {
                        animator.SetInteger("Ai/Moveset", 0);
                        break;
                    }
                case AiDataEnums.CounterAttack:
                    {
                        animator.SetInteger("Ai/CounterAttack", 0);
                        break;
                    }
                case AiDataEnums.TimePassed:
                    {
                        animator.SetInteger("Ai/TimePassed", 0);
                        break;
                    }
                case AiDataEnums.RemainingBudget:
                    {
                        animator.SetInteger("Ai/RemainingBudget", 0);
                        break;
                    }
                case AiDataEnums.Attack:
                    {
                        animator.SetInteger("Ai/Attack", 0);
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
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

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
