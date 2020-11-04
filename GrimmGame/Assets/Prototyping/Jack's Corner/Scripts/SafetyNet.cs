using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNet : MonoBehaviour
{
    public Transform safetyNet;
    public int trigger;

    // Update is called once per frame
    void Update()
    {
        if (EntityStats.Instance.GetObjectOfEntity("Player").transform.position.y <= trigger)
        {
            EntityStats.Instance.GetObjectOfEntity("Player").transform.position = safetyNet.position;
        }
    }
}
