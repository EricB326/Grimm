using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// This is currently used for sword collision.
// Seperated out from player in case there are 
// conflicts with using onCollider ect.
// It is mainly used to trigger particles and sounds
// and will most likely be cut.

// PLEASE MODIFY ATTACKCOLLIDERPLAYER FOR GAMEPLAY AND ONHIT EFFECTS.

public class SwordCollision : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Boss" || other.tag != "Ground")
        {
            // OnHitEffects - Gets the information from whatever it made contact with to paly sounds and particles.
            EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().GetSwordHitBox().enabled = false;
            // Need a sword contact with world.
            // Will have issues if the ground is not tagged.
        }
    }
}
