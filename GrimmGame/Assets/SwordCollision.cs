using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// This is currently used for sword collision.
// Seperated out from player in case there are 
// conflicts with using onCollider ect.
// 

public class SwordCollision : MonoBehaviour
{
    private void Start()
    {
        
    }






    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.tag);
    //    Debug.DrawRay(this.transform.position, collision.transform.position);

    //    if (collision.gameObject.tag == "Boss")
    //    {
    //        Debug.Log("Boss hit");
    //    }
    //}


    // If it hits the boss it disables the hitbox and does damage.
    // If we want to pass information into the animator it would happen
    // here as well
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        Debug.DrawRay(this.transform.position, other.transform.position);

        if (other.gameObject.tag == "Boss")
        {
            Debug.Log("Boss hit");
            //EntityStats.Instance;
        }
    }


}
