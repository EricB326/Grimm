using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


// This is a basic buffer just to see what I
// can get done without reference.
// It needs to detect input
// Inturpret input(eg Was button held or pressed by checking last frame)?
// Decide when input should be discarded or overwritten(Has the buffer time expired or another input received.)
// Send out input(Will send out the inputs to the player which will 


public class BasicBuffer : MonoBehaviour
{
    // This is output if no input.
    private BufferInput m_blankBuffer = new BufferInput();
    // This is the output if input received this frame
    // or within held frame range.
    private BufferInput m_storedBuffer;
    // If no input received for this time
    // will change any trues to falses.
    [Range(0,50)]
    public int m_framesToRetainFor = 20;
    // How long to hold the dodge button 
    // before run.
    [Range(0,20)]
    public float m_heldframesBeforeRun = 15;
    
    private int m_framesSinceLastInput = 0;

    private Buttons m_buttonInfo = new Buttons();

 
    public struct Buttons
    {
        // Was button was pressed this frame
        public bool m_aButton;
        // Value goes up everytime above is true
        // set to 0 if false.
        public int m_aheldFor;
        // In cases of multiple inputs at the same time
        // which should take priority
        public int m_aButtonPriority;
        // Was button was pressed this frame
        public bool m_bButton;
        // Value goes up everytime above is true
        // set to 0 if false.
        public int m_bHeldFor;
        // In cases of multiple inputs at the same time
        // which should take priority
        public int m_bButtonPriority;
        // Internal value to check if input received this frame
        public bool m_inputReceived;
    }

    void Start()
    {
        m_blankBuffer.m_attack = false;
        m_blankBuffer.m_run = false;
        m_blankBuffer.m_dash = false;
        m_buttonInfo.m_aButton = false;
        m_buttonInfo.m_aheldFor = 0;
        m_buttonInfo.m_bButton = false;
        m_buttonInfo.m_bHeldFor = 0;
        m_framesSinceLastInput = 0;
}


    // This probably doesn't need to be an update
    // Since it will occur every frame.
    void Update()
    {
        
    }



    public BufferInput GetBufferInput()
    {
        CheckInput();
        return InterpretInput();
    }

    // Detects input and adjusts 
    // time held value accordingly.
    // Any extra buttons go here.
    private void CheckInput()
    {
        
        m_buttonInfo.m_aButton = false;
        m_buttonInfo.m_bButton = false;
        m_buttonInfo.m_inputReceived = false;
        if (XCI.GetButton(XboxButton.A))
        {
            m_buttonInfo.m_aButton = true;
            m_buttonInfo.m_inputReceived = true;
            m_buttonInfo.m_aheldFor++;
        }
        else
        {
            m_buttonInfo.m_aButton = false;
            m_buttonInfo.m_aheldFor = 0;
        }
        if (XCI.GetButton(XboxButton.B))
        {
            m_buttonInfo.m_bButton = true;
            m_buttonInfo.m_inputReceived = true;
            m_buttonInfo.m_bHeldFor++;
        }
        else
        {
            m_buttonInfo.m_bButton = false;
            m_buttonInfo.m_bHeldFor = 0;
        }
    }


    // What are the rules for the 
    // received input.
    private BufferInput InterpretInput()
    {
        // Firstly check if any input at all
        if (m_buttonInfo.m_inputReceived)
        {
            if (m_buttonInfo.m_aButton)
            {
                // Need to check a condition here that will prevent it 
                // sending input in twice if it has been received
                if(m_buttonInfo.m_aheldFor > 1)
                {
                    return m_blankBuffer;
                }
                else
                {
                    m_storedBuffer.m_attack = true;
                    m_storedBuffer.m_run = false;
                    m_storedBuffer.m_dash = false;
                    m_storedBuffer.m_dataConsumed = false;
                    m_framesSinceLastInput = 0;
                    return m_storedBuffer;
                }
            }
            if (m_buttonInfo.m_bButton)
            {
                if (m_buttonInfo.m_bHeldFor > m_heldframesBeforeRun)
                {
                    m_storedBuffer.m_attack = false;
                    m_storedBuffer.m_run = true;
                    m_storedBuffer.m_dash = false;
                    m_storedBuffer.m_dataConsumed = false;
                    m_framesSinceLastInput = 0;
                    return m_storedBuffer;
                }
                if (m_buttonInfo.m_bHeldFor > m_heldframesBeforeRun || m_storedBuffer.m_dataConsumed)
                {
                    return m_blankBuffer;
                }
                else
                {
                    m_storedBuffer.m_attack = false;
                    m_storedBuffer.m_run = false;
                    m_storedBuffer.m_dash = true;
                    m_storedBuffer.m_dataConsumed = false;
                    m_framesSinceLastInput = 0;
                    return m_storedBuffer;
                }
            }
            // Well technically it wont make it here 
            Debug.Log("Something went wrong in the Interpret Buffer function in the basic" +
                "buffer");
            return m_blankBuffer;

        }
        // If input not received then need to check
        // last input received and see if it has exceeded time limit
        // or been "consumed" already.
        else
        {
            m_framesSinceLastInput++;
            if(m_framesSinceLastInput > m_framesToRetainFor || m_storedBuffer.m_dataConsumed)
            {
                return m_blankBuffer;
            }
            else
            {
                return m_storedBuffer;
            }
        }
    }

    public void ConsumeInput()
    {
        m_storedBuffer.m_dataConsumed = true;
    }


}
// These are the outs.
// The input buffer will make the decisions 
// about what to do with the input and output
// values to be used by the player.
public struct BufferInput
{
    public bool m_attack;
    public bool m_run;
    public bool m_dash;
    // I think I can make this simpler by
    // setting the m_attack on the stored buffer
    // to 0 when consumed data is called by the state
    // machine.
    public bool m_dataConsumed;
}