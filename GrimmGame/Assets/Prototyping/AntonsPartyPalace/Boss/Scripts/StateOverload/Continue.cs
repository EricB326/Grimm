using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evaluates if this attack chain should be continued


public class Continue : StateMachineBehaviour
{
    public float distance = 2;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    Vector3 t = animator.gameObject.GetComponent<BossBrain>().m_target.transform.position - animator.gameObject.transform.position;
    float largestDistance = Mathf.Max(Mathf.Abs(t.x), Mathf.Abs(t.z));
        if (largestDistance < distance)
        {
            animator.SetBool("Ai/Continue", true);
        }
        else
        {
            animator.SetBool("Ai/Continue", false);
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
