using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boss Colliders
// 1. Sword
// 2 Claw left hand
// 3 Foot Right
// 4 Foot Left


// Animation events for the boss to call on attack.

public class AnimationEventsBoss : MonoBehaviour
{
    [Header("Particle List")]
    public List<ParticleSystem> m_particles;





    public void ActivateHitBox(int a_colliderNumber)
    {
       GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
       BossBrain brain = boss.GetComponent<BossBrain>();

        // Need to get correct colider from attack stored within boss.
        // int colliderToModify = (int)brain.m_currentAttackVariables.m_colliderToActivate[brain.m_currentAttackVariables.m_attackInChain];

        boss.GetComponent<BossVariables>().m_weaponColliders[a_colliderNumber].enabled = true;
    }

    public void DeavtivateHitBox(int a_colliderNumber)
    {
        GameObject boss = EntityStats.Instance.GetObjectOfEntity("Boss");
        BossBrain brain = boss.GetComponent<BossBrain>();
      
        boss.GetComponent<BossVariables>().m_weaponColliders[a_colliderNumber].enabled = false;
    }

    // Boss will rotate at speed passed in until RotateOff.
    public void RotateOn(float a_rotationSpeed)
    {
        this.GetComponent<BossBrain>().SetActionRotationSpeed(a_rotationSpeed);
        this.gameObject.GetComponent<Animator>().SetBool("Ai/CanRotate", true);
    }

    // Stops action rotation and sets value to 0.
    public void RotateOff()
    {
        this.GetComponent<BossBrain>().SetActionRotationSpeed(0);
        this.gameObject.GetComponent<Animator>().SetBool("Ai/CanRotate", false);
    }
    
    // Istantiates the particle effect at the point
    public void TriggerDustParticle(int a_colliderPos)
    {
        m_particles[0].Play();
    }

    // Destroys the particle?
    public void TriggerParticleEffectOff(int a_colliderNumber)
    {
        // 
    }



}
