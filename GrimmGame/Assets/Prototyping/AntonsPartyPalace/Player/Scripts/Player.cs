﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A placeholder till input buffer is completed
// Wasd controls passed to animator.


public class Player : MonoBehaviour
{
   

    private Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;
        bool attack = false;
        // Inputs not stored should be 
        if (Input.GetKey(KeyCode.W))
        {
            y++;
        }
        if(Input.GetKey(KeyCode.S))
        {
            y--;
        }
        if (Input.GetKey(KeyCode.A))
        {
            x--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x++;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            attack = true;
        }

        UpdateAnimations(x, y, attack);

       
        PlayerMovementVariables movementstats = this.GetComponent<PlayerMovementVariables>();


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
                Vector3 bossdirection = movementstats.m_target.transform.position - this.transform.position;
                bossdirection = bossdirection.normalized;
                Quaternion targetRotation = Quaternion.LookRotation(bossdirection);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, movementstats.m_roationTime);
                ////Debug.DrawRay(player.transform.position, bossdirection);
                //float targetAngle = Mathf.Atan2(bossdirection.x, bossdirection.z) * Mathf.Rad2Deg;
                //float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementstats.m_roationTime);
                //player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                // Position to look towards
                Quaternion targetRotation = Quaternion.LookRotation(m_cameraPosition);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, movementstats.m_roationTime);

            }

            Vector3 m_movement = new Vector3(m_cameraPosition.x * movementstats.m_walkSpeed * Time.deltaTime, 0, m_cameraPosition.z * movementstats.m_walkSpeed * Time.deltaTime);

            //Debug.Log(m_movement);
            Debug.DrawRay(this.transform.position, m_movement);
            this.GetComponent<Rigidbody>().MovePosition(this.transform.position + m_movement);
            //player.transform.position = player.transform.position + m_movement;
            // What to pass to the animator.
            // May need extra rules to avoid sidesteps into diagonal.
            // They would go after the norm.
            Vector3 toAnim = this.transform.worldToLocalMatrix * m_movement;
            toAnim = toAnim.normalized;
            //Debug.DrawRay(player.transform.position,toAnim);
            //Debug.Log(toAnim);
            animator.SetFloat("Movement/X", toAnim.x);
            animator.SetFloat("Movement/Z", toAnim.z);
        }
    }



    // Send inputs to animator
    void UpdateAnimations(float a_x, float a_y, bool a_attack)
    {

        // x axis
        animator.SetFloat("Input/X", a_x);
        // y axis
        animator.SetFloat("Input/Z", a_y);

        if (a_attack)
            animator.SetTrigger("Input/Attack");

    }

}



