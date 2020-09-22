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

    public int m_selectedCamera = 0; // 0 for free 1 for lock. 2> Different rules
                                      // Defaults to false.

    // Player houses the lockon variable.
    private Player m_player;
    private GameObject m_target;
    private bool m_recenter = false;

    private float m_centerTarget;

    private GameObject m_lookTarget;

    private void Start()
    {
        m_player = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>();
        m_target = EntityStats.Instance.GetObjectOfEntity("Boss");
        m_camList[0].m_XAxis.Value = (180 / 3.14159f) * Mathf.Atan2(m_player.transform.forward.x, m_player.transform.forward.z);
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
            m_recenter = false;

            m_lookTarget = m_camList[m_selectedCamera].LookAt.gameObject;

            // 2 max at largest distance. 5+ meters
            // 0 min at min distance
            // the movement needs to be modifed in world space based on facing.
            // starts at 0.
            float xChange = m_player.m_animator.GetFloat("Movement/X");
            float zChange = m_player.m_animator.GetFloat("Movement/Z");

            xChange += m_target.GetComponent<Animator>().GetFloat("Movement/X");
            zChange += m_target.GetComponent<Animator>().GetFloat("Movement/Z");



            m_lookTarget.transform.position = new Vector3((180 / 3.14159f) * Mathf.Atan2(xChange, zChange),0,0);

            //change = Vector3.ClampMagnitude((m_lookTarget.transform.position + change),1);
            // Vector3 offset = new Vector3(xChange, 0, 0);
            //m_lookTarget.transform.position = m_lookTarget.transform.position + change;


            // Player position
            Vector3 followPos = m_camList[1].Follow.position;
            // Boss position
            Vector3 lookAtPos = m_camList[1].LookAt.position;

            Vector3 direction = followPos - lookAtPos;
                                                                // Very important do not touch
            m_camList[m_selectedCamera].m_XAxis.Value = (180 / 3.14159f) * Mathf.Atan2(-direction.x, -direction.z);
        }
        else if(m_recenter && m_selectedCamera == 0)
        {
            m_camList[m_selectedCamera].m_XAxis.Value = Mathf.Lerp(m_camList[m_selectedCamera].m_XAxis.Value, m_centerTarget, 0.15f);
            if((int)m_camList[m_selectedCamera].m_XAxis.Value == (int)m_centerTarget)
            {
                m_recenter = false;
            }
        }
    }

    public void Recenter(Vector3 a_forward)
    {
        if (!m_recenter)
        {
            Debug.Log("Recenter");
            m_centerTarget = (180 / 3.14159f) * Mathf.Atan2(a_forward.x, a_forward.z);
            m_recenter = true;
        }
    }
}