using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementVariables : MonoBehaviour
{
    // Multiplied by direction stick input. -1 to 1 on both axis
    public float m_walkSpeed;

    // If the player is locked on palyer will strafe will facing camera.
    // If not player will rotate towards and run forward.
    public bool m_lockOn;
    
    public float m_roationTime;

    public GameObject m_target;
    // Center of screen or directly in front of player.
    public GameObject m_center;
}
