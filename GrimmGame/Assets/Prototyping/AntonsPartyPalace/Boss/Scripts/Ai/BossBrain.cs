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
    private float m_lastDecision;
    // 0 wander, 1 seek, 2 evade, 3 attack.
    public int m_mode;
    
    private bool m_getNewBehavior = true;
    // Selected attack from phase list.
    public BossAttackVariables m_currentAttackVariables;

    public List<BossPhase> m_bossPhaseList;

    public BossPhase m_curreentPhase;

    public int m_revengeValue = 0;

    private void Start()
    {
        m_curreentPhase = m_bossPhaseList[0];
        m_animator = this.GetComponent<Animator>();
        m_target = EntityStats.Instance.GetObjectOfEntity("Player");
        m_lastDecision = 0;
    }


    private void Update()
    {
        // Get the decision.
        if (m_lastDecision < Time.time && !m_animator.GetBool("Ai/IsDashing") && !m_animator.GetBool("Ai/IsPursuing"))
        {
            m_revengeValue += m_curreentPhase.m_revengeValueIncrease;
            m_mode = Random.Range(0, 3);
            m_getNewBehavior = true;
        }




        Vector3 directionToMove = m_target.transform.position - transform.position;
        if (!m_animator.GetBool("Ai/IsAttacking"))
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToMove);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, m_rotationSpeed);
        }

        if (m_revengeValue < m_curreentPhase.m_revengeValueThreshold)
        {
            // Rotates towards target if capabale of it.
            if (m_lastDecision < Time.time && !m_animator.GetBool("Ai/IsDashing") && !m_animator.GetBool("Ai/IsPursuing"))
            {
                m_revengeValue += m_curreentPhase.m_revengeValueIncrease;
                m_mode = Random.Range(0, 3);
                m_getNewBehavior = true;
            }
            // Regular ai behaviors
            DoThing(directionToMove);
        }
        else
        {
            m_revengeValue -= m_curreentPhase.m_revengeValueThreshold;
            CounterAttack(directionToMove);
        }
    }

    private void DoThing(Vector3 a_directionToMove)
    {
        switch (m_mode)
        {
            case 0: // Wander state - Walk left or right facing player
                if (m_getNewBehavior)
                {
                    Wander();
                    m_lastDecision = Time.time + 4;
                    m_getNewBehavior = false;
                }
                break;
            case 1: // Dodge state
                if (m_getNewBehavior)
                {
                    Dodge(a_directionToMove);
                }
                break;
            case 2: // Attack
                {
                    Attack(a_directionToMove);
                }
                break;
            default:
                Debug.Log("How did we get here?");
                break;
        }
    // Seek towards target if range not met
    // m_animator.SetFloat("Movement/Z", a_directionToMove.z);
    // m_animator.SetFloat("Movement/X", a_directionToMove.x);
    //a_directionToMove = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
    //m_animator.SetFloat("Movement/Z", 0);
    //m_animator.SetFloat("Movement/X", 0);
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

    // Will dodge away from player.
    public void Dodge(Vector3 a_directionToMove)
    {
        Vector3 thing = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
        if (thing.x <= 0)
        {
            thing.x = -1;
        }
        else
        {
            thing.x = 1;
        }
        m_animator.SetFloat("Movement/Z", 0);
        m_animator.SetFloat("Movement/X", thing.x);

        m_animator.SetBool("Ai/IsDashing", true);
        m_getNewBehavior = false;
    }

    // More like circle but shits okay
    public void Wander()
    {
        int dir = Random.Range(0, 2);
        if (dir == 0)
        {
            // Move left
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("Movement/X", 1);
        }
        else
        {
            // Move right
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("Movement/X", -1);
        }
    }

    // Seek and attack together as one.
    // Should seperate seek out.
    public void Attack(Vector3 a_directionToMove)
    {
        if (!CalculateDistance(a_directionToMove))
        {
            a_directionToMove = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
            m_animator.SetFloat("Movement/Z", a_directionToMove.z);
            m_animator.SetFloat("Movement/X", a_directionToMove.x);
            m_animator.SetInteger("Ai/Attack", 0);
            m_animator.SetBool("Ai/IsPursuing", true);
            m_animator.SetFloat("ForwardMultiplyer", 2);
        }
        else
        {
            CorrectFacing();
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("Movement/X", 0);
            m_animator.SetInteger("Ai/Attack", 1);
            m_animator.SetBool("Ai/IsPursuing", false);
            m_animator.SetFloat("ForwardMultiplyer", 1);
            m_lastDecision = 0;
        }
    }


    // Dodge away then attack.
    public void CounterAttack(Vector3 a_directionToMove)
    {
        Dodge(a_directionToMove);
        m_mode = 2; // ATTACK MODE GO
        Attack(a_directionToMove);
    }


    public void IncreaseRevengeValue()
    {
        m_revengeValue += m_curreentPhase.m_revengeValueIncrease;
    }

    // If boss lands a hit on player should decrease value
    public void DecreaseRevengeValue()
    {
        m_revengeValue -= m_curreentPhase.m_revengeValueDecrease;
        if(m_revengeValue < 0)
        {
            m_revengeValue = 0;
        }
    }
}