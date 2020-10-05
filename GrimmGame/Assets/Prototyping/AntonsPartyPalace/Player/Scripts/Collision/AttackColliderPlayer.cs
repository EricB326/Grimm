﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


// For the player weapon.
// Once target is hit will disable itself unitl next swing.
// Does damage to boss here.

public class AttackColliderPlayer : MonoBehaviour
{
    private Collider m_collider;
    public VisualEffect m_visualeffect;
    public float m_particleOffset;

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
            Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();

            m_visualeffect.transform.LookAt(player.transform.position);
            Vector3 newPos = player.transform.position - other.transform.position;
            m_visualeffect.transform.position = other.transform.position + newPos.normalized + new Vector3(0, m_particleOffset, 0);
            m_visualeffect.Play();
            EntityStats.Instance.DeminishHealthOffEntity("Boss", player.m_attacksDamage[player.m_attackStats]);
            EntityStats.Instance.GetObjectOfEntity("Player").GetComponentInChildren<AnimationEventsPlayer>().DeavtivateHitBox();

            // Should be inside an on hit event.
            //EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().IncreaseRevengeValue(); // REVENGE VALUE REMOVED LOGIC DEFCUNT
            // Need to trigger visual effects on boss taking damage.
            // Should have a OnHitEffects with a list of things to do.
        }
    }
}
