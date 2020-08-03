using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class OnHitBoss : MonoBehaviour
{
    public VisualEffect m_blood;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AttackHitBox")
        {
            m_blood.transform.position = other.ClosestPointOnBounds(other.transform.position);
            m_blood.Play();
        }
    }


}

