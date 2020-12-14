using UnityEngine;

public abstract class Enemy : MonoBehaviour 
{
	protected int reward;
	protected int type;	// 0 = ship or tank			1 = human

	protected float speed;
	protected float health;
	protected float damage;
	
	protected float coolTime;
	protected float coolCounter;
	protected bool isCoolDown;

	protected const float atkRange_min = 6.25f;
	protected const float atkRange_max = 8.5f;
	protected const float atkWidth_min = 3.1f;
	protected const float atkWidth_max = 5.4f;
	protected const float slope = ((atkRange_max - atkRange_min) / (atkWidth_max - atkWidth_min));

	[SerializeField] 
	protected bool canTakeDamage;

    public void Attacked(float dmg)
    {
		if (canTakeDamage)
		{
			health -= dmg;
			if (health <= 0)
			{
				Player.Instance.Earn(reward);
				Death();
			}
		}

    }
	public int GetEnemyType()
	{
		return type;
	}
	public void CanTakeDamage()
	{
		canTakeDamage = true;
	}
	public void CannotTakeDamage()
	{
		canTakeDamage = false;
	}
    abstract public void Attack();
	abstract public void Land();
    abstract public void Death();
	abstract public void SetTargetPos();
}
