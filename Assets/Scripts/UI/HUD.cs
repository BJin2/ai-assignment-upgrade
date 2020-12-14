using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
	[SerializeField] private Text coolTime;
	[SerializeField] private Text resource;
	[SerializeField] private Text numOfEnemy;
	[SerializeField] private Text hp;

	public void UpdateResource(int r)
	{
		resource.text = r.ToString();
	}
	public void UpdateNumOfEnemy(int n)
	{
		numOfEnemy.text = n.ToString();
	}
	public void UpdateCoolTime(float c)
	{
		coolTime.text = Mathf.Floor(c).ToString();
	}
	public void UpdateHP(float h)
	{
		hp.text = h.ToString();
	}
	public void UpdateAll(int r, int n, float c, float h)
	{
		resource.text = r.ToString();
		numOfEnemy.text = n.ToString();
		coolTime.text = Mathf.Floor(c).ToString();
		hp.text = h.ToString();
	}
}
