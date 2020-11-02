using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForLoading : MonoBehaviour
{
	[SerializeField] private GameObject child;

	private void Awake()
	{
		child.SetActive(false);
	}

	// Update is called once per frame
	void Update()
    {
		if (SceneManagement.instance.IsDone())
			child.SetActive(true);
    }
}
