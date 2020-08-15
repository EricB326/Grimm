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
    private GameObject m_player;
    private GameObject m_target;

    private void Start()
    {
        m_player = EntityStats.Instance.GetObjectOfEntity("Player");
    }


    // Update is called once per frame
    void Update()
    {
        if (!m_player.GetComponent<PlayerMovementVariables>().m_lockon)
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



            if (axisX != 0)
            {
                m_camList[m_selectedCamera].m_XAxis.m_InputAxisValue = axisX;
            }
            else if (axisY != 0)
            {
                m_camList[m_selectedCamera].m_YAxis.m_InputAxisValue = axisY;
            }
            else // have to make sure nothing is passed through
            {
                m_camList[m_selectedCamera].m_XAxis.m_InputAxisValue = 0;
                m_camList[m_selectedCamera].m_YAxis.m_InputAxisValue = 0;
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

