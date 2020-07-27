using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Overloading movement state.
// All movement and rotation math occurs here.
// Relevant data is then passed back into the state 
// machine to for animation.

// Note that this can be moved to the player update if so desired without any issues.


public class PlayerWalk : StateMachineBehaviour
{
    // So I don't have to create a new vector all the time?
    private Vector3 m_movement;


    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject player = animator.gameObject;
        PlayerMovementVariables movementstats = player.GetComponent<PlayerMovementVariables>();

        
        // This if check may be redundant.
        // Only way into this state is for input is received.
        if (animator.GetFloat("Input/X") != 0 || animator.GetFloat("Input/Z") != 0)
        {
            // Direction for the player to move towards based on camera.
            Vector3 camerax = (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * animator.GetFloat("Input/X"));
            Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * animator.GetFloat("Input/Z"));
            Vector3 m_cameraPosition = (cameraz + camerax);
            //m_movement = m_movement.normalized;
           

            // Slightly different data is used but the math is the same in either case.
            // If locked on you rotate towards your target when you move resulting in 
            // circling the target.
            if (movementstats.m_lockOn)
            {
                Vector3 bossdirection = movementstats.m_target.transform.position - player.transform.position;
                bossdirection = bossdirection.normalized;
                Quaternion targetRotation = Quaternion.LookRotation(bossdirection);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, movementstats.m_roationTime);
                ////Debug.DrawRay(player.transform.position, bossdirection);
                //float targetAngle = Mathf.Atan2(bossdirection.x, bossdirection.z) * Mathf.Rad2Deg;
                //float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementstats.m_roationTime);
                //player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                // Position to look towards
                Quaternion targetRotation = Quaternion.LookRotation(m_cameraPosition);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, movementstats.m_roationTime);
                
            }

            m_movement = new Vector3(m_cameraPosition.x * movementstats.m_walkSpeed * Time.deltaTime, 0, m_cameraPosition.z * movementstats.m_walkSpeed * Time.deltaTime);

            //Debug.Log(m_movement);
            Debug.DrawRay(player.transform.position, m_movement);
            player.GetComponent<Rigidbody>().MovePosition(player.transform.position + m_movement);
            //player.transform.position = player.transform.position + m_movement;
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


//Old version of rotation - Has issues with cumilative rotation when changing directions
//float targetAngle = Mathf.Atan2(m_movement.x, m_movement.z) * Mathf.Rad2Deg;
//float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementstats.m_roationTime);
//player.transform.rotation = Quaternion.Euler(0f, angle, 0f);


//old version of Player movement -  Player is jittery because of result of math and unreliable movement with other forces.
//player.GetComponent<Rigidbody>().AddForce(m_movement * movementstats.m_walkSpeed, ForceMode.Force);