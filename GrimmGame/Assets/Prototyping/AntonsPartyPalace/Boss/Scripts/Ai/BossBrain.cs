using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// A quick script to house some basic
// variables and decisions the ai can make.

public class BossBrain : MonoBehaviour
{
    // Components to get
    private Animator m_animator;


    [Range(0, 1)]
    public float m_rotationSpeed;
    [Header("DEBUG. VALUES PASSED IN BY ANIMATOR")]
    // The range to reach before attack begins.
    // Passed out of the animator and here for debugging.
    public float m_desiredRange;
    // What the current phase of the boss is.
    // Either this lives in the animator or out here.
    //public int m_phase;
    // The current attack.
    //public int m_attack;
    // Stamina or revenge value.
    // Depending on how we want to the game to go this 
    // will either be a value that once it reaches
    // a threshold will launch a quick coutner attack
    // or a value that the boss uses to work out what
    // attack he is able to use next.
    //public float m_revengeValue;
    // The actual target the boss is seeking towards.
    // Most likely the player at all times.
    public GameObject m_target;
    // When was the last decision made.
    private float m_lastDecision;




    public BossAttackVariables m_currentAttackVariables;

    private void Start()
    {
        m_animator = this.GetComponent<Animator>();
        m_target = EntityStats.Instance.GetObjectOfEntity("Player");
    }


    private void Update()
    {
        // So the ai needs to get information about it's next attack
        // from the animation states themselves.
        // Therefore the boss needs to know if it is within range(both angle and distance)
        // to launch the attack so where is that stored?
        // Damage can be stored on the animation stateoverloads.
        // I'VE GOT IT!
        // The range will just be a value passed in to the animator at all times
        // and be used as a condition to get into the next attacks.
        // So there will be 3 conditions before an attack can be launched.
        Vector3 directionToMove = m_target.transform.position - transform.position;

        if (m_animator.GetBool("Ai/IsMoving"))
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToMove);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, m_rotationSpeed);
        }


        if (directionToMove.magnitude > m_desiredRange)
        {
            directionToMove = this.transform.worldToLocalMatrix * directionToMove.normalized;

            Debug.DrawRay(this.transform.position, directionToMove);

            m_animator.SetFloat("Movement/Z", directionToMove.z);
            m_animator.SetFloat("Movement/X", directionToMove.x);
        }
        else
        {
            m_animator.SetFloat("Movement/Z", 0);
            m_animator.SetFloat("Movement/X", 0);
        }
    }
    


    // Returns true if within range
    private bool CalculateDistance(Vector3 a_direction)
    {
        //Vector3 position = this.transform.position;
        //Vector3 targetPosition = m_target.transform.position;

        //Vector3 difference = position - targetPosition;

        // This isn't correct?... or at best kinda dodgy/can be done a different way.
        float largestDistance = Mathf.Max(Mathf.Abs(a_direction.x), Mathf.Abs(a_direction.z));


        if (largestDistance <= m_desiredRange)
        {
            return true;
        }
        else
            return false;

    }


    // Returns true if facing is acceptable
    private bool CorrectFacing()
    {
        // Probs a dot product.
        // If within certain angle start attack.


        if(true)
        {
            return true;
        }
        else
            return false;
    }


}