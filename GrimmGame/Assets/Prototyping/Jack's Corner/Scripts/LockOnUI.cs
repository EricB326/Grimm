using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOnUI : MonoBehaviour
{
    //public Image UILockOnObject;
    public GameObject UILockOnObject;
    public Transform lockOnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraShaker.Instance.cameraInfo.m_selectedCamera == 1)
        {
            //UILockOnObject.enabled = true;
            UILockOnObject.SetActive(true);
            Vector3 lockOnPos = Camera.main.WorldToScreenPoint(lockOnPoint.position);
            UILockOnObject.transform.position = lockOnPos;
        }
        else
            //UILockOnObject.enabled = false;
            UILockOnObject.SetActive(false);
    }
}
