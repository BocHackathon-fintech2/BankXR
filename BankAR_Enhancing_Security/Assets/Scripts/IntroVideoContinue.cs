using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroVideoContinue : MonoBehaviour 
{
	[SerializeField]
	private float delayBeforeLoading = 40f;
	[SerializeField]
	private string sceneNameToLoad;

	private float timeElapsed;

	private void Update()
	{
		timeElapsed += Time.deltaTime;

		if ((timeElapsed > delayBeforeLoading) || (Input.GetKey(KeyCode.Escape)) || Input.GetMouseButtonDown(0)) 
		{
			SceneManager.LoadScene (sceneNameToLoad);
		}
	}


}
