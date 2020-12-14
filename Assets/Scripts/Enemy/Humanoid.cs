﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Humanoid : Enemy
{
	[SerializeField] protected GameObject muzzleFlash;
	protected GameObject ragdoll;
	protected Transform ragdollTransform;
	protected Animator anim;

	protected Vector3 targetPos;

	protected float maxSpeed;
	protected float minSpeed;

	protected int rotDir;
	private int rayHitChecker;
	protected void Awake()
	{
		canTakeDamage = false;
		type = 1;

		ragdoll = transform.Find("German_Ragdoll").gameObject;
		ragdollTransform = transform.Find("Bip001").Find("RagdollTransform");
		anim = gameObject.GetComponent<Animator>();
		isCoolDown = true;
		targetPos = Vector3.zero;
		speed = 0;
		rotDir = 0;
	}

	protected void Update()
	{
		if (anim.GetBool("isInRange"))// Attack state
		{
			coolCounter += Time.deltaTime * Player.PlayTimeScale;
			if (coolCounter >= 0.1f)
				muzzleFlash.SetActive(false);
			if (coolCounter >= coolTime)
			{
				Attack();
				coolCounter = 0;
			}
		}
		else// Moving State
		{
			rayHitChecker = 0;

			LeftRay();
			RightRay();

			if (rayHitChecker == 2)
			{
				rotDir = 0;
			}

			Move();
		}
	}
	protected void RightRay()
	{
		Vector3 rightDir = ((transform.forward * 2.3f) - (transform.right * 0.7f));
		Vector3 rightStart = transform.position + (transform.right * 0.7f);
		rightStart = new Vector3(rightStart.x, 1.3f, rightStart.z);
		RaycastHit rightHit;
		Ray rightRay = new Ray(rightStart, rightDir.normalized);
		if (Physics.Raycast(rightRay, out rightHit, rightDir.magnitude, 1 << 9))
		{
			if (rightHit.collider.transform.parent.GetComponent<Obstacle>() != null)
			{
				if (rotDir == 0)
				{
					rotDir = -1;
				}
				Debug.Log("Right");
			}
			else
			{
				rayHitChecker++;
			}
		}
		else
		{
			rayHitChecker++;
		}

		Debug.DrawRay(rightRay.origin, rightRay.direction * rightDir.magnitude, Color.blue);
	}
	protected void LeftRay()
	{
		Vector3 leftDir = ((transform.forward * 2.3f) - (transform.right * -0.7f));
		Vector3 leftStart = transform.position + (transform.right * -0.7f);
		leftStart = new Vector3(leftStart.x, 1.3f, leftStart.z);
		RaycastHit leftHit;

		Ray leftRay = new Ray(leftStart, leftDir.normalized);
		if (Physics.Raycast(leftRay, out leftHit, leftDir.magnitude, 1 << 9))
		{
			if (leftHit.collider.transform.parent.GetComponent<Obstacle>() != null)
			{
				if (rotDir == 0)
				{
					rotDir = 1;
				}
				Debug.Log("Left");
			}
			else
			{
				rayHitChecker++;
			}
		}
		else
		{
			rayHitChecker++;
		}

		Debug.DrawRay(leftRay.origin, leftRay.direction * leftDir.magnitude, Color.red);
	}
	public abstract void Move();
	public override void Attack()
	{
		muzzleFlash.SetActive(true);
		anim.Play("Attack", -1, 0f);
		Player.GetPlayer().Attacked(damage);
	}

	public void SetStatus(float health, int reward, float maxSpeed, float minSpeed, float damage, float coolTime)
	{
		this.health = health;
		this.reward = reward;
		this.maxSpeed = maxSpeed;
		this.minSpeed = minSpeed;
		this.damage = damage;
		this.coolTime = coolTime;
	}

	public override void Death()
	{
		Player.ReduceRemainingEnemy();
		ragdoll.transform.position = ragdollTransform.position;
		ragdoll.transform.rotation = ragdollTransform.rotation;
		ragdoll.transform.parent = null;
		ragdoll.SetActive(true);
		Destroy(ragdoll, 5);
		Destroy(gameObject);
	}
	public override void Land()
	{
		//Set speed(random value)
		if (maxSpeed < 0)
		{
			speed = Random.Range(maxSpeed, minSpeed);
			anim.speed = speed / (minSpeed);
		}
		else
		{
			speed = Random.Range(maxSpeed * (-1), minSpeed * (-1));
			anim.speed = speed / (minSpeed * (-1));
		}
				
		transform.parent = null;
		anim.SetBool("isLanded", true);
	}
	public override void SetTargetPos()
	{
		float targetPosZ = Random.Range(atkRange_min, atkRange_max);
		targetPos = new Vector3(((targetPosZ - atkWidth_min) * slope) * Random.Range(-1.0f, 1.0f), transform.position.y, targetPosZ);
	}
	public void InAttackRange()
	{
		anim.SetBool("isInRange", true);
	}
}
