using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwap : MonoBehaviour
{

    public Material m_hit;
    public Material m_noHit;

    private MeshRenderer m_material;

    public float m_delay;
    private float m_timeHit;


    private void Start()
    {
        m_material = this.GetComponent<MeshRenderer>();
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
        if (other.tag == "Sword")
        {
            m_material.material = m_hit;
            m_timeHit = Time.time + m_delay;
        }
    }
}
