using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Machinegun 
{
	private Transform barrel;

	private void Awake()
	{
		barrel = transform.Find("Barrel");
	}
	private new void Update()
	{
		base.Update();
		//*
		barrel.localEulerAngles = new Vector3(barrel.rotation.eulerAngles.x,
											barrel.rotation.eulerAngles.y + 90 * Time.deltaTime * Player.PlayTimeScale,
											barrel.rotation.eulerAngles.z);
		//*/
	}
}
