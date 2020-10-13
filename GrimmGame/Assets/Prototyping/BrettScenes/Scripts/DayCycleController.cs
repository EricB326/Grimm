using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayCycleController : MonoBehaviour
{
    public bool disableDayNightCycle = false;

    [Range(0, 24)]
    public float TimeOfDay;

    public float orbitSpeed = 1.0f;
    public Light sun;
    public Light moon;
    public Volume skyVolume;
    public AnimationCurve starsCurve;

    public float happyBrettRotation;

    public bool changeRotation = true;

    private bool isNight;
    private PhysicallyBasedSky sky;

    // Start is called before the first frame update
    void Start()
    {
        skyVolume.profile.TryGet(out sky);
    }

    // Update is called once per frame
    void Update()
    {
        if (!disableDayNightCycle)
        {
            TimeOfDay += Time.deltaTime * orbitSpeed;
            if (TimeOfDay > 24)
                TimeOfDay = 0;

            UpdateTime();
        }
    }

    private void OnValidate()
    {
        skyVolume.profile.TryGet(out sky);
        UpdateTime();
    }

    private void UpdateTime()
    {
        float alpha = TimeOfDay / 24.0f;
        float sunRotation = Mathf.Lerp(-90, 270, alpha);
        float moonRotation = sunRotation - 180;



        sun.transform.rotation = Quaternion.Euler(sunRotation, happyBrettRotation, 0);
        moon.transform.rotation = Quaternion.Euler(moonRotation, happyBrettRotation, 0);

        sky.spaceEmissionMultiplier.value = starsCurve.Evaluate(alpha) * 0.5f;

        CheckNightDayTransition();
    }

    private void CheckNightDayTransition()
    {
        if (isNight)
        {
            if (moon.transform.rotation.eulerAngles.x > 175)
            {
                StartDay();
            }
        }
        else
        {
            {
                if (sun.transform.rotation.eulerAngles.x > 175)
                {
                    StartNight();
                }
            }
        }
    }

    private void StartDay()
    {
        HDAdditionalLightData sunData = sun.GetComponent<HDAdditionalLightData>();
        HDAdditionalLightData moonData = moon.GetComponent<HDAdditionalLightData>();
        isNight = false;
        sun.shadows = LightShadows.Soft;
        moon.shadows = LightShadows.None;
        sunData.UpdateAllLightValues();
        moonData.UpdateAllLightValues();
    }

    private void StartNight()
    {
        HDAdditionalLightData sunData = sun.GetComponent<HDAdditionalLightData>();
        HDAdditionalLightData moonData = moon.GetComponent<HDAdditionalLightData>();
        isNight = true;
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;
        sunData.UpdateAllLightValues();
        moonData.UpdateAllLightValues();
    }
}