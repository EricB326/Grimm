using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnUI : MonoBehaviour
{
    public Image[] UILockOnObjects;
    public Transform[] lockOnPoint;
    Transform closestTransform;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        closestTransform = lockOnPoint[0];
        player = EntityStats.Instance.GetObjectOfEntity("Player"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraShaker.Instance.cameraInfo.m_selectedCamera == 1)
        {
            for (int i = 0; i < lockOnPoint.Length; i++)
            {
                if (Vector3.Distance(player.transform.position, closestTransform.position) > Vector3.Distance(player.transform.position, lockOnPoint[i].position))
                {
                    closestTransform = lockOnPoint[i];
                }
            }
            for (int i = 0; i < UILockOnObjects.Length; i++)
            {
                UILockOnObjects[i].enabled = true;
                Vector3 lockOnPos = Camera.main.WorldToScreenPoint(closestTransform.position);
                UILockOnObjects[i].transform.position = lockOnPos;
            }
        }
        else
            for (int i = 0; i < UILockOnObjects.Length; i++)
            {
                UILockOnObjects[i].enabled = false;
            }
    }
}
