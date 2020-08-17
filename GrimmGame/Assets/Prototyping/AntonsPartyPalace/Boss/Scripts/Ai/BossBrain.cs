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
    // If the boss is trying to be defensive he will 
    public GameObject m_moveTarget;
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
        m_diagnosticMode = false;
    }

    private void Update()
    {
        // This is a bad name - Really it is both distance and direction.
        Vector3 directionToMove = m_target.transform.position - transform.position;

        // Adds actions to que if needed.
        CheckQue(directionToMove);
       
        // Diagnostic mode disables boss so that you can test weighting
        if (!DiagnosticMode(directionToMove))
        {
            // Rotates the boss
            // Should probably be changed to "Can rotate" 
            // and will be set/unset in the animator.
            if (!m_animator.GetBool("Ai/PlayingAction"))
            {
                // Should be a part of actions.
                RotateBoss(directionToMove);
            }
            // Boss will do it's current behavior that is has chosen above or
            // continue its current behavior.
            if (m_revengeValue < m_bossPhaseList[m_currentPhase].m_threshold)
            {
                DoThing(directionToMove);
            }
            // If the boss gets mad 
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
            case SteeringBehaviours.WANDER_BEHAVIOUR: // Passive
                if (Passive(a_directionToMove))
                {
                    m_actionQue.RemoveAt(0);
                }
                break;
            case SteeringBehaviours.DODGE_BEHAVIOUR: // Defensive
                if (Defensive(a_directionToMove))
                {
                    m_actionQue.RemoveAt(0);
                }
                break;
            case SteeringBehaviours.ATTACK_BEHAVIOUR: // Aggressive
                if (Aggresive(a_directionToMove))
                {
                    m_actionQue.RemoveAt(0);
                }
                break;
            case SteeringBehaviours.SEEK_BEHAVIOUR: // Seek
                if (Seek(a_directionToMove))
                {
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
    private bool CorrectFacing(Vector3 a_direction)
    {
        float dot = Vector3.Dot(a_direction.normalized, this.transform.forward.normalized);

        // Need to add a variable to check if within range to launch attack and if not rotate?
        // Or does the rotation happen in the before?
        if (dot > 0.8f)
        {
            return true;
        }
        else
            return false;
    }

    // Will dodge away from player attacks.
    // Used for tricky attack strings or
    // couterattaking.
    public bool Defensive(Vector3 a_directionToMove)
    {
        // Needs to move away from player.
        // Will try and maintain distance to player

        // Will try to maintain a range from the player
        // and passivly gain stamina until 100% again
        // 

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
            // Mostly a 
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
    // Need to seek avoid target for X seconds before 
    // action.
    public bool Passive(Vector3 a_directionToMove)
    {
        // a_directionToMove
        // Direction to move away from until 
        // 
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
                // Recover all stamina
                // EntityStats.Instance.RecoverStaminaOfEntity();
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

    private bool Aggresive(Vector3 a_directionToMove)
    {
        // Am I in range to launch attack?
        if (CalculateDistance(a_directionToMove)|| m_continue)
        {
            // So you're at the correct distance
            // Stop your movement.
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("Movement/X", 0);


            // So launch attack if facing correct direction
            if (!m_continue && CorrectFacing(a_directionToMove)) 
            {
                m_animator.SetInteger("Ai/Action", 1/*m_currentaction.attackanimation*/);
                m_continue = true;
                return false;
            }
            else
            {
                // If attack variants would need to check what ones to go down somewhere
                // Or do internal logic on state machine.
                if (!m_animator.GetBool("Ai/PlayingAction") && m_continue)
                {
                    ResetState();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            AddSeek(a_directionToMove);
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
            m_animator.SetFloat("AnimationSpeedMultiplyer", 1);
            ResetState();
            return true;
        }
        else
        {
            a_directionToMove = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
            m_animator.SetFloat("Movement/Z", a_directionToMove.z);
            m_animator.SetFloat("Movement/X", a_directionToMove.x);
            m_animator.SetInteger("Ai/Action", 0);
            //m_animator.SetBool("Ai/IsPursuing", true);
            // Should always get the animation speed multiplyer
            m_animator.SetFloat("AnimationSpeedMultiplyer", 2);



            //a_directionToMove = transform.worldToLocalMatrix * a_directionToMove.normalized;
            //Debug.Log(a_directionToMove);

            //m_animator.SetFloat("Movement/X", a_directionToMove.x);
            //m_animator.SetFloat("Movement/Y", a_directionToMove.y);
            return false;
        }
    }

    // This needs to be either predefined phase by phase.
    // Ideally you would want to put only a defensive action in here
    // As the boss will choose the best attack after the dodge.
    private void CounterAttack(Vector3 a_directionToMove)
    {
        ResetState();
        m_actionQue.Clear();
        // I don't know if 
        if (m_bossPhaseList[m_currentPhase].m_counterMoves != null || m_bossPhaseList[m_currentPhase].m_counterMoves.Count > 0)
        {
            foreach (SteeringBehaviours behavior in m_bossPhaseList[m_currentPhase].m_counterMoves)
            {
                switch (behavior)
                {
                    case SteeringBehaviours.ATTACK_BEHAVIOUR:
                        {
                            m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateAggresiveAction(GetDistanceToPlayer(a_directionToMove)));
                            break;
                        }
                    case SteeringBehaviours.DODGE_BEHAVIOUR:
                        {
                            m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateDefensiveAction(a_directionToMove.normalized));
                            break;
                        }
                    case SteeringBehaviours.WANDER_BEHAVIOUR:
                        {
                            // Not correct right now.
                            // Need to have an evaluate wander.
                            m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateAggresiveAction(GetDistanceToPlayer(a_directionToMove)));
                            break;
                        }
                    case SteeringBehaviours.SEEK_BEHAVIOUR:
                        {
                            // Need a better way to evaluate seek range
                            m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateSeek(GetDistanceToPlayer(a_directionToMove)));
                            break;
                        }
                }
            }
        }
        else
        {
            // Should probably be passed in by 
            m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateDefensiveAction(a_directionToMove.normalized));
            m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateAggresiveAction(GetDistanceToPlayer(a_directionToMove)));
            Debug.Log("Counter attack has no specific actions. Defaults to dodge and attack.");
        }

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
        m_animator.SetFloat("AnimationSpeedMultiplyer", 1);
        m_continue = false;
        m_timeOut = 0;
    }

    // Seeks should be generated only if unable 
    // to complete action.
    // Therefore they will always know what range they
    // need to get into if they check the next action in the list
    // if no action after choose a random action.
    private void AddSeek(Vector3 a_directionToMove)
    {
        m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateSeek(GetDistanceToPlayer(a_directionToMove)));
        // Check if seeking to a task.
        if(m_actionQue[1] != null)
        {
            // Make sure seeking to tasks range
            m_desiredRange = m_actionQue[1].AttackRange;
        }
        // If not evaluate self and get a task
        else
        {
           m_bossPhaseList[m_currentPhase].EvaluateSelf(GetDistanceToPlayer(a_directionToMove), a_directionToMove);
           m_desiredRange = m_actionQue[1].AttackRange;
        }
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

    // I don't remember doing this...
    //public bool DiagnosticMode
    //{
    //    get { return m_diagnosticMode; }
    //    set { m_diagnosticMode = value; }
    //}


    // The boss will look at its current que
    // and add actions depending on its internal state
    private void CheckQue(Vector3 a_directionToMove)
    {
        // Wait(x sec) wander(x sec) randomly attack or do preset action list
        if (m_actionQue.Count == 0)
        {
            // If action list is empty and the predefined list 
            // has occured too recently we do a random attack from
            // the phase.
            if (m_bossPhaseList[m_currentPhase].m_timeBetweenPreDefinedAndRandom > Time.time || m_bossPhaseList[m_currentPhase].m_preDefinedActions.Count != 0)
            {
                // Pick an action based on internal state.
                m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateSelf(GetDistanceToPlayer(a_directionToMove), a_directionToMove));
            }
            else
            {
                foreach (int action in m_bossPhaseList[m_currentPhase].m_preDefinedActions)
                {
                    BossActions t = m_bossPhaseList[m_currentPhase].m_bossActions[action];
                    m_actionQue.Insert(0, t);
                }
                m_bossPhaseList[m_currentPhase].m_storedTime = Time.time + m_bossPhaseList[m_currentPhase].m_timeBetweenPreDefinedAndRandom;
            }
            // Find the 1st ability that has a range
            // If not we add an ability to it that does.
            foreach (BossActions action in m_actionQue)
            {
                if (action.GetBehaviourType != SteeringBehaviours.SEEK_BEHAVIOUR)
                {
                    m_desiredRange = action.AttackRange;
                }
                else
                {
                    m_bossPhaseList[m_currentPhase].EvaluateSelf(GetDistanceToPlayer(a_directionToMove), a_directionToMove);
                }
            }
        }
    }

    private bool DiagnosticMode(Vector3 a_directionToMove)
    {
        if(m_diagnosticMode)
        {
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("Movement/X", 0);
            // All the logic for diagnostic mode happens here
            return true;
        }
        else
        {
            return false;
        }
    }

}
