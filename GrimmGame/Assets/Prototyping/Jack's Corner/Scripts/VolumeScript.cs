using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeScript : MonoBehaviour
{
    #region bus and VCA initalisation
    //master
    private string masterBusString = "bus:/";
    private FMOD.Studio.Bus masterBus; 

    //sfx
    //[SerializeField]
    private string sfxBusString = "bus:/submaster/notmusic/sfx";
    private FMOD.Studio.Bus sfxBus;

    //music
    //[SerializeField]
    private string musicBusString = "bus:/submaster/music";
    private FMOD.Studio.Bus musicBus;

    //ambience
    //[SerializeField]
    private string ambienceBusString = "bus:/submaster/notmusic/ambience";
    private FMOD.Studio.Bus ambienceBus;

    #endregion

    #region volume variables
    public int lowerVolumeBound = -60;

    [SerializeField]
    [Range(-60, 0f)] //lower number here should be lowerVolumeBound
    public float masterVolumeDB, sfxVolumeDB, musicVolumeDB, ambienceVolumeDB;

    public enum busIndex
    {
        master,
        sfx,
        music,
        ambience
    }

    [FMODUnity.EventRef]
    public string masterExample, sfxExample, ambienceExample;

    [SerializeField]
    private Transform audioListener;
    #endregion

    void Awake()
    {
        SetBus(ref masterBus,   masterBusString);
        SetBus(ref sfxBus,      sfxBusString);
        SetBus(ref musicBus,    musicBusString);
        SetBus(ref ambienceBus, ambienceBusString);
    }

    void Update()
    {
        //sfxBus      .setVolume(DecimalToLinear(sfxVolumeDB));
        //musicBus    .setVolume(DecimalToLinear(musicVolumeDB));
        //ambienceBus .setVolume(DecimalToLinear(ambienceVolumeDB));

        //if(Input.GetKeyDown(KeyCode.Keypad0))
        //{
        //    masterBus.setVolume(0);
        //}
        //if (Input.GetKeyDown(KeyCode.Keypad1))
        //{
        //    masterBus.setVolume(1);
        //}

    }

    public void SetBusVolume(busIndex busIndex)
    {
        switch (busIndex)
        {
            case busIndex.master:
                masterBus.setVolume(DecimalToLinear(masterVolumeDB));
                FMODUnity.RuntimeManager.PlayOneShot(masterExample, audioListener.position);
                break;
            case busIndex.sfx:
                sfxBus.setVolume(DecimalToLinear(sfxVolumeDB));
                FMODUnity.RuntimeManager.PlayOneShot(sfxExample, audioListener.position);
                break;
            case busIndex.music:
                musicBus.setVolume(DecimalToLinear(musicVolumeDB));
                //FMODUnity.RuntimeManager.PlayOneShot(musicExample, audioListener.position);
                break;
            case busIndex.ambience:
                ambienceBus.setVolume(DecimalToLinear(ambienceVolumeDB));
                FMODUnity.RuntimeManager.PlayOneShot(ambienceExample, audioListener.position);
                break;
            default:
                Debug.Log("No volume bus specified");
                break;
        }
    }

    private float DecimalToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20f);
        return linear;
    }

    public void SetBus(ref FMOD.Studio.Bus bus, string path)
    {
        bus = FMODUnity.RuntimeManager.GetBus(path);
    }
}
