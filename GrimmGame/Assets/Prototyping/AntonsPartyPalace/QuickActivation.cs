using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickActivation : MonoBehaviour
{
    public bool m_playedIntro = false;
    public GameObject m_cutscene;

    public GameObject m_BossHealth;
    public GameObject m_BossCounters;

    public GameObject m_playerHealth;
    public GameObject m_playerStamina;
    public GameObject m_playerLives;

    public GameObject m_bossMusic;

    public GameObject m_fogWall;

    public bool m_fogWallFade = false;

    private float m_fadeInRate = 0.001f;
    private float m_currentRate = 0.0f;

    // After cutscene reapply old x rotation.
    [HideInInspector]
    public float m_xRotation;

    private void Awake()
    {
        m_playedIntro = false;
        m_cutscene.SetActive(false);
        m_BossHealth.SetActive(false);
        m_BossCounters.SetActive(false);
        ResetWall();
    }


    private void Update()
    {

        if(m_fogWallFade)
        {
            float rate =  Mathf.Lerp(0, 1, m_currentRate);
            m_currentRate += m_fadeInRate;
            m_fogWall.GetComponent<MeshRenderer>().material.SetFloat("Alpha_Int", Mathf.Lerp(0, 1, rate));
            if(m_fogWall.GetComponent<MeshRenderer>().material.GetFloat("Alpha_Int") >= 1)
            {
                m_fogWallFade = false;
            }
        }  
    }


    public void ResetWall()
    {
        m_fogWall.GetComponent<MeshRenderer>().material.SetFloat("Alpha_Int", 0.0f);
        m_currentRate = 0;
        m_fogWall.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (m_playedIntro)

             //{ 
             //EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_disableControls = true;
             //EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Input/Z", 0);
             //EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Input/X", 0);

             //EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Movement/X", 0);
             //EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetFloat("Movement/Z", 0);
            
             //m_playedIntro = true;
             //m_cutscene.SetActive(true);
             //}
           
            {
                GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
                boss.SetActive(true);
                boss.GetComponent<BossBrain>().enabled = true;
                boss.GetComponent<Animator>().SetInteger("CutScene", 1);
                m_BossHealth.SetActive(true);
                m_BossCounters.SetActive(true);
                EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_target = boss;
                Camera.main.GetComponent<CameraRotation>().m_camList[1].LookAt = boss.GetComponent<BossVariables>().m_lookPoint;

                // Turn on fog gate.
                m_fogWall.SetActive(true);
                m_fogWallFade = true;
            }
        }
    }
}
