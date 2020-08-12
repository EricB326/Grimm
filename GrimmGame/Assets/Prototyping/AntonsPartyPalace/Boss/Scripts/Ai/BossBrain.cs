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
    // The range to reach before attack begins.
    // Passed out of the animator and here for debugging.
    public float m_desiredRange;
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

    // Selected attack from phase list.
    public BossAttackVariables m_currentAttackVariables;

    public List<BossPhase> m_bossPhaseList;

    public List<int> m_actionQue;

    // When a state requires 2 steps.
    private bool m_continue = false;
    // If behaviors require a timer.
    private float m_timeOut = 0;
    // Should be empty unless attack in 
    private BossAttacks m_currentAttack;

    // 0 wander, 1 seek, 2 evade, 3 attack.
    public int m_currentAction;
    
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
        // If action in list do action

        // else decide on next action
        // Wait(x sec) wander(x sec) randomly attack or do preset action list
        if(m_actionQue.Count == 0)
        {
            // Either get list of pre determined combinations.
            // Or randomly choose attack
        }
        
        // Ideally the bosses movements will then give movements direction
        Vector3 directionToMove = m_target.transform.position - transform.position;


        if (!m_animator.GetBool("Ai/IsAttacking"))
        {
            RotateBoss(directionToMove);
        }

        if (m_revengeValue < m_bossPhaseList[m_currentPhase].m_threshold)
        {
            // Rotates towards target if capabale of it.
            //if (m_lastDecision < Time.time && !m_animator.GetBool("Ai/IsDashing") && !m_animator.GetBool("Ai/IsPursuing"))
            //{
            //    m_revengeValue +=  m_bossPhaseList[m_currentPhase].m_increase;
            //    m_currentAction = Random.Range(0, 3);
            //}
            //// Regular ai behaviors
            DoThing(directionToMove);
        }
        else
        {
            m_revengeValue =- m_bossPhaseList[m_currentPhase].m_threshold;
            CounterAttack();
        }
    }

    private void DoThing(Vector3 a_directionToMove)
    {
        switch (m_currentAction)
        {
            case 0: // Wander
                if (Wander())
                    m_actionQue.Remove(0);
                break;
            case 1: // Dodge
                if (Dodge())
                    m_actionQue.Remove(0);
                break;
            case 2: // Attack
                if (Attack())
                    m_actionQue.Remove(0);
                break;
            case 3: // Seek
                if (Seek())
                    m_actionQue.Remove(0);
                break;
            default:
                Debug.Log("How did we get here? Current action is: " + m_currentAction);
                break;
        }
    // Seek towards target if range not met
    // m_animator.SetFloat("Movement/Z", a_directionToMove.z);
    // m_animator.SetFloat("Movement/X", a_directionToMove.x);
    //a_directionToMove = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
    //m_animator.SetFloat("Movement/Z", 0);
    //m_animator.SetFloat("Movement/X", 0);
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

    // Returns true if facing is acceptable
    // Called by the attack
    private bool CorrectFacing()
    {
        // Probs a dot product.
        // If within certain angle start attack.

        // Not 
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
        ////Vector3 thing = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
        //if (a_directionToMove.x <= 0)
        //{
        //    a_directionToMove.x = -1;
        //}
        //else
        //{
        //    a_directionToMove.x = 1;
        //}
        //m_animator.SetFloat("Movement/Z", 0);
        //m_animator.SetFloat("Movement/X", a_directionToMove.x);

        // Dodge away from player.



        m_animator.SetBool("Ai/IsDashing", true);
        return true;
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
                m_animator.SetInteger("Movement/X", 1);
            }
            else
            {
                m_animator.SetInteger("Movement/X", -1);
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

    private bool Attack()
    {
        if (CorrectFacing() && !m_continue)
        {
            m_animator.SetInteger("Ai/Attack", 0/*m_currentAttack*/);
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

    // Move towards targets.
    private bool Seek()
    {
        // if()
        // return true if within range
        // else return false.
        return true;
    }

    // Dodge away then attack.
    private void CounterAttack()
    {
        // Remove everything from the list
        // should get specific behaviors located in counter
        Dodge();
        Attack();
    }

    // A helper function to simplify resetting
    // the behaviors. Any time the list is
    // emptied this will be called.
    private void ResetState()
    {
        m_currentAttack = null;
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
}

