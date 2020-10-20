using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Framerate : MonoBehaviour
{
	[SerializeField] private TMP_Text framerateText;
	[SerializeField] private TMP_Text minFramerateText;
	[SerializeField] private TMP_Text maxFramerateText;
	[SerializeField] private TMP_Text avgFramerateText;

	private float frameCounter = 0f;

    private float minFramerate = 9999f;
    private float maxFramerate = 0f;

    private float averageFramerate = 0f;
    private float averageResetTimer = 0;

    private float timeCounter = 0f;
    private float refreshTime = 0.1f;

	// Update is called once per frame
	void Update()
    {
        if (!PauseMenuController.isPaused)
        { 
            if (timeCounter < refreshTime)
            {
                timeCounter += Time.deltaTime;
                frameCounter++;
            }
            else 
            {
                // Calculate the framerate.
                float lastFramerate = frameCounter / timeCounter;
			    framerateText.text = lastFramerate.ToString("n2");

                // Calculate min framerate.
                if (lastFramerate < minFramerate)
                {
                    minFramerate = lastFramerate;
                    minFramerateText.text = minFramerate.ToString("n2");
                }

                // Calculate max framerate.
                if (lastFramerate > maxFramerate)
                {
                    maxFramerate = lastFramerate;
                    maxFramerateText.text = maxFramerate.ToString("n2");
                }

                // Calculate average framerate.
			    averageFramerate += lastFramerate;
			    averageResetTimer++;

			    if (averageResetTimer >= 5)
			    {
				    averageFramerate /= 6;
                    avgFramerateText.text = averageFramerate.ToString("n2");
                    averageResetTimer = 0;

                }


                // Reset counters;
			    frameCounter = 0f;
			    timeCounter = 0f;
		    }
        }
    }
}
