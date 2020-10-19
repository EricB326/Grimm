using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackShakeTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            CameraShaker.Instance.startShake(10);
        if (Input.GetKeyDown(KeyCode.O))
            CameraShaker.Instance.stopShake();
    }
}
