using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// For the player weapon.
// Once target is hit will disable itself unitl next swing.
// Does damage to boss here.

public class AttackCollider : MonoBehaviour
{
    private Collider m_collider;


    // Disables trigger box at start.
    void Start()
    {
        m_collider = this.GetComponent<Collider>();
        m_collider.isTrigger = true;
        m_collider.enabled = false;
    }

    // When trigger box collides with boss do damage.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boss")
        {
            Debug.Log("Hit Boss");
            EntityStats.Instance.DeminishHealthOffEntity("Boss", EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_attackDamage);
            EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<AnimationEventsPlayer>().DeavtivateHitBox(0);
            // Need to trigger visual effects on boss taking damage.
            // Should have a OnHitEffects with a list of things to do.
        }
    }
}
