using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class AnimationEventsCutscene : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject m_cutScene;
    public QuickActivation m_quick;


    private void Update()
    {
        if(XCI.GetButtonDown(XboxButton.Start))
        {
            CompleteCutscene();
        }
    }


    public void CompleteCutscene()
    {
        // Enable player ui
        m_quick.m_playerHealth.SetActive(true);
        m_quick.m_playerStamina.SetActive(true);
        m_quick.m_playerLives.SetActive(true);

        GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
        //FadeScreen.instance.FadeIn();
        boss.SetActive(true);
        boss.GetComponent<BossBrain>().enabled = true;
        m_quick.m_BossHealth.SetActive(true);
        m_quick.m_BossCounters.SetActive(true);
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_target = boss;
        Camera.main.GetComponent<CameraRotation>().m_camList[1].LookAt = boss.GetComponent<BossVariables>().m_lookPoint;
        boss.GetComponent<AnimationEventsBoss>().EnableModel();
        m_quick.m_fogWall.SetActive(true);
        m_quick.m_fogWallFade = true;
        m_cutScene.SetActive(false);

        Camera.main.GetComponent<CameraRotation>().m_camList[0].m_XAxis.Value = m_quick.m_xRotation;
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_disableControls = false;
    }
}