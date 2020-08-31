using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
   [SerializeField] private Image backgroundBarImage;
   [SerializeField] private Image foregroundBarImage;
   [SerializeField] private float sliderBarDeplenishSpeed;
   [SerializeField] private float sliderBarDeplenishTimer;
   private float sliderTimer;

    private void Start()
    {
        SetBarPercentage(EntityStats.Instance.GetNormalisedStaminaOfEntity("Player"));
        backgroundBarImage.fillAmount = foregroundBarImage.fillAmount;
        
        EntityStats.Instance.onStaminaTaken += EntityStats_OnStaminaTaken;
        EntityStats.Instance.onStaminaReplenish += EntityStats_OnStaminaReplenish;
    }

    private void Update()
    {
        sliderTimer -= Time.deltaTime;
        if (sliderTimer < 0f)
        {
            if (foregroundBarImage.fillAmount < backgroundBarImage.fillAmount)
                backgroundBarImage.fillAmount -= sliderBarDeplenishSpeed * Time.deltaTime;
            else
                backgroundBarImage.fillAmount = foregroundBarImage.fillAmount;


        }
    }

    private void EntityStats_OnStaminaTaken(object sender, System.EventArgs e)
    {
        sliderTimer = sliderBarDeplenishTimer;
        SetBarPercentage(EntityStats.Instance.GetNormalisedStaminaOfEntity("Player"));
    }    
    
    private void EntityStats_OnStaminaReplenish(object sender, System.EventArgs e)
    {
        sliderTimer = sliderBarDeplenishTimer;
        SetBarPercentage(EntityStats.Instance.GetNormalisedStaminaOfEntity("Player"));
    }

    private void SetBarPercentage(float _healthNormalized)
    {
        foregroundBarImage.fillAmount = _healthNormalized;
    }
}
