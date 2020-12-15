using UnityEngine;
using TMP = TMPro.TextMeshProUGUI;

public class HUD : MonoBehaviour 
{
	public static HUD Instance { get; private set; }

	[SerializeField] private TMP coolTime = null;
	[SerializeField] private TMP resource = null;
	[SerializeField] private TMP numOfEnemy = null;
	[SerializeField] private TMP hp = null;

	private void Awake()
	{
		Instance = this;
	}

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
	public void UpdateAll(int _resource = 0, int _numOfEnemy = 0, float _coolTime = 0.0f, float _hp = 0.0f)
	{
		UpdateResource(_resource);
		UpdateNumOfEnemy(_numOfEnemy);
		UpdateCoolTime(_coolTime);
		UpdateHP(_hp);
	}
}
