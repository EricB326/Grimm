using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// This is currently used for sword collision.
// Seperated out from player in case there are 
// conflicts with using onCollider ect.
// It is mainly used to trigger particles and sounds
// for the environment

public class SwordCollision : MonoBehaviour
{
 

    // If it hits the boss it disables the hitbox and does damage.
    // If we want to pass information into the animator it would happen
    // here as well
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Boss" || other.tag != "Ground")
        {
            // OnHitEffects - Gets the information from whatever it made contact with to paly sounds and particles.
            EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_swordHitBox.enabled = false;
            // Need a sword sword contact with world.
            // Will have issues if the ground is not tagged.
        }
    }


}
