using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsCutscene : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject m_cutScene;
    public QuickActivation m_quick;


    public void CompleteCutscene()
    {
        GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
        FadeScreen.instance.FadeIn();
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
    }

}
