using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlane : PlayerProjectile 
{
	private int state = 0;

	[SerializeField] 
	private float numberOfExplosion = 0;
	private float curExplosion = 0;
	private Transform from = null;
	private Transform to = null;
	private Vector3 dir = Vector3.zero;

	new private void Awake()
	{
		base.Awake();
		from = GameObject.Find("From").transform;
		to = GameObject.Find("To").transform;
		dir = to.position - from.position;
	}
	new private void Update()
	{
		base.Update();

		if (transform.position.z >= (from.position + (dir * (curExplosion / numberOfExplosion))).z)
		{
			GameObject temp = (GameObject)Instantiate(explosion, new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), 1, transform.position.z), explosion.transform.rotation);
			temp.GetComponent<DamageExplosion>().Damage = damage;
			Destroy(temp, 1.2f);
			curExplosion++;
			if (curExplosion >= numberOfExplosion)
			{
				curExplosion = numberOfExplosion * 2;
			}
		}

		switch (state)
		{
			case 0:
				if (Vector3.Distance(transform.position, midPoint.position) <= 0.1f)
				{
					//transform.rotation = midPoint.rotation;
					state = 1;
				}
				break;
			case 1:
				Quaternion destRot = Quaternion.LookRotation(endPoint.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, destRot, 2 * Time.deltaTime);
				if (Vector3.Distance(transform.position, endPoint.position) <= 0.2f)
				{
					state = 0;
					curExplosion = 0;
					gameObject.SetActive(false);
				}
				break;
		}
	}
}
