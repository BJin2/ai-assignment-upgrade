using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : Overlay 
{
	[SerializeField]
	private GameObject shopItemPanel = null;
	private List<RectTransform> itemPanels = null;
	private ShopItemCollection items = null;

	public override void Initialize()
	{
		string jsonstring = (Resources.Load(@"ShopItem", typeof(TextAsset)) as TextAsset).text;
		items = JsonUtility.FromJson<ShopItemCollection>(jsonstring);
		itemPanels = new List<RectTransform>();
		float anchor = 0;
		float x = 320;
		for (int i = 0; i < 3; i++)
		{
			//Instantiate -> position -> button function -> increment

			GameObject itemPanel = Instantiate(shopItemPanel, transform);

			RectTransform rectTransform = itemPanel.GetComponent<RectTransform>();
			rectTransform.anchorMin = new Vector2(anchor, 0.5f);
			rectTransform.anchorMax = new Vector2(anchor, 0.5f);
			rectTransform.anchoredPosition = new Vector2(x, 0.0f);

			anchor += 0.5f;
			x -= 320;

			itemPanels.Add(rectTransform);
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
		public override string ToString()
		{
			string result = "";
			result += $"id : {id}\n";
			result += $"price : {price}\n";
			result += $"name : {name}\n";
			return result;
		}
	}
	public List<ShopItem> items;
	public override string ToString()
	{
		string result = $"ShopItemCollection\nitem count : {items.Count}\n";
		for (int i = 0; i < items.Count; i++)
		{
			result += items[i].ToString();
		}
		return result;
	}
}