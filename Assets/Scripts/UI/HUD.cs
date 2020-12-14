using UnityEngine;
using TMP = TMPro.TextMeshProUGUI;

public class HUD : MonoBehaviour 
{
	[SerializeField] private TMP coolTime;
	[SerializeField] private TMP resource;
	[SerializeField] private TMP numOfEnemy;
	[SerializeField] private TMP hp;

	public void UpdateResource(int _resource)
	{
		resource.text = _resource.ToString();
	}
	public void UpdateNumOfEnemy(int _numOfEnemy)
	{
		numOfEnemy.text = _numOfEnemy.ToString();
	}
	public void UpdateCoolTime(float _coolTime)
	{
		coolTime.text = Mathf.Floor(_coolTime).ToString();
	}
	public void UpdateHP(float _hp)
	{
		hp.text = _hp.ToString();
	}
	public void UpdateAll(int _resource, int _numOfEnemy, float _coolTime, float _hp)
	{
		UpdateResource(_resource);
		UpdateNumOfEnemy(_numOfEnemy);
		UpdateCoolTime(_coolTime);
		UpdateHP(_hp);
	}
}
