using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSupport : Weapon 
{
	private static Transform aim;
	private Transform startPos;
	
	private void Awake()
	{
		if (aim == null)
			aim = GameObject.Find("Airsupport_Aim").transform;
		startPos = GameObject.Find("Air_Start").transform;
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
			//aim.position = new Vector3(hitPoint.x, 1.0f, hitPoint.z);
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
		effect.transform.position = startPos.position;
		effect.transform.rotation = startPos.rotation;
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
