using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackShakeTest : MonoBehaviour
{
    public int intensity;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            CameraShaker.Instance.startShake(intensity);
        if (Input.GetKeyDown(KeyCode.O))
            CameraShaker.Instance.stopShake();
    }
}
