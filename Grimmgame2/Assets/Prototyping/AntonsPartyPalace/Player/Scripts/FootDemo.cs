using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FootDemo : StateMachineBehaviour
{
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IkFootVariables feet = animator.gameObject.GetComponentInParent<IkFootVariables>();
        float maxDistanceDown = feet.m_castDown;
        float maxDistanceUp = feet.m_castUp;


        float leftFootWeight = animator.GetFloat("Ik/LeftFoot");
        float rightFootWeight = animator.GetFloat("Ik/RightFoot");

        // Left foot
        Vector3 downOffsetCast = new Vector3(feet.m_leftFoot.transform.position.x,
            feet.m_leftFoot.transform.position.y + maxDistanceUp, feet.m_leftFoot.transform.position.z);

        Ray raycastUp = new Ray(feet.m_leftFoot.transform.position, animator.transform.up);
        Ray raycastDown = new Ray(downOffsetCast, -animator.transform.up);
        RaycastHit info;
        // Up
        if(Physics.Raycast(raycastUp, out info, maxDistanceUp))
        {
            Vector3 pos = new Vector3(info.point.x, info.point.y + feet.m_offsetUp, info.point.z);
            // If we hit something above out foots position move foot.
            leftFootWeight += 0.05f;
            if (leftFootWeight > 1)
            {
                leftFootWeight = 1;
            }
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, pos);
        }
        // Down                                             //
        else if(Physics.Raycast(raycastDown, out info, maxDistanceDown + maxDistanceUp))
        {
            Vector3 pos = new Vector3(info.point.x, info.point.y + feet.m_offsetDown, info.point.z);
            // If we hit something above out foots position move foot.
            leftFootWeight += 0.05f;
            if (leftFootWeight > 1)
            {
                leftFootWeight = 1;
            }
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, pos);
            //float rotationX = Vector3.Angle(info.normal, animator.transform.up);
            //Debug.Log(rotationX);
            //float 
            //animator.SetIKRotation(AvatarIKGoal.LeftFoot, quaternion.Euler(rotationX, 0, 0));
        }
        else
        {
            leftFootWeight -= 0.5f;
            if (leftFootWeight < 0)
            {
                leftFootWeight = 0;
            }
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }
        animator.SetFloat("Ik/LeftFoot", leftFootWeight);

        downOffsetCast = new Vector3(feet.m_rightFoot.transform.position.x,
           feet.m_rightFoot.transform.position.y + maxDistanceUp, feet.m_rightFoot.transform.position.z);

        // Right foot
        raycastUp = new Ray(feet.m_rightFoot.transform.position, animator.transform.up);
        raycastDown = new Ray(downOffsetCast, -animator.transform.up);

        if (Physics.Raycast(raycastUp, out info, maxDistanceUp))
        {
            //Debug.Log("Contact");
            Vector3 pos = new Vector3(info.point.x, info.point.y + 0.2f, info.point.z);
            // If we hit something above out foots position move foot.
            rightFootWeight += 0.05f;

            if(rightFootWeight > 1)
            {
                rightFootWeight = 1;
            }
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, pos);
            //animator.SetIKRotation(AvatarIKGoal.RightFoot, quaternion.Euler(rotationX, 0, 0));


        }
        else if (Physics.Raycast(raycastDown, out info, maxDistanceDown + maxDistanceUp))
        {
            Vector3 pos = new Vector3(info.point.x, info.point.y + 0.1f, info.point.z);
            rightFootWeight += 0.05f;
            if (rightFootWeight > 1)
            {
                rightFootWeight = 1;
            }

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, pos);
        }
        else
        {
            rightFootWeight -= 0.5f;
            if (rightFootWeight < 0)
            {
                rightFootWeight = 0;
            }

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
        }
        animator.SetFloat("Ik/RightFoot", rightFootWeight);

    }
}
