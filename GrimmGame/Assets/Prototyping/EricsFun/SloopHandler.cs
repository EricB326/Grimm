using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloopHandler : MonoBehaviour
{
    public float height = 1f;
    public float heightPadding = 0.1f;
    public float heightCorrectionSpeed = 5f;
    public LayerMask ground;
    public float maxGroundAngle = 120f;
    public bool debugMode;

    float angle;
    float groundAngle;

    Vector3 forward;
    RaycastHit hitInfo;
    bool isGrounded;

    public void CalculateForward()
    {
        if (!isGrounded)
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(transform.right, hitInfo.normal);
    }

    public void CalculateGroundAngle()
    {
        if (!isGrounded)
        {
            groundAngle = 90f;
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    public void CheckGround()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), -Vector3.up, out hitInfo, height + heightPadding, ground))
        {
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
            {
                // transform.position = Vector3.Slerp(transform.position, hitInfo.point, heightCorrectionSpeed * Time.deltaTime);
                transform.position = hitInfo.point;
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void ApplyGravity()
    {
        if (!isGrounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }

    public void DrawDebugLines()
    {
        if (!debugMode)
            return;

        Debug.DrawLine(transform.position + new Vector3(0, 1, 0), (transform.position + new Vector3(0, 1, 0)) + forward * height * 2, Color.blue);
        Debug.DrawLine(transform.position + new Vector3(0, 1, 0), (transform.position + new Vector3(0, 1, 0)) - Vector3.up * (height + heightPadding), Color.green);
    }
}

