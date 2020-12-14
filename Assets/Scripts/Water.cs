using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour 
{
	Material mat;
	float offset_y = 0;
	void Start () 
	{
		mat = gameObject.GetComponent<Renderer>().material;
	}
	
	void Update () 
	{
		offset_y -= 0.005f * Time.deltaTime * Player.PlayTimeScale;
		mat.SetTextureOffset("_MainTex", new Vector2(0, offset_y));
	}
}
