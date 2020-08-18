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
    IEnumerator SlowDown(Player playermovementVar)
    {
        playermovementVar.gameObject.GetComponent<Animator>().speed = timescaleSlowDownOnIFrames;
        EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>().speed = timescaleSlowDownOnIFrames;
        yield return new WaitForSecondsRealtime(m_hangTime);

        for(; playermovementVar.gameObject.GetComponent<Animator>().speed < 1.0f; 
            playermovementVar.gameObject.GetComponent<Animator>().speed +=0.1f)
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
            GameObject player = other.gameObject; // So we can do damage
            Animator playerAnimator = player.GetComponentInChildren<Animator>(); // So we can animate result
            Player playermovementVar = player.GetComponent<Player>();
            // Need to check if player is not in Iframes. If so skip.
            if (!playermovementVar.m_InvinceFrames)
            {
                // Get damage from attack list. Needs to be neatened up a fair bit.
                float damageToDo = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().m_actionQue[hitboxToDeactivate].AttackDamage;
                // Check if currently rolling. If so reduce damage by multiplyer.

                // If not do full damage.
                if (playerAnimator.GetBool("Output/IsRolling"))
                {
                    damageToDo *= playermovementVar.m_rollDamagemultiplier;
                }
                EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<AnimationEventsBoss>().DeavtivateHitBox(hitboxToDeactivate);
                EntityStats.Instance.DeminishHealthOffEntity("Player", damageToDo);
                // Trigger on hit effects in the animation controller.
                EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().DecreaseRevengeValue();
            }
            else
            {
                EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<AnimationEventsBoss>().DeavtivateHitBox(hitboxToDeactivate);
                StartCoroutine(SlowDown(playermovementVar));
            }
        }
    }

}
