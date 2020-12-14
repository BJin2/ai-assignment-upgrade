using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditText : MonoBehaviour 
{
	public void On()
	{
		transform.localPosition = new Vector3(0, -7, 90); 
	}
	public void Update()
	{
		transform.Translate(Vector3.up * 4 * Time.deltaTime);
		if (transform.localPosition.y > 5)
		{
			MainMenu.GetMainMenu().Back();
		}
	}
}
