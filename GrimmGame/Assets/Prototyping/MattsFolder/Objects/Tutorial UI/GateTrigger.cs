using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [SerializeField]
    private TutorialScript tutorialUIScript;
    //[SerializeField]
    //private int tutGroupLockon, tutGroupGate;

    private void OnTriggerEnter(Collider other)
    {
        tutorialUIScript.GateTriggerEnter();
    }
    private void OnTriggerExit(Collider other)
    {
        tutorialUIScript.GateTriggerExit();
    }
}
