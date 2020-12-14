using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	private GameObject mainMenu;
	private GameObject instructionMenu;
	private GameObject creditMenu;
	private void Awake()
	{
		mainMenu = GameObject.Find("MainMenu");
		instructionMenu = GameObject.Find("Instruction");
		creditMenu = GameObject.Find("Credit");
	}
	private void Start()
	{
		instructionMenu.SetActive(false);
		creditMenu.SetActive(false);
	}
	public void StartGame()
	{
		SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
	}
	public void Credit()
	{
		mainMenu.SetActive(false);
		creditMenu.SetActive(true);
		creditMenu.transform.Find("CreditText").GetComponent<CreditText>().On();
	}
	public void Instruction()
	{
		mainMenu.SetActive(false);
		instructionMenu.SetActive(true);
	}
	public void ExitGame()
	{
		Application.Quit();
	}
	public void Back()
	{
		mainMenu.SetActive(true);
		instructionMenu.SetActive(false);
		creditMenu.SetActive(false);
	}
	public static MainMenu GetMainMenu()
	{
		return GameObject.Find("Canvas").GetComponent<MainMenu>();
	}
}
