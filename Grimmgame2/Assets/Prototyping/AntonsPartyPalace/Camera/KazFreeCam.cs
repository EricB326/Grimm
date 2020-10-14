using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KazFreeCam : MonoBehaviour
{
    //float mouse_y = 0,
    //      mouse_x = 0;

    public bool m_lock = false;

    [Range(0, 2)]
    public float m_movementSpeed;

    [Range(0, 1)]
    public float m_rotationX;
    [Range(0, 1)]
    public float m_rotationY;
    [Range(0, 1)]
    public float m_rotationZ;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (!m_lock)
            {
                m_lock = true;
            }
            else
            {
                m_lock = false;
            }

        }

        if (!m_lock)
        {
            int x = 0,
                y = 0,
                z = 0;
            if (Input.GetKey(KeyCode.W))
            {
                z++;
            }
            if (Input.GetKey(KeyCode.D))
            {
                x++;
            }
            if (Input.GetKey(KeyCode.S))
            {
                z--;
            }
            if (Input.GetKey(KeyCode.A))
            {
                x--;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                y++;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                y--;
            }


            float mouse_y = 0,
                mouse_x = 0,
                yaw_z = 0;


            if (Input.GetKey(KeyCode.Q))
            {
                yaw_z--;
            }
            if (Input.GetKey(KeyCode.E))
            {
                yaw_z++;
            }


            if(Input.GetAxisRaw("Mouse X") != 0)
            {
                mouse_y += Input.GetAxisRaw("Mouse X");
            }
            else
            {
                mouse_y = 0;
            }
            if(Input.GetAxisRaw("Mouse Y") != 0)
            {
                mouse_x -= Input.GetAxisRaw("Mouse Y");
            }
            else
            {
                mouse_x = 0;
            }

            //Vector3 roationDebug = new Vector3(mouse_x, mouse_y, yaw_z);

            Vector3 roationDebug = Input.mousePosition.normalized * 2 - new Vector3(1, 1, 0);

           
            // Rotate towards 
            this.transform.Rotate(mouse_x * m_rotationY, -mouse_y * m_rotationX, yaw_z * m_rotationZ);

            Vector3 movement = new Vector3(x, y, z);

            movement = transform.localToWorldMatrix * movement;

            this.transform.position = this.transform.position + movement.normalized * m_movementSpeed;
            
        }
    }
}
