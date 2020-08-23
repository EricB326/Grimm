using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


// To be attached to the boss weapons.


public class AttackColliderBoss : MonoBehaviour
{
    public string m_tagToLookFor;
    public float m_hangTime = 0.5f;

    public float timescaleSlowDownOnIFrames = 0.2f;



    // I just wanted to try this out.
    IEnumerator SlowDown(Player player)
    {
        player.m_animator.speed = timescaleSlowDownOnIFrames;
        EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>().speed = timescaleSlowDownOnIFrames;
        yield return new WaitForSecondsRealtime(m_hangTime);

        for(; player.m_animator.speed < 1.0f;
            player.m_animator.speed +=0.1f)
        {
            EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>().speed += 0.1f;
            yield return new WaitForSecondsRealtime(m_hangTime / 8);
        }
    }


    private void OnTriggerEnter(Collider other)
    {


        int hitboxToDeactivate = 0;

        // Confirm it is an entity that can take damage
        if(other.tag == m_tagToLookFor)
        {
            // Confirm if in a touchable state
            Player player = other.GetComponent<Player>();
            // Need to check if player is not in Iframes. If so skip.
            if (!player.m_InvinceFrames)
            {
                // Get damage from attack list. Needs to be neatened up a fair bit.
                float damageToDo = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().m_actionQue[hitboxToDeactivate].AttackDamage;
                // Check if currently rolling. If so reduce damage by multiplyer.

                // If not do full damage.
                if (player.m_animator.GetBool("Output/IsRolling"))
                {
                    damageToDo *= player.m_rollDamagemultiplier;
                }
                EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<AnimationEventsBoss>().DeavtivateHitBox(hitboxToDeactivate);
                EntityStats.Instance.DeminishHealthOffEntity("Player", damageToDo);
                // Trigger on hit effects in the animation controller.
                EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().DecreaseRevengeValue();
            }
            else
            {
                EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<AnimationEventsBoss>().DeavtivateHitBox(hitboxToDeactivate);
                StartCoroutine(SlowDown(player));
            }
        }
    }

}
