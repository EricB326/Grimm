using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;



// Use this 


public class ColorSwap : MonoBehaviour
{

    public Material m_hit;
    public Material m_noHit;

    private MeshRenderer m_material;

    public float m_delay;
    private float m_timeHit;
    public VisualEffect m_visual;

    private void Start()
    {
        m_material = this.GetComponent<MeshRenderer>();
        m_visual.Stop();

    }

    private void Update()
    {
        if(Time.time > m_timeHit)
        {
            m_material.material = m_noHit;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackHitBox")
        {
            Debug.Log("Hit");
            m_material.material = m_hit;
            m_timeHit = Time.time + m_delay;
            EntityStats.Instance.DeminishHealthOffEntity("Boss", EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<PlayerMovementVariables>().m_attackDamage);
            m_visual.transform.position = other.transform.position;
            m_visual.Play();
        }
    }
}
