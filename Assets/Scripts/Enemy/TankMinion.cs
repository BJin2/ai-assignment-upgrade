using UnityEngine;

public class TankMinion : Humanoid 
{
	private Tank boss = null;
	private static int dir = 0;
	private float side = 0.0f;

	protected new void Awake()
	{
		base.Awake();
		SetStatus(500.0f, 50, -4.5f, -4.0f, 1.5f, 2.0f);
		boss = GameObject.Find("Tank").GetComponent<Tank>();
		if (dir == 0)
		{
			side = 0.5f;
			dir = 1;
		}
		else
			side =  -0.5f;
	}
	private new void Update()
	{
		if (boss == null)
			Death();
		if (boss.IsArrived)// attack with tank
		{
			anim.SetBool("isInRange", true);
			if (anim.GetBool("isInRange"))// Attack state
			{
				coolCounter += Time.deltaTime;
				if (coolCounter >= 0.1f)
					muzzleFlash.SetActive(false);
				if (coolCounter >= coolTime)
				{
					muzzleFlash.SetActive(true);
					anim.Play("Attack", -1, 0f);
					Player.Instance.Attacked(damage);
					coolCounter = 0;
				}
			}
		}
		else if(transform.parent == null)// follow tank
		{
			if (boss.IsStopped)//attack obstacle
			{
				if (side > 0)side = 1;
				else side = -1;
				//speed = 0.1f;
				if (Vector3.Distance(targetPos, transform.position) < 0.3f)
				{
					anim.SetBool("isInRange", true);
					if (anim.GetBool("isInRange"))// Attack state
					{
						if(boss.CurObstacle!= null)
							transform.rotation = Quaternion.LookRotation(boss.CurObstacle.transform.position - transform.position);
						coolCounter += Time.deltaTime;
						if (coolCounter >= 0.1f)
							muzzleFlash.SetActive(false);
						if (coolCounter >= coolTime)
						{
							muzzleFlash.SetActive(true);
							anim.Play("Attack", -1, 0f);
							boss.CurObstacle.Attacked(damage*20);
							coolCounter = 0;
						}
					}
					return;
				}
			}
			else
			{
				if (side > 0) side = 0.5f;
				else side = -0.5f;
				anim.SetBool("isInRange", false);
				muzzleFlash.SetActive(false);
				speed = boss.Speed * -0.7f;
			}

			Move();
		}
	}
	public override void Land()
	{
		transform.parent = null;
		anim.SetBool("isLanded", true);
	}
	public override void SetTargetPos()
	{
		targetPos = boss.transform.position + boss.transform.forward * -1.5f + boss.transform.right*side;
		targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
	}
	public override void Move()
	{
		if (targetPos != Vector3.zero)
		{
			SetTargetPos();
			transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
		}
		transform.Translate(transform.forward * speed * Time.deltaTime);
	}
}
