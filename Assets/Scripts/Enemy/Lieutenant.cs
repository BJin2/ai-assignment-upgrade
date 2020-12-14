using UnityEngine;

public class Lieutenant : Humanoid 
{
	[SerializeField] 
	private GameObject mortarAmmo = null;
	[SerializeField] 
	private Transform mortarSpawn = null;
	private Vector3 tempHit = Vector3.zero;
	private float hitDist = 0.0f;

	private bool hasAttacked = false;
	private float mortarCoolTIme = 0.0f;
	private float mortarCoolCount = 0.0f;
	private int prevRotDir = 0;
	private float tempSpeed = 0.0f;

	protected new void Awake()
	{
		base.Awake();
		SetStatus(30.0f, 15, -3.0f, -1.7f, 2.0f, 2.0f);
		hasAttacked = false;
		mortarCoolCount = 0;
		mortarCoolTIme = 3;
	}

	public override void Move()
	{
		if (anim.GetBool("isHiding"))
		{
			hasAttacked = true;
			mortarCoolCount += Time.deltaTime;
			if (mortarCoolCount >= mortarCoolTIme && mortarCoolCount < mortarCoolTIme*2)
			{
				MortarAttack();
				anim.Play("Mortar_Attack", -1, 0f);
				mortarCoolCount = mortarCoolTIme*2;
			}
			else if (mortarCoolCount > mortarCoolTIme * 2.5f)
			{
				anim.SetBool("isHiding", false);
				speed = tempSpeed;
			}
		}
		else
		{
			tempHit = new Vector3(Player.Instance.HitPoint.x, transform.position.y, Player.Instance.HitPoint.z);
			hitDist = Vector3.Distance(tempHit, transform.position);
			if (hitDist <= 1.0f)
			{
				rotDir = 2;
			}
		}

		DynamicRotatation();
		
		//Move to target (Straight)
		transform.Translate(transform.forward * speed * Time.deltaTime);
		if (transform.position.z <= targetPos.z)// Arrived
		{
			InAttackRange();
		}
		else if (rotDir != 0 && Vector3.Distance(transform.position, targetPos) < 2.5f)
		{
			InAttackRange();
		}
	}
	private void MortarAttack()
	{
		GameObject projectile = (GameObject)Instantiate(mortarAmmo, mortarSpawn.position, mortarSpawn.rotation);
		projectile.GetComponent<Enemy>().SetTargetPos();
	}
	private void DynamicRotatation()
	{
		if (targetPos != Vector3.zero)// When target is set
		{
			switch (rotDir)
			{
				case 0://Turn Toward target
					transform.rotation = Quaternion.RotateTowards(transform.rotation,
															Quaternion.LookRotation(targetPos - transform.position),
															90 * Time.deltaTime);
					break;
				case 1://Turn Right
					transform.Rotate(0, 90 * Time.deltaTime, 0);
					break;
				case -1://Turn Left
					transform.Rotate(0, -90 * Time.deltaTime, 0);
					break;
				case 2://When running away from player's aim
					transform.rotation = Quaternion.RotateTowards(transform.rotation,
															Quaternion.LookRotation(transform.position - tempHit),
															90 * Time.deltaTime);
					break;
			}
			if (!hasAttacked && ((prevRotDir == -1) || (prevRotDir == 1) && rotDir == 0))
			{
				tempSpeed = speed;
				anim.SetBool("isHiding", true);
				speed = 0;
			}
			prevRotDir = rotDir;
		}
	}
}
