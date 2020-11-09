using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicScript : MonoBehaviour
{
    private int bossPhase;
    private FMODUnity.StudioEventEmitter bossMusicEmitter;
    [SerializeField]
    private GameObject atmosObject, cutsceneMusicObject;

    private void Awake()
    {
        bossPhase = 0;
        bossMusicEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void OnEnable()
    {
        cutsceneMusicObject.SetActive(false);
    }

    public void IncrementBossPhase()
    {
        bossPhase++;

        if(bossPhase < 3)
        {
            bossMusicEmitter.SetParameter("boss phase", bossPhase);
        } else
        {
            atmosObject.SetActive(true);
            atmosObject.GetComponent<AtmosSound>().RavenIsDead();
            gameObject.SetActive(false);
        }
    }
}
