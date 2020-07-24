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

    // If the player is locked on palyer will strafe will facing camera.
    // If not player will rotate towards and run forward.
    public bool m_lockOn;
    
    public float m_roationTime;
    // Used in the player walk for the time being.
    public GameObject m_target;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(m_lockOn)
            {
                m_lockOn = false;
            }
            else
            {
                m_lockOn = true;
            }
        }
    }
}
