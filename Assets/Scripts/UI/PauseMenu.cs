using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMP = TMPro.TextMeshProUGUI;

public class PauseMenu : Overlay 
{
	private readonly Color translucent = new Color(0, 0, 0, 0.74f);

	private Image bg = null;
	private Image BG
	{
		get
		{
			if (bg == null)
				bg = transform.Find("BG").GetComponent<Image>();
			return bg;
		}
	}

	private TMP title = null;
	private TMP Title
	{
		get
		{
			if (title == null)
				title = transform.Find("Title").GetComponent<TMP>();
			return title;
		}
	}

	private Dictionary<string, Button> buttons = null;

	private void TurnOn()
	{
		foreach (Button button in buttons.Values)
		{
			button.gameObject.SetActive(true);
		}
		gameObject.SetActive(true);
	}

	private void Pause()
	{
		TurnOn();
		buttons["Restart"].gameObject.SetActive(false);
		Time.timeScale = 0.0f;
		BG.color = translucent;
		Title.text = "P A U S E";
	}
	private void Lose()
	{
		TurnOn();
		buttons["Resume"].gameObject.SetActive(false);
		Title.text = "Time paradox occurred";
		CameraMove.Instance.GetComponent<Animator>().Play("Death");
		gameObject.GetComponent<Animator>().Play("Death");
	}

	public override void Initialize()
	{
		if (buttons == null)
		{
			buttons = new Dictionary<string, Button>();
			foreach (Button button in transform.Find("Buttons").GetComponentsInChildren<Button>())
			{
				buttons.Add(button.gameObject.name, button);
			}
		}

		buttons["Resume"].onClick.AddListener(()=>
		{
			Time.timeScale = 1.0f;
			gameObject.SetActive(false);
		});
		buttons["Restart"].onClick.AddListener(()=>
		{
			Time.timeScale = 1.0f;
			SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
		});
		buttons["Exit"].onClick.AddListener(()=> 
		{
			Time.timeScale = 1.0f;
			SceneManager.LoadScene("MainMenu");
		});
		Player.Instance.OnDeath += Lose;
		Player.Instance.OnPause += Pause;
	}
}
