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
            if (!m_activation.m_playedIntro)
            {
                // Disable player ui
                m_activation.m_playerHealth.SetActive(false);
                m_activation.m_playerLives.SetActive(false);
                m_activation.m_playerStamina.SetActive(false);

                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_disableControls = true;
                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Input/Z", 0);
                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Input/X", 0);

                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Movement/X", 0);
                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Movement/Z", 0);

                // Get the x rotation to reapply later.
                m_activation.m_xRotation = Camera.main.GetComponent<CameraRotation>().m_camList[0].m_XAxis.Value;

                m_activation.m_playedIntro = true;
                m_activation.m_cutscene.SetActive(true);
            }
            else
            {
                m_activation.m_bossMusic.SetActive(true);
                m_activation.m_bossMusic.GetComponent<BossMusicScript>().bossMusicEmitter.SetParameter("boss phase", m_activation.m_bossMusic.GetComponent<BossMusicScript>().bossPhase);
            }
            m_colliderToEnable.isTrigger = false;
        }
    }
}
