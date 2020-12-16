using System;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public static Player Instance { get; private set; }

	//Weapon
	private Weapon[] weaponList = null;
	private int curWeaponIndex = 0;

	//Player status
	private int resource = 0;
	public int Resource
	{
		get
		{
			return resource;
		}
		set
		{
			resource = value;
			HUD.Instance.UpdateResource(resource);
		}
	}
	private float health = 0;
	private const float MAX_HEALTH = 10;

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

	//Events
	public event Action OnPause = null;
	public event Action OnDeath = null;

	private void Awake()
	{
		Instance = this;
		weaponList = new Weapon[6];
		for (int i = 0; i < 6; i++)
		{
			weaponList[i] = transform.Find(i.ToString()).GetComponent<Weapon>(); ;
		}
		OnDeath += ()=>
		{
			gameObject.GetComponent<Animator>().Play("Death");
			Destroy(this);
		};
	}

	private void Start ()
	{
		foreach (Weapon weapon in weaponList)
		{
			weapon.gameObject.SetActive(false);
		}
		curWeaponIndex = 0;
		Resource = 0;
		health = MAX_HEALTH;
		layerMask = BOTH_LAYER;
		targetEnemy = null;

		weaponList[curWeaponIndex].ActivateWeapon(true);
		HUD.Instance.UpdateAll(_coolTime : weaponList[curWeaponIndex].CoolCounter, _hp : health);
	}
	
	private void Update () 
	{
		if (Time.deltaTime == 0.0f)
			return;

		WeaponSelect weaponToSelect = null;
		Weapon currWeapon = weaponList[curWeaponIndex];

		foreach (Weapon weapon in weaponList)
		{
			weapon.CoolDown();
		}

		//Raycast (To where the mouse cursor is pointing)
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 80, layerMask))
		{
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
		else // hitting nothing
		{
			HitPoint = new Vector3(100, 100, 100);
			weaponToSelect = null;
			targetEnemy = null;
		}
		Debug.DrawRay(ray.origin, ray.direction * 80, Color.red);

		if (layerMask != BOTH_LAYER)	// When aiming with mortar or air support
		{
			currWeapon.HitPoint = HitPoint;
		}

		//Shoot or Select Weapon
		if (Input.GetMouseButton(0))
		{
			if (weaponToSelect != null)	// Select Weapon or Open Shop
			{
				if (weaponToSelect.WeaponType >= 0) // Select Weapon
				{
					currWeapon.ActivateWeapon(false);

					curWeaponIndex = weaponToSelect.WeaponType;
					currWeapon = weaponList[curWeaponIndex];

					currWeapon.ActivateWeapon(true);
					layerMask = weaponToSelect.LayerMask;
				}
				else // Open Shop
				{
					//TODO shopMenu.SetActive(true);
				}
			}
			else if (currWeapon.Cooled) // Shoot
			{
				currWeapon.Fire(targetEnemy, HitPoint);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OnPause?.Invoke();
		}
		else if (Input.GetKeyDown(KeyCode.Space))//For test
		{
			ResetHealth();
		}

		if (currWeapon.Cooled)
		{
			HUD.Instance.UpdateCoolTime(0);
		}
		else
		{
			HUD.Instance.UpdateCoolTime(currWeapon.CoolCounter);
		}

        //Player Rotation(Rotate toward cursor)
        targetPos = new Vector3(ray.direction.x*1.3f, transform.position.y, ray.direction.z);
        lookRot = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 40 * Time.timeScale);
	}
	public void Attacked(float dmg)
	{
		if (health <= 0)
			return;

		health -= dmg;
		if (health <= 0)
		{
			HUD.Instance.UpdateHP(0);
			OnDeath?.Invoke();
			OnDeath = null;
		}
		HUD.Instance.UpdateHP(health);
	}

//Testing
	private void ResetHealth()
	{
		health = MAX_HEALTH;
	}
}
