using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerProjectile : MonoBehaviour 
{
	[SerializeField] 
	protected float speed = 0.0f;
	[SerializeField] 
	protected string midPointName = null;
	[SerializeField] 
	protected string endPointName = null;
	[SerializeField] 
	protected GameObject explosion = null;

	protected Transform midPoint;
	protected Transform endPoint;
	protected Vector3 targetPoint;
	public Vector3 TargetPoint { set { targetPoint = value; } }

	protected float damage;
	public float Damage { set { damage = value; } }

	protected void Awake()
	{
		midPoint = GameObject.Find(midPointName).transform;
		endPoint = GameObject.Find(endPointName).transform;
	}

	protected void Update () 
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
