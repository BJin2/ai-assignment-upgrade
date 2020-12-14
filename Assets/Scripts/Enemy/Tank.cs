using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
	[SerializeField] GameObject explosion;
	[SerializeField] GameObject muzzleFlash;

	private bool isArrived;
	public bool IsArrived { get { return isArrived; } }

	[SerializeField]private bool isStopped;
	public bool IsStopped { get { return isStopped; } }

	private Transform colChecker;
	private Collider[] obstacles;

	[SerializeField]private Obstacle curObstacle;
	public Obstacle CurObstacle { get { return curObstacle; } }

	public float Speed { get { return speed; } }

	private Vector3 targetPos;

	private void Awake()
	{
		health = 2500;
		reward = 5000;
		speed = 0;
		damage = 150;
		coolTime = 5;
		coolCounter = 0;
		isCoolDown = true;

		isArrived = false;
		isStopped = true;
		colChecker = transform.Find("Checker");

		targetPos = Vector3.zero;
	}
	private void Update()
	{
		if (isArrived)//Attack state
		{
			coolCounter += Time.deltaTime * Player.PlayTimeScale;
			if (coolCounter >= 0.12f)
				muzzleFlash.SetActive(false);
			if (coolCounter >= coolTime)
			{
				muzzleFlash.SetActive(true);
				Player.GetPlayer().Attacked(damage);
				coolCounter = 0;
			}
		}
		else if(transform.parent == null)//Moving state
		{
			CollisionCheck();
			if (isStopped)//Slow down and then stop due to obstacle
			{
				float increase = (25 / (speed+0.001f)) * Time.deltaTime * Player.PlayTimeScale;
				speed = Mathf.Clamp(speed - increase, 0, 4);
			}
			else// accelerate when there's nothing blocking
			{
				float increase = (Mathf.Log(speed+1.5f))*Time.deltaTime * Player.PlayTimeScale;
				speed = Mathf.Clamp(speed+increase, 0, 4);
			}
			transform.Translate(Vector3.forward * speed * Time.deltaTime * Player.PlayTimeScale);
			if (targetPos != Vector3.zero)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation,
															Quaternion.LookRotation(targetPos - transform.position),
															10 * Time.deltaTime * Player.PlayTimeScale);

				if (transform.position.z <= targetPos.z + 2)
				{
					isArrived = true;
				}
			}
		}
	}
	private void CollisionCheck()
	{
		isStopped = false;
		curObstacle = null;
		obstacles = Physics.OverlapSphere(colChecker.position, 1, 1 << 9);
		if (obstacles.Length > 0)
		{
			for (int i = 0; i < obstacles.Length; i++)
			{
				if (obstacles[i].transform.parent.GetComponent<Obstacle>() != null)
				{
					curObstacle = obstacles[i].transform.parent.GetComponent<Obstacle>();
					isStopped = true;
				}
			}
		}
	}
	public override void Attack()
	{
		muzzleFlash.SetActive(true);
		Player.GetPlayer().Attacked(damage);
	}
	public override void Death()
	{
		Player.ReduceRemainingEnemy();
		GameObject temp = (GameObject)Instantiate(explosion, transform.position, explosion.transform.rotation);
		Destroy(temp, 2);
		Destroy(gameObject);
	}
	public override void Land()
	{
		isStopped = false;
		transform.parent = null;
	}
	public override void SetTargetPos()
	{
		float targetPosZ = Random.Range(atkRange_min, atkRange_max);
		targetPos = new Vector3(((targetPosZ - atkWidth_min) * slope) * Random.Range(-1.0f, 1.0f), transform.position.y, targetPosZ);
	}
}
