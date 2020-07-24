using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerWalk : StateMachineBehaviour
{
    // SO I don't have to create a new vector all the time?
    private Vector3 m_movement;

    // For rotation
    private float turnSmoothVelocity;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject player = animator.gameObject;
        PlayerMovementVariables movementstats = player.GetComponent<PlayerMovementVariables>();

        

        if (animator.GetFloat("Input/X") != 0 || animator.GetFloat("Input/Z") != 0)
        {
            // Direction of the camera to move in.
            Vector3 movementx = (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * animator.GetFloat("Input/X"));
            Vector3 movementz = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * animator.GetFloat("Input/Z"));
            Vector3 m_movement = (movementz + movementx);
            m_movement = m_movement.normalized;
           

            // Player will be rotated towards camera and strafe
            if (movementstats.m_lockOn)
            {
                // Rotate towards what camera is looking at or camera target.
                //player.transform.LookAt(player.GetComponent<PlayerMovementVariables>().m_target.transform);
                Vector3 bossdirection = movementstats.m_target.transform.position - player.transform.position;
                bossdirection = bossdirection.normalized;
                Debug.DrawRay(player.transform.position, bossdirection);
                float targetAngle = Mathf.Atan2(bossdirection.x, bossdirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementstats.m_roationTime);
                player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                
                float targetAngle = Mathf.Atan2(m_movement.x, m_movement.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementstats.m_roationTime);
                player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            // Player movement.
            player.GetComponent<Rigidbody>().AddForce(m_movement * movementstats.m_walkSpeed, ForceMode.Impulse);
            Vector3 toAnim = player.transform.worldToLocalMatrix* m_movement;
            toAnim = toAnim.normalized;
            //Debug.DrawRay(player.transform.position,toAnim);
            //Debug.Log(toAnim);
            animator.SetFloat("Movement/X", toAnim.x);
            animator.SetFloat("Movement/Z", toAnim.z);
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
