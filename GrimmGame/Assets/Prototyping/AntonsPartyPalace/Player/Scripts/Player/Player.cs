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
    [HideInInspector]
    public float m_walkSpeed = 5;
    // Speed will increase to this value.
    [HideInInspector]
    public float m_runSpeed = 7;
    // Roll speed. Ideally should accelerate to this speed rather rapidly.
    [HideInInspector]
    public float m_rollSpeed = 12;
    [Range(0, 1)]
    public float m_rotationTime;

    private GameObject m_lifter;


    public bool m_disableControls  = false;

    // Could be removed 
    //public float m_rollAttackStaminaDrain = 20;
    // Roll Stamina drain
    public float m_rollStaminaDrain = 25;

    public float m_runStaminaDrain = 2;
    // Roll damage multiplier value.
    [Range(0, 1)]
    public float m_rollDamagemultiplier = 0.80f;
    // Damage per attack. Should be consistent.
    [SerializeField]
    [Tooltip("0 Light attack" +
        " 1 Roll attack" +
        " 2 Heavy attack" +
        " 3 Sprint attack")]
    public List<float> m_attacksDamage;

    [SerializeField]
    [Tooltip("0 Light attack" +
        " 1 Roll attack" +
        " 2 Heavy attack" +
        " 3 Sprint attack")]
    public List<float> m_attackStamina;


    //public float m_lightAttackDamage = 10;
    //public float m_heavyAttackDamage = 15;

    // Passed in to drain stamina and do damage.
    // 0 Light
    // 1 Heavy
    // 2 Roll
    public int m_attackStats = 0;

    // Only used to decide if the player can be hit. Basicaly god mode.
    // Unable to be hit if IFrames true;
    // Modified by an animation event.
    public bool m_InvinceFrames = false;

    public bool m_lockon = false;

    public BasicBuffer m_inputBuffer;

    public GameObject m_respawnPos;
    [HideInInspector]
    public Vector3 m_startPos;
    //[HideInInspector]
    public bool m_Dead = false; // A workaround. Player was constantly triggering the death transition event.

    private void Start()
    {       
        // This is better to be set with in game logic but I'll
        // cover that again when I go over the camera.
        if(m_target == null)
        {
            m_target = EntityStats.Instance.GetObjectOfEntity("Boss");
        }
        //m_swordHitBox = GameObject.Find("Sword").GetComponent<Collider>();
        //m_attackHitBox = GameObject.Find("AttackHitBox").GetComponent<Collider>();
        m_startPos = this.transform.position;
    }


    // Movement occurs in this update.
    private void FixedUpdate()
    {
        if (!m_disableControls && !PauseMenuController.isPaused)
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

            // This is super dumb
            if (m_lockon && axisZ != 0 || m_lockon && axisX != 0)
            {
                LockOnLook(axisZ, axisX);
            }
            else if (m_animator.GetBool("Output/CanMove"))
            {
                FreeLook(axisZ, axisX);
            }
            else if(m_animator.GetBool("Output/CanRotate"))
            {
                AttackRotation();
            }
        }
    }



    // Camera and input detection occurs here.
    // Every frame
    void Update()
    {
        //Debug.Log(EntityStats.Instance.GetHealthOfEntity("Player"));
        //if (EntityStats.Instance.GetHealthOfEntity("Player") <= 0)
        //{
        //    EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_animator.SetInteger("AnyState/Death", 1);
        //}

        if (!PauseMenuController.isPaused || m_disableControls)
        {
            float axisX = XCI.GetAxis(XboxAxis.LeftStickX);
            float axisZ = XCI.GetAxis(XboxAxis.LeftStickY);

            // Camera lockon
            if (XCI.GetButtonDown(XboxButton.RightStick))
            {
                GameObject Boss = EntityStats.Instance.GetObjectOfEntity("Boss");
                if (this.m_lockon || Boss.GetComponent<BossVariables>().m_dead)
                {
                    this.m_lockon = false;

                }
                else
                {
                    // Target aquizition needs to occur here.
                   
                    Vector3 direction = m_target.transform.position - this.transform.position;

                    //Ray toBoss = new Ray(this.transform.position + (this.transform.up / 2), this.transform.position + direciton);
                    float lockOnRange = 20;
                    
                    float largestDistance = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.z));
                    if (lockOnRange > largestDistance && !Boss.GetComponent<BossVariables>().m_dead)
                    {
                        //Debug.Log(largestDistance);
                        this.m_lockon = true;
                    }
                    else
                    {
                        Camera.main.GetComponent<CameraRotation>().Recenter(this.transform.forward);
                    }
                    // Raycast to target
                }
            }

            m_currentFrame = m_inputBuffer.GetBufferInput();


            if (!m_animator.GetBool("Input/Stop") || m_Dead == false)
            {
                UpdateAnimations(axisX, axisZ, m_currentFrame);
            }


            if (m_animator.GetBool("Input/Roll") && !m_animator.GetBool("Output/IsRolling"))
            {
                StartRoll(axisX, axisZ);
            }

        }
        //else if(/*Cutscene*/)
        //{

        //    // do fade and play complete cutscene.
        //}
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
        if (a_input.m_lightAttack && EntityStats.Instance.CanEntityMoveOccur("Player", this.m_attackStamina[0]))
        {
            m_animator.SetBool("Input/AttackLight", true);
        }
        else
        {
            m_animator.SetBool("Input/AttackLight", false);
        }
        if (a_input.m_heavyAttack && EntityStats.Instance.CanEntityMoveOccur("Player", this.m_attackStamina[1]))
        {
            m_animator.SetBool("Input/AttackHeavy", true);
        }
        else
        {
            m_animator.SetBool("Input/AttackHeavy", false);
        }


        if (a_input.m_dash && EntityStats.Instance.CanEntityMoveOccur("Player", this.m_rollStaminaDrain) && !m_animator.GetBool("Output/IsRolling"))
        {
            m_animator.SetBool("Input/Roll", true);
        }
        else
        {
            m_animator.SetBool("Input/Roll", false);
        }
        m_animator.SetBool("Input/Running", a_input.m_run);
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
            // If facing backwards rotate then move.
            if (dotOfFwdhding < -0.2f && !m_lockon && !a_running)
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
                acceleration = 1.5f;
            }
            else
            {
                acceleration = 0.1f;
            }

            //Debug.Log("Current: " + currentNorm);
            // Stored so it can be used in the roll                                                     0.1f is base
            m_animator.SetFloat("MovementSpeedMult", Mathf.Lerp(currentSpeed, desiredSpeed * input, acceleration));



            // RAYCAST FORWARD AND BACK TO ADJUST THE PLAYERS Y POSITION

            //// Offset should be set in insepctor range of 0.5f to 2f.
            //Vector3 rayOffset = new Vector3(0, 0.7f, 0);
            //// Fwd ray
            //Ray FwdRay = new Ray(this.transform.position + (this.transform.forward * 1f) + rayOffset, -this.transform.up);
            //RaycastHit fwdHit = new RaycastHit();
            //// Bck ray
            //Ray BckRay = new Ray(this.transform.position + -(this.transform.forward * 1f) + rayOffset, -this.transform.up);
            //RaycastHit bckHit = new RaycastHit();
            //float maxDistance = 2;

            //Debug.DrawRay(this.transform.position + this.transform.forward + rayOffset, -this.transform.up);
            //Debug.DrawRay(this.transform.position + -this.transform.forward + rayOffset, -this.transform.up);


            



            // get a direction of the 2 points and use that to work out an angle of movement.
            // at the same time measure a point between the 2 and lerp the player towards
            // that point on the y axis.
            //Vector3 offset;
            //if (Physics.Raycast(FwdRay, out fwdHit, maxDistance) && Physics.Raycast(BckRay, out bckHit, maxDistance))
            //{
            //    Debug.Log("Hit");

            //    // Positions of hits
            //    Vector3 fwd = fwdHit.point;
            //    Vector3 bck = bckHit.point;


            //    // Movement due to lifter
            //    // Need to move upwards if greater than a threshold upward push
            //    Vector3 direction = this.transform.position - m_lifter.transform.position;
            //    Vector3 pos = this.transform.position;
            //    if (direction.y < m_restingOffset)
            //    {
            //        pos.y = m_restingOffset + m_lifter.transform.position.y;
            //    }

            //    // Move player from lifter info.
            //    m_lifter.transform.position = bck + direction * 0.5f;
            //    // Middle of the raycasts
            //    m_lifter.transform.position = (fwdHit.point + bckHit.point) / 2;
            //    this.transform.position = Vector3.Lerp(this.transform.position, pos, m_lifterSpeed);

            //    //Debug.Log(direction);
            //    float angle = Vector3.Angle(direction.normalized, transform.forward);
            //    movement = Quaternion.Euler(angle, 0f, 0f) * movement;
            //    Debug.DrawRay(this.transform.position, movement);
            //}

            // Rotation based on cast down normal.
            //{
            //    // Works as intended.
            //    Ray Raycast = new Ray(this.transform.position, -this.transform.up);
            //    RaycastHit info;
            //    if (Physics.Raycast(Raycast, out info))
            //    {
            //        float angle = Vector3.Angle(info.normal, transform.up);
            //        //Debug.Log(angle);
            //        movement = Quaternion.Euler(0f, 0f, angle) * movement;
            //    }
            //}


            movement = movement * m_animator.GetFloat("MovementSpeedMult") * Time.deltaTime;
            //Debug.Log(m_movement);                                      // + lerp from lifter.
            // Without root motion
            //this.GetComponent<Rigidbody>().MovePosition(this.transform.position + movement/* + offset*/);
            // What to pass to the animator.
            // Used for blend trees.
            // This should always be passed in.
            Vector3 toAnim = this.transform.worldToLocalMatrix * movement;
                toAnim = toAnim.normalized;
            if (!a_running && m_lockon)
            {
                // reduce the input values to the animator if not running
                toAnim.x = Mathf.Clamp(toAnim.x, -0.5f, 0.5f);
                toAnim.z = Mathf.Clamp(toAnim.z, -0.5f, 0.5f);
            } else if(!a_running)
            {
                toAnim.x = Mathf.Clamp(toAnim.x, -0.8f, 0.8f);
                toAnim.z = Mathf.Clamp(toAnim.z, -0.8f, 0.8f);
            }

            // If running values are 1.

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

            // To animator                       
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
        //Vector3 toAnim;

        // Need to check if any input at all.
        // If not the direction to move is always back
        if (a_axisX == 0 && a_axisZ == 0)
        {
            // Backstep
            m_animator.SetFloat("Input/RollOrStep", -1);
            a_axisZ = -1;

            Vector3 movementX = (new Vector3(this.transform.right.x, 0, this.transform.right.z) * a_axisX);
            Vector3 movementZ = (new Vector3(this.transform.forward.x, 0, this.transform.forward.z) * a_axisZ);


            Vector3 cameraPosition = (movementZ + movementX);
            m_storedRollDirection = cameraPosition;
        }
        else
        {
            // Roll
            m_animator.SetFloat("Input/RollOrStep", 1);
            Vector3 movementX = (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * a_axisX);
            Vector3 movementZ = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * a_axisZ);
            Vector3 cameraPosition = (movementZ + movementX);
            //Relative to the direction player is facing.
            // Stored for rotation.
            m_storedRollDirection = cameraPosition;
        }
        m_storedRollDirection.y = 0;

        //m_animator.SetFloat("Movement/X", 0);
        //m_animator.SetFloat("Movement/Z", 0);
        //m_animator.SetFloat("MovementSpeedMult", 0);

    }

    // While player is rolling this funciton is called.
    private void Rolling()
    {
        // Removed due to root motion
        //Vector3 m_movement = new Vector3((m_storedRollDirection.x * m_rollSpeed) * Time.deltaTime, 0, (m_storedRollDirection.z * m_rollSpeed) * Time.deltaTime * m_animator.speed);
        //this.GetComponent<Rigidbody>().MovePosition(this.transform.position + m_movement);
        // Check if rolling or not.
        if (m_animator.GetFloat("Input/RollOrStep") > 0)
        {
            // Rotate in direction rolling in.
            Quaternion targetRotation = Quaternion.LookRotation(m_storedRollDirection);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, 0.7f);
        }

    }

    // Character rotates towards target when locked on.
    // Occurs seperate from move.
    private void LockOnLook(float axisZ, float axisX)
    {
        // If Player moving should look at boss(not camera)
        // if player rolling or running should be rotating 
        // in direction of the movement. Or should it be skipped entirely for roll?
        //if(axisZ != 0 || axisX != 0 && !m_currentFrame.m_run && !m_animator.GetBool("Output/IsRolling"))
        if (!m_currentFrame.m_run && !m_animator.GetBool("Output/IsRolling"))
        {
            Vector3 bossdirection = this.m_target.transform.position - this.transform.position;
            bossdirection = bossdirection.normalized;

            // To make sure player doesn't up or down. Only facing.
            // Take not that head will need the y.
            bossdirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(bossdirection);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.m_rotationTime);

        }
        else if(m_currentFrame.m_run)
        {
            Vector3 camerax = (new Vector3(Camera.main.transform.right.x, this.transform.up.x, Camera.main.transform.right.z) * axisX);
            Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, this.transform.up.x, Camera.main.transform.forward.z) * axisZ);
            Vector3 cameraPosition = (cameraz + camerax);

            Quaternion targetRotation = Quaternion.LookRotation(cameraPosition);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.m_rotationTime);

            //m_animator.gameObject.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, this.m_rotationTime);
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


    public void AttackRotation()
    {
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        if (!player.m_lockon)
        {
            // Get the input out of the animator
            //float z = player.m_animator.GetFloat("Input/Z");
            //float x = player.m_animator.GetFloat("Input/X");

            float x = XCI.GetAxis(XboxAxis.LeftStickX);
            float z = XCI.GetAxis(XboxAxis.LeftStickY);
            if (z != 0 || x != 0)
            {
                Vector3 camerax = (new Vector3(Camera.main.transform.right.x, this.transform.up.x, Camera.main.transform.right.z) * x);
                Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, this.transform.up.x, Camera.main.transform.forward.z) * z);
                Vector3 cameraPosition = (cameraz + camerax);

                cameraPosition.y = 0;


                //cameraPosition = this.transform.worldToLocalMatrix * cameraPosition;

                Quaternion targetRotation = Quaternion.LookRotation(cameraPosition.normalized);
                Debug.DrawRay(this.transform.position, cameraPosition.normalized, Color.red, 2f);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, 0.15f);
            }
        }
        else
        {
            Vector3 bossdirection = this.GetComponent<Player>().m_target.transform.position - this.transform.position;
            bossdirection = bossdirection.normalized;

            // To make sure player doesn't up or down. Only facing.
            // Take not that head will need the y.
            bossdirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(bossdirection);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, 0.15f);
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