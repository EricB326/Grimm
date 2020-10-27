using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crowFly : MonoBehaviour
{
    private Animator m_animator;
    private Vector3 m_startPosition;
    private Collider m_collisionCheck;
    private bool m_canReturn = false;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = this.GetComponent<Animator>();
        // Ranomizes their starting position for then animation.
        ResetAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        // If the birds come back it will occur in this loop.
    }

    private void ResetAnimation()
    {
        m_animator.Play("Idle_Blend Tree", -1, Random.Range(0, 1f));
    }

    // Player has spooked them fly away from them.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            this.transform.LookAt(other.transform);

            
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            this.transform.LookAt(other.transform);


        }
    }



    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            // Return to position after x amount of time.
        }
    }


}
