using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// DEEFUNCT

// Mostly a class to store all input in 1 place.
// It will be in charge of:
// What the input is.
// Recording when the input occured in time.
// Which input to pass next if a request is made.
// When to discard an input.
// Doing this will also alow for multiple button 
// press prompts.


    [Serializable]
    // The funcitons and containter for the inputs.
    // Allows for multiple presses of inputs per frame.
    // Could have some form of logic that 
public class DEFUNCTInputBuffer
{
    // Not in the right place.
    public float m_timeBeforeDiscard;
    public List<InputBufferVariables> inputs = new List<InputBufferVariables>();
}


[Serializable]
// The date types
public class InputBufferVariables
{
    // The key pressed using unitys input system
    public KeyCode m_keyPress;
    // On the frame the input is detected.
    public float m_detectedWhen;
  

}
