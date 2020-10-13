using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public GameObject volumeSlider;
    Slider _volumeSlider;
    VolumeScript volumeScript;

    // Start is called before the first frame update
    void Start()
    {
        _volumeSlider = volumeSlider.GetComponent<Slider>();
        volumeScript = GetComponent<VolumeScript>();
    }

    public void MasterVolumeChange()
    {
        volumeScript.masterVolumeDB = volumeScript.lowerVolumeBound - ((_volumeSlider.value / _volumeSlider.maxValue) * volumeScript.lowerVolumeBound);
        volumeScript.SetBusVolume(VolumeScript.busIndex.master);
    }

    public void SFXVolumeChange()
    {
        volumeScript.sfxVolumeDB = volumeScript.lowerVolumeBound - ((_volumeSlider.value / _volumeSlider.maxValue) * volumeScript.lowerVolumeBound);
        volumeScript.SetBusVolume(VolumeScript.busIndex.sfx);
    }

    public void MusicVolumeChange()
    {
        volumeScript.musicVolumeDB = volumeScript.lowerVolumeBound - ((_volumeSlider.value / _volumeSlider.maxValue) * volumeScript.lowerVolumeBound);
        volumeScript.SetBusVolume(VolumeScript.busIndex.music);
    }

    public void AmbientVolumeChange()
    {
        volumeScript.ambienceVolumeDB = volumeScript.lowerVolumeBound - ((_volumeSlider.value / _volumeSlider.maxValue) * volumeScript.lowerVolumeBound);
        volumeScript.SetBusVolume(VolumeScript.busIndex.ambience);
    }
}
