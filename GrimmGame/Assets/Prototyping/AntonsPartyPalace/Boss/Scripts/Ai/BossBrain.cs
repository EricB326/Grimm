using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// A quick script to house some basic
// variables and decisions the ai can make.
public class BossBrain : MonoBehaviour
{
    // Components to get
    private Animator m_animator;


    [Range(0, 1)]
    public float m_rotationSpeed;
    [Header("DEBUG. VALUES PASSED IN BY ANIMATOR")]
    // Used to check if can.
    public bool m_diagnosticMode = false;
    // The range to reach before attack begins.
    // Passed out of the animator and here for debugging.
    // This is set in the boss Phase.
    public float m_desiredRange = 0;
    // What the current phase of the boss is.
    // Either this lives in the animator or out here.
    //public int m_phase;
    // The current attack.
    //public int m_attack;
    // Stamina or revenge value.
    // Depending on how we want to the game to go this 
    // will either be a value that once it reaches
    // a threshold will launch a quick coutner attack
    // or a value that the boss uses to work out what
    // attack he is able to use next.
    //public float m_revengeValue;
    // The actual target the boss is seeking towards.
    // Most likely the player at all times.
    public GameObject m_target;
    // When was the last decision made.

    public List<BossPhase> m_bossPhaseList = new List<BossPhase>();
    // List of actions
    // Will one day become a enums.
    public List<BossActions> m_actionQue = new List<BossActions>();

    // When a state requires 2 steps.
    private bool m_continue = false;
    // If behaviors require a timer.
    private float m_timeOut = 0;
    
    // When ai makes is attacking(either by command or random)
    // It places the attack in here so it has all the info it needs
    // at that time.
    // private BossSeek m_currentSeek = null;
    public int m_currentPhase;

    public int m_revengeValue = 0;

    private void Start()
    {
        m_currentPhase = 0;
        m_animator = this.GetComponent<Animator>();
        m_target = EntityStats.Instance.GetObjectOfEntity("Player");
        m_timeOut = 0;
    }

    private void Update()
    {
        // This is a bad name - Really it is both distance and direction.
        Vector3 directionToMove = m_target.transform.position - transform.position;

        // Wait(x sec) wander(x sec) randomly attack or do preset action list
        if (m_actionQue.Count == 0)
        {
            // If action list is empty and the predefined list 
            // has occured too recently we do a random attack from
            // the phase.
            if (m_bossPhaseList[m_currentPhase].m_storedTime > Time.time)
            {
                m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateAtack(GetDistanceToPlayer(directionToMove)));
            }
            else
            {
                foreach (int action in m_bossPhaseList[m_currentPhase].m_preDefinedActions)
                {
                    BossActions t = m_bossPhaseList[m_currentPhase].m_bossActions[action];
                    // Check if the behavior added to the list will be an attack.
                    if (t.GetBehaviourType == SteeringBehaviours.ATTACK_BEHAVIOUR)
                    {
                        m_desiredRange = t.AttackRange;
                    }
                    m_actionQue.Add(t);
                }
                m_bossPhaseList[m_currentPhase].m_storedTime = Time.time + m_bossPhaseList[m_currentPhase].m_timeBetweenPreDefinedAndRandom;
            }
            // Calls the eveluate attack function or
            // the predefined funciton list.
            // Sets the desired range here.
        }
        if (!m_diagnosticMode)
        {
            if (!m_animator.GetBool("Ai/IsAttacking"))
            {
                RotateBoss(directionToMove);
            }
            if (m_revengeValue < m_bossPhaseList[m_currentPhase].m_threshold)
            {
                DoThing(directionToMove);
            }
            else
            {
                m_revengeValue = -m_bossPhaseList[m_currentPhase].m_threshold;
                CounterAttack(directionToMove);
            }
        }
    }

    private void DoThing(Vector3 a_directionToMove)
    {
        if(m_actionQue.Count != 0)
        switch (m_actionQue[0].GetBehaviourType)
        {
            case SteeringBehaviours.WANDER_BEHAVIOUR: // Wander
                if (Wander())
                {
                    Debug.Log(m_actionQue[0].GetBehaviourType);
                    m_actionQue.RemoveAt(0);
                }
                break;
            case SteeringBehaviours.DODGE_BEHAVIOUR: // Dodge
                if (Dodge())
                {
                    Debug.Log(m_actionQue[0].GetBehaviourType);
                    m_actionQue.RemoveAt(0);
                }
                break;
            case SteeringBehaviours.ATTACK_BEHAVIOUR: // Attack
                if (Attack(a_directionToMove))
                {
                    Debug.Log(m_actionQue[0].GetBehaviourType);
                    m_actionQue.RemoveAt(0);
                }
                break;
            case SteeringBehaviours.SEEK_BEHAVIOUR: // Seek
                if (Seek(a_directionToMove))
                {
                    Debug.Log(m_actionQue[0].GetBehaviourType);
                    m_actionQue.RemoveAt(0);
                }
                break;
            default:
                Debug.Log("How did we get here? Current action is: " + m_actionQue[0].GetBehaviourType);
                break;
        }
    }

    // Rotates the boss by the speed stored in the phase.
    // Can probably be moved into a state overload.
    private void RotateBoss(Vector3 a_direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(a_direction);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, m_rotationSpeed);
    }

    // Returns true if within range
    private bool CalculateDistance(Vector3 a_direction)
    {
        //Vector3 position = this.transform.position;
        //Vector3 targetPosition = m_target.transform.position;

        //Vector3 difference = position - targetPosition;

        // This isn't correct?... or at best kinda dodgy/can be done a different way.
        float largestDistance = Mathf.Max(Mathf.Abs(a_direction.x), Mathf.Abs(a_direction.z));

        if (largestDistance <= m_desiredRange)
        {
            return true;
        }
        else
            return false;

    }
    private float GetDistanceToPlayer(Vector3 a_direction)
    { 
        float largestDistance = Mathf.Max(Mathf.Abs(a_direction.x), Mathf.Abs(a_direction.z));
        return largestDistance;
    }



    // Returns true if facing is acceptable
    // Called by the attack
    private bool CorrectFacing()
    {
        // Probs a dot product.
        // If within certain angle start attack.

        
        if(true)
        {
            return true;
        }
        else
            return false;
    }

    // Will dodge away from player attacks.
    // Used for tricky attack strings or
    // couterattaking.
    public bool Dodge()
    {
        // Needs to move away from player.
        // Pick a direction then constantly move until animation is done


        // Acquire dodge direction
        if (!m_continue)
        {
            Vector3 dodgeAway = this.transform.position - m_target.transform.position;
            if (dodgeAway.x <= 0)
            {
                dodgeAway.x = -1;
            }
            else
            {
                dodgeAway.x = 1;
            }
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("Movement/X", dodgeAway.x);
            m_animator.SetBool("Ai/IsDashing", true);
            m_continue = true;
            return false;
        }
        // Check if still dodging
        if(!m_animator.GetBool("Ai/IsDashing")) 
        {
            return true;
        }
        return false;
    }

    // More like circle but shits okay
    public bool Wander()
    {
        // If in wander state without a direction
        // Get a direction.
        if (!m_continue)
        {
           
            if (Random.Range(0, 2) == 0)
            {
                m_animator.SetFloat("Movement/X", 1);
            }
            else
            {
                m_animator.SetFloat("Movement/X", -1);
            }
            // Avoid getting another.
            m_continue = true;
            // Get a time to exit the wander state.
            m_timeOut = Time.time + m_bossPhaseList[m_currentPhase].m_wanderTime;
        }
        else
        {
            // Check if time to exit wander state. 
            if (m_timeOut < Time.time)
            {
                // Remove old information.
                ResetState();
                // Will be removed from list.
                return true;
            }
        }
        // Will not be removed from list.
        return false;
    }

    // Try to launch attack.
    // Will be the action in list and will try for x amount
    // of time (2 secs)

    private bool Attack(Vector3 a_directionToMove)
    {
        if (CalculateDistance(a_directionToMove))
        {
            if (CorrectFacing() && !m_continue)
            {
                //m_animator.SetBool("Ai/IsAttacking", true);
                m_animator.SetInteger("Ai/Attack", 1/*m_currentaction.attackanimation*/);
                m_continue = true;
                return false;
            }
            else
            {
                // If attack variants would need to check what ones to go down somewhere

                if (m_animator.GetBool("Ai/IsAttacking"))
                {
                    return false;
                }
                else
                {
                    ResetState();
                    return true;
                }
            }
        }
        else
        {
            // Evaluate best way to get to target.
            m_actionQue.Insert(0 ,m_bossPhaseList[m_currentPhase].EvaluateSeek(GetDistanceToPlayer(a_directionToMove), m_desiredRange));
            return false;
        }
    }

    // Move towards targets.
    // Needs to play an animation if special
    // ability.
    private bool Seek(Vector3 a_directionToMove)
    {
        //a_directionToMove = transform.worldToLocalMatrix * a_directionToMove.normalized;
        if (!m_continue) // Also needs to see if the target is within special seek behavior range(not too far or close)
        {
            // Do special seek behavior
            // Shoould be an animation and a direction right?
            // The animation should contain what it needs so if
            // it needs x and z they are passed in below.
            m_continue = true;
        }
        if(CalculateDistance(a_directionToMove))
        {
            ResetState();
            return true;
        }
        else
        {
            a_directionToMove = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
            m_animator.SetFloat("Movement/Z", a_directionToMove.z);
            m_animator.SetFloat("Movement/X", a_directionToMove.x);
            m_animator.SetInteger("Ai/Attack", 0);
            m_animator.SetBool("Ai/IsPursuing", true);
            m_animator.SetFloat("ForwardMultiplyer", 2);



            //a_directionToMove = transform.worldToLocalMatrix * a_directionToMove.normalized;
            //Debug.Log(a_directionToMove);

            //m_animator.SetFloat("Movement/X", a_directionToMove.x);
            //m_animator.SetFloat("Movement/Y", a_directionToMove.y);
            return false;
        }
    }

    // Dodge away then attack.
    private void CounterAttack(Vector3 a_directionToMove)
    {
        m_actionQue.Clear();
        m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateDodge(a_directionToMove.normalized));
        m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateAtack(GetDistanceToPlayer(a_directionToMove)));
        //m_actionQue.Add(AI_BEHAVIOR.ATTACK); // Need to get from phase
        //m_actionQue.Add(AI_BEHAVIOR.DODGE); // Need to get from phase
        // Empty list out and add dodge and
        // attack to list.
    }

    // A helper function to simplify resetting
    // the behaviors. Any time the list is
    // emptied this will be called.
    private void ResetState()
    {

        // Current attack will be removed.
        m_continue = false;
        m_timeOut = 0;
    }

    //************************************************************
    //Revenge value 
    //************************************************************
    //************************************************************
    //Used to up challenging if player doing well
    //or force the player into combat if they're
    //avoiding boss.
    //************************************************************

    public void IncreaseRevengeValue()
    {
        m_revengeValue += m_bossPhaseList[m_currentPhase].m_increase;
    }

    // If boss lands a hit on player should decrease value
    public void DecreaseRevengeValue()
    {
        m_revengeValue -= m_bossPhaseList[m_currentPhase].m_increase;
        if(m_revengeValue < 0)
        {
            m_revengeValue = 0;
        }
    }

    public bool DiagnosticMode
    {
        get { return m_diagnosticMode; }
        set { m_diagnosticMode = value; }
    }
}
