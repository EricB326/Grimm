using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateActivater : MonoBehaviour
{
    public Collider m_colliderToEnable; 


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_colliderToEnable.isTrigger = false;
        }
    }

}
