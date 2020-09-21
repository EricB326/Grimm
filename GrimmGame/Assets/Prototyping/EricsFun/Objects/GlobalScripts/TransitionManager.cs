using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX.Utility;

public class TransitionManager : MonoBehaviour
{
    [Serializable]
    private struct particleData
    {
        public GameObject particle;
        public Vector3 particlePosition;
    }

    [Serializable]
    private struct transitionEffects
    {
        public string name;
        public List<particleData> particlesToPlay;
    }

    [SerializeField] private List<transitionEffects> bossPhaseTransistions = new List<transitionEffects>();
    //public List<transitionEffects> bossPhaseTransistions = new List<transitionEffects>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
