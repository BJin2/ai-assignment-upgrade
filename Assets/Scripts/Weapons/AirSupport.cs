using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSupport : Weapon 
{
	private static Transform aim = null;
	private Transform startPos = null;
	
	private void Awake()
	{
		if(aim == null)
			aim = GameObject.Find("Airsupport_Aim").transform;
		startPos = aim.Find("Air_Start").transform;
		effect.GetComponent<PlayerProjectile>().Damage = damage;
		WeaponDeactivated();
	}

	private void Update()
	{
		if (hitPoint != Vector3.zero)
		{
			//aim.position = new Vector3(hitPoint.x, 1.0f, hitPoint.z);
		}

		coolCounter += Time.deltaTime;
		if (coolCounter >= coolTime)
		{
			Player.Instance.CoolDown(true);
			coolCounter = coolTime;
		}
	}
	public override void Fire(Enemy enemy, Vector3 hitPoint)
	{
		effect.SetActive(true);
		effect.transform.position = startPos.position;
		effect.transform.rotation = startPos.rotation;
		Player.Instance.CoolDown(false);
		CameraMove.Instance.Recoil(recoil);
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
