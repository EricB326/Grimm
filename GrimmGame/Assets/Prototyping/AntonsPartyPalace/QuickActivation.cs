using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickActivation : MonoBehaviour
{

    public GameObject m_BossHealth;
    public GameObject m_BossCounters;
    public GameObject m_fogWall;

    public bool m_fogWallFade = false;

    private float m_fadeInRate = 0.001f;
    private float m_currentRate = 0.0f;
    // Update is called once per frame


    private void Awake()
    {

        m_BossHealth.SetActive(false);
        m_BossCounters.SetActive(false);
        ResetWall();
    }


    private void Update()
    {

        if(m_fogWallFade)
        {
            float rate =  Mathf.Lerp(0, 1, m_currentRate);
            Debug.Log(rate);
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
        m_fogWall.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
            boss.SetActive(true);
            boss.GetComponent<BossBrain>().enabled = true;
            boss.GetComponent<Animator>().SetInteger("CutScene", 1);
            m_BossHealth.SetActive(true);
            m_BossCounters.SetActive(true);
            EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_target = boss;
            Camera.main.GetComponent<CameraRotation>().m_camList[1].LookAt = boss.GetComponent<BossVariables>().m_lookPoint;

            // Triggers Kaz cutscene

            // Turn on fog gate.
            m_fogWall.SetActive(true);
            m_fogWallFade = true;
        }
    }
}
