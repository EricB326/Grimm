using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Values for each boss attack
public class BossAttackVariables : StateMachineBehaviour
{
    // If multiple attacks needs to be aware of 
    // what values to use.
    [HideInInspector]
    public int m_attackInChain = 0;
    public List<BossWeaponColliderEnums> m_colliderToActivate;
    // List of the attack values.
    // Will probably crash without attacknumber matching the rest of the lists.
    public List<float> m_damage;
    // On hit effects.
    // Should be a list of lists. So multiple on hit effects may occur.
    // public List<OnHitEffects> m_damage;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_attackInChain = 0;
        animator.gameObject.GetComponent<BossBrain>().m_currentAttackVariables = this;
    }

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
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
