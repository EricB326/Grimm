//========== Grimm - GraphicsSelector.cs - 12/10/2020 ==========//
// Author:  Eric Brkic
// Purpose: 
//=========================================================//

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSelector : MonoBehaviour
{
    public TMP_Dropdown resDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

	private void Awake()
	{
        QualitySettings.SetQualityLevel(qualityDropdown.value);

        if (fullscreenToggle.isOn)
			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
		else
			Screen.fullScreenMode = FullScreenMode.Windowed;
	}

	public void SetQuality(int a_qualityIndex)
    {
        QualitySettings.SetQualityLevel(a_qualityIndex);
        Debug.Log(a_qualityIndex);
    }

    public void SetResolution(int a_resIndex)
    {
        string[] splitRes = resDropdown.options[a_resIndex].text.Split(' ');
        int xRes = Convert.ToInt32(splitRes[0]);
        int yRes = Convert.ToInt32(splitRes[2]);

        Screen.SetResolution(xRes, yRes, fullscreenToggle.isOn);
    }

    public void SetFullscreen()
    {
        if (fullscreenToggle.isOn)
			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;
    }
}