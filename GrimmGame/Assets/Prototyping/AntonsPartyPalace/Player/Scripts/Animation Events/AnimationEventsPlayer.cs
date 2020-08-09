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
    public void ActivateHitBox(int a_colliderNumber)
    {
        //Debug.Log(a_colliderNumber);
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_attackHitBox.enabled = true;
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_swordHitBox.enabled = true;
        EntityStats.Instance.DeminishStaminaOffEntity("Player", GetComponent<PlayerMovementVariables>().m_attackStaminaDrain);
    }
    // Disables hitboxes on player model to no longer cause damage
    // Occurs when hitting boss or swing near end.
    public void DeavtivateHitBox(int a_colliderNumber)
    {
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_attackHitBox.enabled = false;
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_swordHitBox.enabled = false;
    }

    // Bool to true and stamina drain
    // May have visual/sound issues depending how they are handled.
    public void IFramesOn()
    {
        if (EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_InvinceFrames == false)
        {
            EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_InvinceFrames = true;
            EntityStats.Instance.DeminishStaminaOffEntity("Player", GetComponent<PlayerMovementVariables>().m_rollStaminaDrain);
            // And visual effects.
        }
    }

    // Bool to false.
    // May have visual/sound issues depending how they are handled.
    public void IFramesOff()
    {
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_InvinceFrames = false;
    }

}


