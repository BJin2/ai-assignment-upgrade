using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	//Weapon
	[SerializeField] private Weapon[] weaponList;
	private WeaponSelect weaponToSelect;
	private int curWeaponIndex;

	//Player status
	private int money;
	private float health;
	private const float MAX_HEALTH = 1000;

	//Targeting
    private RaycastHit hit;
    private Ray ray;
	private int layerMask;
	private const int DEFAULT_LAYER = 1;
	private const int ENEMY_LAYER = 1 << 9;
	private const int BOTH_LAYER = DEFAULT_LAYER | ENEMY_LAYER;

    private Vector3 targetPos;
    private Quaternion lookRot;

	private Enemy targetEnemy;
	private static Vector3 hitPoint;
	public static Vector3 HitPoint { get { return hitPoint; } }

    private bool isCooldown;

	public static float PlayTimeScale = 1.0f;
	public static int RemainingEnemy;

	private GameObject pauseMenu;
	private GameObject shopMenu;
	private GameObject gameoverMenu;
	private GameObject winMenu;
	private HUD hud;

	private void Awake()
	{
		pauseMenu = GameObject.Find("PauseMenu");
		shopMenu = GameObject.Find("ShopMenu");
		gameoverMenu = GameObject.Find("GameoverMenu");
		winMenu = GameObject.Find("WinMenu");
		hud = GameObject.Find("HUD").GetComponent<HUD>();
	}
	private void Start ()
	{
		PlayTimeScale = 1.0f;
		weaponToSelect = null;
		curWeaponIndex = 0;
		money = 0;
		health = MAX_HEALTH;
		layerMask = BOTH_LAYER;
		isCooldown = true;
		targetEnemy = null;

		weaponList[curWeaponIndex].gameObject.SetActive(true);
		hud.UpdateAll(money, RemainingEnemy, weaponList[curWeaponIndex].CoolCounter, health);
	}
	
	private void Update () 
	{
        //Raycast (To where the mouse cursor is pointing)
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 80, layerMask)) // Detect Default Layer only
		{
			/*/
			if (hit.collider.isTrigger == true)
				Debug.Log("Trigger");
			//*/
			weaponToSelect = null;
			targetEnemy = null;
			if(hit.collider.tag == "Weapon") // When selecting weapon
			{
				weaponToSelect = hit.collider.GetComponent<WeaponSelect>();
			}
			else if (layerMask == BOTH_LAYER && (hit.collider.transform.parent.tag == "Enemy" || hit.collider.tag == "Enemy"))
			{
				targetEnemy = hit.collider.transform.parent.GetComponent<Enemy>();
			}

			hitPoint = hit.point;
		}
		else
		{
			hitPoint = new Vector3(100, 100, 100);
			weaponToSelect = null;
			targetEnemy = null;
		}
		Debug.DrawRay(ray.origin, ray.direction * 80, Color.red);

		if (layerMask != BOTH_LAYER)	// When aiming with mortar or air support
		{
			weaponList[curWeaponIndex].HitPoint = hitPoint;
		}

		//Shoot or Select Weapon
		if (Input.GetMouseButton(0))
		{
			if (weaponToSelect != null)	// Select Weapon or Open Shop
			{
				if (weaponToSelect.WeaponType >= 0) // Select Weapon
				{
					weaponList[curWeaponIndex].WeaponDeactivated();
					weaponList[curWeaponIndex].gameObject.SetActive(false);

					curWeaponIndex = weaponToSelect.WeaponType;

					weaponList[curWeaponIndex].gameObject.SetActive(true);
					weaponList[curWeaponIndex].WeaponActivated();
					layerMask = weaponToSelect.LayerMask;
					if (weaponList[curWeaponIndex].CoolCounter <= 0)
					{
						CoolDown(true);
					}
					else
					{
						CoolDown(false);
					}
				}
				else // Open Shop
				{
					if(PlayTimeScale != 0)
					{
						PlayTimeScale = 0;
						shopMenu.SetActive(true);
					}
				}
			}
			else if (isCooldown && PlayTimeScale > 0) // Shoot
			{
				weaponList[curWeaponIndex].Fire(targetEnemy, hitPoint);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape) && PlayTimeScale!= 0)
		{
			PlayTimeScale = 0;
			pauseMenu.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.Space))//For test
		{
			ResetHealth();
		}

		if (isCooldown)
		{
			hud.UpdateCoolTime(0);
		}
		else
		{
			hud.UpdateCoolTime(weaponList[curWeaponIndex].CoolCounter);
		}

        //Player Rotation(Rotate toward cursor)
        targetPos = new Vector3(ray.direction.x*1.3f, transform.position.y, ray.direction.z);
        lookRot = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 40 * PlayTimeScale);
	}
	public void CoolDown(bool cooled)
	{
		isCooldown = cooled;
	}
	public void Earn(int amount)
	{
		money += amount;
		hud.UpdateResource(money);
	}
	public int GetMoney()
	{
		return money;
	}
	public void Attacked(float dmg)
	{
		health -= dmg;
		if (health <= 0)
		{
			health = 0;
			Debug.Log("Game Over");
			//*
			PlayTimeScale = 0;
			pauseMenu.SetActive(false);
			shopMenu.SetActive(false);
			gameoverMenu.SetActive(true);
			//*/
		}
		hud.UpdateHP(health);
	}
	public void ResetHealth()
	{
		health = MAX_HEALTH;
	}
	public static Player GetPlayer()
	{
		return GameObject.Find("Player").GetComponent<Player>();
	}
	public static void ReduceRemainingEnemy()
	{
		RemainingEnemy--;
		if (RemainingEnemy <= 0)
		{
			RemainingEnemy = 0;
			PlayTimeScale = 0;
			Player.GetPlayer().winMenu.SetActive(true);
		}
		Player.GetPlayer().hud.UpdateNumOfEnemy(RemainingEnemy);
	}
}
