using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour 
{
	private void Start()
	{
		gameObject.SetActive(false);
	}
	public void Resume()
	{
		Time.timeScale = 1.0f;
		gameObject.SetActive(false);
	}

	public void Exit()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("MainMenu");
	}
}
