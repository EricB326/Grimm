using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/***************************************************
/***************************************************
/***************************************************
 * THIS IS TESTING ONLY CLASS UWU
 */

public class playertest : MonoBehaviour
{
    public float lightAttackStamCost;
    public float heavyAttackStamCost;
    public float rollStamCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EntityStats.Instance.CanEntityMoveOccur("Player", 100);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EntityStats.Instance.DeminishHealthOffEntity("Player", 20);
    }
}
