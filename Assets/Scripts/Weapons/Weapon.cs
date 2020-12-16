using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour 
{
	[SerializeField] 
	protected float damage;
	[SerializeField] 
	protected float recoil;
	[SerializeField] 
	protected float coolTime;
	[SerializeField] 
	protected GameObject effect;

	private float coolCounter;
	public float CoolCounter { get { return (coolTime - coolCounter); } private set { coolTime = value; } }

	public bool Cooled { get; protected set; }

	public Vector3 HitPoint { protected get; set; }

	public event Action WeaponActivated = null;
	public event Action WeaponDeactivated = null;

	protected virtual void Awake()
	{
		coolCounter = coolTime;
	}

	public virtual void Fire(Enemy enemy, Vector3 hitPoint)
	{
		effect.SetActive(true);
		CameraMove.Instance.Recoil(recoil);
		Cooled = false;
		coolCounter = 0;
	}

	public void ActivateWeapon(bool active)
	{
		gameObject.SetActive(active);
		if (active)
			WeaponActivated?.Invoke();
		else
			WeaponDeactivated?.Invoke();
	}
	public void CoolDown()
	{
		if (Cooled)
			return;

		coolCounter += Time.deltaTime;
		if (coolCounter >= coolTime)
		{
			Cooled = true;
			coolCounter = coolTime;
		}
	}
}
