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
    public int m_revengeValueIncrease;
    // Value decreases
    public int m_revengeValueDecrease;
    // Once this is hit a counter attack happens.
    public int m_revengeValueThreshold;
    // public List<BossAttacks> m_;
    public int m_delayBetweenDecisions;
    // The list of phase attacks?
    public List<BossAttacks> m_phaseAttacks;




    public void EvaluateAtack()
    {
        // Needs to go through attacks in phase and evaluate their suitability
        // Range needed to move like a bell curve around the target.
        //
        
    }


}