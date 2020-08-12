﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.UIElements;

public enum HitEffectsEnum
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
        public HitEffectsEnum hitEffect;
        public GameObject animationsToPlay;   // Needs to be changed from GameObject.
        public List<VisualEffect> particlesToPlay;
        public List<GameObject> soundsToPlay; // Needs to be changed from GameObject.
    }

    [SerializeField] private List<onHitEffectField> hitEffectList = new List<onHitEffectField>();

    private static OnHitEffects instance;

    private void Awake()
    {
        // Assure that the object can be destroyed if has to be created again later.
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    /* @brief Get the instance of the static singleton.
    */
    public static OnHitEffects Instance { get { return instance; } }

    public void ResolveKnockbackWeak(GameObject _effectedEntity, Vector3 _particlePosition)
    {
        int index = LocateHitEffect(HitEffectsEnum.KNOCKBACK_WEAK);

        if (hitEffectList[index].particlesToPlay.Count > 0)
        {
            for (int i = 0; i < hitEffectList[index].particlesToPlay.Count; i++)
            {
                PlayParticleAtPosition(hitEffectList[index].particlesToPlay[i], _particlePosition);
            }
        }
    }

    public void ResolveKnockbackStrong(GameObject _effectedEntity)
    {
        int index = LocateHitEffect(HitEffectsEnum.KNOCKBACK_STRONG);
    }

    private void PlayParticleAtPosition(VisualEffect _particle, Vector3 _position)
    {
        _particle.SetVector3(Shader.PropertyToID("Position"), _position);
        _particle.Play();
    }

    private int LocateHitEffect(HitEffectsEnum _desiredEffect)
    {
        for (int i = 0; i < hitEffectList.Count; i++)
        {
            if (hitEffectList[i].hitEffect == _desiredEffect)
                return i;
        }

        return -1;
    }
}
