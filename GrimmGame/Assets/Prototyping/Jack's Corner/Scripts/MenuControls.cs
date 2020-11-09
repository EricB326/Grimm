using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour
{
    public GameObject[] menuUIs;
    public GameObject menuCollection;
    public GameObject optionCollection;
    public GameObject controlsCollection;
    public GameObject creditsCollection;
    public GameObject test;
    public GameObject test2;
    public GameObject controlsBack;
    public GameObject creditsBack;
    public float fadeSpeed = 5f;
    public float fadeThreshold = 0.5f; //Otherwise the buttons don't properly fade
    bool optionsFade = true;
    bool controlsFade = true;
    bool creditsFade = true;

    // Start is called before the first frame update
    void Start()
    {
		//EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(test2);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManagement.instance.LoadGrimm();
    }

    public void Options()
    {        
        StartCoroutine(FadeImage(optionsFade));
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void Controls()
    {
        StartCoroutine(FadeImageControls(controlsFade));
	}

	public void Credits()
	{
		StartCoroutine(FadeImageCredits(creditsFade));
	}

	IEnumerator FadeImage(bool fade)
    { 
        if (fade)
        {
			foreach (GameObject button in menuUIs)
			{
				button.SetActive(true);

				foreach (Transform child in button.transform)
				{
					Image this_image = child.GetComponent<Image>();
					if (this_image != null && this_image.name == "Highlighted")
						this_image.gameObject.SetActive(false);
				}
			}
            
			for (int i = 0; i < menuUIs.Length; i++)
            {
                Color textMeshColor = menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color;
                for (float j = 1; j >= 0 - fadeThreshold; j -= Time.deltaTime * fadeSpeed)
                {
                    menuUIs[i].GetComponent<Image>().color = new Color(1, 1, 1, j);
                    menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(textMeshColor.r, textMeshColor.g, textMeshColor.b, j);
                    
                    yield return null;
                }
            }

			menuCollection.SetActive(false);
            optionCollection.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(test);
            optionsFade = false;
		}
        else
        {
            menuCollection.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(test2);
            optionCollection.SetActive(false);

			for (int i = 0; i < menuUIs.Length; i++)
            {
                Color textMeshColor = menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color;
                for (float j = 0; j <= 1 + fadeThreshold; j += Time.deltaTime * fadeSpeed)
                {
                    menuUIs[i].GetComponent<Image>().color = new Color(1, 1, 1, j);
                    menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(textMeshColor.r, textMeshColor.g, textMeshColor.b, j);
                    yield return null;
                }
            }

			foreach (Transform child in optionCollection.transform)
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

			optionsFade = true;
        }
    }

	IEnumerator FadeImageControls(bool fade)
	{
		if (fade)
		{
			foreach (GameObject button in menuUIs)
			{
				button.SetActive(true);

				foreach (Transform child in button.transform)
				{
					Image this_image = child.GetComponent<Image>();
					if (this_image != null && this_image.name == "Highlighted")
						this_image.gameObject.SetActive(false);
				}
			}

			for (int i = 0; i < menuUIs.Length; i++)
			{
				Color textMeshColor = menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color;
				for (float j = 1; j >= 0 - fadeThreshold; j -= Time.deltaTime * fadeSpeed)
				{
					menuUIs[i].GetComponent<Image>().color = new Color(1, 1, 1, j);
					menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(textMeshColor.r, textMeshColor.g, textMeshColor.b, j);

					yield return null;
				}
			}

			menuCollection.SetActive(false);
			controlsCollection.SetActive(true);
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(controlsBack);
			controlsFade = false;
		}
		else
		{
			menuCollection.SetActive(true);
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(test2);
			controlsCollection.SetActive(false);

			for (int i = 0; i < menuUIs.Length; i++)
			{
				Color textMeshColor = menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color;
				for (float j = 0; j <= 1 + fadeThreshold; j += Time.deltaTime * fadeSpeed)
				{
					menuUIs[i].GetComponent<Image>().color = new Color(1, 1, 1, j);
					menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(textMeshColor.r, textMeshColor.g, textMeshColor.b, j);
					yield return null;
				}
			}

			foreach (Transform child in controlsCollection.transform)
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

			controlsFade = true;
		}
	}

	IEnumerator FadeImageCredits(bool fade)
	{
		if (fade)
		{
			foreach (GameObject button in menuUIs)
			{
				button.SetActive(true);

				foreach (Transform child in button.transform)
				{
					Image this_image = child.GetComponent<Image>();
					if (this_image != null && this_image.name == "Highlighted")
						this_image.gameObject.SetActive(false);
				}
			}

			for (int i = 0; i < menuUIs.Length; i++)
			{
				Color textMeshColor = menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color;
				for (float j = 1; j >= 0 - fadeThreshold; j -= Time.deltaTime * fadeSpeed)
				{
					menuUIs[i].GetComponent<Image>().color = new Color(1, 1, 1, j);
					menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(textMeshColor.r, textMeshColor.g, textMeshColor.b, j);

					yield return null;
				}
			}

			menuCollection.SetActive(false);
			creditsCollection.SetActive(true);
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(creditsBack);
			creditsFade = false;
		}
		else
		{
			menuCollection.SetActive(true);
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(test2);
			creditsCollection.SetActive(false);

			for (int i = 0; i < menuUIs.Length; i++)
			{
				Color textMeshColor = menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color;
				for (float j = 0; j <= 1 + fadeThreshold; j += Time.deltaTime * fadeSpeed)
				{
					menuUIs[i].GetComponent<Image>().color = new Color(1, 1, 1, j);
					menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(textMeshColor.r, textMeshColor.g, textMeshColor.b, j);
					yield return null;
				}
			}

			foreach (Transform child in creditsCollection.transform)
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

			creditsFade = true;
		}
	}
}
