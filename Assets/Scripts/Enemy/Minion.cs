using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Humanoid 
{
	protected new void Awake()
	{
		base.Awake();
		SetStatus(10.0f, 10, -3.0f, -1.7f, 2.0f, 2.0f);
	}

	public override void Move()
	{
		if (targetPos != Vector3.zero)// When target is set
		{
			switch (rotDir)
			{
				case 0://Turn Toward target
					transform.rotation = Quaternion.RotateTowards(transform.rotation,
															Quaternion.LookRotation(targetPos - transform.position),
															90 * Time.deltaTime * Player.PlayTimeScale);
					break;
				case 1://Turn Right
					transform.Rotate(0, 90 * Time.deltaTime * Player.PlayTimeScale, 0);
					break;
				case -1://Turn Left
					transform.Rotate(0, -90 * Time.deltaTime * Player.PlayTimeScale, 0);
					break;
			}
		}
		//Move to target (Straight)
		transform.Translate(transform.forward * speed * Time.deltaTime * Player.PlayTimeScale);
		if (transform.position.z <= targetPos.z)// Arrived
		{
			InAttackRange();
		}
		else if (rotDir != 0 && Vector3.Distance(transform.position, targetPos) < 2.5f)
		{
			InAttackRange();
		}
	}
}
