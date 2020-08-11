using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// What specific behaviors will the boss have this phase
// Attacks revenge value and specific behaviors on a 
// per phase basis
// 



public class BossPhase : MonoBehaviour
{

    // Amount increased when taking damage or 
    // changing behaviors.
    public int m_revengeValueIncrease = 1;
    // Value decreases
    public int m_revengeValueDecrease = 5;
    // Once this is hit a counter attack happens.
    public int m_revengeValueThreshold = 10;
    // public List<BossAttacks> m_;
    public int m_delayBetweenDecisions;

}