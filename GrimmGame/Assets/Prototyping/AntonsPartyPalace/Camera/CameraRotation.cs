using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    // Update is called once per frame
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



        //float x = 0, y = 0;

        // This could be done a LOT better
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if (m_lockOn)
        //    {
        //        m_lockOn = false;
        //        m_camList[0].Priority = 2;
        //        m_camList[1].Priority = 1;
        //        //Debug.Log("Lockoff");
        //        m_selectedCamera = 0;
        //    }
        //    else
        //    {
        //        m_lockOn = true;
        //        m_camList[0].Priority = 1;
        //        m_camList[1].Priority = 2;
        //        //Debug.Log("LockOn");
        //        m_selectedCamera = 1;
        //    }




        // Switching camera system
        //if(m_freeCamera.Priority == 2)
        //{
        //    m_freeCamera.Priority = 3;
        //    m_lockOnCamera.Priority = 2;
        //}
        //else
        //{
        //    m_freeCamera.Priority = 2;
        //    m_lockOnCamera.Priority = 3;
        //}


        // Ruleset for free cam


        // To be replaced by xbox axis which we only need to read data on x and y out.
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    x++;
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    x--;
        //}

        //if (Input.GetKey(KeyCode.R))
        //{
        //    y++;
        //}
        //if (Input.GetKey(KeyCode.T))
        //{
        //    y--;
        //}

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
        else
        {
            // Player position
            Vector3 followPos = m_camList[1].Follow.position;
            // Boss position
            Vector3 lookAtPos = m_camList[1].LookAt.position;

            Vector3 direction = followPos - lookAtPos;

            float radius =  m_camList[1].m_Orbits[1].m_Radius;
            //Debug.Log(radius);
            // We only need the direction not the y.
            direction.y = 0;
            Vector3 raycastStart = m_player.transform.position;
            raycastStart.y = 0;
            Debug.DrawRay(raycastStart, direction.normalized * radius);

            // Check the cameras current angle vs the angle provided by the direciton.
            // Need to adjust the x value based on the -180 to 180 values.


            // Add that


        }


        // Get the direction from the player to the boss.


        // Most of the below is redundant.
        // Cinemachine will do all this for us but leaving in case.
        //    if (x != 0)
        //{
        //    localMultiplyerX = localMultiplyerX + increment;
        //    if(localMultiplyerX > m_rotationMax)
        //    {
        //        localMultiplyerX = m_rotationMax;
        //    }
        //    freelook.m_XAxis.m_InputAxisValue = x * localMultiplyerX;
        //}
        //else if (y != 0)
        //{
        //    freelook.m_YAxis.m_InputAxisValue = y + localMultiplyerY;
        //}
        //else
        //{
        //    localMultiplyerX = 1;
        //    localMultiplyerY = 1;
        //    freelook.m_XAxis.m_InputAxisValue = 0;
        //    freelook.m_YAxis.m_InputAxisValue = 0;
        //}
        }
    }

