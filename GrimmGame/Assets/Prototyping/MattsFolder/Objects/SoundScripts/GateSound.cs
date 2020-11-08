using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSound : MonoBehaviour
{
    [SerializeField]
    private GameObject gateEmitter;

    [FMODUnity.EventRef]
    public string gateRattle, gateBreak;

    public void PlayGateSound(string action)
    {
        //no capitals and spaces in the action names. Exclude raven/player prefix.
        switch (action)
        {
            case "rattle":
                FMODUnity.RuntimeManager.PlayOneShotAttached(gateRattle, gateEmitter);
                //Debug.Log("glock note runs");
                break;
            case "break":
                FMODUnity.RuntimeManager.PlayOneShotAttached(gateBreak, gateEmitter);
                break;
            default:
                //throw error with class name and the action string
                Debug.Log($"Unhandled input in GateSound: {action}");
                break;
        }
    }
}
