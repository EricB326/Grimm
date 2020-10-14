using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A centralized place for movement.
// Will probably be moved into the state machine layer of movment
// once access to entity list and input buffering are in.


public class PlayerMovementVariables : MonoBehaviour
{
    // Multiplied by direction stick input. -1 to 1 on both axis
    public float m_walkSpeed = 5;
    // Speed will increase to this value.
    public float m_runSpeed = 7;
    // Roll speed. Ideally should accelerate to this speed rather rapidly.
    public float m_rollSpeed = 12;
    [Range(0,1)]
    public float m_rotationTime;
    // Used in the player walk till access to entity list.
    public GameObject m_target;
    // Just a quick way to get to the sword.
    // For visuals and particles.
    private Collider m_swordHitBox;
    // Consistent attack hitbox. 
    // If entity is inside this it will trigger OnHitEffects including damage.
    private Collider m_attackHitBox;
    // Stamina drain on attack. Should be consistent.
    public int m_attackStaminaDrain = 20;
    // Roll Stamina drain
    public int m_rollStaminaDrain = 25;

    public int m_runStaminaDrain = 2;
    // Roll damage multiplier value.
    [Range(0,1)]
    public float m_rollDamagemultiplier = 0.80f;
    // Damage per attack. Should be consistent.
    public int m_attackDamage = 10;

    private GameObject m_lookPoint;
    // Only used to decide if the player can be hit. Basicaly god mode.
    // Unable to be hit if IFrames true;
    // Modified by an animation event.
    public bool m_InvinceFrames = false;

    public bool m_lockon = false;

    public BasicBuffer m_inputBuffer;


    private void Start()
    {
        // This is better to be set with in game logic but I'll
        // cover that again when I go over the camera.
        m_target = EntityStats.Instance.GetObjectOfEntity("Boss");
        m_lookPoint = GameObject.Find("LookPointPlayer");
        m_swordHitBox = GameObject.Find("Sword").GetComponent<Collider>();
        m_attackHitBox = GameObject.Find("AttackHitBox").GetComponent<Collider>();
    }

    // Getters
    public Collider GetAttackHitBox()
    {
        return m_attackHitBox;
    }

    public Collider GetSwordHitBox()
    {
        return m_swordHitBox;
    }

    public GameObject GetLookPoint()
    {
        return m_lookPoint;
    }

}
