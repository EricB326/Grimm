using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkFootVariables : MonoBehaviour
{
    [Range(-1,1)]
    public float m_castDown = 0;
    [Range(-1, 1)]
    public float m_castUp = 0;

    [Range(-1, 1)]
    public float m_offsetUp = 0;
    [Range(-1, 1)]
    public float m_offsetDown = 0;

    public GameObject m_leftFoot;
    public GameObject m_rightFoot;

    private void Start()
    {

    }
}
