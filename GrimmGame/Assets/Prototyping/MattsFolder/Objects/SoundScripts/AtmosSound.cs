using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosSound : MonoBehaviour
{
    private float distanceMax;
    [SerializeField]
    private Transform crowTransform;
    private bool ravenDead;

    private FMODUnity.StudioEventEmitter atmosEmitter;

    // Start is called before the first frame update
    void Awake()
    {
        distanceMax = GetCrowDistance();
        atmosEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private float GetCrowDistance()
    {
        return Vector3.Distance(transform.position, crowTransform.position);
    }

    public void RavenIsDead()
    {
        ravenDead = true;
        atmosEmitter.SetParameter("proximity", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ravenDead)
        {
            atmosEmitter.SetParameter("proximity", Mathf.Clamp01(1 - GetCrowDistance() / distanceMax));
        }  
    }
}
