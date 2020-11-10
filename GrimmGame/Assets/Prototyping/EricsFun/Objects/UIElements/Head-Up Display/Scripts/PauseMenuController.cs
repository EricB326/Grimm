using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XboxCtrlrInput;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject[] menuButtons;
    public GameObject optionsUI;
    public GameObject controlsUI;

    public GameObject defaultStartingButton;
    public GameObject optionsStartingButton;
    public GameObject controlsStartingButton;

    public static bool isPaused;
    public static bool isInOptions = false;
    public static bool isInControls = false;

    //public Camera blurCamera;
    //public Material blurMaterial;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);

        #region blur
        //if (blurCamera.targetTexture != null)
        //    blurCamera.targetTexture.Release();
        //
        //blurCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
        //blurMaterial.SetTexture("_RenTex", blurCamera.targetTexture);
        #endregion
    }

	// Update is called once per frame
	void Update()
    {
        // I hate this and am sorry for doing this. Especially every frame
        // Starting the cutscene could change a value here instead.
        bool disableControls = EntityStats.Instance.GetObjectOfEntity("Player").GetComponent<Player>().m_disableControls;
        if (Input.GetKeyDown(KeyCode.Escape) && !disableControls || XCI.GetButtonDown(XboxButton.Start) && !disableControls)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultStartingButton);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenSettings()
    {
        if (isInOptions)
        {
            isInOptions = false;
            foreach (GameObject button in menuButtons)
            {
                button.SetActive(true);

                foreach (Transform child in button.transform)
                {
                    Image this_image = child.GetComponent<Image>();
					if (this_image != null && this_image.name == "Highlight")
                        this_image.gameObject.SetActive(false);
				}
            }

            optionsUI.SetActive(false);

			EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultStartingButton);
        }
        else
        {
            isInOptions = true;
            foreach (GameObject button in menuButtons)
			{
				button.SetActive(false);
			}

			optionsUI.SetActive(true);

			foreach (Transform child in optionsUI.transform)
			{
                if (child.gameObject.name == "Back Button")
                {
                    foreach (Transform veryChild in child.transform)
                    {
                        Image this_image = veryChild.GetComponent<Image>();
                        if (this_image != null && this_image.name == "Highlight")
                            this_image.gameObject.SetActive(false);
                    }
                }
			}

			EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(optionsStartingButton);
		}
    }

    public void OpenControls()
    {
		if (isInControls)
		{
            isInControls = false;
			foreach (GameObject button in menuButtons)
			{
				button.SetActive(true);

				foreach (Transform child in button.transform)
				{
					Image this_image = child.GetComponent<Image>();
					if (this_image != null && this_image.name == "Highlight")
						this_image.gameObject.SetActive(false);
				}
			}

            controlsUI.SetActive(false);

			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(defaultStartingButton);
		}
		else
		{
            isInControls = true;
			foreach (GameObject button in menuButtons)
			{
				button.SetActive(false);
			}

            controlsUI.SetActive(true);

			foreach (Transform child in optionsUI.transform)
			{
				if (child.gameObject.name == "Back Button")
				{
					foreach (Transform veryChild in child.transform)
					{
						Image this_image = veryChild.GetComponent<Image>();
						if (this_image != null && this_image.name == "Highlight")
							this_image.gameObject.SetActive(false);
					}
				}
			}

			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(controlsStartingButton);
		}
	}

    public void ExitGame()
    {
        Application.Quit();
    }
}
