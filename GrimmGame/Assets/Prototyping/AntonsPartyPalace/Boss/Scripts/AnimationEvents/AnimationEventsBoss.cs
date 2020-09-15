using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

// Boss Colliders
// 1. Sword
// 2 Claw left hand
// 3 Foot Right
// 4 Foot Left


// Animation events for the boss to call during animations

public class AnimationEventsBoss : MonoBehaviour
{
    // Don't forget you can store data in this class
    // for on offs and safetys.
    // For calls to the camera
    CameraRotation m_cameraList;

    


    private void Start()
    {
        m_cameraList = Camera.main.GetComponent<CameraRotation>();
    }



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
    
    // Istantiates the particle effect at the point?
    // Probably better to just play a particle on a collider
    // Which will include sounds?
    public void FootDownLeft()
    {
        // Play particle and sound effect on foot.
        this.gameObject.GetComponent<BossVariables>().m_weaponColliders[3].gameObject.GetComponent<VisualEffect>().Play();
    }


    public void FootDownRight()
    {
        // Play particle and sound effect on foot.
        this.gameObject.GetComponent<BossVariables>().m_weaponColliders[2].gameObject.GetComponent<VisualEffect>().Play();
    }


    // Need to do.
    public void ShakeCameraOn(float a_value)
    {
        // Needs to call camera.
        // Function should call something in the camera
        // so the camera itself is in charge of applying 
        // and removing effects.
        //m_cameraList.m
    }

    public void ShakeCameraOff()
    {

    }

    public void ResetPosition()
    {
        this.transform.position = this.GetComponent<BossBrain>().m_startPos;
    }


    public void EnableModel()
    {
        EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_swordModel.enabled = true;
        EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_model.enabled = true;
    }

    public void DisableModel()
    {
        EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_swordModel.enabled = false;
        EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_model.enabled = false;
    }

}
