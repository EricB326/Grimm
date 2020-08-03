using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacksMonobehaviour : MonoBehaviour
{
    public static JacksMonobehaviour jacksMonobehaviour;
    

    // Start is called before the first frame update
    void Start()
    {
        jacksMonobehaviour = this;
    }

    // Update is called once per frame
    void Update()
    {
        inputBuffer.Update();
    }


    public InputBuffer inputBuffer = new InputBuffer();
    void OnGUI()
    {
        for (int i = 0; i < inputBuffer.m_inputList.Count; i++)
        {
                GUI.Label(new Rect(20, i * 25, 100, 20), inputBuffer.m_inputList[i].m_button + ": ");

            for (int j = 0; j < inputBuffer.m_inputList[i].m_buffer.Count; j++)
            {

                GUI.Label(new Rect(j * 20 + 75, i * 25, 100, 20), inputBuffer.m_inputList[i].m_buffer[j].m_hold.ToString());

            }
        }
    }
}
