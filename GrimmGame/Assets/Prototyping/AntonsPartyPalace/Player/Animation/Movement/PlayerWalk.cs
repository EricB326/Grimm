using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : StateMachineBehaviour
{
    // SO I don't have to create a new vector all the time?
    private UnityEngine.Vector3 m_movement;

    public Transform m_transform;
    public int m_damage;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject player = animator.gameObject;
        PlayerMovementVariables movementstats = player.GetComponent<PlayerMovementVariables>();

        

        if (animator.GetFloat("Input/X") != 0 || animator.GetFloat("Input/Z") != 0)
        {
            // Direction of the camera to move in.
            UnityEngine.Vector3 movementx = (new UnityEngine.Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * animator.GetFloat("Input/X"));
            UnityEngine.Vector3 movementz = (new UnityEngine.Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * animator.GetFloat("Input/Z"));
            UnityEngine.Vector3 m_movement = (movementz + movementx);


            // Player will be rotated towards camera and strafe
            if (movementstats.m_lockOn)
            {
                // Rotate towards what camera is looking at or camera target.
                player.transform.LookAt(player.GetComponent<PlayerMovementVariables>().m_target.transform);
            }
            else
            {
                //Vector3 direction = Camera.main.transform.TransformDirection(m_movement);
                //direction.y = 0;

                //// Rotate towards where player is moving towards.
                ////Vector3 newDirection =  Vector3.RotateTowards(player.transform.position, m_movement, movementstats.m_roationSpeed, 0.0f);
                ////newDirection.y = 0;
                ////Debug.Log(newDirection);
                //player.transform.rotation = Quaternion.LookRotation(direction);
            }
            //m_movement = new UnityEngine.Vector3(animator.GetFloat("Input/X") * player.transform.position.x, 0, animator.GetFloat("Input/Z") * player.transform.position.z);
            //player.transform.Translate(m_movement * movementstats.m_walkSpeed * Time.deltaTime);

            // Player movement.
            player.GetComponent<Rigidbody>().AddForce(m_movement * movementstats.m_walkSpeed, ForceMode.Impulse);
        }
    }

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
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
