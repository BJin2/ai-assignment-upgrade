using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : Weapon 
{
	private static Transform aim;
	private Transform spawner;

	private void Awake()
	{
		if(aim == null)
			aim = GameObject.Find("Mortar_Aim").transform;
		spawner = transform.Find("Spawner");
		effect.GetComponent<PlayerProjectile>().Damage = damage;
		hitPoint = Vector3.zero;
		coolCounter = coolTime;
	}
	private void Start()
	{
		aim.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}
	private void Update()
	{
		if (hitPoint != Vector3.zero)
		{
			aim.position = new Vector3(hitPoint.x, 1.0f, hitPoint.z);
		}

		coolCounter += Time.deltaTime * Player.PlayTimeScale;
		if (coolCounter >= coolTime)
		{
			Player.GetPlayer().CoolDown(true);
			coolCounter = coolTime;
		}
	}
	public override void Fire(Enemy enemy, Vector3 hitPoint)
	{
		effect.SetActive(true);
		effect.transform.position = spawner.position;
		effect.transform.rotation = spawner.rotation;
		effect.GetComponent<PlayerProjectile>().TargetPoint = aim.Find("Drop_Point").position;
		Player.GetPlayer().CoolDown(false);
		CameraMove.GetCam().Recoil(recoil);
		coolCounter = 0;
	}
	public override void WeaponActivated()
	{
		aim.gameObject.SetActive(true);
	}
	public override void WeaponDeactivated()
	{
		ResetCoolTime();
		aim.gameObject.SetActive(false);
		hitPoint = Vector3.zero;
	}
}
