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
        Debug.Log("Activated hitbox");
    }
    // Disables hitboxes on player model to no longer cause damage
    // Occurs when hitting boss or swing near end.
    public void DeavtivateHitBox(int a_colliderNumber)
    {
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_attackHitBox.enabled = false;
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_swordHitBox.enabled = false;
        Debug.Log("Deactivated hitbox");
    }
}


