using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Enemy 
{
	[SerializeField] GameObject explosion;
	private Vector3 targetPos;
	private void Awake()
	{
		reward = 0;
		type = 0;
		damage = 10;
		health = 1;
		canTakeDamage = true;
	}
	private void Update()
	{
		transform.rotation = Quaternion.RotateTowards(transform.rotation,
															Quaternion.LookRotation(targetPos - transform.position),
															30 * Time.deltaTime * Player.PlayTimeScale);
		transform.Translate(Vector3.forward * 5 * Time.deltaTime * Player.PlayTimeScale);

		if (transform.position.z <= -2)
		{
			if ((transform.position.x < 8) && (transform.position.x > -3))
			{
				Attack();
			}
			Death();
		}
	}
	public override void Attack()
	{
		Player.GetPlayer().Attacked(damage);
	}
	public override void Land()
	{

	}
	public override void Death()
	{
		GameObject temp = (GameObject)Instantiate(explosion, transform.position, explosion.transform.rotation);
		Destroy(temp, 1.5f);
		Destroy(gameObject);
	}
	public override void SetTargetPos()
	{
		float x = (transform.position + transform.forward * Vector3.Distance(transform.position, Player.GetPlayer().transform.position)).x;
		float y = 1;
		float z = Player.GetPlayer().transform.position.z;
		targetPos = new Vector3(x, y, z);
	}
}
