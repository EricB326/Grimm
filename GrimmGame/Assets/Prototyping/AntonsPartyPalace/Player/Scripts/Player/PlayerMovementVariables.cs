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
    // Just a Quick way to get to the sword
    public Collider m_AttackHitbox;

    public int m_attackStaminaDrain = 20;

}
