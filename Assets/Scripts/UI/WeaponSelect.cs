using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour 
{
	[SerializeField] 
	private int weaponType = 0;
	public int WeaponType { get { return weaponType; } }
	public int LayerMask { get; private set; }

	private void Awake()
	{
		switch (weaponType)
		{
			case 0:
			case 1:
				LayerMask = Player.BOTH_LAYER; // When using guns
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				LayerMask = Player.DEFAULT_LAYER; // When using mortar or air support
				break;
			default :
				LayerMask = Player.BOTH_LAYER;
				break;
		}
	}
}
