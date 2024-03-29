﻿using UnityEngine;

public class Spawner : MonoBehaviour 
{
	[SerializeField] 
	private Transform enemy = null;
	[SerializeField] 
	private Transform ships = null;
	private Ship[] shipList = null;
	private GameObject[] enemiesList = null;

	private int shipListIndex = 0;
	private int enemiesListIndex = -1;

	private float spawnCounter = 0;
	private float spawnInterval = float.MaxValue;

	private void Awake ()
	{
		shipListIndex = 0;
		enemiesListIndex = -1;
		spawnCounter = 0;
		spawnInterval = Random.Range(2.0f, 5.0f);
		enemiesList = new GameObject[enemy.childCount];
		for (int i = 0; i < enemiesList.Length; i++)
		{
			enemiesList[i] = enemy.GetChild(i).gameObject;
			Enemy.Remaining += enemiesList[i].transform.childCount;
		}
		shipList = new Ship[ships.childCount];
		for (int i = 0; i < shipList.Length; i++)
		{
			shipList[i] = ships.GetChild(i).GetComponent<Ship>();
		}
	}

	private void Start()
	{
		Player.Instance.OnDeath += () => { Destroy(this); };
	}

	private void Update() 
	{
		spawnCounter += Time.deltaTime;
		if (spawnCounter >= spawnInterval)
		{
			if (shipList[shipListIndex].IsReady())
			{
				enemiesListIndex++;
				if (enemiesListIndex >= enemiesList.Length)
				{
					Destroy(this);
					return;
				}
				spawnCounter = 0;
				spawnInterval = Random.Range(2.0f, 5.0f);
				shipList[shipListIndex].Deploy(enemiesList[enemiesListIndex].transform);
			}
			shipListIndex = (++shipListIndex) % shipList.Length;
		}
	}
}
