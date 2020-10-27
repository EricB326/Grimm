using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Quick script to have the birds cycle their idles
// randomly.
// Oh god. Unity lies about how to lerp.
public class BirdIdleCycle : StateMachineBehaviour
{
    private float m_timeBetweenNewIdle = 4f;
    private float m_NextTimeCycle = 0;
    // Should take 10 frames to reach destination
    private float m_rate = 0.01f;
    // How far along the lerp is.
    // Starts at 0 and increases to 1
    private float m_distanceSoFar = 1;

    // The blend value at start.
    private float m_startPosition;

    // The blend value at the end
    private float m_endPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_distanceSoFar >= 1f)
        {
            if (m_NextTimeCycle < Time.time)
            {
                // Get a new number to move towards.
                m_endPosition = Random.Range(0, 1f);
                // Get current position
                m_startPosition = animator.GetFloat("Blend");
                // Set how far we've traveled so far.
                m_distanceSoFar = 0;
                // Set next time the idle change
                m_NextTimeCycle = Time.time + m_timeBetweenNewIdle;
            }
        }
        else
        {
            m_distanceSoFar += m_rate;
            animator.SetFloat("Blend", (Mathf.Lerp(m_startPosition, m_endPosition, m_distanceSoFar)));
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
