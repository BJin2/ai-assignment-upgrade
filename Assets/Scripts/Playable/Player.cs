using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public static Player Instance { get; private set; }

	//Weapon
	[SerializeField]
	private Weapon[] weaponList = null;
	private WeaponSelect weaponToSelect = null;
	private int curWeaponIndex = 0;

	//Player status
	private int money = 0;
	private float health = 0;
	private const float MAX_HEALTH = 1000;

	//Targeting
    private RaycastHit hit;
    private Ray ray;
	private int layerMask;
	public const int DEFAULT_LAYER = 1;
	public const int ENEMY_LAYER = 1 << 8;
	public const int BOTH_LAYER = DEFAULT_LAYER | ENEMY_LAYER;

    private Vector3 targetPos = Vector3.zero;
    private Quaternion lookRot = Quaternion.identity;

	private Enemy targetEnemy = null;
	public Vector3 HitPoint { get; private set; }

    private bool isCooldown;

	private int RemainingEnemy = -1;

	private GameObject pauseMenu;
	private GameObject shopMenu;
	private GameObject gameoverMenu;
	private GameObject winMenu;
	private HUD hud;

	private void Awake()
	{
		Instance = this;

		pauseMenu = GameObject.Find("PauseMenu");
		shopMenu = GameObject.Find("ShopMenu");
		gameoverMenu = GameObject.Find("GameoverMenu");
		winMenu = GameObject.Find("WinMenu");
		hud = GameObject.Find("HUD").GetComponent<HUD>();
		weaponList = new Weapon[6];
		for (int i = 0; i < 6; i++)
		{
			weaponList[i] = transform.Find(i.ToString()).GetComponent<Weapon>(); ;
		}
	}
	private void Start ()
	{
		foreach (Weapon weapon in weaponList)
		{
			weapon.gameObject.SetActive(false);
		}
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
		if (Time.deltaTime == 0.0f)
			return;

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

			HitPoint = hit.point;
		}
		else
		{
			HitPoint = new Vector3(100, 100, 100);
			weaponToSelect = null;
			targetEnemy = null;
		}
		Debug.DrawRay(ray.origin, ray.direction * 80, Color.red);

		if (layerMask != BOTH_LAYER)	// When aiming with mortar or air support
		{
			weaponList[curWeaponIndex].HitPoint = HitPoint;
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
					shopMenu.SetActive(true);
				}
			}
			else if (isCooldown && Time.timeScale > 0) // Shoot
			{
				weaponList[curWeaponIndex].Fire(targetEnemy, HitPoint);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale  != 0)
		{
			Time.timeScale = 0;
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 40 * Time.timeScale);
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
			Time.timeScale = 0;
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
	public void SetInitialRemainingEnemy(int num)
	{
		if (RemainingEnemy == -1)
		{
			RemainingEnemy = num;
		}
	}
	public void ReduceRemainingEnemy()
	{
		RemainingEnemy--;
		if (RemainingEnemy <= 0)
		{
			RemainingEnemy = 0;
			Time.timeScale = 0;
			winMenu.SetActive(true);
		}
		hud.UpdateNumOfEnemy(RemainingEnemy);
	}
}
