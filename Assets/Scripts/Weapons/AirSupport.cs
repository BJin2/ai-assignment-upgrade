using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSupport : Weapon 
{
	private static Transform aim = null;
	private Transform startPos = null;
	
	protected override void Awake()
	{
		base.Awake();

		if(aim == null)
			aim = GameObject.Find("Airsupport_Aim").transform;
		startPos = aim.Find("Air_Start").transform;
		effect.GetComponent<PlayerProjectile>().Damage = damage;

		WeaponActivated += () =>
		{
			aim.gameObject.SetActive(true);
		};
		WeaponDeactivated += () =>
		{
			aim.gameObject.SetActive(false);
			HitPoint = Vector3.zero;
		};
	}

	public override void Fire(Enemy enemy, Vector3 hitPoint)
	{
		base.Fire(enemy, hitPoint);
		effect.transform.position = startPos.position;
		effect.transform.rotation = startPos.rotation;
	}
}
