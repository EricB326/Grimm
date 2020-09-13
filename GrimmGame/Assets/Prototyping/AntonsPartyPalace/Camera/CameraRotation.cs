using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.WSA;
using XboxCtrlrInput;


// A test to see if the axis values can be edited outside of
// unity input system.


public class CameraRotation : MonoBehaviour
{
    public List<CinemachineFreeLook> m_camList;
    //public CinemachineFreeLook m_freeCamera;
    //public CinemachineFreeLook m_lockOnCamera;

    private int m_selectedCamera = 0; // 0 for free 1 for lock. 2> Different rules
                                      // Defaults to false.

    // Player houses the lockon variable.
    private Player m_player;
    private GameObject m_target;

    private void Start()
    {
        m_player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
    }


    // Input for camera is done here
    void Update()
    {
        if (!m_player.m_lockon)
        {
            m_camList[0].Priority = 2;
            m_camList[1].Priority = 1;
            m_selectedCamera = 0;
        }
        else
        {
            m_camList[0].Priority = 1;
            m_camList[1].Priority = 2;
            m_selectedCamera = 1;
        }


        float axisX = XCI.GetAxis(XboxAxis.RightStickX);

        float axisY = XCI.GetAxis(XboxAxis.RightStickY);



        if (!m_player.m_lockon)
        {
            if (axisX != 0)
            {
                m_camList[m_selectedCamera].m_XAxis.m_InputAxisValue = axisX;
            }
            else
            {
                m_camList[m_selectedCamera].m_XAxis.m_InputAxisValue = 0;
            }
            if (axisY != 0)
            {
                m_camList[m_selectedCamera].m_YAxis.m_InputAxisValue = axisY;
            }
            else
            {
                m_camList[m_selectedCamera].m_YAxis.m_InputAxisValue = 0;
            }
        }


    }

    // Adjustments for camera are here.
    private void FixedUpdate()
    {
        if (m_player.m_lockon)
        {
            // Player position
            Vector3 followPos = m_camList[1].Follow.position;
            // Boss position
            Vector3 lookAtPos = m_camList[1].LookAt.position;

            Vector3 direction = followPos - lookAtPos;

            float radius = m_camList[1].m_Orbits[1].m_Radius;
 
                                                            // Very important do not touch
            m_camList[m_selectedCamera].m_XAxis.Value = (180 / 3.14159f) * Mathf.Atan2(-direction.x, -direction.z);
        }
    }
}