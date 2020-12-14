using UnityEngine;

public class Tank : Enemy
{
	[SerializeField] 
	private GameObject explosion = null;
	[SerializeField]
	GameObject muzzleFlash = null;

	public bool IsArrived { get; private set; }
	public bool IsStopped { get; private set; }

	private Transform colChecker = null;
	private Collider[] obstacles = null;

	public Obstacle CurObstacle { get; private set; }

	public float Speed { get { return speed; } }

	private Vector3 targetPos = Vector3.zero;

	private void Awake()
	{
		health = 2500;
		reward = 5000;
		speed = 0;
		damage = 150;
		coolTime = 5;
		coolCounter = 0;
		isCoolDown = true;

		IsArrived = false;
		IsStopped = true;
		colChecker = transform.Find("Checker");

		targetPos = Vector3.zero;
	}
	private void Update()
	{
		if (IsArrived)//Attack state
		{
			coolCounter += Time.deltaTime;
			if (coolCounter >= 0.12f)
				muzzleFlash.SetActive(false);
			if (coolCounter >= coolTime)
			{
				muzzleFlash.SetActive(true);
				Player.Instance.Attacked(damage);
				coolCounter = 0;
			}
		}
		else if(transform.parent == null)//Moving state
		{
			CollisionCheck();
			if (IsStopped)//Slow down and then stop due to obstacle
			{
				float increase = (25 / (speed+0.001f)) * Time.deltaTime;
				speed = Mathf.Clamp(speed - increase, 0, 4);
			}
			else// accelerate when there's nothing blocking
			{
				float increase = (Mathf.Log(speed+1.5f))*Time.deltaTime;
				speed = Mathf.Clamp(speed+increase, 0, 4);
			}
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
			if (targetPos != Vector3.zero)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation,
															Quaternion.LookRotation(targetPos - transform.position),
															10 * Time.deltaTime);

				if (transform.position.z <= targetPos.z + 2)
				{
					IsArrived = true;
				}
			}
		}
	}
	private void CollisionCheck()
	{
		IsStopped = false;
		CurObstacle = null;
		obstacles = Physics.OverlapSphere(colChecker.position, 1, Player.ENEMY_LAYER);
		if (obstacles.Length > 0)
		{
			for (int i = 0; i < obstacles.Length; i++)
			{
				if (obstacles[i].transform.parent.GetComponent<Obstacle>() != null)
				{
					CurObstacle = obstacles[i].transform.parent.GetComponent<Obstacle>();
					IsStopped = true;
				}
			}
		}
	}
	public override void Attack()
	{
		muzzleFlash.SetActive(true);
		Player.Instance.Attacked(damage);
	}
	public override void Death()
	{
		Player.Instance.ReduceRemainingEnemy();
		GameObject temp = (GameObject)Instantiate(explosion, transform.position, explosion.transform.rotation);
		Destroy(temp, 2);
		Destroy(gameObject);
	}
	public override void Land()
	{
		IsStopped = false;
		transform.parent = null;
	}
	public override void SetTargetPos()
	{
		float targetPosZ = Random.Range(atkRange_min, atkRange_max);
		targetPos = new Vector3(((targetPosZ - atkWidth_min) * slope) * Random.Range(-1.0f, 1.0f), transform.position.y, targetPosZ);
	}
}
