using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


// To be attached to the boss weapons.


public class Weapon : MonoBehaviour
{
    public string m_tagToLookFor;

    private void OnTriggerEnter(Collider other)
    {
        // Confirm it is an entity that can take damage
        if(other.tag == m_tagToLookFor)
        {
            // Confirm if in a touchable state
            GameObject player = other.gameObject; // So we can do damage
            Animator playerAnimator = player.GetComponent<Animator>(); // So we can animate result
            // Need to check if player is not in Iframes. If so skip.

            // If not need do damage. Check if currently rolling. If so reduce damage by multiplyer
            // If not full damage.
            float damageToDo = 10; // Ten for test purposes. Should actually check the current attack for values.
            if(playerAnimator.GetBool("Output/IsRolling"))
            {
                damageToDo  *= player.GetComponent<PlayerMovementVariables>().m_rollDamagemultiplier;
            }
            EntityStats.Instance.DeminishHealthOffEntity("Player", damageToDo);
            // Send damage animation to animator.
        }
    }

}
