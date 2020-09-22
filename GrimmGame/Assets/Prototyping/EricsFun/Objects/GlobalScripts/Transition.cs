using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Transition
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private Camera cinematicCam;

    [Serializable]
    private struct particleData
    {
        public string particleName;
        public GameObject particle;
        public Vector3 particlePosition;
    }

    [Serializable]
    private struct animationData
    {
        public string characterToPlayAnimation;
        public string animationParameter;
        public bool isParameterTypeBool;
        public bool isParameterTypeInt;
        public bool isParameterTypeFloat;
        public bool newBoolValue;
        public int newIntValue;
        public float newFloatValue;
    }

    [SerializeField] private string transitionName;
    [SerializeField] private List<animationData> animationsToPlay;
    [SerializeField] private List<particleData> particlesToPlay;

    public string TransitionName
    {
        get { return transitionName; } 
        set { transitionName = value; } 
    }

    public void Execute()
    {
        foreach (animationData thisAnimation in animationsToPlay)
        {
            if (thisAnimation.isParameterTypeBool)
            {
                EntityStats.Instance.GetObjectOfEntity(thisAnimation.characterToPlayAnimation).GetComponent<Animator>().SetBool(thisAnimation.animationParameter, thisAnimation.newBoolValue);
            }
            else if (thisAnimation.isParameterTypeFloat)
            {
                EntityStats.Instance.GetObjectOfEntity(thisAnimation.characterToPlayAnimation).GetComponent<Animator>().SetFloat(thisAnimation.animationParameter, thisAnimation.newFloatValue);
            }
            else if (thisAnimation.isParameterTypeInt)
            {
                EntityStats.Instance.GetObjectOfEntity(thisAnimation.characterToPlayAnimation).GetComponent<Animator>().SetInteger(thisAnimation.animationParameter, thisAnimation.newIntValue);
            }
		}
	}

    public void PhaseOneTransition()
    {
        // Set the positions of the characters for this transition.
        //EntityStats.Instance.GetObjectOfEntity("Player").transform.position = . . .
        //EntityStats.Instance.GetObjectOfEntity("Boss").transform.position = . . .

        // Disable the player camera and enable the cinematic camera.
        //playerCam.gameObject.SetActive(false);
        //cinematicCam.gameObject.SetActive(true);

        // Do other cinematic camera functionality to get it ready.
        //cinematicCam.transform.postion = . . .
        //cinematicCam.transform.LookAt(cameraLockOnPoint);

        // Begin animations of the characters
        //EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Animator>().SetInteger("SomeAnimPara/PhaseOne", 0);
        //EntityStats.Instance.GetObjectOfEntity("Boss").GetComponent<Animator>().SetInteger("SomeAnimPara/PhaseOne", 0);
    }
}
