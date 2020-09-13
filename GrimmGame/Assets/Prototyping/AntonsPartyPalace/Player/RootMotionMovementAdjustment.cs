using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionMovementAdjustment : StateMachineBehaviour
{

    float lastY = 0;

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
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Position to move to
        Vector3 posToMove = animator.deltaPosition;
        float forwardOffset = 0.5f;
        float length = 0.1f;
        Vector3 upwardOffest = new Vector3(0, 1, 0);
        Vector3 stepUpHeight = new Vector3(0, 0, 0);
        Vector3 straightDownOffset = new Vector3(0, 0.5f, 0);
        // Move position up or down on y axis depending on angle below.
        // Direct Down
        Ray directDown = new Ray(animator.gameObject.transform.position + straightDownOffset, -Vector3.up);
       
        RaycastHit directDownhit;
        // In Front
        Ray stepInFrontDown = new Ray(animator.gameObject.transform.position +
            animator.gameObject.transform.forward * forwardOffset + stepUpHeight + upwardOffest, -Vector3.up);
        RaycastHit stepInFrontHit;
       

        // Debug rays
        Debug.DrawRay(animator.gameObject.transform.position + straightDownOffset, -Vector3.up * 
            (length + stepUpHeight.y + straightDownOffset.y));
        Debug.DrawRay(animator.gameObject.transform.position + 
            animator.gameObject.transform.forward * forwardOffset + 
            stepUpHeight + upwardOffest, -Vector3.up * (length + stepUpHeight.y + upwardOffest.y));


        //if (Physics.Raycast(stepInFrontDown, out stepInFrontHit, length + stepUpHeight.y + upwardOffest.y))
        //{
        //    Vector3 direction1 = stepInFrontHit.point - animator.gameObject.transform.position;
        //    if (direction1.y != 0)
        //    {
        //        ////Debug.Log(direction.normalized);
        //        //float angle = (180 / 3.14159f) * Mathf.Atan(direction.y);
        //        //// Almost right... maybe?
        //        //// Need to get an angle
        //        //posToMove = Quaternion.Euler(angle, 0, 0) * posToMove;
        //        if (Physics.Raycast(directDown, out directDownhit, length + straightDownOffset.y))
        //        {
        //            Vector3 direction2 = directDownhit.point - animator.gameObject.transform.position;
        //            if (direction2.y != 0)
        //            {
        //                //Debug.Log(direction.normalized);
        //                float angle = (180 / 3.14159f) * Mathf.Atan(direction1.y - direction2.y);
        //                // Almost right... maybe?
        //                // Need to get an angle
        //                posToMove = Quaternion.Euler(angle, 0, 0) * posToMove;
        //            }
        //        }
                

        //    }
        //}
        /*else */if (Physics.Raycast(directDown, out directDownhit, length + straightDownOffset.y))
        {
            Debug.Log("hit");
            float angle = Vector3.Angle(Vector3.up, directDownhit.normal);
            posToMove = Quaternion.Euler(angle, 0, 0) * posToMove;
        }
        else
        {
            // Assumed not grounded
            posToMove.y = -0.2f;
        }


        // Neeed to remove gravity for this to work.        // Just a translate in the end
        animator.gameObject.transform.position = animator.gameObject.transform.position + posToMove;

    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}