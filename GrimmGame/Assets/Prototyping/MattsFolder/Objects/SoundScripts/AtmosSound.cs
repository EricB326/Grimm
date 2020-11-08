using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosSound : MonoBehaviour
{
    public float distanceMax;
    [SerializeField]
    private Transform crowTransform;

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

    // Update is called once per frame
    void Update()
    {
        atmosEmitter.SetParameter("proximity", Mathf.Clamp01(1-GetCrowDistance() / distanceMax));
        Debug.Log(GetCrowDistance() / distanceMax);
    }
}
