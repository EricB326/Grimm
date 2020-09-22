using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

// May be old and defunct
public class OnHitBoss : MonoBehaviour
{
    // Needs to be changed to a vfx
    //public VisualEffect m_blood;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AttackHitBox")
        {
            //m_blood.transform.position = other.ClosestPointOnBounds(other.transform.position);
            //m_blood.Play();
        }
    }
}

