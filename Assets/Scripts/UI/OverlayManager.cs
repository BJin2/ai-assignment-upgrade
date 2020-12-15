using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
	[SerializeField]
	private List<Overlay> overlays = null;

	private void Awake()
	{
		foreach (Overlay overlay in overlays)
		{
			overlay.gameObject.SetActive(true);
			overlay.Initialize();
			overlay.gameObject.SetActive(false);
		}
	}
}
