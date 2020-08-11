using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum hitEffectsEnum
{
    KNOCKBACK_WEAK,
    KNOCKBACK_STRONG
}


public class OnHitEffects : MonoBehaviour
{
    [Serializable]
    private struct onHitEffectField
    {
        public string name;
        public hitEffectsEnum hitEffect;
        public List<GameObject> animationsToPlay;
        public List<GameObject> particlesToPlay;
    }

    [SerializeField] private List<onHitEffectField> hitEffectList = new List<onHitEffectField>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void KnockbackWeak(GameObject _effectedEntity)
    {
    }

    private void KnockbackStrong(GameObject _effectedEntity)
    {
    }
}
