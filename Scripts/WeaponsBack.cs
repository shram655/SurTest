using UnityEngine;

public class WeaponsBack : MonoBehaviour
{
	public AudioClip[] longShoots;

	public ParticleSystem[] smokeEffects;

	private int oldSlot1;

	private int oldSlot2;

	private int oldModSlot1;

	public int _slotWeapon1;

	public int _slotWeapon2;

	public int _modWeapon1;

	public int _modWeapon2;

	public bool isMyHero;

	public bool Pistol;

	public bool Hands;

	public Transform[] handsPositions;

	public Renderer[] MAGAZINE;

	public GameObject[] WEAPON_GMO;

	public GameObject[] WEAPON_MOD;

	public AudioClip[] SND_FIRE;

	public Renderer FLASH;

	private int FLASH_TIME;

	public Light FLASH_LIGHT;

	public AudioClip SND_RELOAD;

	public Light flashLight;

	private void Awake()
	{
	}

	private void Start()
	{
		if (isMyHero)
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			Renderer[] array2 = array;
			foreach (Renderer renderer in array2)
			{
				renderer.gameObject.layer = 5;
			}
		}
		HideAllWeapons();
		if ((bool)FLASH)
		{
			FLASH.enabled = false;
			FLASH_LIGHT.enabled = false;
		}
	}

	private void Reload()
	{
		base.GetComponent<AudioSource>().minDistance = 3f;
		base.GetComponent<AudioSource>().maxDistance = 25f;
		base.GetComponent<AudioSource>().clip = SND_RELOAD;
		base.GetComponent<AudioSource>().Play();
	}

	private void LongFire()
	{
		if (_slotWeapon1 != 17 && _slotWeapon1 != 1 && _slotWeapon1 != 9 && _slotWeapon1 != 22)
		{
			FLASH.enabled = true;
			FLASH_LIGHT.enabled = true;
			base.GetComponent<AudioSource>().minDistance = 1f;
			base.GetComponent<AudioSource>().maxDistance = 1000f;
			base.GetComponent<AudioSource>().clip = longShoots[Random.Range(0, longShoots.Length)];
			base.GetComponent<AudioSource>().volume = Random.Range(0.7f, 0.9f);
			base.GetComponent<AudioSource>().Play();
		}
	}

	private void Fire()
	{
		if (_slotWeapon1 == 17)
		{
			base.GetComponent<AudioSource>().minDistance = 10f;
			base.GetComponent<AudioSource>().maxDistance = 75f;
			smokeEffects[0].Play();
			smokeEffects[1].Play();
		}
		else if (_slotWeapon1 != 1 && _slotWeapon1 != 9 && _slotWeapon1 != 22)
		{
			FLASH.enabled = true;
			FLASH_LIGHT.enabled = true;
			base.GetComponent<AudioSource>().minDistance = 1f;
			base.GetComponent<AudioSource>().maxDistance = 200f;
			base.GetComponent<AudioSource>().volume = Random.Range(0.7f, 1f);
			smokeEffects[0].Play();
			smokeEffects[1].Play();
		}
		else
		{
			base.GetComponent<AudioSource>().minDistance = 1f;
			base.GetComponent<AudioSource>().maxDistance = 25f;
		}
		if (_slotWeapon1 != 0)
		{
			base.GetComponent<AudioSource>().clip = SND_FIRE[_slotWeapon1 - 1];
			base.GetComponent<AudioSource>().Play();
		}
	}

	private void HideAllWeapons()
	{
		for (int i = 0; i < WEAPON_GMO.Length; i++)
		{
			if ((bool)WEAPON_GMO[i])
			{
				WEAPON_GMO[i].SetActive(false);
			}
		}
	}

	private void FixedUpdate()
	{
		if (!isMyHero && Hands && FLASH.enabled)
		{
			FLASH_TIME++;
			if (FLASH_TIME > 3)
			{
				FLASH_LIGHT.enabled = false;
				FLASH.enabled = false;
				FLASH_TIME = 0;
			}
		}
	}

	private void Update()
	{
		if (!isMyHero)
		{
			if (_slotWeapon1 == oldSlot1 && _slotWeapon2 == oldSlot2 && _modWeapon1 == oldModSlot1)
			{
				return;
			}
			HideAllWeapons();
			oldSlot1 = _slotWeapon1;
			oldModSlot1 = _modWeapon1;
			oldSlot2 = _slotWeapon2;
			if (_slotWeapon1 != 0 && Hands && WEAPON_GMO.Length >= _slotWeapon1 && (bool)WEAPON_GMO[_slotWeapon1 - 1])
			{
				WEAPON_GMO[_slotWeapon1 - 1].SetActive(true);
				if (WEAPON_MOD.Length >= _slotWeapon1)
				{
					if ((bool)WEAPON_MOD[_slotWeapon1 - 1])
					{
						if (_modWeapon1 > 0)
						{
							WEAPON_MOD[_slotWeapon1 - 1].SetActive(true);
							if (WEAPON_MOD[_slotWeapon1 - 1].name == "FLASHLIGHT")
							{
								flashLight.enabled = true;
							}
							else
							{
								flashLight.enabled = false;
							}
						}
						else
						{
							WEAPON_MOD[_slotWeapon1 - 1].SetActive(false);
							flashLight.enabled = false;
						}
					}
					else
					{
						flashLight.enabled = false;
					}
				}
			}
			if (_slotWeapon2 != 0 && !Hands && WEAPON_GMO.Length >= _slotWeapon2 && (bool)WEAPON_GMO[_slotWeapon2 - 1])
			{
				WEAPON_GMO[_slotWeapon2 - 1].SetActive(true);
			}
		}
		else
		{
			if (BaseLogic._weaponSlot1_Real == oldSlot1 && BaseLogic._weaponSlot2_Real == oldSlot2 && BaseLogic._weaponMod1 == oldModSlot1)
			{
				return;
			}
			HideAllWeapons();
			oldSlot1 = BaseLogic._weaponSlot1_Real;
			oldModSlot1 = BaseLogic._weaponMod1;
			oldSlot2 = BaseLogic._weaponSlot2_Real;
			if (BaseLogic._weaponSlot1_Real != 0 && Hands && WEAPON_GMO.Length >= BaseLogic._weaponSlot1_Real && (bool)WEAPON_GMO[BaseLogic._weaponSlot1_Real - 1])
			{
				WEAPON_GMO[BaseLogic._weaponSlot1_Real - 1].SetActive(true);
				if (WEAPON_MOD.Length >= BaseLogic._weaponSlot1_Real && (bool)WEAPON_MOD[BaseLogic._weaponSlot1_Real - 1])
				{
					if (BaseLogic._weaponMod1 > 0)
					{
						WEAPON_MOD[BaseLogic._weaponSlot1_Real - 1].SetActive(true);
					}
					else
					{
						WEAPON_MOD[BaseLogic._weaponSlot1_Real - 1].SetActive(false);
					}
				}
			}
			if (BaseLogic._weaponSlot2_Real != 0 && !Hands && WEAPON_GMO.Length >= BaseLogic._weaponSlot2_Real && (bool)WEAPON_GMO[BaseLogic._weaponSlot2_Real - 1])
			{
				WEAPON_GMO[BaseLogic._weaponSlot2_Real - 1].SetActive(true);
			}
		}
	}
}
