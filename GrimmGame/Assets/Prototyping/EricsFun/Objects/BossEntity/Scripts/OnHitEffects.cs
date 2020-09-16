//========== Grimm - OnHitEffects.cs - 11/08/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//==========================================================//
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.UIElements;

public enum HitEffectsEnum
{
    KNOCKBACK_WEAK,
    KNOCKBACK_STRONG,
    KNOCKDOWN,
    PARTICLE_EFFECT
}


public class OnHitEffects : MonoBehaviour
{
    [Serializable]
    private struct onHitEffectField
    {
        public string name;
        public HitEffectsEnum hitEffect;
        public Vector3 effectForce;
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
        // Pass in the damage direcitons as well
        // Note that particle position is currently checking 2 colliders against
        // eachother to get the point of contact.



        _effectedEntity.GetComponent<Animator>().SetInteger("AnyState/Damage", 1);
        _effectedEntity.GetComponent<Animator>().SetFloat("HitDirection/X", 1);
        _effectedEntity.GetComponent<Animator>().SetFloat("HitDirection/Z", 1);
    }

    public void ResolveKnockbackStrong(GameObject _effectedEntity)
    {
        int index = LocateHitEffect(HitEffectsEnum.KNOCKBACK_STRONG);
    }

    public void ResolveKnockdown(GameObject _effectedEntity)
    {
        _effectedEntity.GetComponent<Animator>().SetInteger("AnyState/Damage", 2);
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
