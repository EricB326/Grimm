using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Serializable]
    private struct healthBarData
    {
        public string name;
        public Image sliderBarImage;
        public Image healthBarImage;
        public float sliderBarDeplenishSpeed;
        public float sliderBarDeplenishTimer;
        [HideInInspector] public float sliderTimer;

        /* Default constructor on a per entity bases. Trying to get this to still work within inspector view.
           Note: Beginning to think it isnt possible to have structs have base values by default within the inspector prior runtime. Going to keep this here,
           for when and if a new entity needs to be instantiated.
        */
        //private healthBarData(string _name = "New Entity", GameObject _entityObject = null, float _health = 100f, float _stamina = 50f, float _timeBeforeStaminaRegain = 3f, float _speedOfStaminaRegain = 3f)
        //{}
    }

    [SerializeField] private List<healthBarData> healthBarList = new List<healthBarData>();
    private healthBarData holdingData;

    private void Start()
    {
        foreach (healthBarData thisHealthBar in healthBarList)
        {
            SetBarPercentage(thisHealthBar, EntityStats.Instance.GetNormalisedHealthOfEntity(thisHealthBar.name));
            thisHealthBar.sliderBarImage.fillAmount = thisHealthBar.healthBarImage.fillAmount;
        }

        EntityStats.Instance.onDamageTaken += EntityStats_OnDamage;
        EntityStats.Instance.onHealthReplenish += EntityStats_OnHealthReplenish;
    }

    private void Update()
    { 
        for (int i = 0; i < healthBarList.Count; i++)
        {
            holdingData = healthBarList[i];
            holdingData.sliderTimer -= Time.deltaTime;
            healthBarList[i] = holdingData;

            if (healthBarList[i].sliderTimer < 0f)
            {
                if (healthBarList[i].healthBarImage.fillAmount < healthBarList[i].sliderBarImage.fillAmount)
                    healthBarList[i].sliderBarImage.fillAmount -= healthBarList[i].sliderBarDeplenishSpeed * Time.deltaTime;
            }
            else if (healthBarList[i].healthBarImage.fillAmount > healthBarList[i].sliderBarImage.fillAmount)
                healthBarList[i].sliderBarImage.fillAmount = healthBarList[i].healthBarImage.fillAmount; ;
        }
    }

    private void EntityStats_OnDamage(object sender, System.EventArgs e)
    {
        for (int i = 0; i < healthBarList.Count; i++)
        {
            // If the health bar's current value is not equal to the new value, then the bar should update.
            if (healthBarList[i].healthBarImage.fillAmount != EntityStats.Instance.GetNormalisedHealthOfEntity(healthBarList[i].name))
            {
                holdingData = healthBarList[i];
                holdingData.sliderTimer = holdingData.sliderBarDeplenishTimer;
                healthBarList[i] = holdingData;
                SetBarPercentage(healthBarList[i], EntityStats.Instance.GetNormalisedHealthOfEntity(healthBarList[i].name));
            }
        }
    }    
    
    private void EntityStats_OnHealthReplenish(object sender, System.EventArgs e)
    {
        for (int i = 0; i < healthBarList.Count; i++)
        {
            holdingData = healthBarList[i];
            holdingData.sliderTimer = holdingData.sliderBarDeplenishTimer;
            healthBarList[i] = holdingData;

            SetBarPercentage(healthBarList[i], EntityStats.Instance.GetNormalisedHealthOfEntity(healthBarList[i].name));
        }
    }

    private void SetBarPercentage(healthBarData _healthBar, float _healthNormalized)
    {
        _healthBar.healthBarImage.fillAmount = _healthNormalized;
    }
}
