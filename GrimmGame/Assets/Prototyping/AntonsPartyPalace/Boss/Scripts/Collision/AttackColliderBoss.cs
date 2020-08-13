using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


// To be attached to the boss weapons.


public class AttackColliderBoss : MonoBehaviour
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
            PlayerMovementVariables playermovementVar = player.GetComponent<PlayerMovementVariables>();
            // Need to check if player is not in Iframes. If so skip.
            if (!playermovementVar.m_InvinceFrames)
            {
                // Get damage from attack list. Needs to be neatened up a fair bit.
                float damageToDo = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().m_actionQue[0].AttackDamage;
                // Check if currently rolling. If so reduce damage by multiplyer.

                // If not do full damage.
                if (playerAnimator.GetBool("Output/IsRolling"))
                {
                    damageToDo *= playermovementVar.m_rollDamagemultiplier;
                }
                Debug.Log(damageToDo);
                EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<AnimationEventsBoss>().DeavtivateHitBox(0);
                EntityStats.Instance.DeminishHealthOffEntity("Player", damageToDo);
                // Trigger on hit effects in the animation controller.
                EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().DecreaseRevengeValue();
            }
        }
    }

}
