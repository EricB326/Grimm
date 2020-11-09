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

    //public List<SkinnedMeshRenderer> m_model;
    public List<GameObject> m_model;
    //public Collider m_sword;
    //public Collider m_claw;

    public GameObject m_leftFootParticle;
    public GameObject m_rightFootParticle;
    public GameObject m_leftHandParticle;


    [Tooltip("1. Sword 2. Claw")]
    public List<GameObject> m_trails;
 
    public QuickActivation m_activationScript;
    public GateActivater m_gate;

    public bool m_dead = false;

    // Reference to turn off cloth colliders.
    public Cloth m_cloth;





    private void Start()
    {
        //m_sword.enabled = false;
        //m_sword.enabled = false;
        //m_leftFoot.enabled = false;
        //m_rightFoot.enabled = false;

        foreach (GameObject t in m_trails)
        {
            t.SetActive(false);
        }

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