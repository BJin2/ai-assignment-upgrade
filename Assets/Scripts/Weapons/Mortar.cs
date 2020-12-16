using UnityEngine;

public class Mortar : Weapon 
{
	private static Transform aim = null;
	private Transform spawner = null;

	protected override void Awake()
	{
		base.Awake();

		if(aim == null)
			aim = GameObject.Find("Mortar_Aim").transform;
		spawner = transform.Find("Spawner");
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

	private void Update()
	{
		if (HitPoint != Vector3.zero)
		{
			aim.position = new Vector3(HitPoint.x, 1.0f, HitPoint.z);
		}
	}
	public override void Fire(Enemy enemy, Vector3 hitPoint)
	{
		base.Fire(enemy, hitPoint);
		effect.transform.position = spawner.position;
		effect.transform.rotation = spawner.rotation;
		effect.GetComponent<PlayerProjectile>().TargetPoint = aim.Find("Drop_Point").position;
	}
}
