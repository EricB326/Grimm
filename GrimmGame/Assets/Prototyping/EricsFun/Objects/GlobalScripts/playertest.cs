using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/***************************************************/
/***************************************************/
/***************************************************/
/* THIS IS TESTING ONLY CLASS UWU
/***************************************************/
/***************************************************/
/***************************************************/

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
        transform.position = Vector3.Slerp(transform.position, new Vector3(40f, 0f, 0f), 0.3f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P))
            EntityStats.Instance.RestoreAllStats();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EntityStats.Instance.CanEntityMoveOccur("Player", 115);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Player", 1);
        }        
        if (Input.GetKeyDown(KeyCode.W))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Player", 2);
        }       
        if (Input.GetKeyDown(KeyCode.E))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Player", 4);
        }        
        if (Input.GetKeyDown(KeyCode.R))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Player", 8);
        }        
        if (Input.GetKeyDown(KeyCode.T))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Player", 16);
        }        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Player", 32);
        }
        //=================
        if (Input.GetKeyDown(KeyCode.A))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Boss", 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Boss", 2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Boss", 4);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Boss", 8);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Boss", 16);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            EntityStats.Instance.DeminishHealthOffEntity("Boss", 32);
        }
    }
}
