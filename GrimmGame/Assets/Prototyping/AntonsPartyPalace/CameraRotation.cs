using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A test to see if the axis values can be edited outside of
// unity input system.


public class CameraRotation : MonoBehaviour
{
    public CinemachineFreeLook freelook;

    public bool lockon = false;
    // Update is called once per frame
    void Update()
    {
        float x = 0, y = 0;

        if (!lockon)
        {
            // To be replaced by xbox axis which we only need to read data on x and y out.
            if (Input.GetKey(KeyCode.Q))
            {
                x++;
            }
            if (Input.GetKey(KeyCode.E))
            {
                x--;
            }

            if (Input.GetKey(KeyCode.R))
            {
                y++;
            }
            if (Input.GetKey(KeyCode.T))
            {
                y--;
            }

            if (x != 0)
            {
                freelook.m_XAxis.m_InputAxisValue = x;
            }
            else if(y != 0)
            {
                freelook.m_YAxis.m_InputAxisValue = y;
            }
            else // have to make sure nothing is passed through
            {
                freelook.m_XAxis.m_InputAxisValue = 0;
                freelook.m_YAxis.m_InputAxisValue = 0;
            }



            // Most of the below is redundant.
            // Cinemachine will do all this for us.
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
}
