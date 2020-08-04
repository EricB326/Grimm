using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


// A placeholder till input buffer is completed
// Wasd controls passed to animator.

[RequireComponent(typeof(PlayerMovementVariables))]
[RequireComponent(typeof(AnimationEventsPlayer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
   

    private Animator animator;

    private float turnSmoothVelocity;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float x = 0;
        float y = 0;
        bool attack = false;
        bool roll = false;

        float axisX = XCI.GetAxis(XboxAxis.LeftStickX);

        float axisY = XCI.GetAxis(XboxAxis.LeftStickY);


        //// Keyboard controls
        //if (Input.GetKey(KeyCode.W))
        //{
        //    y++;
        //}
        //if(Input.GetKey(KeyCode.S))
        //{
        //    y--;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    x--;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    x++;
        //}

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    attack = true;
        //    //Debug.Log("attack");
        //}

        //if(Input.GetKey(KeyCode.LeftShift))
        //{
        //    roll = true;
        //}


        if (XCI.GetButton(XboxButton.A))
        {
            attack = true;
        }

        if (XCI.GetButton(XboxButton.B))
        {
            roll = true;
        }



        UpdateAnimations(axisX, axisY, attack, roll);


        // Value are adjusted by animation state overloading.
        // Rules for rolling and movement are almost exactley the same.
        if (!animator.GetBool("IsRolling"))
            {
            if (animator.GetBool("Output/CanMove"))
            {
                Movement(axisX, axisY);
            }
            else if (animator.GetBool("Input/Roll"))
            {
                StartRoll(axisX, axisY);
            }
        }
        else
        {
            Rolling();
        }
        

    }



    // Send inputs to animator
    void UpdateAnimations(float a_x, float a_y, bool a_attack, bool a_roll)
    {
        // x axis
        animator.SetFloat("Input/X", a_x);
        // y axis
        animator.SetFloat("Input/Z", a_y);
        // If attack pressed and enough stamina.
        if (a_attack && EntityStats.Instance.CanEntityMoveOccur("Player", this.GetComponent<PlayerMovementVariables>().m_attackStaminaDrain))
        {
            //Debug.Log("Player has enough stamina");
            animator.SetBool("Input/Attack", a_attack);
        }
        else
        {
            animator.SetBool("Input/Attack", false);
        }
        if(a_roll)
        {
            animator.SetBool("Input/Roll", true);
        }
        else
        {
            animator.SetBool("Input/Roll", false);
        }
    }


    // Walking/Running
    private void Movement(float a_axisX, float a_axisY)
    {
        PlayerMovementVariables movementstats = this.GetComponent<PlayerMovementVariables>();

        // This if check may be redundant.
        // Only way into this state is for input is received.
        if (a_axisX != 0 || a_axisY != 0)
        {
            // Direction for the player to move towards based on camera.
            Vector3 camerax = (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * a_axisX);
            Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * a_axisY);
            Vector3 m_cameraPosition = (cameraz + camerax);
            //m_movement = m_movement.normalized;


            // Slightly different data is used but the math is the same in either case.
            // If locked on you rotate towards your target when you move resulting in 
            // circling the target.
            CameraRotation cameraReference = Camera.main.GetComponent<CameraRotation>();
            if (Camera.main.GetComponent<CameraRotation>().m_lockOn)
            {
                Vector3 bossdirection = movementstats.m_target.transform.position - this.transform.position;
                bossdirection = bossdirection.normalized;
                // To make sure player doesn't up or down. Only facing.
                // Take not that head will need the y.
                bossdirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(bossdirection);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, movementstats.m_rotationTime);

                //Debug.DrawRay(player.transform.position, bossdirection);
                //float targetAngle = Mathf.Atan2(bossdirection.x, bossdirection.z) * Mathf.Rad2Deg;
                //float angle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementstats.m_roationTime);
                //this.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                // Position to look towards
                Quaternion targetRotation = Quaternion.LookRotation(m_cameraPosition);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, movementstats.m_rotationTime);
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
            float scale = Mathf.Max(Mathf.Abs(a_axisX), Mathf.Abs(a_axisY));
            animator.SetFloat("Movement/X", toAnim.x * scale);
            animator.SetFloat("Movement/Z", toAnim.z * scale);
        }
        else
        {
            animator.SetFloat("Movement/X", 0);
            animator.SetFloat("Movement/Z", 0);
        }
    }

    // Initial roll math.
    private void StartRoll(float a_axisX, float a_axisY)
    {
        // Stores direction wanting to move reletive to camera.
        // if no input back by default.
        // then do the rolling
    }

    private void Rolling()
    {

    }
}



