using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Animation events for the boss to call on attack.

public class AnimationEventsBoss : MonoBehaviour
{
    public void ActivateHitBox(int a_colliderNumber)
    {
       GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
       BossBrain brain = boss.GetComponent<BossBrain>();

        // Need to get correct colider from attack stored within boss.
        // int colliderToModify = (int)brain.m_currentAttackVariables.m_colliderToActivate[brain.m_currentAttackVariables.m_attackInChain];

        boss.GetComponent<BossVariables>().m_weaponColliders[a_colliderNumber].enabled = true;
        //boss.GetComponent<BossVariables>().m_weaponColliders[colliderToModify].enabled = true;
    }

    public void DeavtivateHitBox(int a_colliderNumber)
    {
        GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
        BossBrain brain = boss.GetComponent<BossBrain>();
      
        // Need to get correct colider from attack stored within boss.
        // int colliderToModify = (int)brain.m_currentAttackVariables.m_colliderToActivate[brain.m_currentAttackVariables.m_attackInChain];

        boss.GetComponent<BossVariables>().m_weaponColliders[a_colliderNumber].enabled = false;
        //boss.GetComponent<BossVariables>().m_weaponColliders[colliderToModify].enabled = false;
        //boss.GetComponent<BossBrain>().m_currentAttackVariables.m_attackInChain++;
    }
}
