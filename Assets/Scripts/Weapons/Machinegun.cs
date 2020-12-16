using UnityEngine;

public class Machinegun : Weapon 
{
	protected float effectCounter = 0;
	protected bool isEffectOn = false;
	[SerializeField]
	protected GameObject effect_02 = null;	// dust
	[SerializeField] 
	protected GameObject effect_03 = null;	// blood
	[SerializeField] 
	protected GameObject effect_04 = null;	// spark

	public override void Fire(Enemy enemy, Vector3 hitPoint)
	{
		base.Fire(enemy, hitPoint);
		isEffectOn = true;

		GameObject secondEffect;
		if (enemy == null)
		{
			secondEffect = (GameObject)Instantiate(effect_02, hitPoint, effect_02.transform.rotation);
		}
		else
		{
			switch (enemy.GetEnemyType())
			{
				case 0: // metal
					secondEffect = (GameObject)Instantiate(effect_04, hitPoint, effect_03.transform.rotation);
					break;
				case 1: // human
					secondEffect = (GameObject)Instantiate(effect_03, hitPoint, effect_03.transform.rotation);
					break;
				default :
					secondEffect = null;
					break;
			}
			enemy.Attacked(damage);
		}
		Destroy(secondEffect, 1);
	}

	protected void Update()
	{
		if (isEffectOn)
		{
			effectCounter += Time.deltaTime;
			if (effectCounter > 0.05f)
			{
				effect.SetActive(false);
				effectCounter = 0;
				isEffectOn = false;
			}
		}
	}
}
