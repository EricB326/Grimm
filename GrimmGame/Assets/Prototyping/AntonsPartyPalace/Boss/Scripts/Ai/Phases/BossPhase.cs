using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// What specific behaviors will the boss have this phase
// Attacks revenge value and specific behaviors on a 
// per phase basis

[Serializable]
public class BossPhase 
{
    public string m_name;
    // Amount increased when taking damage or 
    // changing behaviors.
    public float m_healthAtStart;


    //[Header("REVENGE VALUE")]
    //public int m_increase;
    //// Value decreases
    //public int m_decrease;
    //// Once this is hit a counter attack happens.
    //public int m_threshold;
    // The list of phase attacks?
    // How long between bosses movment
    // decisions. So by default boss will walk
    // towards player and if this time is hit
    // will roll 1 of the "seek behaviors"
    public float m_timeBetweenMovementAction;

    public List<BossActions> m_bossActions;

    // If so desired you can choose a bunch of action 
    // from the above list and gaurentee them happening
    // in the order they are in.
    // Ideally you need a seek followed by an attack
    // Great for testing.
    //public float m_timeBetweenPreDefinedAndRandom;
    //public List<int> m_preDefinedActions; -- REMOVED DUE TO SIMPLIFIED BOSS
    //public List<SteeringBehaviours> m_counterMoves; -- REMOVED DUE TO SIMPLIFIED BOSS
    [HideInInspector]
    public float m_storedTime;

    //public float m_staminaThisPhase;
    // Used to asses if should attack, defend or 
    //[SerializeField]
    //[Range(0, 100)]
    //private float m_aggresiveStaminaThreshold = 75.0f;
    //[SerializeField]
    //[Range(0,100)]
    //private float m_defensiveStaminaThreshold = 50.0f;
    // For modifying next evaluation
    private EvaluatedAction m_lastAttackAction = null;
    private BossActions m_lastSeekAction = null;


    private float m_lastNeutralAction = 0;
    public float m_timeBetweenNeutral = 5;

    public int multiplyNegativeWeight = 50;


    // Stores the evaluated actions and the actions themselves.
    private class EvaluatedAction
    {
        // A reference to the aciton itself.
        // Passed out on success.
        public BossActions m_action;
        // The evaluated score.
        public float m_score;
        public float m_rollMin;
        public float m_rollMax;
    }


    // Boss checks to see what it's stamina is at.
    // DEFUNCT
    //public BossActions EvaluateSelf(float a_distanceToTarget, Vector3 m_targetDirection)
    //{
    //   // Might need to be removed
    //   // Boss should only need to act agressivly

    //    // Did i do a dodge last action?
    //    // if yes choose attack and seek towards player
    //    // if no choose either an attack or dodge.


    //    //float currentStam = EntityStats.Instance.GetStaminaOfEntity("Boss");
    //    //if (currentStam > m_aggresiveStaminaThreshold)
    //    //{
    //        return EvaluateAggresiveAction(a_distanceToTarget);
    //    //}
    //    //else if(currentStam > m_defensiveStaminaThreshold)
    //    //{   
    //    //    return EvaluateDefensiveAction(m_targetDirection);
    //    //}
    //    //else
    //    //{
    //    //    return EvaluatePassiveAction();
    //    //}

    //}

    // Before attack needs to evaluate if it should evaluate 
    // whether it should do an action that would result in no change
    // and launch the attack straight after.

    // Needs to know if the attack is launchable.
    // Attacks that need to be launched from a larger 
    // distance will be weighted
    public BossActions EvaluateAggresiveAction(float a_distanceToTarget)
    {
        BossActions output = null;
        List<EvaluatedAction> actions = new List<EvaluatedAction>();
        foreach (BossActions t in m_bossActions)
        {
            if (t.GetBehaviourType == SteeringBehaviours.AGGRESSIVE)
            {       
                EvaluatedAction z = new EvaluatedAction();
                // Need to give values for evaluations
                z.m_action = t;

                float range = t.AttackRange;      // Higher value closter to the target range
                //float distanceCovered = t.GetDestinationDistance; // How far the move travels.
                float weight = t.AttackWeight;    // Flat value
                float timesUsed = t.NumberOfUses; // Subtraction based on times used
                float rangeThreshold = 2;         // needs to be passed in by attack
                                                  // Did we want a random number added?
                z.m_score = 0;
                // Range calc
                if (range <= a_distanceToTarget + rangeThreshold && range >= a_distanceToTarget - rangeThreshold)
                {
                    z.m_score = 60;
                }
                else if (range <= a_distanceToTarget + (rangeThreshold * 2) &&
                    range >= a_distanceToTarget - (rangeThreshold * 2))
                {
                    z.m_score = 30;
                }
                else
                {
                    z.m_score = 10;
                }
                // Weight
                z.m_score += weight;
                // Time used
                z.m_score -= timesUsed * multiplyNegativeWeight;

                if (z.m_score < 0)
                {
                    z.m_score = 0;
                }
                    

                actions.Add(z);

                // Distance to player, damage, coolness factor(set by designer) timeused

                // more ways to be effect weighing.

                // Weighing happens here
                // If desirability is greater than previous replace.
                // bossaction
                // Only here to test
                //output = t;
                //break;
            }
            // Add all the evaluated values together
        }
        // Add up all scores
        float total = 0;
        foreach (EvaluatedAction t in actions)
        {
            total += t.m_score;
        }
        float norm = 1 / total;
        float assaignedSoFar = 0;
        // Apply normal value to every attack
        // and set min and max roll
        foreach (EvaluatedAction t in actions)
        {
            float normalizedScore = norm * t.m_score;
            t.m_rollMin = assaignedSoFar;
            t.m_rollMax = assaignedSoFar + normalizedScore + 0.01f; 
            // Will this be okay? I dunno lol
            assaignedSoFar = t.m_rollMax - 0.01f;
        }


        // roll and convert to 0 - 1.
        // pass out action within range.
        float number = UnityEngine.Random.Range(0, assaignedSoFar);
        //1 to 100
        
        //number = number / assaignedSoFar;
        foreach (EvaluatedAction t in actions)
        {
            if( number <= t.m_rollMax &&  number >= t.m_rollMin )
            {
                output = t.m_action;
                //t.m_action.LastTimeUsed++;
                if(m_lastAttackAction != null)
                {
                    m_lastAttackAction.m_action.NumberOfUses--;
                }
                t.m_action.NumberOfUses++;
                m_lastAttackAction = t;
                break;
            }
        }

        //Debug.Log(number);

        // Adjust the weighing values of the list

        // Needs an output other than null otherwise breakage.
        if(output == null)
        {
            Debug.Log("Fuck ai is busted.");
            Debug.Log("Random number was: " + number + " and " +
                assaignedSoFar + " was the total assaigned values.");
        }

        return output;
    }

    // Seeks to desired range. Since seeks don't require a range this probably works.
    //public BossActions EvaluateSeek(float a_distanceToTarget, float m_desiredRange)
    //{
    //    BossActions output = null;
    //    foreach (BossActions t in m_bossActions)
    //    {
    //        if(t.GetBehaviourType == SteeringBehaviours.SEEK_BEHAVIOUR)
    //        {
    //            // from 10 to 0       Range from 020    0 or neg * 5
    //            // optimal distance, coolness factor, timeused
    //            // Weighing happens here
    //            // If desirability is greater than previous replace.
    //            // bossaction
    //            // Only here to test
    //            output = t;
    //            break;
    //        }
    //        // Adjust the weighing values of the list
    //    }
    //    return output;
    //}

    // Seek to desired target.
    // Think point to point movement with animations
    public BossActions EvaluateSeek(float a_distanceToTarget, float m_desiredRange)
    {
        // Chosen action
        BossActions output = null;
        // If it's good it goes here to be chosen later.
        List<BossActions> desired = new List<BossActions>();
        // If the move does not benefit but can be done.
        List<BossActions> neutral = new List<BossActions>();
        foreach (BossActions t in m_bossActions)
        {
            // List to be populated.
            // Behaviors are seeks therefore we should be checking
            // if they put the boss in a good position.
            if (t.GetBehaviourType == SteeringBehaviours.SEEK)
            {
                // If the distance covered is beneficial or negligalble
                // does the movement get the boss closer or further?
                float distanceCovered = t.GetDestinationDistance;
                // Need to check if the distance covered is beneficial

                float resultingDistance = a_distanceToTarget - distanceCovered;
                // Should be passed in by action.
                

                float range = 2;
                // Check if the moves result is neutral.

                if (resultingDistance == a_distanceToTarget)
                    {
                    if (m_lastSeekAction != null)
                    {
                        if (m_lastSeekAction == t)
                        {
                            continue;
                        }
                    }
                    // No benefit but do able.
                    //Debug.Log(t.GetAnimNum + " is neutral");
                    neutral.Add(t);
                }
                // Check if the move gets boss closer to optimal position without passing
                // through player.
                else if (resultingDistance <= m_desiredRange && resultingDistance > 0)
                {
                    if (m_lastSeekAction != null)
                    {
                        if (m_lastSeekAction == t)
                        {
                            continue;
                        }
                    }
                    // A good choice
                    //Debug.Log(t.GetAnimNum + " is desired");
                    desired.Add(t);
                }
                // Otherwise move is skipped.
            }
        }

        int reroll = 0;
        if (desired.Count > 0)
        {
            while (reroll < 1)
            {
                int chosen = UnityEngine.Random.Range(0, desired.Count);

                BossActions t = desired[chosen];

                // Reset the last actions used
                if (m_lastSeekAction != null)
                {

                    if (t == m_lastSeekAction)
                    {
                        t.NumberOfUses = 0;
                        desired.Remove(t);
                    }
                }
                else
                {
                    output = t;
                    reroll = 2;
                }
                reroll++;
            }
        }
        // Same loop as above but with neutral evalations
        else if(neutral.Count > 0 && m_lastNeutralAction < Time.time)
        {
            while (reroll < 1)
            {
                int chosen = UnityEngine.Random.Range(0, neutral.Count);

                BossActions t = neutral[chosen];

                // Reset the last actions used
                if (m_lastSeekAction != null)
                {

                    if (t == m_lastSeekAction)
                    {
                        t.NumberOfUses = 0;
                        neutral.Remove(t);
                    }
                }
                else
                {
                    output = t;
                    m_lastNeutralAction = m_timeBetweenNeutral + Time.time;
                    reroll = 2;
                }
                reroll++;
            }
        }
        else
        {
            output = null;
        }

        // Used to stop moves occuring twice in a row.
        if(output != null)
        {
            output.NumberOfUses++;
            m_lastSeekAction = output;
        }
        else
        {
            m_lastSeekAction = null;
        }
        return output;
    }
    // Dodge away from the player.
    // Requries their direction
    //public BossActions EvaluateDefensiveAction(Vector3 m_targetDirection)
    //{
    //    BossActions output = null;
    //    foreach (BossActions t in m_bossActions)
    //    {
    //        if (t.GetBehaviourType == SteeringBehaviours.DEFENSIVE)
    //        {
    //            //          10 to  0                                        range 0 20      0 or neg * 5
    //            // player direction relative to the forward of the boss, coolness factor, timeused. 
    //            // Weighing happens here
    //            // If desirability is greater than previous replace.
    //            // bossaction
    //            // Only here to test
    //            output = t;
    //            break;
    //        }
    //        // Adjust the weighing values of the list
    //    }
    //    return output;
    //}

    // So these will recover full stamina when 1 action
    // occurs. A last ditch effort or a good way to slow the boss down
    // If applying pressure.
    //public BossActions EvaluatePassiveAction()
    //{
    //    BossActions output = null;
    //    foreach (BossActions t in m_bossActions)
    //    {
    //        if (t.GetBehaviourType == SteeringBehaviours.PASSIVE)
    //        {
                
    //            output = t;
    //            break;
    //        }
    //        // Adjust the weighing values of the list
    //    }
    //    return output;
    //}


}