using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementVariables : MonoBehaviour
{
    // Multiplied by direction stick input. -1 to 1 on both axis
    public float m_walkSpeed;

    // When roll animaiton occurs the initial velocity.
    public float m_rollSpeedStart;
    // DEFCUNT
    public float m_rollSpeedEnd;
    // If the player is locked on palyer will strafe will facing camera.
    // If not player will rotate towards and run forward.
    public bool m_lockOn;
    // For orientating.
    public float m_roationSpeed;

    public GameObject m_target;
}
