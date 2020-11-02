using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;

public class SceneManagement : MonoBehaviour
{
	public enum sceneIndicies
	{
		MANAGER = 0,
		TITLE = 1,
		GRIMM = 2
	}

	public static SceneManagement instance;
	[SerializeField] private GameObject loadingDisplay;
	[SerializeField] private Slider progBar;
	[SerializeField] private TextMeshProUGUI textDisplay;
	private bool isDone = false;

	private void Awake()
	{
		// Assure that the object can be destroyed if has to be created again later.
		if (instance != null && instance != this)
			Destroy(this.gameObject);
		else
			instance = this;

		SceneManager.LoadSceneAsync(Convert.ToInt32(sceneIndicies.TITLE), LoadSceneMode.Additive);
	}

	List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
	public void LoadGrimm()
	{
		loadingDisplay.SetActive(true);

		scenesLoading.Add(SceneManager.UnloadSceneAsync(Convert.ToInt32(sceneIndicies.TITLE)));
		scenesLoading.Add(SceneManager.LoadSceneAsync(Convert.ToInt32(sceneIndicies.GRIMM), LoadSceneMode.Additive));

		StartCoroutine(GetSceneLoadingProgress());
	}

	float totalSceneProgress;
	bool playerStarted = false;
	public IEnumerator GetSceneLoadingProgress()
	{
		for (int i = 0; i < scenesLoading.Count; i++)
		{
			while (!scenesLoading[i].isDone)
			{
				totalSceneProgress = 0;

				foreach (AsyncOperation this_op in scenesLoading)
				{
					totalSceneProgress += this_op.progress;
				}

				while (progBar.value != totalSceneProgress)
				{
					totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
					if (totalSceneProgress > 100f)
						totalSceneProgress = 100f;

					progBar.value = Mathf.Lerp(progBar.value, totalSceneProgress, .1f);
					progBar.value = Mathf.CeilToInt(progBar.value);

					yield return false;
				}

				yield return false;
			}
		}

		textDisplay.text = "Press 'A' on your controller to continue...";
		while (!playerStarted)
		{
			if (Input.GetKeyDown(KeyCode.Return) || XCI.GetButtonDown(XboxButton.A))
				playerStarted = true;

			yield return false;
		}

		loadingDisplay.SetActive(false);
		isDone = true;
	}

	public bool IsDone()
	{
		return isDone;
	}

}
