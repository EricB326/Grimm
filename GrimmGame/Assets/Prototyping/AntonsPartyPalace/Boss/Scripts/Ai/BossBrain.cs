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
    // To be passed in by animation event
    // functions.
    private float m_actionRotationSpeed = 0;

    [Header("DEBUG. VALUES PASSED IN BY ANIMATOR")]
    // Used to check if can.
    public bool m_diagnosticMode = false;
    // The range to reach before attack begins.
    // Passed out of the animator and here for debugging.
    // This is set in the boss Phase.
    public float m_desiredRange = 0;
    public float m_desiredRangeMin = 0;
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
    public Player m_target;
    // If the boss is trying to be defensive he will 
    public GameObject m_moveTarget;
    // When was the last decision made.

    private int m_chanceOfPreMove;
    public int m_PreMoveFloor = 60;
    public int m_increasePerPremove = 30;

    public List<BossPhase> m_bossPhaseList = new List<BossPhase>();
    // List of actions
    // Will one day become a enums.
    public List<BossActions> m_actionQue = new List<BossActions>();

    // Ideally used instead of above with locally storage of the actions.
    public List<SteeringBehaviours> m_baseActions = new List<SteeringBehaviours>();

    // If an attack needs to be stored.
    public BossActions m_currentAttack = null;
    // If a seek needs to be stored
    public BossActions m_currentSeek = null;

    // When a state requires 2 steps.
    private bool m_continue = false;
    private bool m_launchAttack = false;

    // If behaviors require a timer.
    [SerializeField]
    private float m_timeOutSeek = 0;
    [SerializeField]
    private float m_timeOutAttack = 0;
    // When ai makes is attacking(either by command or random)
    // It places the attack in here so it has all the info it needs
    // at that time.
    // private BossSeek m_currentSeek = null;
    public int m_currentPhase;

    public int m_revengeValue = 0;
    [HideInInspector]
    public Vector3 m_startPos;


    private void Start()
    {
        m_startPos = this.transform.position;
        m_currentPhase = 0;
        m_animator = this.GetComponent<Animator>();
        m_target = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        m_timeOutSeek = 0;
        m_diagnosticMode = false;
        m_chanceOfPreMove = m_PreMoveFloor;
        this.enabled = false;
        // Boss will dissapear if these are set
        if (this.GetComponent<BossVariables>().m_swordModel != null)
        {
            this.GetComponent<BossVariables>().m_swordModel.enabled = false;
        }
        if (this.GetComponent<BossVariables>().m_model != null)
        {
            this.GetComponent<BossVariables>().m_model.enabled = false;
        }

    }

    private void Update()
    {
        // This is a bad name - Really it is both distance and direction.
        // THis currently does not work. Players location is different now with
        // the new setup
        Vector3 directionToMove = m_target.transform.position - transform.position;
        // Adds actions to que if needed.
        CheckQue(directionToMove);

            // Diagnostic mode disables boss so that you can test weighting
            if (!DiagnosticMode(directionToMove))
        { 
            // Rotates the boss
            // Should probably be changed to "Can rotate" 
            // and will be set/unset in the animator.
            if (!m_animator.GetBool("Ai/PlayingAction") || m_animator.GetBool("Ai/CanRotate"))
            {
                if (!m_animator.GetBool("Ai/CanRotate"))
                {
                    // Default rotation speed
                    RotateBoss(directionToMove);

                }
                else
                {
                    // Rotation from actions.
                    RotateBossInAction(directionToMove);
                }
                // Should be a part of actions.
            }
            // Boss will do it's current behavior that is has chosen above or
            // continue its current behavior - REVENGE VALUE REMOVED LOGIC DEFCUNT
            //if (m_revengeValue < m_bossPhaseList[m_currentPhase].m_threshold)
            {
                DoThing(directionToMove);
            }
            // If the boss gets mad - REVENGE VALUE REMOVED LOGIC DEFCUNT
            //else
            //{
            //    m_revengeValue = -m_bossPhaseList[m_currentPhase].m_threshold;
            //    CounterAttack(directionToMove);
            //}
        }

    }
    // Checks what actions it's doing.
    // Probably can be removed.
    // Boss should be checking if he should attack or not
    // and by default he will attack.
    // if not he will seek.
    private void DoThing(Vector3 a_directionToMove)
    {
        if(m_baseActions.Count != 0)
            switch(m_baseActions[0])
            { 
        //if (m_actionQue.Count != 0)
        //    switch (m_actionQue[0].GetBehaviourType)
        //    {
                //case SteeringBehaviours.PASSIVE: // Passive
                //    if (Passive(a_directionToMove))
                //    {
                //        m_actionQue.RemoveAt(0);
                //    }
                //    break;
                //case SteeringBehaviours.DEFENSIVE: // Defensive
                //    if (Defensive(a_directionToMove))
                //    {
                //        m_actionQue.RemoveAt(0);
                //    }
                //    break;
                case SteeringBehaviours.AGGRESSIVE: // Aggressive
                    if (Aggresive(a_directionToMove))
                    {
                        m_baseActions.RemoveAt(0);
                        //m_actionQue.RemoveAt(0);
                    }
                    break;
                case SteeringBehaviours.SEEK: // Seek
                    if (Seek(a_directionToMove))
                    {
                        m_baseActions.RemoveAt(0);
                        //m_actionQue.RemoveAt(0);
                    }
                    break;
                default:
                    Debug.Log("How did we get here? Current action is: " + m_actionQue[0].GetBehaviourType);
                    break;
            }
    }



    //************************************************************
    //Rotation
    //************************************************************
    //************************************************************
    // 2 Seperate cases of boss rotation. Can probably be reviewed
    // and simplified.
    //************************************************************
    private void RotateBoss(Vector3 a_direction)
    {
        a_direction.y = 0;
        //a_direction.x = 0;
        Quaternion targetRotation = Quaternion.LookRotation(a_direction);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, m_rotationSpeed);
    }

    // While the boss is in action this rotation will be called
    // when animation function triggers.
    private void RotateBossInAction(Vector3 a_direction)
    {
        a_direction.y = 0;
        //a_direction.x = 0;
        Quaternion targetRotation = Quaternion.LookRotation(a_direction);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, m_actionRotationSpeed);
    }

    //************************************************************
    //Distance and Facing
    //************************************************************
    //************************************************************
    // Small functions that can be used for working out distance
    // to player and correct facing
    //************************************************************


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
        float dot = Vector3.Dot(a_direction.normalized, this.transform.forward);

        // Need to add a variable to check if within range to launch attack and if not rotate?
        // Or does the rotation happen in the before?
        if (dot > 0.9f)
        {
            return true;
        }
        else
            return false;
    }

    //************************************************************
    //Action moves
    //************************************************************
    //************************************************************
    // Evaluation of moves on the action list.
    //************************************************************



    // Evaluate best attack 
    // Try to launch attack
    // If can't add a seek above this action
    // If can decide if another action should occur
    // before and/or after the attack action.
    // Once complete or player is dead or knockeddown
    // remove from list.


    // Used with base actions.
    private bool Aggresive(Vector3 a_target)
    {
        if (!m_continue)
        {
            // If aggresive but no action get action and time out 
            // Or launching attack (in case time breaks it.
            if (m_currentAttack != null && m_timeOutAttack > Time.time)
            {
                // If in action skip entirely
                if (!m_animator.GetBool("Ai/PlayingAction"))
                {
                    // Check if you're at the correct distance or should continue
                    if (m_launchAttack || CalculateDistance(a_target) && CorrectFacing(a_target))
                    {
                        // Stop your movement. Mostly a safety.
                        m_animator.SetFloat("Movement/Z", 0);
                        m_animator.SetFloat("Movement/X", 0);
                        int random = Random.Range(0, 100);
                        //Debug.Log(random);
                        // Should I do an action before I attack or currenlty not in action
                        if (random < m_chanceOfPreMove && !m_launchAttack)
                        {
                            BossActions t = m_bossPhaseList[m_currentPhase].EvaluateSeek(GetDistanceToPlayer(a_target), m_desiredRange);
                            if (t != null)
                            {
                                m_chanceOfPreMove = m_PreMoveFloor;
                                m_animator.SetInteger("Ai/Action", t.GetAnimNum);
                                m_launchAttack = true;
                                return false;
                            }
                        }
                        else
                        {
                            // Increase chance boss will do something before
                            // an move.
                            m_chanceOfPreMove += m_increasePerPremove;
                            if(m_chanceOfPreMove > 100)
                            {
                                m_chanceOfPreMove = 100;
                            }
                        }
                        // if it is not launching an extra animation it should get here.
                        m_animator.SetInteger("Ai/Action", m_currentAttack.GetAnimNum);
                        m_continue = true;
                    }
                    else
                    {
                        // If null or not within distance launch attack launch attack 
                        AddSeek();
                    }
                }
            }
            else
            {
                // A safety in case no action chosen.
                BossActions t = m_bossPhaseList[m_currentPhase].EvaluateAggresiveAction(GetDistanceToPlayer(a_target));
                if (t != null)
                {
                    m_currentAttack = t;
                    m_desiredRange = t.AttackRange;
                    m_launchAttack = false;
                    m_continue = false;
                    //Debug.Log("New attack");
                    // Will try to launch attack for x time
                    m_timeOutAttack = Time.time + 10;
                    // Sometimes enters here unintentionally
                }
            }
            return false;
        }
        // logic is wrong. Enters here when it shouldn't
        else if (!m_animator.GetBool("Ai/PlayingAction"))
        {
            // Should be hitting this whenever attacks are finished.
            m_continue = false;
            m_launchAttack = false;
            // Null so new attack needed. V dangerous.
            m_currentAttack = null;
            //Debug.Log("Attack Complete");
            return true;
        }
        else
            return false;
    }




    // Used with action que.
    //private bool Aggresive(Vector3 a_directionToMove)
    //{
    //    // Am I in range to launch attack or am I already
    //    // attacking?
    //    if (CalculateDistance(a_directionToMove) || m_continue)
    //    {
    //        // So you're at the correct distance
    //        // Stop your movement.
    //        m_animator.SetFloat("Movement/Z", 0);
    //        m_animator.SetFloat("Movement/X", 0);


    //        // So launch attack if facing correct direction
    //        if (!m_continue && CorrectFacing(a_directionToMove))
    //        {
    //            m_animator.SetInteger("Ai/Action", m_actionQue[0].GetAnimNum);
    //            m_continue = true;
    //            return false;
    //        }
    //        else
    //        {
    //            // If attack variants would need to check what ones to go down somewhere
    //            // Or do internal logic on state machine.
    //            if (!m_animator.GetBool("Ai/PlayingAction") && m_continue)
    //            {
    //                ResetState();
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        AddSeek(a_directionToMove);
    //        return false;
    //    }
    //}

    // Move towards target by default.
    // Needs to evaluate an animation if special
    // ability time limit is triggered.

    private bool Seek(Vector3 a_directionToMove)
    {
        // Check if within range or currently playing action
        if (CalculateDistance(a_directionToMove) && !m_animator.GetBool("Ai/PlayingAction"))
        {
            return true;
        }

        // Needs to check if it's time to do an action like dodge forward ect
        if (m_timeOutSeek < Time.time)
        {
            // Set time for next evaluation
            m_timeOutSeek = Time.time + m_bossPhaseList[m_currentPhase].m_timeBetweenMovementAction;
            // Evaluate action
            BossActions t = m_bossPhaseList[m_currentPhase].EvaluateSeek(GetDistanceToPlayer(a_directionToMove), m_desiredRange);
            // If action is returned instantly do action
            if (t != null)
            {
                // Do the action straight away
                m_animator.SetInteger("Ai/Action", t.GetAnimNum);
                // Should set plaing action to true.
                return false;
            }
            // Else seek towards player still.
        }

        // If not time boss will walk towards target.
        a_directionToMove = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
        m_animator.SetFloat("Movement/Z", a_directionToMove.z);
        m_animator.SetFloat("Movement/X", a_directionToMove.x);
        m_animator.SetInteger("Ai/Action", 0);
        // Should always get the animation speed multiplyer
        //m_animator.SetFloat("AnimationSpeedMultiplyer", 2);

        // Has not reached target.
        return false;
    }


    // Will dodge away from player attacks.
    // Used for tricky attack strings or
    // couterattaking.
    // DEFUNCT
    //public bool Defensive(Vector3 a_directionToMove)
    //{
    //    // Needs to move away from player.
    //    // Will try and maintain distance to player

    //    // Will try to maintain a range from the player
    //    // and passivly gain stamina until 100% again
    //    // 

    //    if (!m_continue)
    //    {
    //        Vector3 dodgeAway = this.transform.position - m_target.transform.position;
    //        if (dodgeAway.x <= 0)
    //        {
    //            dodgeAway.x = -1;
    //        }
    //        else
    //        {
    //            dodgeAway.x = 1;
    //        }
    //        // Mostly a 
    //        m_animator.SetFloat("Movement/Z", 0);
    //        m_animator.SetFloat("Movement/X", dodgeAway.x);
    //        m_animator.SetBool("Ai/IsDashing", true);
    //        m_continue = true;
    //        return false;
    //    }
    //    // Check if still dodging
    //    if(!m_animator.GetBool("Ai/IsDashing")) 
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    // More like circle but shits okay
    // Need to seek avoid target for X seconds before 
    // action.
    // DEFUNCT
    //public bool Passive(Vector3 a_directionToMove)
    //{
    //    // a_directionToMove
    //    // Direction to move away from until 
    //    // 
    //    if (!m_continue)
    //    {

    //        if (Random.Range(0, 2) == 0)
    //        {
    //            m_animator.SetFloat("Movement/X", 1);
    //        }
    //        else
    //        {
    //            m_animator.SetFloat("Movement/X", -1);
    //        }
    //        // Avoid getting another.
    //        m_continue = true;
    //        // Get a time to exit the wander state.
    //        m_timeOut = Time.time + m_bossPhaseList[m_currentPhase].m_timeBetweenMovementAction;
    //    }
    //    else
    //    {
    //        // Check if time to exit wander state. 
    //        if (m_timeOut > Time.time)
    //        {
    //            // Remove old information.
    //            ResetState();
    //            // Recover all stamina
    //            // EntityStats.Instance.RecoverStaminaOfEntity();
    //            // Will be removed from list.
    //            return true;
    //        }
    //    }
    //    // Will not be removed from list.
    //    return false;
    //}



    // Old variant of seek
    //private bool Seek(Vector3 a_directionToMove)
    //{
    //    //a_directionToMove = transform.worldToLocalMatrix * a_directionToMove.normalized;
    //    if (!m_continue) // Also needs to see if the target is within special seek behavior range(not too far or close)
    //    {
    //        // Do special seek behavior
    //        // Shoould be an animation and a direction right?
    //        // The animation should contain what it needs so if
    //        // it needs x and z they are passed in below.
    //        m_continue = true;
    //    }
    //    if(CalculateDistance(a_directionToMove))
    //    {
    //        m_animator.SetFloat("AnimationSpeedMultiplyer", 1);
    //        ResetState();
    //        return true;
    //    }
    //    else
    //    {
    //        a_directionToMove = this.transform.worldToLocalMatrix * a_directionToMove.normalized;
    //        m_animator.SetFloat("Movement/Z", a_directionToMove.z);
    //        m_animator.SetFloat("Movement/X", a_directionToMove.x);
    //        m_animator.SetInteger("Ai/Action", 0);
    //        //m_animator.SetBool("Ai/IsPursuing", true);
    //        // Should always get the animation speed multiplyer
    //        //m_animator.SetFloat("AnimationSpeedMultiplyer", 2);



    //        //a_directionToMove = transform.worldToLocalMatrix * a_directionToMove.normalized;
    //        //Debug.Log(a_directionToMove);

    //        //m_animator.SetFloat("Movement/X", a_directionToMove.x);
    //        //m_animator.SetFloat("Movement/Y", a_directionToMove.y);
    //        return false;
    //    }
    //}



    // This needs to be either predefined phase by phase.
    // Ideally you would want to put only a defensive action in here
    // As the boss will choose the best attack after the dodge.

    // OH GOD THIS IS DEFUNCT RIGHT NOW
    // AND GOING TO BE THE 1ST THING TOUCHED
    // WHEN I SAY IT'S GOOD TO GO
    //private void CounterAttack(Vector3 a_directionToMove)
    //{
    //    ResetState();
    //    m_actionQue.Clear();
    //    // I don't know if 
    //    if (m_bossPhaseList[m_currentPhase].m_counterMoves != null || m_bossPhaseList[m_currentPhase].m_counterMoves.Count > 0)
    //    {
    //        foreach (SteeringBehaviours behavior in m_bossPhaseList[m_currentPhase].m_counterMoves)
    //        {
    //            switch (behavior)
    //            {
    //                case SteeringBehaviours.AGGRESSIVE:
    //                    {
    //                        m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateAggresiveAction(GetDistanceToPlayer(a_directionToMove)));
    //                        break;
    //                    }
    //                //case SteeringBehaviours.DEFENSIVE:
    //                //    {
    //                //        m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateDefensiveAction(a_directionToMove.normalized));
    //                //        break;
    //                //    }
    //                //case SteeringBehaviours.PASSIVE:
    //                //    {
    //                //        // Not correct right now.
    //                //        // Need to have an evaluate wander.
    //                //        m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateAggresiveAction(GetDistanceToPlayer(a_directionToMove)));
    //                //        break;
    //                //    }
    //                case SteeringBehaviours.SEEK:
    //                    {
    //                        // Need a better way to evaluate seek range         // Might break as there is no desired range yet?
    //                        m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateSeek(GetDistanceToPlayer(a_directionToMove), m_desiredRange));
    //                        break;
    //                    }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // Should probably be passed in by 
    //        //m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateDefensiveAction(a_directionToMove.normalized));
    //        m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateAggresiveAction(GetDistanceToPlayer(a_directionToMove)));
    //        Debug.Log("Counter attack has no specific actions. Defaults to dodge and attack.");
    //    }

    //    //m_actionQue.Add(AI_BEHAVIOR.ATTACK); // Need to get from phase
    //    //m_actionQue.Add(AI_BEHAVIOR.DODGE); // Need to get from phase
    //    // Empty list out and add dodge and
    //    // attack to list.
    //}

    // A helper function to simplify resetting
    // the behaviors. Any time the list is
    // emptied this will be called.


    private void ResetState()
    {
        m_animator.SetFloat("AnimationSpeedMultiplyer", 1);
        m_continue = false;
        m_timeOutSeek = Time.time + m_bossPhaseList[m_currentPhase].m_timeBetweenMovementAction;
    }

    // Seeks should be generated only if unable 
    // to complete action.
    // Therefore they will always know what range they
    // need to get into if they check the next action in the list
    // if no action after choose a random action.
    //private void AddSeek(Vector3 a_directionToMove)
    //{
    //    m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateSeek(GetDistanceToPlayer(a_directionToMove), m_desiredRange));
    //    // Check if seeking to a task.
    //    if(m_actionQue[1] != null)
    //    {
    //        // Make sure seeking to tasks range
    //        m_desiredRange = m_actionQue[1].AttackRange;
    //    }
    //    // If not evaluate self and get a task
    //    else
    //    {
    //       m_bossPhaseList[m_currentPhase].EvaluateSelf(GetDistanceToPlayer(a_directionToMove), a_directionToMove);
    //       m_desiredRange = m_actionQue[1].AttackRange;
    //    }
    //}
    // Needs to run the seek script instead.

    // Not sure about this one.
    private void AddSeek()
    {
        // Seems dumb. Mostly to replace the above.
        m_baseActions.Insert(0, SteeringBehaviours.SEEK);
        m_timeOutSeek = Time.time + m_bossPhaseList[m_currentPhase].m_timeBetweenMovementAction;
    }

    #region Revenge Value
    //************************************************************
    //Revenge value - REVENGE VALUE REMOVED LOGIC DEFCUNT
    //************************************************************
    //************************************************************
    //Used to up challenging if player doing well
    //or force the player into combat if they're
    //avoiding boss.
    //************************************************************

    //public void IncreaseRevengeValue()
    //{
    //    m_revengeValue += m_bossPhaseList[m_currentPhase].m_increase;
    //}

    //// If boss lands a hit on player should decrease value
    //public void DecreaseRevengeValue()
    //{
    //    m_revengeValue -= m_bossPhaseList[m_currentPhase].m_increase;
    //    if(m_revengeValue < 0)
    //    {
    //        m_revengeValue = 0;
    //    }
    //}

    // I don't remember doing this...
    //public bool DiagnosticMode
    //{
    //    get { return m_diagnosticMode; }
    //    set { m_diagnosticMode = value; }
    //}
    #endregion

    // The boss will look at its current que
    // and add actions depending on its internal state.
    // This is pointless.
    private void CheckQue(Vector3 a_directionToMove)
    {
        // Wait(x sec) wander(x sec) randomly attack or do preset action list
        if (m_baseActions.Count == 0)
        {
            // If action list is empty and the predefined list 
            // has occured too recently we do a random attack from
            // the phase.
            //if (m_bossPhaseList[m_currentPhase].m_timeBetweenPreDefinedAndRandom > Time.time || m_bossPhaseList[m_currentPhase].m_preDefinedActions.Count != 0)
            {
                // Pick an action based on internal state.
                m_baseActions.Insert(0, SteeringBehaviours.AGGRESSIVE);
                //m_actionQue.Insert(0, m_bossPhaseList[m_currentPhase].EvaluateSelf(GetDistanceToPlayer(a_directionToMove), a_directionToMove));
            }
            //else
            // REMOVED PRE DEFINED ACTIONS -- BOSS WILL ALWAYS CHOOSE ACTIONS BASED ON BEST
            // OPTION AT THIS TIME.
            //{
            //    foreach (int action in m_bossPhaseList[m_currentPhase].m_preDefinedActions)
            //    {
            //        BossActions t = m_bossPhaseList[m_currentPhase].m_bossActions[action];
            //        m_actionQue.Insert(0, t);
            //    }
            //    m_bossPhaseList[m_currentPhase].m_storedTime = Time.time + m_bossPhaseList[m_currentPhase].m_timeBetweenPreDefinedAndRandom;
            //}
            // Find the 1st ability that has a range
            // If not we add an ability to it that does.
            //foreach (BossActions action in m_actionQue)
            //{
            //    if (action.GetBehaviourType != SteeringBehaviours.SEEK)
            //    {
            //        m_desiredRange = action.AttackRange;
            //    }
            //    else
            //{
            //    m_bossPhaseList[m_currentPhase].EvaluateSelf(GetDistanceToPlayer(a_directionToMove), a_directionToMove);
            //}
        }
    }


    // Stops the boss from animating and view his 
    // current state.
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

    // For setting action rotation speed outside of class.
    // Mostly by animation events.
    public void SetActionRotationSpeed(float a_rotationSpeed)
    {
        m_actionRotationSpeed = a_rotationSpeed;
    }


    public void BossReset()
    {
        float healthRecover = EntityStats.Instance.GetHealthOfEntity("Boss") - 100;
        EntityStats.Instance.DeminishHealthOffEntity("Boss", healthRecover);
        BossVariables bossVar = this.GetComponent<BossVariables>(); 
        bossVar.m_gate.m_colliderToEnable.isTrigger = true;
        this.transform.position = m_startPos;
        bossVar.m_activationScript.m_BossHealth.SetActive(false);
        bossVar.m_activationScript.m_BossCounters.SetActive(false);
        m_animator.SetFloat("Movement/Z", 0);
        m_animator.SetFloat("Movement/X", 0);
        m_animator.SetInteger("Ai/Action", 0);
        m_animator.SetInteger("CutScene", 2);
        this.enabled = false;
    }

}
