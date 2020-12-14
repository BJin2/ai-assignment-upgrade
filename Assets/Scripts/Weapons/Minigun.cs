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

		if(barrel)
			barrel.Rotate(Vector3.forward * 180 * Time.deltaTime);
	}
}
