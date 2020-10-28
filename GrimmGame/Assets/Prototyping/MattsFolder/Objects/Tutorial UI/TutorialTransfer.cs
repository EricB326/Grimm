using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTransfer : MonoBehaviour
{
    public TutorialScript tutorialScript;

    public void PressedAnimationOver()
    {
        tutorialScript.PressedAnimationOver();
    }

    public void UntriggerCurrentButton()
    {
        tutorialScript.UntriggerCurrentButton();
    }
}
