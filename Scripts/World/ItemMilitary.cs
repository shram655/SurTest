using UnityEngine;

public class ItemMilitary : MonoBehaviour
{
	public bool inSceneObj;

	public GameObject[] WEAPON;

	public int _weaponType;

	private void Start()
	{
		if (inSceneObj)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("_WEAPON_BS"));
			gameObject.GetComponent<ItemMilitary>()._weaponType = _weaponType;
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = base.transform.rotation;
			Object.Destroy(base.gameObject);
		}
		else
		{
			base.name = "WEAPON_LOOT_ID: " + BaseLogic.LOOT_WEAPON;
			BaseLogic.LOOT_WEAPON++;
			HideAllItems();
			GenerateLoot();
		}
	}

	private void HideAllItems()
	{
		for (int i = 0; i < WEAPON.Length; i++)
		{
			WEAPON[i].GetComponent<WeaponPickup>().isLoot = true;
			WEAPON[i].SetActive(false);
		}
	}

	private void GenerateLoot()
	{
		if (_weaponType == 0)
		{
			int num = Random.Range(0, 400);
			if (num < 280)
			{
				if (num < 40)
				{
					WEAPON[0].SetActive(true);
				}
				else if (num < 70)
				{
					WEAPON[1].SetActive(true);
				}
				else if (num < 90)
				{
					WEAPON[4].SetActive(true);
				}
				else if (num < 110)
				{
					WEAPON[3].SetActive(true);
				}
				else if (num < 130)
				{
					WEAPON[8].SetActive(true);
				}
				else if (num < 145)
				{
					WEAPON[2].SetActive(true);
				}
				else if (num < 165)
				{
					WEAPON[9].SetActive(true);
				}
				else if (num < 180)
				{
					WEAPON[11].SetActive(true);
				}
				else if (num < 210)
				{
					WEAPON[12].SetActive(true);
				}
				else if (num < 220)
				{
					WEAPON[13].SetActive(true);
				}
				else if (num < 280)
				{
					WEAPON[21].SetActive(true);
				}
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			for (int i = 0; i < WEAPON.Length; i++)
			{
				if (!WEAPON[i].activeInHierarchy)
				{
					Object.Destroy(WEAPON[i]);
				}
			}
		}
		else if (_weaponType == 1)
		{
			int num2 = Random.Range(0, 200);
			if (num2 < 90)
			{
				if (num2 < 10)
				{
					WEAPON[8].SetActive(true);
				}
				else if (num2 < 20)
				{
					WEAPON[2].SetActive(true);
				}
				else if (num2 < 30)
				{
					WEAPON[7].SetActive(true);
				}
				else if (num2 < 40)
				{
					WEAPON[5].SetActive(true);
				}
				else if (num2 < 50)
				{
					WEAPON[9].SetActive(true);
				}
				else if (num2 < 60)
				{
					WEAPON[11].SetActive(true);
				}
				else if (num2 < 70)
				{
					WEAPON[12].SetActive(true);
				}
				else if (num2 < 80)
				{
					WEAPON[13].SetActive(true);
				}
				else if (num2 < 90)
				{
					WEAPON[22].SetActive(true);
				}
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			for (int j = 0; j < WEAPON.Length; j++)
			{
				if (!WEAPON[j].activeInHierarchy)
				{
					Object.Destroy(WEAPON[j]);
				}
			}
		}
		else if (_weaponType == 2)
		{
			int num3 = Random.Range(0, 200);
			if (num3 < 100)
			{
				if (num3 < 10)
				{
					WEAPON[7].SetActive(true);
				}
				else if (num3 < 20)
				{
					WEAPON[5].SetActive(true);
				}
				else if (num3 < 30)
				{
					WEAPON[6].SetActive(true);
				}
				else if (num3 < 40)
				{
					WEAPON[9].SetActive(true);
				}
				else if (num3 < 50)
				{
					WEAPON[10].SetActive(true);
				}
				else if (num3 < 60)
				{
					WEAPON[18].SetActive(true);
				}
				else if (num3 < 70)
				{
					WEAPON[22].SetActive(true);
				}
				else if (num3 < 80)
				{
					WEAPON[23].SetActive(true);
				}
				else if (num3 < 90)
				{
					WEAPON[24].SetActive(true);
				}
				else if (num3 < 100)
				{
					WEAPON[26].SetActive(true);
				}
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			for (int k = 0; k < WEAPON.Length; k++)
			{
				if (!WEAPON[k].activeInHierarchy)
				{
					Object.Destroy(WEAPON[k]);
				}
			}
		}
		else if (_weaponType == 3)
		{
			int num4 = Random.Range(0, 130);
			if (num4 < 90)
			{
				if (num4 < 10)
				{
					WEAPON[6].SetActive(true);
				}
				else if (num4 < 20)
				{
					WEAPON[10].SetActive(true);
				}
				else if (num4 < 30)
				{
					WEAPON[17].SetActive(true);
				}
				else if (num4 < 40)
				{
					WEAPON[18].SetActive(true);
				}
				else if (num4 < 50)
				{
					WEAPON[20].SetActive(true);
				}
				else if (num4 < 60)
				{
					WEAPON[23].SetActive(true);
				}
				else if (num4 < 70)
				{
					WEAPON[24].SetActive(true);
				}
				else if (num4 < 80)
				{
					WEAPON[25].SetActive(true);
				}
				else if (num4 < 90)
				{
					WEAPON[26].SetActive(true);
				}
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			for (int l = 0; l < WEAPON.Length; l++)
			{
				if (!WEAPON[l].activeInHierarchy)
				{
					Object.Destroy(WEAPON[l]);
				}
			}
		}
		else
		{
			if (_weaponType != 4)
			{
				return;
			}
			int num5 = Random.Range(0, 80);
			if (num5 < 80)
			{
				if (num5 < 10)
				{
					WEAPON[15].SetActive(true);
				}
				else if (num5 < 20)
				{
					WEAPON[16].SetActive(true);
				}
				else if (num5 < 30)
				{
					WEAPON[17].SetActive(true);
				}
				else if (num5 < 40)
				{
					WEAPON[19].SetActive(true);
				}
				else if (num5 < 50)
				{
					WEAPON[20].SetActive(true);
				}
				else if (num5 < 60)
				{
					WEAPON[24].SetActive(true);
				}
				else if (num5 < 70)
				{
					WEAPON[25].SetActive(true);
				}
				else if (num5 < 80)
				{
					WEAPON[27].SetActive(true);
				}
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			for (int m = 0; m < WEAPON.Length; m++)
			{
				if (!WEAPON[m].activeInHierarchy)
				{
					Object.Destroy(WEAPON[m]);
				}
			}
		}
	}
}
