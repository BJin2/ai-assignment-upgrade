using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour 
{
	[SerializeField] private Transform enemy;
	[SerializeField] private Transform ships;
	private Ship[] shipList;
	private GameObject[] enemiesList;

	private int shipListIndex;
	private int enemiesListIndex;

	private float spawnCounter;
	private float spawnInterval;

	private static int remainingEnemy;

	void Awake () 
	{
		
		shipListIndex = 0;
		enemiesListIndex = -1;
		spawnCounter = 0;
		spawnInterval = Random.Range(2.0f, 5.0f);
		remainingEnemy = 0;
		enemiesList = new GameObject[enemy.childCount];
		for (int i = 0; i < enemiesList.Length; i++)
		{
			enemiesList[i] = enemy.GetChild(i).gameObject;
			remainingEnemy += enemiesList[i].transform.childCount;
		}
		shipList = new Ship[ships.childCount];
		for (int i = 0; i < shipList.Length; i++)
		{
			shipList[i] = ships.GetChild(i).GetComponent<Ship>();
		}
		Player.RemainingEnemy = remainingEnemy;
	}

	void Update() 
	{
		spawnCounter += Time.deltaTime;
		if (spawnCounter >= spawnInterval)
		{
			if (shipList[shipListIndex].IsReady())
			{
				enemiesListIndex++;
				if (enemiesListIndex >= enemiesList.Length)
				{
					Debug.Log("No more enemies");
					return;
				}
				spawnCounter = 0;
				spawnInterval = Random.Range(2.0f, 5.0f);
				Debug.Log("Deploy");
				shipList[shipListIndex].Deploy(enemiesList[enemiesListIndex].transform);
			}
			shipListIndex = (++shipListIndex) % shipList.Length;
		}
	}
}
