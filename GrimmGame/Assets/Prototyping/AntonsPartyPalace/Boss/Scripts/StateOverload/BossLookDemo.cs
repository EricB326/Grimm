using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLookDemo : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //   
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
       
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject lookpoint = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().GetLookPoint();
        float ikWeight = animator.GetFloat("Ai/LookWeight");
        Vector3 target = animator.gameObject.GetComponent<BossBrain>().m_target.transform.position - animator.gameObject.transform.position;
        Vector3 thispos = animator.gameObject.transform.forward;
        float dot = Vector3.Dot(target.normalized, thispos.normalized);
        if (dot > 0.5f)
        {
            ikWeight += 0.05f;
            if (ikWeight > 1)
            {
                ikWeight = 1;
            }
            animator.SetLookAtWeight(ikWeight);
            // This should be target
            animator.SetLookAtPosition(lookpoint.transform.position);
        }
        else
        {
            ikWeight -= 0.1f;
            if (ikWeight < 0)
            {
                ikWeight = 0;
            }
            animator.SetLookAtWeight(ikWeight);
            animator.SetLookAtPosition(lookpoint.transform.position);
        }
        animator.SetFloat("Ai/LookWeight", ikWeight);
    }

}
