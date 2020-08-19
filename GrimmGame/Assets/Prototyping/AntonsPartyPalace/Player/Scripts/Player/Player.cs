using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using XboxCtrlrInput;


// A placeholder till input buffer is completed
// Wasd controls passed to animator.
// Now supports xboxinput.

//[RequireComponent(typeof(PlayerMovementVariables))]
//[RequireComponent(typeof(AnimationEventsPlayer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [Header("Drag from scene")]
    public Animator m_animator;
    // Camera follow point
    public GameObject m_lookPoint;
    // Used in the player walk till access to entity list.
    public GameObject m_target;
    // Just a quick way to get to the sword.
    // For visuals and particles.
    public Collider m_swordHitBox;
    // Consistent attack hitbox. 
    // If entity is inside this it will trigger OnHitEffects including damage.
    public Collider m_attackHitBox;

    private float turnSmoothVelocity;
         
    private Vector3 m_storedRollDirection;

    private float m_rollMultipliyer;

    private BufferInput m_currentFrame;

    // Multiplied by direction stick input. -1 to 1 on both axis
    public float m_walkSpeed = 5;
    // Speed will increase to this value.
    public float m_runSpeed = 7;
    // Roll speed. Ideally should accelerate to this speed rather rapidly.
    public float m_rollSpeed = 12;
    [Range(0, 1)]
    public float m_rotationTime;



    // Stamina drain on attack. Should be consistent.
    public int m_attackStaminaDrain = 20;
    // Roll Stamina drain
    public int m_rollStaminaDrain = 25;

    public int m_runStaminaDrain = 2;
    // Roll damage multiplier value.
    [Range(0, 1)]
    public float m_rollDamagemultiplier = 0.80f;
    // Damage per attack. Should be consistent.
    public int m_attackDamage = 10;
    

    // Only used to decide if the player can be hit. Basicaly god mode.
    // Unable to be hit if IFrames true;
    // Modified by an animation event.
    public bool m_InvinceFrames = false;

    public bool m_lockon = false;

    public BasicBuffer m_inputBuffer;



    private void Start()
    {       
        // This is better to be set with in game logic but I'll
        // cover that again when I go over the camera.
        m_target = EntityStats.Instance.GetObjectOfEntity("Boss");
        //m_swordHitBox = GameObject.Find("Sword").GetComponent<Collider>();
        //m_attackHitBox = GameObject.Find("AttackHitBox").GetComponent<Collider>();
    }


    // Movement occurs in this update.
    private void FixedUpdate()
    {

        // May need to move camera to occur everyFrame instead
        // to remove jittering from physics
        float axisZ = m_animator.GetFloat("Input/Z");
        float axisX = m_animator.GetFloat("Input/X");
        //if (m_lockon)
        //{
        //    LockOnLook(axisZ, axisX);
        //}
        //else if (m_animator.GetBool("Output/CanMove"))
        //{
        //    FreeLook(axisZ, axisX);
        //}

        // Can only occur when currently rolling

        if (m_animator.GetBool("Output/IsRolling"))
        {
            Rolling();
        }
        // Least important. Rotation occurs here for movement.
        else if (m_animator.GetBool("Output/CanMove"))
        {
            Movement(m_currentFrame.m_run);
        }


    }



    // Camera and input detection occurs here.
    // Every frame
    void Update()
    {
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX);

        float axisY = XCI.GetAxis(XboxAxis.LeftStickY);



        if (XCI.GetButtonDown(XboxButton.RightStick))
        {
            if (this.m_lockon)
            {
                this.m_lockon = false;
            }
            else
            {
                this.m_lockon = true;
            }
        }


        m_currentFrame = m_inputBuffer.GetBufferInput();


        if (!m_animator.GetBool("Input/Stop"))
        {
            UpdateAnimations(axisX, axisY, m_currentFrame);
        }

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

        // Input Buffer happens here
        // instead of the below.


        // 12*
        // Need to fix the angle

        if (m_animator.GetBool("Input/Roll") && !m_animator.GetBool("Output/IsRolling"))
        {
            StartRoll(axisX, axisY);
        }




        if (m_lockon)
        {
            LockOnLook(axisY, axisX);
        }
        else if (m_animator.GetBool("Output/CanMove"))
        {
            FreeLook(axisY, axisX);
        }

    }

    // Send inputs to animator.
    // Has a few rules to check if player has stamina but that may need
    // to be moved to the input buffer on request.
    void UpdateAnimations(float a_x, float a_y, BufferInput a_input)
    {
        // I need a way to consume data without actually going in


        // x axis
        m_animator.SetFloat("Input/X", a_x);
        // y axis
        m_animator.SetFloat("Input/Z", a_y);
        // If attack pressed and enough stamina.
        if (a_input.m_attack && EntityStats.Instance.CanEntityMoveOccur("Player", this.m_attackStaminaDrain))
        {
            m_animator.SetBool("Input/Attack", true);
        }
        else
        {
            m_animator.SetBool("Input/Attack", false);
        }
        if (a_input.m_dash && EntityStats.Instance.CanEntityMoveOccur("Player", this.m_rollStaminaDrain) && !m_animator.GetBool("Output/IsRolling"))
        {
            m_animator.SetBool("Input/Roll", true);
        }
        else
        {
            m_animator.SetBool("Input/Roll", false);
        }
    }


    // Walking/Running with rotation towards movement direciton.
    private void Movement(bool a_running)
    {
        float a_axisX = m_animator.GetFloat("Input/X");
        float a_axisZ = m_animator.GetFloat("Input/Z");
        
        
        // This if check may be redundant.
        // Only way into this state is for input is received.
        if (a_axisX != 0 || a_axisZ != 0)
        {
            // Direction for the player to move towards based on camera.
            Vector3 camerax = (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * a_axisX);
            Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * a_axisZ);
            Vector3 cameraPosition = (cameraz + camerax);

            // Directon based on camera position
            Vector3 movement = new Vector3(cameraPosition.x, 0, cameraPosition.z);


            // Adjust movement direction by angle below 
            // Eg angle is 45 therfore forward is 45 up.
            // cast in the direciton moved and cast straight down.
            // Cast straight down
            // cast down in direciton heading.
            // If conatct position in direciton heading higher pos movement
            // If conatct position in direciton heading lower neg movement
            //RaycastHit hit1;
            //if (Physics.Raycast(transform.position, -transform.up, out hit1, 2)) // Cast down
            //{
            //    //Debug.Log(hit1.point);
            //    RaycastHit hit2;
            //    if (Physics.Raycast(transform.position + (cameraPosition.normalized/2), -transform.up, out hit2, 2)) // cast in direction
            //    {

            //    }

            //}

            // Desired speed calc
            // Are you running or walking firstly?
            float desiredSpeed;
            if(a_running && !this.m_inputBuffer.m_staminaDrained)
            {
                if (EntityStats.Instance.GetStaminaOfEntity("Player") > 0)
                {
                    EntityStats.Instance.DeminishStaminaOffEntity("Player", this.m_runStaminaDrain);
                    desiredSpeed = this.m_runSpeed;
                }
                else
                {
                    this.m_inputBuffer.m_staminaDrained = true;
                    this.m_inputBuffer.ConsumeInput();
                    desiredSpeed = this.m_walkSpeed;
                    // Walk speed needs to pass 
                }
            }
            else
            {
                desiredSpeed = this.m_walkSpeed;
            }

            // Desired speed modified by angle below as well as heading.

            float dotOfFwdhding = Vector3.Dot(this.transform.forward.normalized, movement.normalized);
            if(dotOfFwdhding < -0.2f && !m_lockon && !a_running)
            {
                desiredSpeed = 0;
                // stored speed value reduced to 0
                m_animator.SetFloat("MovementSpeedMult", 0);
            }
            
            // Get the current speed moving at
            float currentSpeed = m_animator.GetFloat("MovementSpeedMult");
            // Move it towards the target speed. Needs to be aware of input.    
            Vector3 axis = new Vector3(a_axisX, 0, a_axisZ);
            float input = Vector3.Magnitude(axis);
            input = Mathf.Clamp(input, -1, 1);

            // Placeholder. probs a value from 0.5 to 2
            // Need a normalized value
            float normalizer = 1 / desiredSpeed;

            float currentNorm = currentSpeed * normalizer;
            // If coming from a stop more acceleration based on
            // how close you are to target.
            // Needs to be modified further by the angle
            // player is moving towards
            float acceleration;
            if(currentNorm < 0.5f)
            {
                acceleration = 0.2f;
            }
            else if(currentNorm < 0.8f)
            {
                acceleration = 0.1f;
            }
            else
            {
                acceleration = 0.05f;
            }

            //Debug.Log("Current: " + currentNorm);
            // The speed needs to b
            // Stored so it can be used in the roll                                                     0.1f is base
            m_animator.SetFloat("MovementSpeedMult", Mathf.Lerp(currentSpeed, desiredSpeed * input, acceleration));


            // Offset should be set in insepctor range of 0.5f to 2f.
            Vector3 rayOffset = new Vector3(0, 1, 0);
            // Fwd ray
            Ray FwdRay = new Ray(this.transform.position + this.transform.forward + rayOffset, -this.transform.up);
            RaycastHit fwdHit = new RaycastHit();
            // Bck ray
            Ray BckRay = new Ray(this.transform.position + -this.transform.forward + rayOffset, -this.transform.up);
            RaycastHit bckHit = new RaycastHit();
            float maxDistance = 2;

            Debug.DrawRay(this.transform.position + this.transform.forward + rayOffset, -this.transform.up);
            Debug.DrawRay(this.transform.position + -this.transform.forward + rayOffset, -this.transform.up);

            // get a direction of the 2 points and use that to work out an angle of movement.
            // at the same time measure a point between the 2 and lerp the player towards
            // that point on the y axis.
            if (Physics.Raycast(FwdRay, out fwdHit, maxDistance) && Physics.Raycast(FwdRay, out bckHit, maxDistance))
            {
                // Positions of hits
                Vector3 fwd = fwdHit.point;
                Vector3 bck = bckHit.point;


                //float angle = Vector3.Angle(info.normal, transform.up);
                //Debug.Log(angle);
                //movement = Quaternion.Euler(angle, 0f, 0f) * movement;
            }
            // Raycast down and get the normal of the position underneath and
            // use that for rotation adjustment / body placement if
            // no contact with either above. Should be okay unless floating in air.
            else
            {
                Ray Raycast = new Ray(this.transform.position, -this.transform.up);
                RaycastHit info;
                if (Physics.Raycast(Raycast, out info))
                {
                    float angle = Vector3.Angle(info.normal, transform.up);
                    //Debug.Log(angle);
                    movement = Quaternion.Euler(angle, 0f, 0f) * movement;

                }
            }

            //Vector3 lifterPos = feet.m_leftFoot.transform.position - feet.m_rightFoot.transform.position;
            //lifterPos = feet.m_rightFoot.transform.position + lifterPos * 0.5f;

            //feet.m_lifter.transform.position = lifterPos;



            // Modifiying movement based on normal or below terrain







            // All right but should be doing a raycast forward and back.
            // if it doesn't hit anything after x it goes down.
            // if no contact slide until normal is 0.
            // 

            //Ray front = new Ray(this.transform.position, );
            //Debug.DrawRay(this.transform.position, );

            // Lifter should be between 2 raycasts instead or feet.
            //GameObject lifter = GetComponent<IkFootVariables>().m_lifter;
            //Vector3 pos = new Vector3(lifter.transform.position.x, lifter.transform.position.y + 0.4f, lifter.transform.position.z);
            //Vector3 lerp = Vector3.Lerp(this.transform.position, pos, 0.5f);




            movement = movement * m_animator.GetFloat("MovementSpeedMult") * Time.deltaTime;
            //Debug.Log(m_movement);                                      // + lerp from lifter.
            this.GetComponent<Rigidbody>().MovePosition(this.transform.position + movement);
            //player.transform.position = player.transform.position + m_movement;
            // What to pass to the animator.
            // Used for blend trees.
            // This should always be passed in.
            Vector3 toAnim = this.transform.worldToLocalMatrix * movement;
                toAnim = toAnim.normalized;
            if (!a_running)
            {
                // reduce the input values to the animator if not running
                toAnim.x = Mathf.Clamp(toAnim.x, -0.5f, 0.5f);
                toAnim.z = Mathf.Clamp(toAnim.z, -0.5f, 0.5f);
            }
            // Lerp towards the target acceleration                 // This value to be modified by speed
            float animX = Mathf.Lerp(m_animator.GetFloat("Movement/X"), toAnim.x, 0.2f);
            float animZ = Mathf.Lerp(m_animator.GetFloat("Movement/Z"), toAnim.z, 0.2f);
            float scale;
            if (Mathf.Abs(a_axisX) + Mathf.Abs(a_axisZ) >= 1)
            {
                scale = 1;
            }
            else
            {
                scale = Mathf.Max(Mathf.Abs(a_axisX), Mathf.Abs(a_axisZ));
            }

                                                        
            m_animator.SetFloat("Movement/X", animX * scale);
            m_animator.SetFloat("Movement/Z", animZ * scale);
        }

        // No input case
        else
        {
            m_animator.SetFloat("Movement/X", 0);
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("MovementSpeedMult", 0);
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
        Vector3 m_movement = new Vector3((m_storedRollDirection.x * m_rollSpeed) * Time.deltaTime, 0, (m_storedRollDirection.z * m_rollSpeed) * Time.deltaTime * m_animator.speed);
        this.GetComponent<Rigidbody>().MovePosition(this.transform.position + m_movement);
    }

    // Character rotates towards target when locked on.
    // Occurs seperate from move.
    private void LockOnLook(float axisZ, float axisX)
    {
        if (!m_currentFrame.m_run)
        {
            Vector3 bossdirection = this.m_target.transform.position - this.transform.position;
            bossdirection = bossdirection.normalized;

            // To make sure player doesn't up or down. Only facing.
            // Take not that head will need the y.
            bossdirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(bossdirection);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.m_rotationTime);
            
        }
        else
        {
            Vector3 camerax = (new Vector3(Camera.main.transform.right.x, this.transform.up.x, Camera.main.transform.right.z) * axisX);
            Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, this.transform.up.x, Camera.main.transform.forward.z) * axisZ);
            Vector3 cameraPosition = (cameraz + camerax);

            Quaternion targetRotation = Quaternion.LookRotation(cameraPosition);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.m_rotationTime);

            m_animator.gameObject.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.m_rotationTime);
        }
    }    

    // Character rotates towards direction they're moving.
    private void FreeLook(float axisZ, float axisX)
    {
        if (axisZ != 0 || axisX != 0)
        {
            Vector3 camerax = (new Vector3(Camera.main.transform.right.x, this.transform.up.x, Camera.main.transform.right.z) * axisX);
            Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, this.transform.up.x, Camera.main.transform.forward.z) * axisZ);
            Vector3 cameraPosition = (cameraz + camerax);

            Quaternion targetRotation = Quaternion.LookRotation(cameraPosition);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.m_rotationTime);
            // Need to adjust the rotation.
            //m_animator.gameObject.transform.rotation;
        }
    }


    // Getters
    public Collider GetAttackHitBox()
    {
        return m_attackHitBox;
    }

    public Collider GetSwordHitBox()
    {
        return m_swordHitBox;
    }

    public GameObject GetLookPoint()
    {
        return m_lookPoint;
    }

}