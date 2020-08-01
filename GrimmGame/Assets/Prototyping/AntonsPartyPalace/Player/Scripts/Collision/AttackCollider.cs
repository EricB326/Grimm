using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Quick script for 

public class AttackCollider : MonoBehaviour
{

    private Collider m_collider;

    void Start()
    {
        m_collider = this.GetComponent<Collider>();
        m_collider.isTrigger = true;
        m_collider.enabled = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("Hit");
        }
    }
}
