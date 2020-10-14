using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickActivation : MonoBehaviour
{

    public GameObject m_BossHealth;
    public GameObject m_BossCounters;

    // Update is called once per frame


    private void Start()
    {
        m_BossHealth.SetActive(false);
        m_BossCounters.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EntityStats.Instance.GetObjectOfEntity("Boss").gameObject.SetActive(true);
            EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().enabled = true;
            EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>().SetInteger("CutScene", 1);
            m_BossHealth.SetActive(true);
            m_BossCounters.SetActive(true);
        }
    }
}
