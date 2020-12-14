using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour 
{
	[SerializeField] protected float damage;
	[SerializeField] protected float recoil;
	[SerializeField] protected float coolTime;
	[SerializeField] protected GameObject effect;
	protected float coolCounter;
	public float CoolCounter { get { return (coolTime - coolCounter); } }

	protected Vector3 hitPoint;
	public Vector3 HitPoint { set { hitPoint = value; } }
	public void ResetCoolTime()
	{
		coolCounter = coolTime;
	}
	abstract public void Fire(Enemy enemy, Vector3 hitPoint);
	abstract public void WeaponActivated();
	abstract public void WeaponDeactivated();
}
