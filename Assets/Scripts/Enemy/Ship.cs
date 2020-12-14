using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Enemy 
{
	private enum State
	{
		Going = 0,
		Leaving,
		Opening,
		Closing,
		Landing
	};
	private const float DEST_Z = 80.0f;
	[SerializeField] private GameObject explosion;
	private Transform door;
	private int state;
	private float openSpeed;
	private bool isReady;

	private Enemy[] enemyList;
	private float enemyCount;

	private void Initialize()
	{
		speed = 10;
		health = 300;
		damage = 0;
		reward = 500;

		
		state = (int)State.Going;
		openSpeed = 40;
		enemyCount = 0;
		door.localEulerAngles = new Vector3(0, 0, 296);
		transform.position = new Vector3(transform.position.x, transform.position.y, 80);
		isReady = true;
	}
	private void Awake () 
	{
		type = 0;
		canTakeDamage = true;
		door = transform.Find("Bone001").Find("Bone002");
		Initialize();
	}
	private void Start()
	{
		gameObject.SetActive(false);
	}
	
	private void Update () 
	{
		//Debug.Log(isReady);
		switch (state)
		{
			case (int)State.Going://Ship is going to beach
			{
				enemyList = transform.Find("Enemies").GetComponentsInChildren<Enemy>();
				speed = Mathf.Clamp(speed - 1.5f * Time.deltaTime * Player.PlayTimeScale, 2, speed);
				transform.Translate(transform.forward * speed * Time.deltaTime * Player.PlayTimeScale * (-1));
				canTakeDamage = true;
				break;
			}
			case (int)State.Leaving:// Ship is leaving the beach
			{
				speed = Mathf.Clamp(speed + 3.5f * Time.deltaTime * Player.PlayTimeScale, speed, 10);
				transform.Translate(transform.forward * speed * Time.deltaTime * Player.PlayTimeScale);
				canTakeDamage = true;
				if (transform.position.z >= DEST_Z)
				{
					Initialize();
					Destroy(transform.Find("Enemies").gameObject);
					enemyList = null;
					gameObject.SetActive(false);
				}
				break;
			}
			case (int)State.Opening:// Opening the door for enemies
			{
				door.localEulerAngles = new Vector3(0, 0, door.localEulerAngles.z + openSpeed * Time.deltaTime * Player.PlayTimeScale);
				if (door.localEulerAngles.z <= 180)
				{
					if (door.localEulerAngles.z >= 14)
					{
						state = (int)State.Landing;
						//set all enemies' speed(Random value) and state
						if (enemyList != null)
						{
							foreach (Enemy enemy in enemyList)
							{
								if (enemy == null)
									continue;
								enemy.Land();
							}
							enemyList = null;
						}
					}
				}
				canTakeDamage = false;
				break;
			}
			case (int)State.Landing:// Enemies are getting out of the ship
			{
				enemyCount += Time.deltaTime * Player.PlayTimeScale;
				if (enemyCount >= 4.0f)
				{
					enemyCount = 0;
					speed = 0;
					state = (int)State.Closing;
				}
				canTakeDamage = false;
				break;
			}
			case (int)State.Closing:// Closing the door before leaving
			{
				door.localEulerAngles = new Vector3(0, 0, door.localEulerAngles.z - openSpeed * Time.deltaTime * Player.PlayTimeScale);
				if (door.localEulerAngles.z >= 180)
				{
					if (door.localEulerAngles.z <= 296)
					{
						state = (int)State.Leaving;
					}
				}
				canTakeDamage = false;
				break;
			}
			default :
				Initialize();
				break;
		}
	}

	public override void Death()
	{
		if (enemyList != null)
		{
			for (int i = 0; i < enemyList.Length; i++)
			{
				if (enemyList[i] != null)
				{
					enemyList[i].Death();
				}
			}
		}
		enemyList = null;
		GameObject effect = (GameObject)Instantiate(explosion, transform.position, explosion.transform.rotation);
		Initialize();
		Destroy(effect, 2);
		Destroy(transform.Find("Enemies").gameObject);
		gameObject.SetActive(false);
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Map" && state == (int)State.Going)
		{
			state = (int)State.Opening;
			//Debug.Log(state);
		}
	}
	void OnTriggerExit(Collider col)
	{
		//Debug.Log(col.gameObject.name);
		if (col.tag == "Enemy")
		{
			Enemy tempEnemy = col.transform.parent.GetComponent<Enemy>();
			if (tempEnemy != null)
			{
				tempEnemy.SetTargetPos();
				tempEnemy.CanTakeDamage();
			}
		}
	}

	public void Deploy(Transform enemies)//Called at Spawner script
	{
		isReady = false;
		gameObject.SetActive(true);
		enemies.gameObject.SetActive(true);
		state = (int)State.Going;
		//Add child objects of enemies to transform.Find("Enemies")
		enemies.position = transform.position;
		enemies.parent = transform;
		enemyList = transform.Find("Enemies").GetComponentsInChildren<Enemy>();
	}
	public bool IsReady()
	{
		return isReady;
	}
	public override void Attack()//Ship doesn't have attack ability
	{
		//Does nothing
	}
	public override void Land()
	{

	}
	public override void SetTargetPos()
	{

	}
}
