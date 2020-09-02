using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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


    [Header("REVENGE VALUE")]
    public int m_increase;
    // Value decreases
    public int m_decrease;
    // Once this is hit a counter attack happens.
    public int m_threshold;
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
    public float m_timeBetweenPreDefinedAndRandom;
    public List<int> m_preDefinedActions;
    public List<SteeringBehaviours> m_counterMoves;
    [HideInInspector]
    public float m_storedTime;

    public float m_staminaThisPhase;
    // Used to asses if should attack, defend or 
    [SerializeField]
    [Range(0, 100)]
    private float m_aggresiveStaminaThreshold = 75.0f;
    [SerializeField]
    [Range(0,100)]
    private float m_defensiveStaminaThreshold = 50.0f;
    // For modifying next evaluation
    private EvaluatedAction m_lastAttackAction = null;
    private EvaluatedAction m_lastDodgeAction = null;





    // Stores the evaluated actions and the actions themselves.
    public class EvaluatedAction
    {
        // A reference to the aciton itself.
        // Passed out on success.
        public BossActions m_action;
        // The evaluated score.
        public float m_score;
    }


    // Boss checks to see what it's stamina is at.
    public BossActions EvaluateSelf(float a_distanceToTarget, Vector3 m_targetDirection)
    {
       // Might need to be removed
       // Boss should only need to act agressivly

        // Did i do a dodge last action?
        // if yes choose attack and seek towards player
        // if no choose either an attack or dodge.


        //float currentStam = EntityStats.Instance.GetStaminaOfEntity("Boss");
        //if (currentStam > m_aggresiveStaminaThreshold)
        //{
            return EvaluateAggresiveAction(a_distanceToTarget);
        //}
        //else if(currentStam > m_defensiveStaminaThreshold)
        //{   
        //    return EvaluateDefensiveAction(m_targetDirection);
        //}
        //else
        //{
        //    return EvaluatePassiveAction();
        //}
        
    }

    // Before attack needs to evaluate if it should evaluate 
    // whether it should do an action that would result in no change
    // and launch the attack straight after.

    // Needs to know if the attack is launchable.
    // Attacks that need to be launched from a larger 
    // distance will be weighted
    public BossActions EvaluateAggresiveAction(float a_distanceToTarget)
    {
        BossActions output = null;
        foreach (BossActions t in m_bossActions)
        {
            if (t.GetBehaviourType == SteeringBehaviours.AGGRESSIVE)
            {       // from 10 to 0            Range from 0-20             0 or neg * 5  +  roll 2 random 0 - 6
                // Distance to player, damage, coolness factor(set by designer) timeused
                
                // more ways to be effect weighing.

                // Weighing happens here
                // If desirability is greater than previous replace.
                // bossaction
                // Only here to test
                output = t;
                break;
            }
            // Adjust the weighing values of the list
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
        // Evaluation of each move - 1 to 1.
        List<EvaluatedAction> m_evaluatedList = new List<EvaluatedAction>();

        foreach (BossActions t in m_bossActions)
        {
            // Behaviors are seeks therefore we should be checking
            // if they put the boss in a good position.
            if (t.GetBehaviourType == SteeringBehaviours.SEEK)
            {
                // If the distance covered is beneficial or negligalble
                // does the attack move close or further?
                float distanceCovered = t.GetDestinationDistance;



                // Now knowing how far the boss moves
                // relative to the player if it moves them out of
                // range don't do it otherwise do it

                // If it is last used.
                if (m_lastDodgeAction.m_action == t)
                {
                    
                }
            
                // from 10 to 0       Range from 020    0 or neg * 5
                // optimal distance, coolness factor, timeused
                // Weighing happens here
                // If desirability is greater than previous replace.
                // bossaction
                // Only here to test
                output = t;
                break;
            }
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