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
    public void EvaluateAtack()
    {
        // Needs to go through attacks in phase and evaluate their suitability
        // Range needed to move like a bell curve around the target.
    }
    public void EvaluateSeek()
    {
        // Needs to go through attacks in phase and evaluate their suitability
        // Range needed to move like a bell curve around the target.
    }
    public void EvaluateDodge()
    {

    }

}