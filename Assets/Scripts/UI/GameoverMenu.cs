using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverMenu : MonoBehaviour 
{
	private void Start () 
	{
		gameObject.SetActive(false);
	}
	public void Restart()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
	}
	public void Exit()
	{
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}
