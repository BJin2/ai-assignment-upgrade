using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageExplosion : MonoBehaviour 
{
	private float damage;
	public float Damage { set { damage = value; } }

	private void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Enemy")
		{
			col.transform.parent.GetComponent<Enemy>().Attacked(damage);
		}
	}
}
