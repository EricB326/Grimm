using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float m_wanderTime;

    public List<BossActions> m_bossActions;

    // If so desired you can choose a bunch of action 
    // from the above list and gaurentee them happening
    // in the order they are in.
    // Ideally you need a seek followed by an attack
    // Great for testing.
    public float m_timeBetweenPreDefinedAndRandom;
    public List<int> m_preDefinedActions;
    public float m_storedTime;

    // Needs to know if the attack is launchable.
    // Attacks that need to be launched from a larger 
    // distance will be weighted
    public BossActions EvaluateAtack(float a_distanceToPlayer)
    {
        BossActions output = null;
        foreach (BossActions t in m_bossActions)
        {
            if (t.GetBehaviourType == SteeringBehaviours.ATTACK_BEHAVIOUR)
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

    // Seeks are evaluated on. 
    public BossActions EvaluateSeek(float a_distanceToPlayer, float m_desiredRange)
    {
        BossActions output = null;
        foreach (BossActions t in m_bossActions)
        {
            if(t.GetBehaviourType == SteeringBehaviours.SEEK_BEHAVIOUR)
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
    public BossActions EvaluateDodge(Vector3 m_playerDirection)
    {
        BossActions output = null;
        foreach (BossActions t in m_bossActions)
        {
            if (t.GetBehaviourType == SteeringBehaviours.DODGE_BEHAVIOUR)
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

}