using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : Overlay 
{
	[SerializeField] 
	private GameObject[] weaponList = new GameObject[6];
	[SerializeField] 
	private Button[] buttonList = new Button[6];
	[SerializeField] 
	private Text[] priceTextList = new Text[6];

	private int[] priceList = new int[6];
	private bool[] isUnlocked = new bool[6];

	ShopItemCollection items;

	public override void Initialize()
	{
		string jsonstring = (Resources.Load(@"ShopItem", typeof(TextAsset)) as TextAsset).text;
		items = JsonUtility.FromJson<ShopItemCollection>(jsonstring);
	}

	private void Awake()
	{
		for (int i = 0; i < 6; i++)
		{
			isUnlocked[i] = false;
			weaponList[i].SetActive(false);
			priceList[i] = 50 * i;
			priceTextList[i].text = priceList[i].ToString();
		}
		isUnlocked[0] = true;
		weaponList[0].SetActive(true);
		buttonList[0].gameObject.SetActive(false);
		priceTextList[0].gameObject.SetActive(false);
	}
	private void Start()
	{
		gameObject.SetActive(false);
	}
	public void Buy(int index)
	{
		if (Player.Instance.GetMoney() >= priceList[index])
		{
			Player.Instance.Earn(priceList[index] * (-1));
			weaponList[index].SetActive(true);
			isUnlocked[index] = true;
			buttonList[index].gameObject.SetActive(false);
		}
	}
	public void Upgrade(int index)
	{
		if (Player.Instance.GetMoney() >= priceList[index] && isUnlocked[index - 1])
		{
			Player.Instance.Earn(priceList[index] * (-1));
			weaponList[index - 1].SetActive(false);
			weaponList[index].SetActive(true);
			buttonList[index].gameObject.SetActive(false);
		}
	}
	public void Back()
	{
		Time.timeScale = 1.0f;
		gameObject.SetActive(false);
	}
}

[System.Serializable]
public class ShopItemCollection
{
	[System.Serializable]
	public class ShopItem
	{
		public int id;
		public int price;
		public string name;
	}
	public List<ShopItem> items;
}