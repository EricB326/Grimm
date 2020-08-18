using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Centralized place for player animation events.
// If something take place on an animation timeline, it'll take place here.
// May be some crossover with collision

public class AnimationEventsPlayer : MonoBehaviour
{
   
    // Activates hitboxes on player model to cause damage
    public void ActivateHitBox()
    {
        //Debug.Log(a_colliderNumber);
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        player.GetAttackHitBox().enabled = true;
        player.GetSwordHitBox().enabled = true;
        EntityStats.Instance.DeminishStaminaOffEntity("Player", player.m_attackStaminaDrain);
    }
    // Disables hitboxes on player model to no longer cause damage
    // Occurs when hitting boss or swing near end.
    public void DeavtivateHitBox()
    {
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        player.GetAttackHitBox().enabled = false;
        player.GetSwordHitBox().enabled = false;
    }

    // Bool to true and stamina drain
    // May have visual/sound issues depending how they are handled.
    public void IFramesOn()
    {
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        if (player.m_InvinceFrames == false)
        {
            player.m_InvinceFrames = true;
            EntityStats.Instance.DeminishStaminaOffEntity("Player", player.m_rollStaminaDrain);
            // And visual effects.
        }
    }

    // Bool to false.
    // May have visual/sound issues depending how they are handled.
    public void IFramesOff()
    {
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_InvinceFrames = false;
    }


    // Deactivates every animation event
    // when leaving animation states.
    // A nice safety if off events aren't reached.
    public void Deactivate()
    {
        
        DeavtivateHitBox();
        IFramesOff();
    }

}


