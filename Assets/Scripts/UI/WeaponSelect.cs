using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour 
{
	[SerializeField] private int weaponType;
	public int WeaponType { get { return weaponType; } }
	private int layerMask;
	public int LayerMask { get { return layerMask; } }

	private const int DEFAULT_LAYER = 1;
	private const int ENEMY_LAYER = 1 << 9;
	private const int BOTH_LAYER = DEFAULT_LAYER | ENEMY_LAYER;

	private void Awake()
	{
		switch (weaponType)
		{
			case 0:
			case 1:
				layerMask = BOTH_LAYER; // When using guns
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				layerMask = DEFAULT_LAYER; // When using mortar or air support
				break;
			default :
				layerMask = BOTH_LAYER;
				break;
		}
	}
}
