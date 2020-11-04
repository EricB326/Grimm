using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Probs should directly be attached to the 
// tutorial script
public class GateBreaker : MonoBehaviour
{
    private bool m_broken = false;
    public Collider m_disabledCollider;

    // Set the boss to the look position on camera 
    // and target for player
    public void GateDestroyed()
    {
        GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_lockon = false;
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_target = boss;
        // Cam will look at boss when locked on
        //Camera.main.GetComponent<CameraRotation>().m_camList[1].LookAt = boss.GetComponent<BossVariables>().m_lookPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword")
        { 
            Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
            Animator animator = this.GetComponent<Animator>();
            if (player.m_attackStats == 2)
            {
                if (animator.GetBool("CanBreak"))
                {
                    animator.SetTrigger("Break");
                    m_disabledCollider.enabled = false;
                    animator.SetBool("Broken", true);
                }
                else
                {
                    animator.SetTrigger("Shake");
                }
            }
            else
            {
                animator.SetTrigger("Shake");
            }
        }

    }



}
