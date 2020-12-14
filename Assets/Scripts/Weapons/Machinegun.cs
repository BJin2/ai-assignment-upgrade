using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machinegun : Weapon 
{
	protected float effectCounter = 0;
	protected bool isEffectOn = false;
	[SerializeField] protected GameObject effect_02;	// dust
	[SerializeField] protected GameObject effect_03;	// blood
	[SerializeField] protected GameObject effect_04;	// spark

	private void Start()
	{
		gameObject.SetActive(false);
	}
	public override void Fire(Enemy enemy, Vector3 hitPoint)
	{
		GameObject secondEffect;
		if (enemy == null)
		{
			secondEffect = (GameObject)Instantiate(effect_02, hitPoint, effect_02.transform.rotation);
			Debug.Log("No Target");
		}
		else
		{
			switch (enemy.GetEnemyType())
			{
				case 0: // metal
					secondEffect = (GameObject)Instantiate(effect_04, hitPoint, effect_03.transform.rotation);
					break;
				case 1: // human
					secondEffect = (GameObject)Instantiate(effect_03, hitPoint, effect_03.transform.rotation);
					break;
				default :
					secondEffect = null;
					break;
			}
			
			enemy.Attacked(damage);
		}

		Player.GetPlayer().CoolDown(false);

		CameraMove.GetCam().Recoil(recoil);

		effect.SetActive(true);
		isEffectOn = true;
		Destroy(secondEffect, 1);

		Debug.Log("Shooting");
	}
	public override void WeaponActivated()
	{

	}
	public override void WeaponDeactivated()
	{

	}
	protected void Update()
	{
		coolCounter += Time.deltaTime * Player.PlayTimeScale;
		if (coolCounter >= coolTime)
		{
			Player.GetPlayer().CoolDown(true);
			coolCounter = 0;
		}

		if (isEffectOn)
		{
			effectCounter += Time.deltaTime * Player.PlayTimeScale;
			if (effectCounter > 0.05f)
			{
				effect.SetActive(false);
				effectCounter = 0;
				isEffectOn = false;
			}
		}
	}
}
