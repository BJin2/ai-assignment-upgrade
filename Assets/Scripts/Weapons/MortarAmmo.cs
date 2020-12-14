using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarAmmo : PlayerProjectile 
{
	new private void Update ()
	{
		base.Update();
		if (Vector3.Distance(transform.position, midPoint.position) <= 0.1f)
		{
			transform.position = targetPoint;
			transform.rotation = endPoint.rotation;
		}
	}
	private void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Map" || transform.position.y <= 1.0f)
		{
			GameObject temp = (GameObject)Instantiate(explosion, transform.position, explosion.transform.rotation);
			temp.GetComponent<DamageExplosion>().Damage = damage;
			Destroy(temp, 1.2f);
			gameObject.SetActive(false);
		}
	}
}
