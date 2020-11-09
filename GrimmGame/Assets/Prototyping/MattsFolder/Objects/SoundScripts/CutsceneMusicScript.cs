using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMusicScript : MonoBehaviour
{
    [SerializeField]
    private GameObject atmosObject; 
    private void OnEnable()
    {
        atmosObject.SetActive(false);
    }
}
