using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockOn : MonoBehaviour
{
    public GameObject m_bossLockOnTarget;
    public GameObject m_center;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            this.gameObject.GetComponent<CinemachineVirtualCamera>().LookAt = m_center.transform;
            Debug.Log("Looking at Player Center");
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            this.gameObject.GetComponent<CinemachineVirtualCamera>().LookAt = m_bossLockOnTarget.transform;
            Debug.Log("Looking at Boss");
        }
    }


    
}
