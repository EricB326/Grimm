using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Quick and dirty closing of application for builds

public class EmergencyExit : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}

