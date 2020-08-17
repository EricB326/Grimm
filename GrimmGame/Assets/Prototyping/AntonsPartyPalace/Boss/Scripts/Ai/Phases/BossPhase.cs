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
    // How long boss wanders.
    // Should be change
    public float m_wanderTime;

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



    // Boss checks to see what it's stamina is at.
    public BossActions EvaluateSelf(float a_distanceToTarget, Vector3 m_targetDirection)
    {
        float currentStam = EntityStats.Instance.GetStaminaOfEntity("Boss");
        if (currentStam > m_aggresiveStaminaThreshold)
        {
            return EvaluateAggresiveAction(a_distanceToTarget);
        }
        else if(currentStam > m_defensiveStaminaThreshold)
        {
            return EvaluateDefensiveAction(m_targetDirection);
        }
        else
        {
            return EvaluatePassiveAction();
        }
    }



    // Needs to know if the attack is launchable.
    // Attacks that need to be launched from a larger 
    // distance will be weighted
    public BossActions EvaluateAggresiveAction(float a_distanceToTarget)
    {
        BossActions output = null;
        foreach (BossActions t in m_bossActions)
        {
            if (t.GetBehaviourType == SteeringBehaviours.AGGRESSIVE)
            {       // from 10 to 0     Dam val     Range from 020             0 or neg * 5
                // Distance to player, damage, coolness factor(set by designer) timeused

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
    // Think point to point movement.
    public BossActions EvaluateSeek(float a_distanceToTarget)
    {
        BossActions output = null;
        foreach (BossActions t in m_bossActions)
        {
            if (t.GetBehaviourType == SteeringBehaviours.SEEK)
            {
                // from 10 to 0       Range from 020    0 or neg * 5
                // optimal distance, coolness factor, timeused
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
    // Dodge away from the player.
    // Requries their direction
    public BossActions EvaluateDefensiveAction(Vector3 m_targetDirection)
    {
        BossActions output = null;
        foreach (BossActions t in m_bossActions)
        {
            if (t.GetBehaviourType == SteeringBehaviours.DEFENSIVE)
            {
                //          10 to  0                                        range 0 20      0 or neg * 5
                // player direction relative to the forward of the boss, coolness factor, timeused. 
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

    // So these will recover full stamina when 1 action
    // occurs. A last ditch effort or a good way to slow the boss down
    // If applying pressure.
    public BossActions EvaluatePassiveAction()
    {
        BossActions output = null;
        foreach (BossActions t in m_bossActions)
        {
            if (t.GetBehaviourType == SteeringBehaviours.PASSIVE)
            {
                
                output = t;
                break;
            }
            // Adjust the weighing values of the list
        }
        return output;
    }


}