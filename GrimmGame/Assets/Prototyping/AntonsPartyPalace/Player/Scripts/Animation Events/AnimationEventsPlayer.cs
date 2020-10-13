using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Centralized place for player animation events.
// If something take place on an animation timeline, it'll take place here.
// May be some crossover with collision

public class AnimationEventsPlayer : MonoBehaviour
{
    // Hitbox activate requires a number
    // The number will signify what kind of attack 
    // it is.
    // 0 light attack drain and damage.
    // 1 heavy attack drain and damage.
    // 2 roll attack drain and damage
    // 3 run attack drain and damage

    // Activates hitboxes on player model to cause damage
    public void ActivateHitBox(int damageAndStaminaID)
    {
        //Debug.Log(a_colliderNumber);  
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        player.GetAttackHitBox().enabled = true;
        player.GetSwordHitBox().enabled = true;

        player.m_attackStats = damageAndStaminaID;
        EntityStats.Instance.DeminishStaminaOffEntity("Player", player.m_attackStamina[damageAndStaminaID]);
    }

    // Activates hitboxes on player model to cause damage
    // The variant for heavy attack - Should really just pass in a number.
    //public void ActivateHitBoxHeavy()
    //{
    //    //Debug.Log(a_colliderNumber);  
    //    Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
    //    player.GetAttackHitBox().enabled = true;
    //    player.GetSwordHitBox().enabled = true;
    //    EntityStats.Instance.DeminishStaminaOffEntity("Player", player.m_heavyStaminaDrain);
    //}


    // Disables hitboxes on player model to no longer cause damage
    // Occurs when hitting boss or swing near end.
    public void DeavtivateHitBox()
    {
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        player.GetAttackHitBox().enabled = false;
        player.GetSwordHitBox().enabled = false;
    }

    // Bool to true and stamina drain
    // May have visual/sound issues depending how they are handled.
    public void IFramesOnRoll()
    {
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        if (player.m_InvinceFrames == false)
        {
            player.m_InvinceFrames = true;
            EntityStats.Instance.DeminishStaminaOffEntity("Player", player.m_rollStaminaDrain);
            // And visual effects.
        }
    }

    public void IFramesOn()
    {
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        if (player.m_InvinceFrames == false)
        {
            player.m_InvinceFrames = true;
            // And visual effects.
        }
    }


    // Bool to false.
    // May have visual/sound issues depending how they are handled.
    public void IFramesOff()
    {
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_InvinceFrames = false;
    }


    // Deactivates every animation event
    // when leaving animation states.
    // A nice safety if off events aren't reached.
    public void Deactivate()
    {
        DeavtivateHitBox();
        IFramesOff();
    }

    public void Rotation()
    {
        // RotateTowards movement destination while
        // mid combo.
        // eg attack 1 in 1 direciton and attack 2 in direction pointing in
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        if (!player.m_lockon)
        {
            // Get the input out of the animator
            float z = player.m_animator.GetFloat("Input/Z");
            float x = player.m_animator.GetFloat("Input/X");
            if (z != 0 && x != 0)
            {
                Vector3 camerax = (new Vector3(Camera.main.transform.right.x, this.transform.up.x, Camera.main.transform.right.z) * x);
                Vector3 cameraz = (new Vector3(Camera.main.transform.forward.x, this.transform.up.x, Camera.main.transform.forward.z) * z);
                Vector3 cameraPosition = (cameraz + camerax);

                Quaternion targetRotation = Quaternion.LookRotation(cameraPosition);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, player.m_rollSpeed);
            }
        }
    }


    // Player has lives.
    // Mostly a placeholder.
    public void Respawn()
    {
        Player player = this.GetComponent<Player>();

        if (player.m_respawnPos != null)
        {
            EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<BossBrain>().BossReset();
            player.m_lockon = false;
            player.transform.position = player.m_respawnPos.transform.position;
            player.transform.rotation = player.m_respawnPos.transform.rotation;
        }
        else
        {
            player.transform.position = player.m_startPos;
        }
        // Player was stuck in infinite without the bool
        EntityStats.Instance.RestoreAllStats();
        player.m_Dead = false;
    }



    // Clear all input in animator.
    // Mostly for any stte/ common states
    public void ResetAnimator()
    {
        Animator playerAnim = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>();
        playerAnim.SetFloat("MovementSpeedMult", 0);
        playerAnim.SetFloat("Movement/X", 0);
        playerAnim.SetFloat("Movement/Z", 0);

        playerAnim.SetInteger("AnyState/Damage", 0);
        playerAnim.SetInteger("AnyState/Cutscene", 0);

        playerAnim.SetBool("Input/Roll", false);
        playerAnim.SetBool("Input/Running", false);
        playerAnim.SetBool("Input/AttackHeavy", false);
        playerAnim.SetBool("Input/AttackLight", false);
    }

    public void Undie()
    {
        Animator playerAnim = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>();
        playerAnim.SetInteger("AnyState/Death", 0);
        EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_InvinceFrames = false;
    }
}