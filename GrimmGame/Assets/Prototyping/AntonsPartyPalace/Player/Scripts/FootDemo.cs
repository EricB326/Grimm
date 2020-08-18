using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FootDemo : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IkFootVariables feet = animator.gameObject.GetComponentInParent<IkFootVariables>();
        float maxDistanceDown = feet.m_castDown;
        float maxDistanceUp = feet.m_castUp;

        Vector3 LifterPosL;
        Vector3 LifterPosR;


        // In the case of the raycast starting inside of something 
        // we offset the second cast by the amount travelled almost.
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
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, pos);
        }
        // Down                                             //
        else if(Physics.Raycast(raycastDown, out info, maxDistanceDown + maxDistanceUp))
        {
            Vector3 pos = new Vector3(info.point.x, info.point.y + feet.m_offsetDown, info.point.z);
            // If we hit something above out foots position move foot.
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            //animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, pos);
            //float rotationX = Vector3.Angle(info.normal, animator.transform.up);
            //Debug.Log(rotationX);
            //float 
            //animator.SetIKRotation(AvatarIKGoal.LeftFoot, quaternion.Euler(rotationX, 0, 0));
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }


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
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            //animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, pos);
            float rotationX = Vector3.Angle(info.normal, animator.transform.up);
            //animator.SetIKRotation(AvatarIKGoal.RightFoot, quaternion.Euler(rotationX, 0, 0));
        }
        else if (Physics.Raycast(raycastDown, out info, maxDistanceDown + maxDistanceUp))
        {
            Vector3 pos = new Vector3(info.point.x, info.point.y + 0.1f, info.point.z);
            // If we hit something above out foots position move foot.
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, pos);
        }
        else
        {

            // Should be a gradual reduction
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
        }

        // Should set bumper position to here.


        Vector3 lifterPos = feet.m_leftFoot.transform.position - feet.m_rightFoot.transform.position;
        lifterPos = feet.m_rightFoot.transform.position + lifterPos * 0.5f;

        feet.m_lifter.transform.position = lifterPos;



    }
}
