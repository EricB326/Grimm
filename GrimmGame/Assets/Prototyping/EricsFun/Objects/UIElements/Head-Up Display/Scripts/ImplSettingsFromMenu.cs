using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImplSettingsFromMenu : MonoBehaviour
{
	private float resWidth, resHeight;
	private bool isFullscreen = true;
	private int qualitySettings, resolutionIndex = 1;
	private float masterVolume, sfxVolume, musicVolume, ambianceVolume;
	private float sliderMasterVolume = 100, sliderSfxVolume = 100, sliderMusicVolume = 100, sliderAmbianceVolume = 100;


    /* Instance of the singleton class
    */
    private static ImplSettingsFromMenu instance;

    /* @brief Set the instance of the singleton to this class data.
     */
    private void Awake()
    {
        // Assure that the object can be destroyed if has to be created again later.
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

		DontDestroyOnLoad(this);
	}

    /* @brief Get the instance of the static singleton.
    */
    public static ImplSettingsFromMenu Instance { get { return instance; } }

    public float ResWidth
    {
        get { return resWidth; }
        set { resWidth = value; }
    }

	public float ResHeight
	{
		get { return resHeight; }
		set { resHeight = value; }
	}

	public bool IsFullscreen
    {
		get { return isFullscreen; }
		set { isFullscreen = value; }
	}

	public int QualitySettings
    {
		get { return qualitySettings; }
		set { qualitySettings = value; }
	}

	public int ResolutionIndex
	{
		get { return resolutionIndex; }
		set { resolutionIndex = value; }
	}

	public float MasterVolume
    {
		get { return masterVolume; }
		set { masterVolume = value; }
	}

	public float SFXVolume
	{
		get { return sfxVolume; }
		set { sfxVolume = value; }
	}

	public float MusicVolume
	{
		get { return musicVolume; }
		set { musicVolume = value; }
	}

	public float AmbianceVolume
	{
		get { return ambianceVolume; }
		set { ambianceVolume = value; }
	}

	public float SliderMaster
	{
		get { return sliderMasterVolume; }
		set { sliderMasterVolume = value; }
	}

	public float SliderSFX
	{
		get { return sliderSfxVolume; }
		set { sliderSfxVolume = value; }
	}

	public float SliderMusic
	{
		get { return sliderMusicVolume; }
		set { sliderMusicVolume = value; }
	}

	public float SliderAmbiance
	{
		get { return sliderAmbianceVolume; }
		set { sliderAmbianceVolume = value; }
	}

}