using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    #region soundObjects

    [SerializeField]
    private GameObject swordEmitter;
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
    public string playerSwordAttack;

    //Living and breathing
    [FMODUnity.EventRef]
    public string playerHitConfirm;

    //Locomotion
    [FMODUnity.EventRef]
    public string playerRoll;
    [FMODUnity.EventRef]
    public string playerFootstep;
    #endregion

    #region other variables

    public int footstepMaterial;
    private int swordSwingNumber;
    #endregion

    PlayerSound(string action)
    {
        //no capitals and spaces in the action names. Exclude raven/player prefix.
        //cases should only carry information based on the animation.
        //eg: "sword attack 1" and "sword attack 2" are different cases but "footstep path" and "footstep mud" are the same.
        switch (action)
        {
            case "sword attack 1":
                FMODUnity.RuntimeManager.PlayOneShotAttached(playerSwordAttack, swordEmitter);
                //Debug.Log("glock note runs");
                break;
            case "sword attack 2":
                FMODUnity.RuntimeManager.PlayOneShotAttached(playerSwordAttack, swordEmitter);
                break;
            case "hit confirm":
                FMODUnity.RuntimeManager.PlayOneShotAttached(playerHitConfirm, centreEmitter);
                break;
            case "footstep":
                int surfaceIndex = 0;
                FMOD.Studio.EventInstance footsteps = FMODUnity.RuntimeManager.CreateInstance(playerFootstep);
                footsteps.setParameterByName("surface", surfaceIndex);
                footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(footEmitter));
                footsteps.start();
                footsteps.release();
                break;
            default:
                //throw error with class name and the action string
                Debug.Log($"Unhandled input in RavenSound: {action}");
                break;

        }
    }
}
