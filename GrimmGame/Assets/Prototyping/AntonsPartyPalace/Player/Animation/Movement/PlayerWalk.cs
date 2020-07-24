using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Overloading movement state.
// All movement and rotation math occurs here.
// Relevant data is then passed back into the state 
// machine to for animation.

public class PlayerWalk : StateMachineBehaviour
{
    // So I don't have to create a new vector all the time?
    private Vector3 m_movement;

    // For rotation dampening
    private float turnSmoothVelocity;
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject player = animator.gameObject;
        PlayerMovementVariables movementstats = player.GetComponent<PlayerMovementVariables>();

        
        // This if check may be redundant.
        // Only way into this state is for input is received.
        if (animator.GetFloat("Input/X") != 0 || animator.GetFloat("Input/Z") != 0)
        {
            // Direction of the camera to move in.
            Vector3 movementx = (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * animator.GetFloat("Input/X"));
            Vector3 movementz = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * animator.GetFloat("Input/Z"));
            Vector3 m_movement = (movementz + movementx);
            m_movement = m_movement.normalized;
           

            // Slightly different data is used but the math is the same in either case.
            // If locked on you rotate towards your target when you move resulting in 
            // circling the target.
            if (movementstats.m_lockOn)
            {
                Vector3 bossdirection = movementstats.m_target.transform.position - player.transform.position;
                bossdirection = bossdirection.normalized;
                //Debug.DrawRay(player.transform.position, bossdirection);
                float targetAngle = Mathf.Atan2(bossdirection.x, bossdirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementstats.m_roationTime);
                player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            // Otherwise you rotate towards the direction your walking towards
            // and never sidestep or circle.
            // Rule needs to be preserving the direction started moving in if it is within <120
            // then derive all movement from that.
            // 1. Rotate towards target
            // 2. Run while preserving direction with any changes compared against preserved rather then camera.
            // 3. Clamp movement to the 120 forward degree area(Can only run forward, slightly left, slightly right)
            // 4. If change in direction is not infront anymore or no more input Start from step.1.
            else
            {
                float targetAngle = Mathf.Atan2(m_movement.x, m_movement.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementstats.m_roationTime);
                player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            // Player movement. Player is jittery because of result of math.
            player.GetComponent<Rigidbody>().AddForce(m_movement * movementstats.m_walkSpeed, ForceMode.Force);

            // What to pass to the animator.
            // May need extra rules to avoid sidesteps into diagonal.
            // They would go after the norm.
            Vector3 toAnim = player.transform.worldToLocalMatrix * m_movement;
            toAnim = toAnim.normalized;
            //Debug.DrawRay(player.transform.position,toAnim);
            //Debug.Log(toAnim);
            animator.SetFloat("Movement/X", toAnim.x);
            animator.SetFloat("Movement/Z", toAnim.z);
        }
    }
}
