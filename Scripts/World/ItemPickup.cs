using System.Collections;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
	public bool inSceneObj;

	public bool isLoot;

	public int lootType;

	public Renderer[] ITEM;

	public int ITEM_ID;

	public int ITEM_COUNT;

	public AudioClip pickupSound;

	private Transform myTransform;

	private void Awake()
	{
		if (isLoot)
		{
			Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
		}
	}

	private IEnumerator RespawnItems(int timez)
	{
		yield return new WaitForSeconds(timez);
		CreateLoot();
	}

	private void CreateLoot()
	{
		if (inSceneObj)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("_ITEM_BS"));
			gameObject.GetComponent<ItemPickup>().lootType = lootType;
			if (lootType == 100)
			{
				gameObject.GetComponent<ItemPickup>().ITEM_ID = ITEM_ID;
				gameObject.GetComponent<ItemPickup>().ITEM_COUNT = ITEM_COUNT;
			}
			gameObject.GetComponent<ItemPickup>().isLoot = isLoot;
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = base.transform.rotation;
			Object.Destroy(gameObject, 1200f);
			base.GetComponent<Renderer>().enabled = false;
			StartCoroutine("RespawnItems", Random.Range(1200, 2400));
		}
		else
		{
			myTransform = base.transform;
			if (isLoot)
			{
				base.name = "ITEM_LOOT_ID: " + BaseLogic.LOOT_ITEM;
				BaseLogic.LOOT_ITEM++;
				Object.Destroy(base.gameObject.GetComponent<Rigidbody>());
				GenerateLoot();
			}
			else
			{
				Object.Destroy(base.gameObject.GetComponent<Rigidbody>(), 5f);
			}
		}
	}

	private void Start()
	{
		CreateLoot();
	}

	private void GenerateLoot()
	{
		if (lootType == 0)
		{
			int num = Random.Range(0, 820);
			ITEM_COUNT = 100;
			if (num < 10)
			{
				ShowItem(1);
			}
			else if (num < 20)
			{
				ShowItem(2);
			}
			else if (num < 30)
			{
				ShowItem(4);
			}
			else if (num < 40)
			{
				ShowItem(5);
			}
			else if (num < 45)
			{
				ITEM_COUNT = Random.Range(5, 39);
				ShowItem(6);
			}
			else if (num < 55)
			{
				ShowItem(7);
			}
			else if (num < 65)
			{
				ShowItem(8);
			}
			else if (num < 70)
			{
				ShowItem(9);
			}
			else if (num < 80)
			{
				ShowItem(10);
			}
			else if (num < 90)
			{
				ShowItem(11);
			}
			else if (num < 93)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(12);
			}
			else if (num < 96)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(13);
			}
			else if (num < 100)
			{
				ITEM_COUNT = Random.Range(8, 15);
				ShowItem(14);
			}
			else if (num < 103)
			{
				ITEM_COUNT = Random.Range(10, 20);
				ShowItem(15);
			}
			else if (num < 107)
			{
				ITEM_COUNT = Random.Range(5, 15);
				ShowItem(16);
			}
			else if (num < 111)
			{
				ITEM_COUNT = Random.Range(5, 10);
				ShowItem(21);
			}
			else if (num < 115)
			{
				ITEM_COUNT = Random.Range(6, 18);
				ShowItem(22);
			}
			else if (num < 119)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(23);
			}
			else if (num < 123)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(24);
			}
			else if (num < 126)
			{
				ITEM_COUNT = Random.Range(10, 20);
				ShowItem(25);
			}
			else if (num < 130)
			{
				ITEM_COUNT = Random.Range(10, 25);
				ShowItem(26);
			}
			else if (num < 135)
			{
				ITEM_COUNT = 100;
				ShowItem(27);
			}
			else if (num < 140)
			{
				ShowItem(3);
			}
			else if (num < 150)
			{
				ShowItem(28);
			}
			else if (num < 160)
			{
				ShowItem(29);
			}
			else if (num < 170)
			{
				ITEM_COUNT = Random.Range(8, 15);
				ShowItem(16);
			}
			else if (num < 200)
			{
				ITEM_COUNT = Random.Range(30, 100);
				ShowItem(35);
			}
			else if (num < 205)
			{
				ITEM_COUNT = Random.Range(30, 100);
				ShowItem(36);
			}
			else if (num < 210)
			{
				ITEM_COUNT = Random.Range(6, 12);
				ShowItem(37);
			}
			else if (num < 220)
			{
				ITEM_COUNT = Random.Range(5, 39);
				ShowItem(39);
			}
			else if (num < 225)
			{
				ITEM_COUNT = Random.Range(5, 8);
				ShowItem(41);
			}
			else if (num < 230)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(42);
			}
			else if (num < 240)
			{
				ShowItem(51);
			}
			else if (num < 250)
			{
				ShowItem(59);
			}
			else if (num < 260)
			{
				ShowItem(60);
			}
			else if (num < 265)
			{
				ShowItem(63);
			}
			else if (num < 270)
			{
				ShowItem(64);
			}
			else if (num < 274)
			{
				ITEM_COUNT = Random.Range(10, 20);
				ShowItem(66);
			}
			else if (num < 279)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(42);
			}
			else if (num < 295)
			{
				ShowItem(67);
			}
			else if (num < 298)
			{
				ITEM_COUNT = Random.Range(10, 20);
				ShowItem(68);
			}
			else if (num < 305)
			{
				ShowItem(78);
			}
			else if (num < 310)
			{
				ShowItem(76);
			}
			else if (num < 315)
			{
				ITEM_COUNT = Random.Range(25, 100);
				ShowItem(77);
			}
			else if (num < 335)
			{
				ITEM_COUNT = Random.Range(65, 100);
				ShowItem(87);
			}
			else if (num < 355)
			{
				ITEM_COUNT = Random.Range(65, 100);
				ShowItem(88);
			}
			else if (num < 375)
			{
				ITEM_COUNT = Random.Range(65, 100);
				ShowItem(89);
			}
			else if (num < 395)
			{
				ITEM_COUNT = Random.Range(65, 100);
				ShowItem(90);
			}
			else if (num < 420)
			{
				ITEM_COUNT = Random.Range(65, 100);
				ShowItem(91);
			}
			else if (num < 450)
			{
				ShowItem(107);
			}
			else if (num < 470)
			{
				ITEM_COUNT = Random.Range(4, 7);
				ShowItem(109);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (lootType == 1)
		{
			int num2 = Random.Range(0, 800);
			ITEM_COUNT = 100;
			if (num2 < 10)
			{
				ITEM_COUNT = Random.Range(20, 30);
				ShowItem(12);
			}
			else if (num2 < 20)
			{
				ITEM_COUNT = Random.Range(20, 30);
				ShowItem(13);
			}
			else if (num2 < 30)
			{
				ITEM_COUNT = Random.Range(12, 15);
				ShowItem(14);
			}
			else if (num2 < 40)
			{
				ITEM_COUNT = Random.Range(15, 20);
				ShowItem(15);
			}
			else if (num2 < 50)
			{
				ITEM_COUNT = Random.Range(20, 30);
				ShowItem(16);
			}
			else if (num2 < 60)
			{
				ITEM_COUNT = Random.Range(8, 10);
				ShowItem(21);
			}
			else if (num2 < 70)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(22);
			}
			else if (num2 < 80)
			{
				ITEM_COUNT = Random.Range(25, 50);
				ShowItem(23);
			}
			else if (num2 < 90)
			{
				ITEM_COUNT = Random.Range(25, 50);
				ShowItem(24);
			}
			else if (num2 < 100)
			{
				ITEM_COUNT = Random.Range(20, 40);
				ShowItem(25);
			}
			else if (num2 < 110)
			{
				ITEM_COUNT = Random.Range(20, 40);
				ShowItem(26);
			}
			else if (num2 < 130)
			{
				ITEM_COUNT = Random.Range(20, 30);
				ShowItem(16);
			}
			else if (num2 < 150)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(37);
			}
			else if (num2 < 160)
			{
				ITEM_COUNT = Random.Range(7, 10);
				ShowItem(40);
			}
			else if (num2 < 170)
			{
				ITEM_COUNT = Random.Range(7, 8);
				ShowItem(41);
			}
			else if (num2 < 180)
			{
				ITEM_COUNT = Random.Range(20, 50);
				ShowItem(42);
			}
			else if (num2 < 190)
			{
				ShowItem(48);
			}
			else if (num2 < 200)
			{
				ShowItem(43);
			}
			else if (num2 < 210)
			{
				ShowItem(45);
			}
			else if (num2 < 220)
			{
				ShowItem(53);
			}
			else if (num2 < 230)
			{
				ShowItem(54);
			}
			else if (num2 < 240)
			{
				ShowItem(58);
			}
			else if (num2 < 250)
			{
				ShowItem(64);
			}
			else if (num2 < 260)
			{
				ITEM_COUNT = Random.Range(15, 20);
				ShowItem(66);
			}
			else if (num2 < 265)
			{
				ShowItem(67);
			}
			else if (num2 < 275)
			{
				ITEM_COUNT = Random.Range(15, 20);
				ShowItem(68);
			}
			else if (num2 < 285)
			{
				ITEM_COUNT = Random.Range(30, 45);
				ShowItem(69);
			}
			else if (num2 < 295)
			{
				ITEM_COUNT = Random.Range(7, 10);
				ShowItem(74);
			}
			else if (num2 < 305)
			{
				ITEM_COUNT = Random.Range(15, 30);
				ShowItem(75);
			}
			else if (num2 < 315)
			{
				ITEM_COUNT = Random.Range(25, 30);
				ShowItem(82);
			}
			else if (num2 < 330)
			{
				ITEM_COUNT = Random.Range(8, 10);
				ShowItem(83);
			}
			else if (num2 < 340)
			{
				ShowItem(84);
			}
			else if (num2 < 350)
			{
				ITEM_COUNT = Random.Range(3, 5);
				ShowItem(85);
			}
			else if (num2 < 360)
			{
				ITEM_COUNT = Random.Range(35, 45);
				ShowItem(86);
			}
			else if (num2 < 370)
			{
				ITEM_COUNT = Random.Range(9, 15);
				ShowItem(92);
			}
			else if (num2 < 395)
			{
				ITEM_COUNT = Random.Range(35, 55);
				ShowItem(93);
			}
			else if (num2 < 405)
			{
				ITEM_COUNT = Random.Range(22, 30);
				ShowItem(94);
			}
			else if (num2 < 415)
			{
				ITEM_COUNT = Random.Range(22, 30);
				ShowItem(95);
			}
			else if (num2 < 418)
			{
				ITEM_COUNT = Random.Range(12, 20);
				ShowItem(96);
			}
			else if (num2 < 423)
			{
				ShowItem(97);
			}
			else if (num2 < 453)
			{
				ShowItem(102);
			}
			else if (num2 < 470)
			{
				ShowItem(98);
			}
			else if (num2 < 480)
			{
				ShowItem(103);
			}
			else if (num2 < 500)
			{
				ShowItem(106);
			}
			else if (num2 < 510)
			{
				ITEM_COUNT = Random.Range(8, 10);
				ShowItem(40);
			}
			else if (num2 < 550)
			{
				ShowItem(107);
			}
			else if (num2 < 560)
			{
				ITEM_COUNT = Random.Range(50, 64);
				ShowItem(108);
			}
			else if (num2 < 600)
			{
				ITEM_COUNT = Random.Range(4, 7);
				ShowItem(109);
			}
			else if (num2 < 610)
			{
				ShowItem(110);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (lootType == 2)
		{
			int num3 = Random.Range(0, 850);
			ITEM_COUNT = 100;
			if (num3 < 125)
			{
				ITEM_COUNT = 100;
				ShowItem(32);
			}
			else if (num3 < 200)
			{
				ITEM_COUNT = Random.Range(40, 100);
				ShowItem(38);
			}
			else if (num3 < 300)
			{
				ITEM_COUNT = Random.Range(40, 100);
				ShowItem(50);
			}
			else if (num3 < 400)
			{
				ITEM_COUNT = Random.Range(40, 100);
				ShowItem(44);
			}
			else if (num3 < 500)
			{
				ITEM_COUNT = Random.Range(40, 100);
				ShowItem(49);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (lootType == 3 || lootType == 4)
		{
			int num4 = Random.Range(0, 2400);
			ITEM_COUNT = 100;
			if (num4 < 125)
			{
				ITEM_COUNT = Random.Range(5, 20);
				ShowItem(25);
			}
			else if (num4 < 200)
			{
				ITEM_COUNT = Random.Range(0, 20);
				ShowItem(96);
			}
			else if (num4 < 300)
			{
				ITEM_COUNT = 100;
				ShowItem(97);
			}
			else if (num4 < 400)
			{
				ITEM_COUNT = Random.Range(0, 30);
				ShowItem(95);
			}
			else if (num4 < 500)
			{
				ITEM_COUNT = Random.Range(10, 30);
				ShowItem(93);
			}
			else if (num4 < 600)
			{
				ITEM_COUNT = Random.Range(10, 30);
				ShowItem(93);
			}
			else if (num4 < 700)
			{
				ITEM_COUNT = Random.Range(0, 30);
				ShowItem(94);
			}
			else if (num4 < 800)
			{
				ITEM_COUNT = Random.Range(0, 30);
				ShowItem(13);
			}
			else if (num4 < 900)
			{
				ITEM_COUNT = Random.Range(35, 45);
				ShowItem(86);
			}
			else if (num4 < 1000)
			{
				ITEM_COUNT = Random.Range(0, 5);
				ShowItem(85);
			}
			else if (num4 < 1100)
			{
				ITEM_COUNT = Random.Range(0, 10);
				ShowItem(74);
			}
			else if (num4 < 1200)
			{
				ITEM_COUNT = Random.Range(5, 15);
				ShowItem(75);
			}
			else if (num4 < 1300)
			{
				ITEM_COUNT = Random.Range(0, 100);
				ShowItem(69);
			}
			else if (num4 < 1400)
			{
				ShowItem(105);
			}
			else if (num4 < 1500)
			{
				ShowItem(106);
			}
			else if (num4 < 1600)
			{
				ITEM_COUNT = Random.Range(50, 64);
				ShowItem(108);
			}
			else if (num4 < 1700)
			{
				ITEM_COUNT = Random.Range(4, 7);
				ShowItem(109);
			}
			else if (num4 < 1800)
			{
				ShowItem(110);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (lootType == 5)
		{
			int num5 = Random.Range(0, 5000);
			ITEM_COUNT = 100;
			if (num5 < 1500)
			{
				ShowItem(105);
			}
			else if (num5 < 3000)
			{
				ShowItem(106);
			}
			else if (num5 < 5000)
			{
				ShowItem(107);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (lootType == 100)
		{
			ShowItem(ITEM_ID);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void HideAllItems()
	{
		for (int i = 0; i < ITEM.Length; i++)
		{
			if ((bool)ITEM[i])
			{
				ITEM[i].enabled = false;
			}
		}
	}

	private void ShowItem(int helm)
	{
		HideAllItems();
		if (helm > 0)
		{
			ITEM[helm - 1].enabled = true;
			ITEM_ID = helm - 1;
			for (int i = 0; i < ITEM.Length; i++)
			{
				if (helm - 1 != i && (bool)ITEM[i])
				{
					Object.Destroy(ITEM[i].gameObject);
				}
			}
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void PickUpItem()
	{
		if ((bool)GetComponent<MissionObject>())
		{
			GameObject gameObject = GameObject.Find("_missionLogic");
			if (GetComponent<MissionObject>().QUEST_ID != gameObject.GetComponent<MissionScenario>().ID_QUEST)
			{
				return;
			}
		}
		bool flag = true;
		for (int i = 0; i < BaseLogic.PLAYER_ALLSLOTS; i++)
		{
			if (BaseLogic.PLAYER_SLOT_REAL[i] == 0)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			return;
		}
		if (lootType == 100)
		{
			if (ITEM_ID == 2 || ITEM_ID == 6 || ITEM_ID == 38 || ITEM_ID == 58 || ITEM_ID == 64 || ITEM_ID == 101)
			{
				if (BaseLogic.PLAYER_MONEY >= 50)
				{
					BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - 50);
					PickupZ();
				}
			}
			else if (ITEM_ID == 77 || ITEM_ID == 53 || ITEM_ID == 11 || ITEM_ID == 14 || ITEM_ID == 12 || ITEM_ID == 81 || ITEM_ID == 15 || ITEM_ID == 82 || ITEM_ID == 94 || ITEM_ID == 24)
			{
				if (BaseLogic.PLAYER_MONEY >= 100)
				{
					BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - 100);
					PickupZ();
				}
			}
			else if (ITEM_ID == 8 || ITEM_ID == 57 || ITEM_ID == 91 || ITEM_ID == 93 || ITEM_ID == 36 || ITEM_ID == 39)
			{
				if (BaseLogic.PLAYER_MONEY >= 150)
				{
					BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - 150);
					PickupZ();
				}
			}
			else if (ITEM_ID == 85 || ITEM_ID == 84 || ITEM_ID == 43 || ITEM_ID == 106)
			{
				if (BaseLogic.PLAYER_MONEY >= 200)
				{
					BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - 200);
					PickupZ();
				}
			}
			else if (ITEM_ID == 68 || ITEM_ID == 96)
			{
				if (BaseLogic.PLAYER_MONEY >= 350)
				{
					BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - 350);
					PickupZ();
				}
			}
			else if ((ITEM_ID == 75 || ITEM_ID == 76 || ITEM_ID == 105) && BaseLogic.PLAYER_MONEY >= 500)
			{
				BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - 500);
				PickupZ();
			}
		}
		else
		{
			PickupZ();
		}
	}

	private void PickupZ()
	{
		if (lootType == 100)
		{
			AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().KASSA, base.transform.position);
		}
		if (ITEM_ID != 60)
		{
			ITEM[ITEM_ID].enabled = false;
		}
		base.GetComponent<Collider>().enabled = false;
		if ((bool)Network.PLAYER)
		{
			if ((bool)pickupSound)
			{
				AudioSource.PlayClipAtPoint(pickupSound, myTransform.position, 0.75f);
			}
			if (!GetComponent<MissionObject>())
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.Others, base.gameObject.name);
			}
			StartCoroutine("Pickup");
		}
	}

	private IEnumerator Pickup()
	{
		yield return new WaitForSeconds(1f);
		for (int item = 0; item < BaseLogic.PLAYER_ALLSLOTS; item++)
		{
			if (BaseLogic.PLAYER_SLOT_REAL[item] == 0)
			{
				if (ITEM_COUNT > 100)
				{
					ITEM_COUNT = 1;
				}
				BaseLogic.SetSlot(item, ITEM_ID + 1);
				BaseLogic.SetSlotCount(item, ITEM_COUNT);
				BaseLogic._inventory2.SendMessageUpwards("CheckInventory", SendMessageOptions.DontRequireReceiver);
				Object.Destroy(base.gameObject);
				break;
			}
		}
	}
}
