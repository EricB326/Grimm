using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Storage of attack colliders.
// May store variables for attacks.
public class BossVariables : MonoBehaviour
{

    public List<Collider> m_weaponColliders;
    public Transform m_lookPoint;
    public GameObject m_facingPosition;

    public Collider m_hitbox;

    public List<SkinnedMeshRenderer> m_model;

    //public Collider m_sword;
    //public Collider m_claw;

    //public Collider m_leftFoot;
    //public Collider m_rightFoot;

    public QuickActivation m_activationScript;
    public GateActivater m_gate;
    private void Start()
    {
        //m_sword.enabled = false;
        //m_sword.enabled = false;
        //m_leftFoot.enabled = false;
        //m_rightFoot.enabled = false;

        foreach(Collider c in m_weaponColliders)
        {
            c.enabled = false;
        }
    }

    public void DisableBrain()
    {
        this.GetComponent<BossBrain>().enabled = false;
    }


}