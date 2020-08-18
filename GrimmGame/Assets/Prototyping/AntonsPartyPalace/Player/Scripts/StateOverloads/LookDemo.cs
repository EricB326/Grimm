using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDemo : StateMachineBehaviour
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
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float ikWeight = animator.GetFloat("LookWeight");
        // Probably better for lock on and not running.
        // Direction isn't really needed.
        Vector3 direction = EntityStats.Instance.GetObjectOfEntity("Boss").transform.position 
            - animator.gameObject.transform.position;
        if (animator.gameObject.GetComponentInParent<Player>().m_lockon)
        {
            ikWeight += 0.05f;
            if (ikWeight > 1)
            {
                ikWeight = 1;
            }
            animator.SetLookAtWeight(ikWeight, 0.55f, 0.8f, 1.0f);
            // This should be target
            Vector3 targetPos = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_lookPoint.transform.position;
            targetPos.y -= 1f;
            animator.SetLookAtPosition(targetPos);
        }
        else
        {
            ikWeight -= 0.1f;
            if (ikWeight < 0)
            {
                ikWeight = 0;
            }
            animator.SetLookAtWeight(ikWeight, 0.55f, 0.8f, 1.0f);
            Vector3 targetPos = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_lookPoint.transform.position;
            targetPos.y = 1;
            animator.SetLookAtPosition(targetPos);
        }
        animator.SetFloat("LookWeight", ikWeight);

        // Could this be used in ik to move the body to the correct
        // place when legs are applying Ik?
        //animator.MatchTarget(AvatarTarget.Body);
    }

}
