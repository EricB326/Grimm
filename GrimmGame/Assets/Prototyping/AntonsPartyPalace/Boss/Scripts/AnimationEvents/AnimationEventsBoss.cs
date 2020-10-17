using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;


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

    public VisualEffect m_leftFoot;
    public VisualEffect m_rightFoot;


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

    public void DeactivateHitBox(int a_colliderNumber)
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
        // Play particle and sound effect on foot. NEEDS TO BE CHANGED TO A VFX GRAPH
        //m_leftFoot.Play();
    }

    public void FootDownRight()
    {
        // Play particle and sound effect on foot. NEEDS TO BE CHANGED TO A VFX GRAPH
        //m_rightFoot.Play();
    }

    public void HandDownLeft()
    {

    }

    public void HandDownRight()
    {

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
        List<SkinnedMeshRenderer> model = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_model;
        foreach (SkinnedMeshRenderer t in model)
        {
            t.enabled = true;
        }
    }

    public void DisableModel()
    {
        List<SkinnedMeshRenderer> model = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossVariables>().m_model;

        foreach (SkinnedMeshRenderer t in model)
        {
            t.enabled = false;
        }
    }

    public void ResetAnimator()
    {
        Animator animator = EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>();

        animator.SetFloat("Movement/X", 0);
        animator.SetFloat("Movement/Z", 0);
    }


    public IEnumerator ChangeScene()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(0);
    }

    public void DisableCollider()
    {
        this.GetComponent<BossVariables>().m_hitbox.enabled = false;
    }

    public void DisableLockon()
    {
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_lockon = false;
    }

    public void BossDeath()
    {
        DisableLockon();
        this.gameObject.GetComponent<BossVariables>().m_dead = true;
    }
}
