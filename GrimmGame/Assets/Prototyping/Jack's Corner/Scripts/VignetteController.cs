using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VignetteController : MonoBehaviour
{
    public float vignetteStart;
    public float maxVignetteStrength;
    private Volume v;
    private Vignette vg;

    // Update is called once per frame
    void Update()
    {
        v = GetComponent<Volume>();
        v.profile.TryGet(out vg);

        if(EntityStats.Instance.GetStaminaOfEntity("Player") <= vignetteStart)
        {
            float _maxVignetteStrength = maxVignetteStrength / 100.0f;
            float lerpTime = EntityStats.Instance.GetStaminaOfEntity("Player") / EntityStats.Instance.GetMaxStaminaOfEntity("Player");
            vg.intensity.value = _maxVignetteStrength - Mathf.Lerp(0, _maxVignetteStrength, lerpTime);
        }
    }
}
