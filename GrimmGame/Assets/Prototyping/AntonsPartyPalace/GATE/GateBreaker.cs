using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBreaker : MonoBehaviour
{
    private bool m_broken = false;
    public Collider m_disabledCollider;





    // Set the boss to the look position on camera 
    // and target for player
    public void GateDestroyed()
    {
        GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_lockon = false;
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_target = boss;
        // Cam will look at boss when locked on
        //Camera.main.GetComponent<CameraRotation>().m_camList[1].LookAt = boss.GetComponent<BossVariables>().m_lookPoint;
    }


}
