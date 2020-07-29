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
        //EntityStats.Instance. ("Player",/* EntityStats.Instance*/);
        Debug.Log("Activated hitbox");
    }
    // Disables hitboxes on player model to no longer cause damage
    public void DeavtivateHitBox(Collider hitbox)
    {


        Debug.Log("Activated hitbox ");
    }
}


