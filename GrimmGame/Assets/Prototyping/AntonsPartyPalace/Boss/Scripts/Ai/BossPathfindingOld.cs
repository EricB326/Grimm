using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



// THIS SCRIPT IS DEFUNCT AND NEEDS TO BE REPLACED.
// Nav mesh is no longer used.
// It can be used to send nodes that the boss can cycle through.

public class BossPathfindingOld : MonoBehaviour
{
    // Set in start.
    private NavMeshAgent m_navMeshAgent;
    private Animator m_animator;

    // Values passed to nave mesh Agent upon changes.
    // They will be passed in by the animation states.
    public GameObject m_target;
    // Top speed.
    public float m_moveSpeed;
    // How to top speed.
    public float m_acceleration;
    // The range needed to get to before attack starts.
    public float m_desiredRange;
    // Time to accomplishing task.
    public float m_timeLimit;
    // What phases does the boss have access to currently.
    public int m_phase;
    // Value passed into the animator once goal is reached.
    public int m_desiredAttack;


    private void Start()
    {
        m_navMeshAgent = this.GetComponent<NavMeshAgent>();
        m_animator = this.GetComponent<Animator>();
        m_navMeshAgent.SetDestination(m_target.transform.position);
        //m_navMeshAgent.updateRotation = false;
    }


    // Update is called once per frame
    void Update()
    {

        if(CalculateDistance())
        {
            m_navMeshAgent.isStopped = true;
            //m_animator.SetInteger("Ai/Attack", m_desiredAttack);
        }
        else
        {
            m_navMeshAgent.SetDestination(m_target.transform.position);
            m_navMeshAgent.isStopped = false;

        }


        if (!m_navMeshAgent.isStopped)
        {

            Vector3 direction = m_navMeshAgent.destination - transform.position;

            //float target_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
            //Vector3 move_direction = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
  
            direction = direction.normalized;
            direction = transform.worldToLocalMatrix * direction;

            //Debug.Log(direction);
            //Debug.DrawRay(transform.position, direction);
            // Update animator with movement
            m_animator.SetFloat("Movement/X", direction.x);
            m_animator.SetFloat("Movement/Z", direction.z);
        }
        else
        {
            //this.transform.LookAt(m_target.transform.position);
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("Movement/X", 0);
        }
    }

    // For calculating distance for attacks.
    private bool CalculateDistance()
    {
        Vector3 position = this.transform.position;
        Vector3 targetPosition = m_target.transform.position;

        Vector3 difference = position - targetPosition;

        float largestDistance = Mathf.Max(Mathf.Abs(difference.x), Mathf.Abs(difference.z));


        if (largestDistance <= m_desiredRange)
        {
            return true;
        }
        else
            return false;

    }

}
