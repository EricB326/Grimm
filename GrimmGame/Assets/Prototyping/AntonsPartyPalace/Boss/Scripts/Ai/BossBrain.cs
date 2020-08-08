using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// A quick script to house some basic
// variables and decisions the ai can make.

public class BossBrain : MonoBehaviour
{
    // The range to reach before attack begins.
    public float m_desiredRange;
    // The damage the current attack will do.
    // Passed out of the animator and here for debugging.
    public float m_attackDamage;
    // What the current phase of the boss is.
    // Either this lives in the animator or out here.
    public int m_currentPhase;
    // Stamina or revenge value.
    // Depending on how we want to the game to go this 
    // will either be a value that once it reaches
    // a threshold will launch a quick coutner attack
    // or a value that the boss uses to work out what
    // attack he is able to use next.
    public float m_revengeValue;
    // 
}
