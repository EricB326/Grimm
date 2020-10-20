using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
	public GameObject volumeSlider;
	public GameObject sfxSlider;
	public GameObject musicSlider;
	public GameObject ambienceSlider;
	Slider _volumeSlider;
	Slider _sfxSlider;
	Slider _musicSlider;
	Slider _ambienceSlider;
	VolumeScript volumeScript;


	// Start is called before the first frame update
	void Start()
	{
		_volumeSlider = volumeSlider.GetComponent<Slider>();
		_sfxSlider = sfxSlider.GetComponent<Slider>();
		_musicSlider = musicSlider.GetComponent<Slider>();
		_ambienceSlider = ambienceSlider.GetComponent<Slider>();
		volumeScript = GetComponent<VolumeScript>();

		if (ImplSettingsFromMenu.Instance)
		{
			volumeScript.masterVolumeDB = ImplSettingsFromMenu.Instance.MasterVolume;
			_volumeSlider.value = ImplSettingsFromMenu.Instance.SliderMaster;

			volumeScript.sfxVolumeDB = ImplSettingsFromMenu.Instance.SFXVolume;
			_sfxSlider.value = ImplSettingsFromMenu.Instance.SliderSFX;

			volumeScript.musicVolumeDB = ImplSettingsFromMenu.Instance.MusicVolume;
			_musicSlider.value = ImplSettingsFromMenu.Instance.SliderMusic;

			volumeScript.ambienceVolumeDB = ImplSettingsFromMenu.Instance.AmbianceVolume;
			_ambienceSlider.value = ImplSettingsFromMenu.Instance.SliderAmbiance;
		}
	}

	public void MasterVolumeChange()
	{
		volumeScript.masterVolumeDB = volumeScript.lowerVolumeBound - ((_volumeSlider.value / _volumeSlider.maxValue) * volumeScript.lowerVolumeBound);
		volumeScript.SetBusVolume(VolumeScript.busIndex.master);

		if (ImplSettingsFromMenu.Instance)
		{ 
			ImplSettingsFromMenu.Instance.MasterVolume = volumeScript.masterVolumeDB;
			ImplSettingsFromMenu.Instance.SliderMaster = _volumeSlider.value;
		}
	}

	public void SFXVolumeChange()
	{
		volumeScript.sfxVolumeDB = volumeScript.lowerVolumeBound - ((_sfxSlider.value / _sfxSlider.maxValue) * volumeScript.lowerVolumeBound);
		volumeScript.SetBusVolume(VolumeScript.busIndex.sfx);

		if (ImplSettingsFromMenu.Instance)
		{ 
			ImplSettingsFromMenu.Instance.SFXVolume = volumeScript.sfxVolumeDB;
			ImplSettingsFromMenu.Instance.SliderSFX = _sfxSlider.value;
		}
	}

	public void MusicVolumeChange()
	{
		volumeScript.musicVolumeDB = volumeScript.lowerVolumeBound - ((_musicSlider.value / _musicSlider.maxValue) * volumeScript.lowerVolumeBound);
		volumeScript.SetBusVolume(VolumeScript.busIndex.music);

		if (ImplSettingsFromMenu.Instance)
		{
			ImplSettingsFromMenu.Instance.MusicVolume = volumeScript.musicVolumeDB;
			ImplSettingsFromMenu.Instance.SliderMusic = _musicSlider.value;
		}
	}

	public void AmbientVolumeChange()
	{
		volumeScript.ambienceVolumeDB = volumeScript.lowerVolumeBound - ((_ambienceSlider.value / _ambienceSlider.maxValue) * volumeScript.lowerVolumeBound);
		volumeScript.SetBusVolume(VolumeScript.busIndex.ambience);

		if (ImplSettingsFromMenu.Instance)
		{
			ImplSettingsFromMenu.Instance.AmbianceVolume = volumeScript.ambienceVolumeDB;
			ImplSettingsFromMenu.Instance.SliderAmbiance = _ambienceSlider.value;
		}
	}
}