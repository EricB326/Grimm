/******************************
 Author: Jack Dumaresq
 Date Created: 20.07.2020
 Last Modified: 28.07.2020
 Purpose: To collect and store
 the players inputs
******************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;



public class InputBuffer
{
    public static string[] m_inputListString = new string[]
    {
        "Attack",
        "Roll"
    };

    public List<InputBufferItem> m_inputList = new List<InputBufferItem>();

    public void Update()
    {
        JacksMonobehaviour.jacksMonobehaviour.inputBuffer = this;
        if (m_inputList.Count < m_inputListString.Length || m_inputList.Count == 0)
        {
            InitializeBuffer();
        }

        foreach(InputBufferItem c in m_inputList)
        {
            c.ResolveCommand();

            for (int b = 0; b < c.m_buffer.Count - 1; b++)
            {
                c.m_buffer[b].m_hold = c.m_buffer[b + 1].m_hold;
                c.m_buffer[b].m_used = c.m_buffer[b + 1].m_used;
            }
        }
        
    }

    void InitializeBuffer()
    {
        m_inputList = new List<InputBufferItem>();
        foreach(string s in m_inputListString)
        {
            InputBufferItem newB = new InputBufferItem();
            newB.m_button = s;
            m_inputList.Add(newB);
        }
    }
}

public class InputBufferItem
{
    public string m_button;
    public List<InputBufferItemState> m_buffer;

    public static int bufferWindow = 12;

    public InputBufferItem()
    {
        m_buffer = new List<InputBufferItemState>();
        for (int i = 0; i < bufferWindow; i++)
        {
            m_buffer.Add(new InputBufferItemState());
        }
    }

    public void ResolveCommand()
    {
        if(Input.GetButton(m_button))
        {
            m_buffer[m_buffer.Count - 1].ButtonHold();
        }
        else
        {
            m_buffer[m_buffer.Count - 1].ReleaseHold();
        }
    }
}

/*
 @detail Informs the buffer of the I/O state
 of each button. Passes to Input Buffer Item
 */
public class InputBufferItemState
{
    public int m_hold;
    public bool m_used;

    public bool CanExecute()
    {
        if(m_hold == 1 && !m_used)
        {
            return true;
        }
        return false;
    }

    public void ButtonHold()
    {
        if (m_hold < 0)
        {
            m_hold = 1;
        }
        else
        {
            m_hold += 1;
        }
    }

    public void ReleaseHold()
    {
        if(m_hold > 0) 
        { 
            m_hold = -1; 
            m_used = false;
        }
        else
        {
            m_hold = 0;
        }
    }
}

