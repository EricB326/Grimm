using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class hairTest : MonoBehaviour
{
    public GameObject target;

    private Transform og;

    private Vector3 last;
    // Start is called before the first frame update
    void Start()
    {
        og = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(0.4f * Time.time) * 3f, 0.5833f, 4f);
    }
}
