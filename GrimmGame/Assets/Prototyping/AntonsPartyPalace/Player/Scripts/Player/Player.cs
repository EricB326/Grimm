using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using XboxCtrlrInput;


// A placeholder till input buffer is completed
// Wasd controls passed to animator.
// Now supports xboxinput.

[RequireComponent(typeof(PlayerMovementVariables))]
[RequireComponent(typeof(AnimationEventsPlayer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private Animator m_animator;

    private float turnSmoothVelocity;
         
    private Vector3 m_storedRollDirection;

    private float m_rollMultipliyer;

    private void Start()
    {
        m_animator = this.GetComponent<Animator>();
    }


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

        // Is idealling going to be done by input buffer.
        if (XCI.GetButton(XboxButton.A))
        {
            attack = true;
        }

        if (XCI.GetButton(XboxButton.B))
        {
            roll = true;
        }

        UpdateAnimations(axisX, axisY, attack, roll);
        // Rotate towards target if locked on
        if(Camera.main.GetComponent<CameraRotation>().m_lockOn)
        {
            LockOnLook();
        }

        // Oh god this is an if else loop that is cancer. Why did I do this?
        // Needs to occur only if the player presses the input and is not currently rolling
        if (m_animator.GetBool("Input/Roll") && !m_animator.GetBool("Output/IsRolling"))
        {
            StartRoll(axisX, axisY);
        }
        // Can only occur when currently rolling
        else if (m_animator.GetBool("Output/IsRolling"))
        {
            Rolling();
        }
        // Least important. Rotation occurs here for movement.
        else if (m_animator.GetBool("Output/CanMove"))
        {
            Movement(axisX, axisY);
        }
    }

    // Send inputs to animator.
    // Has a few rules to check if player has stamina but that may need
    // to be moved to the input buffer on request.
    void UpdateAnimations(float a_x, float a_y, bool a_attack, bool a_roll)
    {
        // x axis
        m_animator.SetFloat("Input/X", a_x);
        // y axis
        m_animator.SetFloat("Input/Z", a_y);
        // If attack pressed and enough stamina.
        if (a_attack && EntityStats.Instance.CanEntityMoveOccur("Player", this.GetComponent<PlayerMovementVariables>().m_attackStaminaDrain))
        {
            m_animator.SetBool("Input/Attack", a_attack);
        }
        else
        {
            m_animator.SetBool("Input/Attack", false);
        }
        if (a_roll && EntityStats.Instance.CanEntityMoveOccur("Player", this.GetComponent<PlayerMovementVariables>().m_rollStaminaDrain) && !m_animator.GetBool("Output/IsRolling"))
        {
            m_animator.SetBool("Input/Roll", true);
        }
        else
        {
            m_animator.SetBool("Input/Roll", false);
        }
    }


    // Walking/Running with rotation towards movement direciton.
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
            Vector3 cameraPosition = (cameraz + camerax);

            // If not locked on we want to rotate the player
            if (!Camera.main.GetComponent<CameraRotation>().m_lockOn)
            {
                FreeLook(a_axisX, a_axisY);
            }


            // Adjust movement direction by angle below 
            // Eg angle is 45 therfore forward is 45 up.
            // cast in the direciton moved and cast straight down.
            // Cast straight down
            // cast down in direciton heading.
            // If conatct position in direciton heading higher pos movement
            // If conatct position in direciton heading lower neg movement
            RaycastHit hit1;
            Debug.DrawLine(transform.position, transform.up);
            if (Physics.Raycast(transform.position, -transform.up, out hit1, 2)) // Cast down
            {
                //Debug.Log(hit1.point);
                RaycastHit hit2;
                if (Physics.Raycast(transform.position + (cameraPosition.normalized/2), -transform.up, out hit2, 2)) // cast in direction
                {

                }

            }
            

            Vector3 m_movement = new Vector3(cameraPosition.x * movementstats.m_walkSpeed * Time.deltaTime, 0, cameraPosition.z * movementstats.m_walkSpeed * Time.deltaTime);

            //Debug.Log(m_movement);
            this.GetComponent<Rigidbody>().MovePosition(this.transform.position + m_movement);
            //player.transform.position = player.transform.position + m_movement;
            // What to pass to the animator.
            // Used for blend trees.
            // This should always be passed in.
            Vector3 toAnim = this.transform.worldToLocalMatrix * m_movement;
            toAnim = toAnim.normalized;
            float scale = Mathf.Max(Mathf.Abs(a_axisX), Mathf.Abs(a_axisY));
            m_animator.SetFloat("Movement/X", toAnim.x * scale);
            m_animator.SetFloat("Movement/Z", toAnim.z * scale);
        }
        else
        {
            m_animator.SetFloat("Movement/X", 0);
            m_animator.SetFloat("Movement/Z", 0);
        }
    }

    // Initial roll math to get the direction that the 
    // player intended to travel.
    private void StartRoll(float a_axisX, float a_axisZ)
    {
        // Stores direction wanting to move reletive to camera.
        // if no input back by default.
        // then do the rolling function
        Vector3 toAnim;

        // Need to check if any input at all.
        // If not the direction to move is always back
        if (a_axisX == 0 && a_axisZ == 0)
        {
            a_axisZ = -1;
            m_storedRollDirection = transform.forward * a_axisZ;
            a_axisZ = -1;
        }
        else
        {
            Vector3 movementX = (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * a_axisX);
            Vector3 movementZ = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * a_axisZ);
            Vector3 cameraPosition = (movementZ + movementX);
            //Relative to the direction player is facing.
            m_storedRollDirection = cameraPosition;
        }
        toAnim = this.transform.worldToLocalMatrix * m_storedRollDirection.normalized;

        toAnim = toAnim.normalized;

        float scale = Mathf.Max(Mathf.Abs(a_axisX), Mathf.Abs(a_axisZ));
        m_animator.SetFloat("Movement/X", toAnim.x * scale);
        m_animator.SetFloat("Movement/Z", toAnim.z * scale);
        // Scale to be applied when rolling.
        // m_rollMultipliyer = Mathf.Lerp(0, 1, 0.1f);
        // Should not need to be called here as we check the input above 
        // Rolling();
    }

    // While player is rolling this funciton is called.
    private void Rolling()
    {
        PlayerMovementVariables movementstats = this.GetComponent<PlayerMovementVariables>();
        Vector3 m_movement = new Vector3((m_storedRollDirection.x * movementstats.m_rollSpeed) * Time.deltaTime, 0, (m_storedRollDirection.z * movementstats.m_rollSpeed) * Time.deltaTime);
        this.GetComponent<Rigidbody>().MovePosition(this.transform.position + m_movement);
    }

    // Character rotates towards target when locked on.
    // Occurs seperate from move.
    private void LockOnLook()
    {
        Vector3 bossdirection = this.GetComponent<PlayerMovementVariables>().m_target.transform.position - this.transform.position;
        bossdirection = bossdirection.normalized;
        // To make sure player doesn't up or down. Only facing.
        // Take not that head will need the y.
        bossdirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(bossdirection);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.GetComponent<PlayerMovementVariables>().m_rotationTime);
    }    

    // Character rotates towards direction they're moving.
    private void FreeLook(float a_axisX, float a_axisY)
    {
        Vector3 camerax = (new Vector3(Camera.main.transform.right.x, this.transform.up.x, Camera.main.transform.right.z) * a_axisX);
        Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, this.transform.up.x, Camera.main.transform.forward.z) * a_axisY);
        Vector3 cameraPosition = (cameraz + camerax);

        Quaternion targetRotation = Quaternion.LookRotation(cameraPosition);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.GetComponent<PlayerMovementVariables>().m_rotationTime);
    }
}