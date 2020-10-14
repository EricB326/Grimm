using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenSound : MonoBehaviour
{
    #region soundObjects

    [SerializeField]
    private GameObject swordEmitter;
    [SerializeField]
    private GameObject clawEmitter;
    [SerializeField]
    private GameObject headEmitter;
    [SerializeField]
    private GameObject centreEmitter;
    [SerializeField]
    private GameObject footEmitter;
    #endregion

    #region soundEvents

    //Attacks
    [FMODUnity.EventRef]
    public string ravenSwordAttack1;
    [FMODUnity.EventRef]
    public string ravenClawAttack1;

    //Living and breathing
    [FMODUnity.EventRef]
    public string ravenHitConfirm;

    //Locomotion
    [FMODUnity.EventRef]
    public string ravenFootstep1;
    #endregion

    #region other variables

    //put parameters here
    #endregion

    public void PlayRavenSound(string action)
    {
        //no capitals and spaces in the action names. Exclude raven/player prefix.
        switch (action)
        {
            case "sword attack 1":
                FMODUnity.RuntimeManager.PlayOneShotAttached(ravenSwordAttack1, swordEmitter);
                //Debug.Log("glock note runs");
                break;
            case "claw attack 1":
                FMODUnity.RuntimeManager.PlayOneShotAttached(ravenClawAttack1, clawEmitter);
                break;
            case "hit confirm":
                FMODUnity.RuntimeManager.PlayOneShotAttached(ravenHitConfirm, centreEmitter);
                break;
            case "footstep":
                FMODUnity.RuntimeManager.PlayOneShotAttached(ravenFootstep1, footEmitter);
                break;
            default:
                //throw error with class name and the action string
                Debug.Log($"Unhandled input in RavenSound: {action}");
                break;

        }
    }
}
