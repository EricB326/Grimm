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
    public GameObject test;
    public GameObject test2;
    public float fadeSpeed = 5f;
    public float fadeThreshold = 0.5f; //Otherwise the buttons don't properly fade
    bool optionsFade = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        Debug.Log("Scene Will Change");
        SceneManager.LoadScene(1);
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

    public void Credits()
    {
        Debug.Log("Scene Will Change");
        //SceneManager.LoadScene("Credits");
    }

    IEnumerator FadeImage(bool fade)
    {
        
        if (fade)
        {
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
            EventSystem.current.SetSelectedGameObject(test);
            optionsFade = false;
        }
        else
        {
            menuCollection.SetActive(true);
            EventSystem.current.SetSelectedGameObject(test2);
            optionCollection.SetActive(false);

            for (int i = 0; i < menuUIs.Length; i++)
            {
                Color textMeshColor = menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color;
                for (float j = 0; j <= 1 + fadeThreshold; j += Time.deltaTime * fadeSpeed)
                {
                    Debug.Log("Menu UI" + i + ": " + j);
                    
                    menuUIs[i].GetComponent<Image>().color = new Color(1, 1, 1, j);
                    menuUIs[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(textMeshColor.r, textMeshColor.g, textMeshColor.b, j);
                    yield return null;
                }
            }
            optionsFade = true;
        }
    }
}
