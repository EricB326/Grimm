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
// Send out input(Will send out the inputs to the player which will act on)

// This should probably be one of those serialized classes.
// It really shouldn't be added as
// These are the outs.
// The input buffer will make the decisions 
// about what to do with the input and output
// values to be used by the player.
public struct BufferInput
{
    public bool m_lightAttack;
    public bool m_heavyAttack;
    public bool m_run;
    public bool m_dash;
    public bool m_dataConsumed;
}



[Serializable]
public class BasicBuffer
{
    // This is output if no input.
    private BufferInput m_blankBuffer = new BufferInput();
    // This is the output if input received this frame
    // or within held frame range.
    private BufferInput m_storedBuffer;
    // If no input received for this time
    // will change any trues to falses.
    [Range(0, 50)]
    public int m_framesToRetainFor = 20;
    // How long to hold the dodge button 
    // before run.
    [Range(0, 50)]
    public float m_heldframesBeforeRun = 40;

    private int m_framesSinceLastInput = 0;

    private Buttons m_buttonInfo = new Buttons();

    // Badly named variable that gets gets set true
    // after running has drained all stamina.
    // Meant so that you have to repress the button.
    [HideInInspector]
    public bool m_staminaDrained = false;

    // Will need to be revisted in 
    // and automated
    public struct Buttons
    {

        // LIGHT ATTACK BUTTON

        // Was button was pressed this frame
        public bool m_lightAttackButton;
        // Value goes up everytime above is true
        // set to 0 if false.
        public int m_lightAttackheldFor;
        // Only occurs when held for is returned to 0.
        public bool m_lightAttackReleased;
        // In cases of multiple inputs at the same time
        // which should take priority
        public int m_lightAttackButtonPriority;

        // HEAVY ATTACK BUTTON

        // Was button was pressed this frame
        public bool m_heavyAttackButton;
        // Value goes up everytime above is true
        // set to 0 if false.
        public int m_heavyAttackheldFor;
        // Only occurs when held for is returned to 0.
        public bool m_heavyAttackReleased;
        // In cases of multiple inputs at the same time
        // which should take priority
        public int m_heavyAttackButtonPriority;


        // ROLL/RUN BUTTON

        // Was button was pressed this frame
        public bool m_rollRunButton;
        // Value goes up everytime above is true
        // set to 0 if false.
        public int m_rollRunHeldFor;
        // Only occurs when held for is returned to 0.
        public bool m_rollRunReleased;
        // In cases of multiple inputs at the same time
        // which should take priority
        public int m_rollRunButtonPriority;



        // Internal value to check if input received this frame
        public bool m_inputReceived;
    }

    void Start()
    {
        // Output
        m_blankBuffer.m_lightAttack = false;
        m_blankBuffer.m_heavyAttack = false;
        m_blankBuffer.m_run = false;
        m_blankBuffer.m_dash = false;
        // Buttons
        // May need a for loop
        // if alt/optional controls
        m_buttonInfo.m_lightAttackButton = false;
        m_buttonInfo.m_lightAttackReleased = false;
        m_buttonInfo.m_lightAttackheldFor = 0;

        m_buttonInfo.m_heavyAttackButton = false;
        m_buttonInfo.m_heavyAttackReleased = false;
        m_buttonInfo.m_heavyAttackheldFor = 0;


        m_buttonInfo.m_rollRunButton = false;
        m_buttonInfo.m_rollRunReleased = false;
        m_buttonInfo.m_rollRunHeldFor = 0;


        m_framesSinceLastInput = 0;
    }

    public BufferInput GetBufferInput()
    {
        CheckInput();
        return InterpretInput();
    }

    // Detects input and adjusts 
    // time held value accordingly.
    // Any extra buttons go here.

    // If we do rebindable controls there
    // would be a for loop here that checks these
    // on each input. Alternatively a button press
    // could fire an event that 
    private void CheckInput()
    {

        m_buttonInfo.m_lightAttackButton = false;
        m_buttonInfo.m_heavyAttackButton = false;
        m_buttonInfo.m_rollRunButton = false;
        m_buttonInfo.m_inputReceived = false;
        
        if (XCI.GetButton(XboxButton.RightBumper))        // Should be a value read out from somewhere.
        {
            m_buttonInfo.m_lightAttackButton = true;
            m_buttonInfo.m_inputReceived = true;
            m_buttonInfo.m_lightAttackheldFor++;
        }
        else
        {
            m_buttonInfo.m_lightAttackButton = false;
            if (m_buttonInfo.m_lightAttackheldFor > 0)
            {
                m_buttonInfo.m_lightAttackReleased = true;
                m_buttonInfo.m_lightAttackheldFor = 0;
            }
            else
            {
                m_buttonInfo.m_lightAttackReleased = false;
            }
        }

        if (XCI.GetButton(XboxButton.LeftBumper))        // Should be a value read out from somewhere.
        {
            m_buttonInfo.m_heavyAttackButton = true;
            m_buttonInfo.m_inputReceived = true;
            m_buttonInfo.m_heavyAttackheldFor++;
        }
        else
        {
            m_buttonInfo.m_heavyAttackButton = false;
            if (m_buttonInfo.m_heavyAttackheldFor > 0)
            {
                m_buttonInfo.m_heavyAttackReleased = true;
                m_buttonInfo.m_heavyAttackheldFor = 0;
            }
            else
            {
                m_buttonInfo.m_heavyAttackReleased = false;
            }
        }


        if (XCI.GetButton(XboxButton.B))                // Should be a value read out from somewhere.
        {
            m_buttonInfo.m_rollRunButton = true;
            m_buttonInfo.m_inputReceived = true;
            m_buttonInfo.m_rollRunHeldFor++;
        }
        else
        {
            m_buttonInfo.m_rollRunButton = false;
            if (m_buttonInfo.m_rollRunHeldFor > 0)
            {
                m_buttonInfo.m_rollRunReleased = true;
                m_buttonInfo.m_rollRunHeldFor = 0;
            }
            else
            {
                m_buttonInfo.m_rollRunReleased = false;
            }
        }
    }


    // What are the rules for the 
    // received input.
    // This does not cover the case of multiple button inputs.
    // Shit is fucked.


    private BufferInput InterpretInput()
    {
        // Firstly check if any input at all
        // To do the logic for held buttons
        if (m_buttonInfo.m_inputReceived)
        {
            if (m_buttonInfo.m_lightAttackButton)
            {
                // Need to check a condition here that will prevent it 
                // sending input in twice if it has been received
                if (m_buttonInfo.m_lightAttackheldFor <= 1)       
                {
                    m_storedBuffer.m_lightAttack = true;
                    m_storedBuffer.m_heavyAttack = false;
                    m_storedBuffer.m_run = false;
                    m_storedBuffer.m_dash = false;
                    m_storedBuffer.m_dataConsumed = false;
                    m_framesSinceLastInput = 0;
                    return m_storedBuffer;
                }
            }
            if(m_buttonInfo.m_heavyAttackButton)
            {
                if (m_buttonInfo.m_heavyAttackheldFor <= 1)
                {
                    m_storedBuffer.m_lightAttack = false;
                    m_storedBuffer.m_heavyAttack = true;
                    m_storedBuffer.m_run = false;
                    m_storedBuffer.m_dash = false;
                    m_storedBuffer.m_dataConsumed = false;
                    m_framesSinceLastInput = 0;
                    return m_storedBuffer;
                }
            }


            if (m_buttonInfo.m_rollRunButton)
            {
                if (m_buttonInfo.m_rollRunHeldFor > m_heldframesBeforeRun && !m_staminaDrained)
                {
                    m_storedBuffer.m_lightAttack = false;
                    m_storedBuffer.m_heavyAttack = false;
                    m_storedBuffer.m_run = true;
                    m_storedBuffer.m_dash = false;
                    m_storedBuffer.m_dataConsumed = false;
                    m_framesSinceLastInput = 0;
                    // Running stamina drain will need to occur here.
                    return m_storedBuffer;
                }
            }
            m_framesSinceLastInput++;
            return m_blankBuffer;
        }
        // Second check if any buttons were released
        // last frame and theframes since last input
        // is greater than 0. Might have issues with dodging
        // after running. Oh my god it actually works.
        else if (m_buttonInfo.m_rollRunReleased && m_framesSinceLastInput > 0 && !m_staminaDrained || m_buttonInfo.m_rollRunHeldFor > m_heldframesBeforeRun)
        {
            {
                m_storedBuffer.m_lightAttack = false;
                m_storedBuffer.m_heavyAttack = false;
                m_storedBuffer.m_run = false;
                m_storedBuffer.m_dash = true;
                m_storedBuffer.m_dataConsumed = false;
                m_framesSinceLastInput = 0;
                return m_storedBuffer;
            }
        }
        else
        // If input not received then need to check
        // last input received and see if it has exceeded time limit
        // or been "consumed" already.
        {
            m_staminaDrained = false;
            m_framesSinceLastInput++;           // I should seperate button inputs and any inputs not to be carried over go here.         
            if (m_storedBuffer.m_dataConsumed || m_framesSinceLastInput > m_framesToRetainFor || m_storedBuffer.m_run)
            {
                return m_blankBuffer;
            }
            else
            {
                return m_storedBuffer;
            }
        }
    }

    // When the animator receives an input
    // it 'consumes' the input until 
    // a new one is received.
    public void ConsumeInput()
    {
        m_storedBuffer.m_dataConsumed = true;
    }

}


