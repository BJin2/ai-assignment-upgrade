using UnityEngine;

public class Obstacle : Enemy 
{
	private Rigidbody[] rigidbodyList;

	private void Awake()
	{
		reward = 0;
		type = 0;
		health = 150;
		rigidbodyList = transform.GetComponentsInChildren<Rigidbody>();
		canTakeDamage = true;
	}

	//Does not do anything except dying and blocking path
	public override void Attack()
	{

	}
	public override void Land()
	{

	}
	public override void Death()
	{
		for (int i = 0; i < rigidbodyList.Length; i++)
		{
			rigidbodyList[i].isKinematic = false;
			rigidbodyList[i].useGravity = true;
			rigidbodyList[i].transform.parent = null;
			Destroy(rigidbodyList[i].gameObject, 2);
			Destroy(gameObject);
		}
	}
	public override void SetTargetPos()
	{

	}
}
