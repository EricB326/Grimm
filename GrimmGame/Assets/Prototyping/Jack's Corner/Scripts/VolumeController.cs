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
    }
    public void SFXVolumeChange()
    {
        volumeScript.sfxVolumeDB = volumeScript.lowerVolumeBound - ((_sfxSlider.value / _sfxSlider.maxValue) * volumeScript.lowerVolumeBound);
        volumeScript.SetBusVolume(VolumeScript.busIndex.sfx);
    }

    public void MusicVolumeChange()
    {
        volumeScript.musicVolumeDB = volumeScript.lowerVolumeBound - ((_musicSlider.value / _musicSlider.maxValue) * volumeScript.lowerVolumeBound);
        volumeScript.SetBusVolume(VolumeScript.busIndex.music);
    }

    public void AmbientVolumeChange()
    {
        volumeScript.ambienceVolumeDB = volumeScript.lowerVolumeBound - ((_ambienceSlider.value / _ambienceSlider.maxValue) * volumeScript.lowerVolumeBound);
        volumeScript.SetBusVolume(VolumeScript.busIndex.ambience);
    }
}