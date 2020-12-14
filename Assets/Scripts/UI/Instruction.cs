using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instruction : MonoBehaviour 
{
	[SerializeField]
	private Sprite[] instPic = null;

	private Image inst = null;
	private int imgIndex = 0;

	private void Awake()
	{
		imgIndex = 0;
		inst = transform.Find("Image").GetComponent<Image>();
		inst.sprite = instPic[imgIndex];
		Debug.Log(inst);
	}

	public void NextPicture()
	{
		imgIndex++;
		imgIndex %= instPic.Length;
		inst.sprite = instPic[imgIndex];
	}
	public void PreviousPicture()
	{
		imgIndex--;
		if (imgIndex < 0)
			imgIndex = instPic.Length - 1;
		inst.sprite = instPic[imgIndex];
	}
}
