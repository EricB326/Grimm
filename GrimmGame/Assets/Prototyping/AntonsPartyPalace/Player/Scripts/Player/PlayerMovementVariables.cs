using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A centralized place for movement.
// Will probably be moved into the state machine layer of movment
// once access to entity list and input buffering are in.


public class PlayerMovementVariables : MonoBehaviour
{
    // Multiplied by direction stick input. -1 to 1 on both axis
    public float m_walkSpeed;
    // Speed will increase to this value.
    public float m_runSpeed;

    [Range(0,1)]
    public float m_rotationTime;
    // Used in the player walk till access to entity list.
    public GameObject m_target;
    // Just a quick way to get to the sword.
    // For visuals and particles.
    public Collider m_swordHitBox;
    // Consistent attack hitbox. 
    // If entity is inside this it will trigger OnHitEffects including damage.
    public Collider m_attackHitBox;
    // Stamina drain on attack.
    public int m_attackStaminaDrain = 20;

    public int m_attackDamage = 10;

}
