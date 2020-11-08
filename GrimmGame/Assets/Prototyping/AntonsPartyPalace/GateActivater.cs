using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateActivater : MonoBehaviour
{

    public QuickActivation m_activation;
    public Collider m_colliderToEnable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(!m_activation.m_playedIntro)
            {
                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_disableControls = true;
                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Input/Z", 0);
                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Input/X", 0);

                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Movement/X", 0);
                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Movement/Z", 0);

                m_activation.m_playedIntro = true;
                m_activation.m_cutscene.SetActive(true);
            }
            m_colliderToEnable.isTrigger = false;
        }
    }
}
