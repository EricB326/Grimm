using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    private static CameraShaker instance;

    public CameraRotation cameraInfo;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public static CameraShaker Instance { get { return instance; } }

    public void startShake(float intensity)
    {
        for (int i = 0; i < 2; i++)
            cameraInfo.m_camList[cameraInfo.m_selectedCamera].GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
    }

    public void stopShake()
    {
        for (int i = 0; i < cameraInfo.m_camList.Count; i++)
            for (int j = 0; j < 2; j++)
                cameraInfo.m_camList[i].GetRig(j).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.0f;
    }
}
