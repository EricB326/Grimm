using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// The base class.
// Just a bunch of virtual functions.
[Serializable]
public class BehvaiorBase
{
    // Is this neccesary?
    // Maybe it would be better to have a list of 
    // agents with 'teams' and have it select 1
    // of those instead.
    private GameObject m_boss;
    private GameObject m_player;

    // Do we need to cast the behavior into its type?
    public BossBehavior m_behavior;
  
    // Called on level load. With a proper tree this is redundant.
    public void Setup(GameObject a_boss, GameObject a_player)
    {
        m_boss = a_boss;
        m_player = a_player;
    }

    // Whad does the behavior do. Update loop?
    public virtual void Behavior()
    {

    }

    // If behavior satisfied true.
    public virtual bool CheckCompleted()
    {
        return true;
    }

}
