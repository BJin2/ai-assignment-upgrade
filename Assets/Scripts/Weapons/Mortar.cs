using UnityEngine;

public class Mortar : Weapon 
{
	private static Transform aim = null;
	private Transform spawner = null;

	private void Awake()
	{
		if(aim == null)
			aim = GameObject.Find("Mortar_Aim").transform;
		spawner = transform.Find("Spawner");
		effect.GetComponent<PlayerProjectile>().Damage = damage;
		WeaponDeactivated();
	}

	private void Update()
	{
		if (hitPoint != Vector3.zero)
		{
			aim.position = new Vector3(hitPoint.x, 1.0f, hitPoint.z);
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
		effect.transform.position = spawner.position;
		effect.transform.rotation = spawner.rotation;
		effect.GetComponent<PlayerProjectile>().TargetPoint = aim.Find("Drop_Point").position;
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
