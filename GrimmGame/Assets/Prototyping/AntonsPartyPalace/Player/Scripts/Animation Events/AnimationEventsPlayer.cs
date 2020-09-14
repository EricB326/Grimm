using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Centralized place for player animation events.
// If something take place on an animation timeline, it'll take place here.
// May be some crossover with collision

public class AnimationEventsPlayer : MonoBehaviour
{
   
    // Activates hitboxes on player model to cause damage
    public void ActivateHitBox()
    {
        //Debug.Log(a_colliderNumber);
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        player.GetAttackHitBox().enabled = true;
        player.GetSwordHitBox().enabled = true;
        EntityStats.Instance.DeminishStaminaOffEntity("Player", player.m_attackStaminaDrain);
    }
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
    public void IFramesOn()
    {
        Player player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        if (player.m_InvinceFrames == false)
        {
            player.m_InvinceFrames = true;
            EntityStats.Instance.DeminishStaminaOffEntity("Player", player.m_rollStaminaDrain);
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
}