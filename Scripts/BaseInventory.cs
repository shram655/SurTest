using UnityEngine;

public class BaseInventory : MonoBehaviour
{
	public GameObject _BUTTON_MORTAR;

	public GameObject _BUTTON_PNV;

	public UISprite _BUTTON_PNV_SPRITE;

	public GameObject _WEAPONS_SHOP;

	public GameObject _INVENTORY_SLOTS;

	public int WEAPONS_PACK;

	public int[] WEAPON_SLOT;

	public int[] WEAPON_PRICE;

	private int[] oldWEAPON_PRICE;

	public UISprite[] SLOT;

	public UIButton[] SLOT_BUTTON;

	public UISprite[] CRAFT_SLOT;

	public UIButton[] CRAFT_SLOT_BUTTON;

	public UISprite[] SLOT_WEAPON;

	public UIButton[] SLOT_WEAPON_BUTTON;

	public UILabel[] SLOT_COUNT;

	public UILabel NAME;

	public UILabel WeapPrice;

	public UILabel SERVER_NAME;

	public UILabel MONEY_LABEL;

	public UILabel HEALTH;

	public UILabel HUNGRY;

	public UILabel THRISTY;

	public UILabel TEMPERATURE;

	public AudioClip CANOPEN;

	public AudioClip EAT;

	public AudioClip DRINK;

	public AudioClip RELOAD_MAG;

	public AudioClip GAS;

	public AudioClip BENZIN;

	public AudioClip ZAJIGALKA;

	public AudioClip BINT;

	public AudioClip ENGINE_START;

	public AudioClip ENGINE_START_NoFuel;

	public AudioClip BEEP;

	public AudioClip PNV_ON;

	public AudioClip PNV_OFF;

	public AudioClip FLASH_LIGHT;

	private void Awake()
	{
		if (BaseLogic.GAME_MODE == 6)
		{
			if (BaseLogic.LICENSE == 0)
			{
				BaseLogic.SetMoney(0);
			}
			else if (BaseLogic.LICENSE == 1)
			{
				BaseLogic.SetMoney(500);
			}
			else if (BaseLogic.LICENSE == 2)
			{
				BaseLogic.SetMoney(1000);
			}
			else if (BaseLogic.LICENSE == 3)
			{
				BaseLogic.SetMoney(2000);
			}
			else if (BaseLogic.LICENSE == 4)
			{
				BaseLogic.SetMoney(5000);
			}
		}
		WEAPON_SLOT = new int[8];
		WEAPON_PRICE = new int[100];
		oldWEAPON_PRICE = new int[100];
		SetWeaponPrice(2, 250);
		SetWeaponPrice(3, 500);
		SetWeaponPrice(4, 700);
		SetWeaponPrice(5, 600);
		SetWeaponPrice(6, 800);
		SetWeaponPrice(7, 200);
		SetWeaponPrice(8, 300);
		SetWeaponPrice(10, 650);
		SetWeaponPrice(11, 1400);
		SetWeaponPrice(12, 150);
		SetWeaponPrice(13, 100);
		SetWeaponPrice(14, 400);
		SetWeaponPrice(15, 500);
		SetWeaponPrice(16, 2500);
		SetWeaponPrice(17, 2000);
		SetWeaponPrice(18, 900);
		SetWeaponPrice(19, 400);
		SetWeaponPrice(20, 2200);
		SetWeaponPrice(21, 1800);
		SetWeaponPrice(23, 500);
		SetWeaponPrice(24, 800);
		SetWeaponPrice(25, 900);
		SetWeaponPrice(26, 1100);
		SetWeaponPrice(27, 500);
		SetWeaponPrice(28, 2900);
		CheckInventory();
	}

	private void SetWeaponPrice(int weapon, int price)
	{
		WEAPON_PRICE[weapon] = price;
		oldWEAPON_PRICE[weapon] = price * 683;
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < WEAPON_PRICE.Length; i++)
		{
			if (WEAPON_PRICE[i] != oldWEAPON_PRICE[i] / 683)
			{
				SetWeaponPrice(i, 99999);
				BaseLogic.SetBan(10001);
				BaseLogic.Banz();
			}
		}
	}

	private void Start()
	{
	}

	public void CheckInventory()
	{
		NAME.text = string.Empty;
		if (PhotonNetwork.room != null)
		{
			string text = string.Empty;
			if (BaseLogic.GAME_MODE == 0)
			{
				text = "DM Classic, ";
			}
			else if (BaseLogic.GAME_MODE == 1)
			{
				text = "TDM Classic, ";
			}
			else if (BaseLogic.GAME_MODE == 2)
			{
				text = "Tutorial, ";
			}
			else if (BaseLogic.GAME_MODE == 3)
			{
				text = "DM Classic (NO VIP), ";
			}
			else if (BaseLogic.GAME_MODE == 4)
			{
				text = "Camp, ";
			}
			else if (BaseLogic.GAME_MODE == 5)
			{
				text = "Episode 2 Classic, ";
			}
			else if (BaseLogic.GAME_MODE == 6)
			{
				text = "Arena, ";
			}
			SERVER_NAME.text = "Mode: " + text + string.Format(Localization.Get("Server_Online"), PhotonNetwork.room.name, PhotonNetwork.room.playerCount, PhotonNetwork.room.maxPlayers);
		}
		if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>() && !Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>().vehicleGun && BaseLogic._backpack != 15)
		{
			_BUTTON_MORTAR.SetActive(true);
		}
		else if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>() && BaseLogic._backpack == 15)
		{
			_BUTTON_MORTAR.SetActive(false);
		}
		else if (BaseLogic._backpack == 15)
		{
			_BUTTON_MORTAR.SetActive(true);
		}
		else
		{
			_BUTTON_MORTAR.SetActive(false);
		}
		if (BaseLogic._glasses == 0)
		{
			_BUTTON_PNV.SetActive(false);
		}
		else
		{
			_BUTTON_PNV.SetActive(true);
		}
		if (BaseLogic._glasses == 1)
		{
			_BUTTON_PNV_SPRITE.spriteName = "menu_icon_pnv";
		}
		else if (BaseLogic._glasses == 2)
		{
			_BUTTON_PNV_SPRITE.spriteName = "menu_icon_flash";
		}
		if (BaseLogic._backpack >= 1 && BaseLogic._backpack <= 6)
		{
			BaseLogic.SetAllSlots(12);
		}
		else if (BaseLogic._backpack >= 7 && BaseLogic._backpack <= 11)
		{
			BaseLogic.SetAllSlots(16);
		}
		else if (BaseLogic._backpack >= 12 && BaseLogic._backpack <= 14)
		{
			BaseLogic.SetAllSlots(20);
		}
		else if (BaseLogic._backpack == 15 || ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>()))
		{
			BaseLogic.SetAllSlots(16);
		}
		else
		{
			BaseLogic.SetAllSlots(8);
		}
		if (FPSPlayer.hungerPoints < 20)
		{
			HUNGRY.color = Color.white;
		}
		else if (FPSPlayer.hungerPoints < 30)
		{
			HUNGRY.color = new Color(1f, 0.88235295f, 0.88235295f, 1f);
		}
		else if (FPSPlayer.hungerPoints < 40)
		{
			HUNGRY.color = new Color(1f, 37f / 51f, 37f / 51f, 1f);
		}
		else if (FPSPlayer.hungerPoints < 50)
		{
			HUNGRY.color = new Color(1f, 31f / 51f, 31f / 51f, 1f);
		}
		else if (FPSPlayer.hungerPoints < 60)
		{
			HUNGRY.color = new Color(1f, 23f / 51f, 23f / 51f, 1f);
		}
		else if (FPSPlayer.hungerPoints < 70)
		{
			HUNGRY.color = new Color(1f, 1f / 3f, 1f / 3f, 1f);
		}
		else if (FPSPlayer.hungerPoints < 80)
		{
			HUNGRY.color = new Color(1f, 0.1764706f, 0.1764706f, 1f);
		}
		else
		{
			HUNGRY.color = new Color(1f, 0f, 0f, 1f);
		}
		if (FPSPlayer.thirstPoints < 20)
		{
			THRISTY.color = Color.white;
		}
		else if (FPSPlayer.thirstPoints < 30)
		{
			THRISTY.color = new Color(1f, 0.88235295f, 0.88235295f, 1f);
		}
		else if (FPSPlayer.thirstPoints < 40)
		{
			THRISTY.color = new Color(1f, 37f / 51f, 37f / 51f, 1f);
		}
		else if (FPSPlayer.thirstPoints < 50)
		{
			THRISTY.color = new Color(1f, 31f / 51f, 31f / 51f, 1f);
		}
		else if (FPSPlayer.thirstPoints < 60)
		{
			THRISTY.color = new Color(1f, 23f / 51f, 23f / 51f, 1f);
		}
		else if (FPSPlayer.thirstPoints < 70)
		{
			THRISTY.color = new Color(1f, 1f / 3f, 1f / 3f, 1f);
		}
		else if (FPSPlayer.thirstPoints < 80)
		{
			THRISTY.color = new Color(1f, 0.1764706f, 0.1764706f, 1f);
		}
		else
		{
			THRISTY.color = new Color(1f, 0f, 0f, 1f);
		}
		if (BaseLogic.GAME_MODE == 5)
		{
			TEMPERATURE.enabled = false;
		}
		else if (FPSPlayer.temperaturePoints < 20)
		{
			TEMPERATURE.color = Color.white;
		}
		else if (FPSPlayer.temperaturePoints < 30)
		{
			TEMPERATURE.color = new Color(1f, 0.88235295f, 0.88235295f, 1f);
		}
		else if (FPSPlayer.temperaturePoints < 40)
		{
			TEMPERATURE.color = new Color(1f, 37f / 51f, 37f / 51f, 1f);
		}
		else if (FPSPlayer.temperaturePoints < 50)
		{
			TEMPERATURE.color = new Color(1f, 31f / 51f, 31f / 51f, 1f);
		}
		else if (FPSPlayer.temperaturePoints < 60)
		{
			TEMPERATURE.color = new Color(1f, 23f / 51f, 23f / 51f, 1f);
		}
		else if (FPSPlayer.temperaturePoints < 70)
		{
			TEMPERATURE.color = new Color(1f, 1f / 3f, 1f / 3f, 1f);
		}
		else if (FPSPlayer.temperaturePoints < 80)
		{
			TEMPERATURE.color = new Color(1f, 0.1764706f, 0.1764706f, 1f);
		}
		else
		{
			TEMPERATURE.color = new Color(1f, 0f, 0f, 1f);
		}
		if (FPSPlayer.hitPointsReal >= 90)
		{
			HEALTH.color = Color.white;
		}
		else if (FPSPlayer.hitPointsReal >= 75)
		{
			HEALTH.color = new Color(1f, 0.88235295f, 0.88235295f, 1f);
		}
		else if (FPSPlayer.hitPointsReal >= 60)
		{
			HEALTH.color = new Color(1f, 37f / 51f, 37f / 51f, 1f);
		}
		else if (FPSPlayer.hitPointsReal >= 50)
		{
			HEALTH.color = new Color(1f, 31f / 51f, 31f / 51f, 1f);
		}
		else if (FPSPlayer.hitPointsReal >= 40)
		{
			HEALTH.color = new Color(1f, 23f / 51f, 23f / 51f, 1f);
		}
		else if (FPSPlayer.hitPointsReal >= 30)
		{
			HEALTH.color = new Color(1f, 1f / 3f, 1f / 3f, 1f);
		}
		else if (FPSPlayer.hitPointsReal >= 20)
		{
			HEALTH.color = new Color(1f, 0.1764706f, 0.1764706f, 1f);
		}
		else
		{
			HEALTH.color = new Color(1f, 0f, 0f, 1f);
		}
		MONEY_LABEL.text = "$ " + BaseLogic.PLAYER_MONEY;
		if (BaseLogic.inBuyZone)
		{
			_WEAPONS_SHOP.SetActive(true);
			_INVENTORY_SLOTS.SetActive(false);
			if (WEAPONS_PACK == 0)
			{
				WEAPON_SLOT[0] = 12;
				WEAPON_SLOT[1] = 7;
				WEAPON_SLOT[2] = 2;
				WEAPON_SLOT[3] = 8;
				WEAPON_SLOT[4] = 14;
				WEAPON_SLOT[5] = 15;
				WEAPON_SLOT[6] = 23;
				WEAPON_SLOT[7] = 4;
			}
			if (WEAPONS_PACK == 1)
			{
				WEAPON_SLOT[0] = 5;
				WEAPON_SLOT[1] = 6;
				WEAPON_SLOT[2] = 27;
				WEAPON_SLOT[3] = 25;
				WEAPON_SLOT[4] = 26;
				WEAPON_SLOT[5] = 18;
				WEAPON_SLOT[6] = 24;
				WEAPON_SLOT[7] = 19;
			}
			if (WEAPONS_PACK == 2)
			{
				WEAPON_SLOT[0] = 10;
				WEAPON_SLOT[1] = 11;
				WEAPON_SLOT[2] = 16;
				WEAPON_SLOT[3] = 17;
				WEAPON_SLOT[4] = 20;
				WEAPON_SLOT[5] = 21;
				WEAPON_SLOT[6] = 28;
				WEAPON_SLOT[7] = 27;
			}
			for (int i = 0; i < SLOT_WEAPON.Length; i++)
			{
				if (BaseLogic.PLAYER_ACTIVE_SLOT == i)
				{
					SLOT_WEAPON_BUTTON[i].defaultColor = Color.red;
				}
				else
				{
					SLOT_WEAPON_BUTTON[i].defaultColor = Color.white;
				}
				if (BaseLogic.PLAYER_ACTIVE_HINT == i)
				{
					if (WEAPON_SLOT[i] == 2)
					{
						WeapPrice.text = "Colt Phytoon\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 4)
					{
						WeapPrice.text = "M4A1\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 5)
					{
						WeapPrice.text = "AK74\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 6)
					{
						WeapPrice.text = "HK G3\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 7)
					{
						WeapPrice.text = "Mauser C96\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 8)
					{
						WeapPrice.text = "Glock\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 10)
					{
						WeapPrice.text = "Mosina Rifle\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 11)
					{
						WeapPrice.text = "SVD Rifle\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 12)
					{
						WeapPrice.text = "Makarov\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 14)
					{
						WeapPrice.text = "PP-91\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 15)
					{
						WeapPrice.text = "PP-2000\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 16)
					{
						WeapPrice.text = "Pecheneg\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 17)
					{
						WeapPrice.text = "VSS\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 18)
					{
						WeapPrice.text = "HKSL8\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 19)
					{
						WeapPrice.text = "Sayga 12 mod. 340\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 20)
					{
						WeapPrice.text = "Sako TRG Sniper Rifle\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 21)
					{
						WeapPrice.text = "RPK-74\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 23)
					{
						WeapPrice.text = "AKS-74U\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 24)
					{
						WeapPrice.text = "SKS-45 Rifle\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 25)
					{
						WeapPrice.text = "AK-107\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 26)
					{
						WeapPrice.text = "AK-12\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 27)
					{
						WeapPrice.text = "G36\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}
					else if (WEAPON_SLOT[i] == 28)
					{
						WeapPrice.text = "MK14 Rifle\n[800000]Price: $" + WEAPON_PRICE[WEAPON_SLOT[i]];
					}

				}
				if (WEAPON_SLOT[i] != 0)
				{
					SLOT_WEAPON[i].alpha = 1f;
					if (WEAPON_SLOT[i] == 2)
					{
						SLOT_WEAPON[i].spriteName = "weapon_Colt";
					}
					else if (WEAPON_SLOT[i] == 4)
					{
						SLOT_WEAPON[i].spriteName = "weapon_M4A1";
					}
					else if (WEAPON_SLOT[i] == 5)
					{
						SLOT_WEAPON[i].spriteName = "weapon_AK47";
					}
					else if (WEAPON_SLOT[i] == 6)
					{
						SLOT_WEAPON[i].spriteName = "weapon_HK";
					}
					else if (WEAPON_SLOT[i] == 7)
					{
						SLOT_WEAPON[i].spriteName = "weapon_mauser";
					}
					else if (WEAPON_SLOT[i] == 8)
					{
						SLOT_WEAPON[i].spriteName = "weapon_Glock";
					}
					else if (WEAPON_SLOT[i] == 10)
					{
						SLOT_WEAPON[i].spriteName = "weapon_Mosina";
					}
					else if (WEAPON_SLOT[i] == 11)
					{
						SLOT_WEAPON[i].spriteName = "weapon_SVD";
					}
					else if (WEAPON_SLOT[i] == 12)
					{
						SLOT_WEAPON[i].spriteName = "weapon_PM";
					}
					else if (WEAPON_SLOT[i] == 14)
					{
						SLOT_WEAPON[i].spriteName = "weapon_PP91";
					}
					else if (WEAPON_SLOT[i] == 15)
					{
						SLOT_WEAPON[i].spriteName = "weapon_PP2000";
					}
					else if (WEAPON_SLOT[i] == 16)
					{
						SLOT_WEAPON[i].spriteName = "weapon_Pecheneg";
					}
					else if (WEAPON_SLOT[i] == 17)
					{
						SLOT_WEAPON[i].spriteName = "weapon_VSS";
					}
					else if (WEAPON_SLOT[i] == 18)
					{
						SLOT_WEAPON[i].spriteName = "weapon_HKSL8";
					}
					else if (WEAPON_SLOT[i] == 19)
					{
						SLOT_WEAPON[i].spriteName = "weapon_Sayga";
					}
					else if (WEAPON_SLOT[i] == 20)
					{
						SLOT_WEAPON[i].spriteName = "weapon_Sako";
					}
					else if (WEAPON_SLOT[i] == 21)
					{
						SLOT_WEAPON[i].spriteName = "weapon_RPK";
					}
					else if (WEAPON_SLOT[i] == 23)
					{
						SLOT_WEAPON[i].spriteName = "weapon_AKS74U";
					}
					else if (WEAPON_SLOT[i] == 24)
					{
						SLOT_WEAPON[i].spriteName = "weapon_SKS";
					}
					else if (WEAPON_SLOT[i] == 25)
					{
						SLOT_WEAPON[i].spriteName = "weapon_AK107";
					}
					else if (WEAPON_SLOT[i] == 26)
					{
						SLOT_WEAPON[i].spriteName = "weapon_ak12";
					}
					else if (WEAPON_SLOT[i] == 27)
					{
						SLOT_WEAPON[i].spriteName = "weapon_G36";
					}
					else if (WEAPON_SLOT[i] == 28)
					{
						SLOT_WEAPON[i].spriteName = "weapon_MK14";
					}
				}
				else
				{
					SLOT_WEAPON[i].alpha = 0f;
				}
			}
			return;
		}
		_WEAPONS_SHOP.SetActive(false);
		_INVENTORY_SLOTS.SetActive(true);
		for (int j = 0; j < SLOT.Length; j++)
		{
			if (BaseLogic.PLAYER_ACTIVE_SLOT == j)
			{
				SLOT_BUTTON[j].defaultColor = Color.red;
			}
			else if (j > BaseLogic.PLAYER_ALLSLOTS - 1)
			{
				SLOT_BUTTON[j].defaultColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0.2f);
				SLOT_BUTTON[j].enabled = false;
			}
			else if (j <= BaseLogic.PLAYER_ALLSLOTS - 1)
			{
				SLOT_BUTTON[j].defaultColor = Color.white;
				SLOT_BUTTON[j].enabled = true;
			}
			else
			{
				SLOT_BUTTON[j].defaultColor = Color.white;
				SLOT_BUTTON[j].enabled = true;
			}
			if ((BaseLogic.PLAYER_SLOT_REAL[j] >= 12 && BaseLogic.PLAYER_SLOT_REAL[j] <= 16) || (BaseLogic.PLAYER_SLOT_REAL[j] >= 21 && BaseLogic.PLAYER_SLOT_REAL[j] <= 26) || BaseLogic.PLAYER_SLOT_REAL[j] == 37 || (BaseLogic.PLAYER_SLOT_REAL[j] >= 40 && BaseLogic.PLAYER_SLOT_REAL[j] <= 42) || BaseLogic.PLAYER_SLOT_REAL[j] == 66 || (BaseLogic.PLAYER_SLOT_REAL[j] >= 68 && BaseLogic.PLAYER_SLOT_REAL[j] <= 75) || (BaseLogic.PLAYER_SLOT_REAL[j] >= 82 && BaseLogic.PLAYER_SLOT_REAL[j] <= 83) || (BaseLogic.PLAYER_SLOT_REAL[j] >= 85 && BaseLogic.PLAYER_SLOT_REAL[j] <= 86) || (BaseLogic.PLAYER_SLOT_REAL[j] >= 92 && BaseLogic.PLAYER_SLOT_REAL[j] <= 96) || BaseLogic.PLAYER_SLOT_REAL[j] == 108 || BaseLogic.PLAYER_SLOT_REAL[j] == 109)
			{
				SLOT_COUNT[j].text = string.Empty + BaseLogic.PLAYER_SLOT_COUNT_REAL[j];
			}
			else
			{
				SLOT_COUNT[j].text = string.Empty;
			}
			if (BaseLogic.PLAYER_ACTIVE_HINT == j)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[j] == 1)
				{
					NAME.text = string.Format(Localization.Get("Cans_Soup"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 2)
				{
					NAME.text = string.Format(Localization.Get("Cans_Porridge"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 3)
				{
					NAME.text = string.Format(Localization.Get("Opener_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 4)
				{
					NAME.text = string.Format(Localization.Get("Raw_fish"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 5)
				{
					NAME.text = string.Format(Localization.Get("Cans_sprat"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 6)
				{
					NAME.text = string.Format(Localization.Get("Lighter"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 7)
				{
					NAME.text = string.Format(Localization.Get("Cans_Tinned_meat"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 8)
				{
					NAME.text = string.Format(Localization.Get("Beer"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 9)
				{
					NAME.text = string.Format(Localization.Get("Whiskey"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 10)
				{
					NAME.text = string.Format(Localization.Get("Cola"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 11)
				{
					NAME.text = string.Format(Localization.Get("Dry_breakfast"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 12)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_M4"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 13)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_AK"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 14)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_GLOCK"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 15)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_HK"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 16)
				{
					NAME.text = string.Format(Localization.Get("12_caliber_cartridges"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 17)
				{
					NAME.text = string.Format(Localization.Get("Cans_Soup"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 18)
				{
					NAME.text = string.Format(Localization.Get("Cans_Porridge"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 19)
				{
					NAME.text = string.Format(Localization.Get("Cans_Tinned_meat"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 20)
				{
					NAME.text = string.Format(Localization.Get("Cans_sprat"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 21)
				{
					NAME.text = string.Format(Localization.Get("Charge_from_Mauser_C96"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 22)
				{
					NAME.text = string.Format(Localization.Get("357_Magnum_cartridges"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 23)
				{
					NAME.text = string.Format(Localization.Get("Cartridges_5.45x39"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 24)
				{
					NAME.text = string.Format(Localization.Get("Cartridges_5.56x45_Nato"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 25)
				{
					NAME.text = string.Format(Localization.Get("Cartridges_7.62x51"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 26)
				{
					NAME.text = string.Format(Localization.Get("Cartridges_9x19"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 27)
				{
					NAME.text = string.Format(Localization.Get("Scissors_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 28)
				{
					NAME.text = string.Format(Localization.Get("Pepsi"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 29)
				{
					NAME.text = string.Format(Localization.Get("Lemonade"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 30)
				{
					NAME.text = string.Format(Localization.Get("Syringe_antibiotic_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 31)
				{
					NAME.text = string.Format(Localization.Get("Syringe_anabolic_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 32)
				{
					NAME.text = string.Format(Localization.Get("Syringe_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 33)
				{
					NAME.text = string.Format(Localization.Get("Firewood_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 34)
				{
					NAME.text = string.Format(Localization.Get("Firewood_gasoline_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 35)
				{
					NAME.text = string.Format(Localization.Get("Gasoline"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 36)
				{
					NAME.text = string.Format(Localization.Get("Gas"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 37)
				{
					NAME.text = string.Format(Localization.Get("Cartridges_7.62x54"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 38)
				{
					NAME.text = string.Format(Localization.Get("Penicillin"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 39)
				{
					NAME.text = string.Format(Localization.Get("Lighter"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 40)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_SVD"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 41)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_Makarov_gun"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 42)
				{
					NAME.text = string.Format(Localization.Get("Cartridges_9x18"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 43)
				{
					NAME.text = string.Format(Localization.Get("Fuze_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 44)
				{
					NAME.text = string.Format(Localization.Get("Bandage"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 45)
				{
					NAME.text = string.Format(Localization.Get("Ammonal_1"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 46)
				{
					NAME.text = string.Format(Localization.Get("Ammonal_2"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 47)
				{
					NAME.text = string.Format(Localization.Get("Ammonal_3"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 48)
				{
					NAME.text = string.Format(Localization.Get("Ignition_cartridge_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 49)
				{
					NAME.text = string.Format(Localization.Get("Vitamins"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 50)
				{
					NAME.text = string.Format(Localization.Get("Anabolic"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 51)
				{
					NAME.text = string.Format(Localization.Get("Potato_Fresh"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 52)
				{
					NAME.text = string.Format(Localization.Get("Potato_Baked"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 53)
				{
					NAME.text = string.Format(Localization.Get("Scope_ACOG_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 54)
				{
					NAME.text = string.Format(Localization.Get("Scope_PU-3.5_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 55)
				{
					NAME.text = string.Format(Localization.Get("Fried_fish"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 56)
				{
					NAME.text = string.Format(Localization.Get("Raw_meat"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 57)
				{
					NAME.text = string.Format(Localization.Get("Fried_meat"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 58)
				{
					NAME.text = string.Format(Localization.Get("Scope_PSO_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 59)
				{
					NAME.text = string.Format(Localization.Get("Mineral_water"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 60)
				{
					NAME.text = string.Format(Localization.Get("Juice"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 61)
				{
					NAME.text = string.Format(Localization.Get("Raw_poultry"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 62)
				{
					NAME.text = string.Format(Localization.Get("Fried_poultry"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 63)
				{
					NAME.text = string.Format(Localization.Get("Charge_battery_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 64)
				{
					NAME.text = string.Format(Localization.Get("Portable_radio_set_out"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 65)
				{
					if (BaseLogic.RADIO == 1)
					{
						NAME.text = string.Format(Localization.Get("Portable_radio_set_101"));
					}
					else
					{
						NAME.text = string.Format(Localization.Get("Portable_radio_set_100"));
					}
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 66)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_PP-91"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 67)
				{
					NAME.text = string.Format(Localization.Get("Tactical_flashlight_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 68)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_PP-2000"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 69)
				{
					NAME.text = string.Format(Localization.Get("Cartridge_box_for_PKM"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 70)
				{
					NAME.text = "Магазин от ППС-43: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j] + " патр.";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 71)
				{
					NAME.text = "Магазин от М1А1: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j] + " патр.";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 72)
				{
					NAME.text = "Магазин от ТТ: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j] + " патр.";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 73)
				{
					NAME.text = "Патроны 7.62х38: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j] + " шт.";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 74)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_VSS"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 75)
				{
					NAME.text = string.Format(Localization.Get("Cartridges_9х39"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 76)
				{
					NAME.text = string.Format(Localization.Get("Accumulator_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 77)
				{
					NAME.text = string.Format(Localization.Get("Gasoline"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 78)
				{
					NAME.text = string.Format(Localization.Get("Turn-screw_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 79)
				{
					NAME.text = "Взрывчатка: Новая";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 80)
				{
					NAME.text = "Противтанковая мина";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 81)
				{
					NAME.text = "Противопехотная мина";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 82)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_HK_SL8"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 83)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_Saiga_12"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 84)
				{
					NAME.text = string.Format(Localization.Get("Collimator_scope_New"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 85)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_TRG"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 86)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_RPK74"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 87)
				{
					NAME.text = string.Format(Localization.Get("Rotten_carrot"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 88)
				{
					NAME.text = string.Format(Localization.Get("Rotten_tomato"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 89)
				{
					NAME.text = string.Format(Localization.Get("Rotten_radish"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 90)
				{
					NAME.text = string.Format(Localization.Get("Rotten_banana"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 91)
				{
					NAME.text = string.Format(Localization.Get("Rotten_apple"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 92)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_SKS"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 93)
				{
					NAME.text = string.Format(Localization.Get("Cartridges_7.62x39"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 94)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_AK12"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 95)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_AKM"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 96)
				{
					NAME.text = string.Format(Localization.Get("Magazine_for_MK14"), BaseLogic.PLAYER_SLOT_COUNT_REAL[j]);
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 97)
				{
					NAME.text = string.Format(Localization.Get("Scope_TBO"));
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 98)
				{
					NAME.text = "Grenade RGD";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 99)
				{
					NAME.text = "Grenade F1";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 100)
				{
					NAME.text = "Grenade RGD Mine";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 101)
				{
					NAME.text = "Grenade F1 Mine";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 102)
				{
					NAME.text = "50mm Fugas";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 103)
				{
					NAME.text = "100mm Fugas";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 104)
				{
					NAME.text = "30mm Ammo";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 105)
				{
					NAME.text = "PNV";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 106)
				{
					NAME.text = "Flashlight";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 107)
				{
					NAME.text = "Smoke grenade";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 108)
				{
					NAME.text = "Bizon Magazine 9x18: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j];
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 109)
				{
					NAME.text = "Delisle Magazine 9x19: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j];
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 110)
				{
					NAME.text = "Mod";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 111)
				{
					NAME.text = "MP40 Magazine 9x19: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j];
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 112)
				{
					NAME.text = "Vector Magazine 9x19: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j];
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 113)
				{
					NAME.text = "PPS43 Magazine 9x19: " + BaseLogic.PLAYER_SLOT_COUNT_REAL[j];
				}
			}
			if (BaseLogic.PLAYER_SLOT_REAL[j] != 0)
			{
				SLOT[j].alpha = 1f;
				if (BaseLogic.PLAYER_SLOT_REAL[j] == 1)
				{
					SLOT[j].spriteName = "01_canned";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 2)
				{
					SLOT[j].spriteName = "02_canned";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 3)
				{
					SLOT[j].spriteName = "03_canopener";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 4)
				{
					SLOT[j].spriteName = "04_fish";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 5)
				{
					SLOT[j].spriteName = "05_canned";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 6)
				{
					SLOT[j].spriteName = "06_lighter";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 7)
				{
					SLOT[j].spriteName = "07_canned";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 8)
				{
					SLOT[j].spriteName = "08_beer";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 9)
				{
					SLOT[j].spriteName = "09_whiskey";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 10)
				{
					SLOT[j].spriteName = "10_cola";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 11)
				{
					SLOT[j].spriteName = "11_flakes";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 12)
				{
					SLOT[j].spriteName = "12_mag_m4a1";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 13)
				{
					SLOT[j].spriteName = "13_mag_ak74";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 14)
				{
					SLOT[j].spriteName = "14_mag_glock";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 15)
				{
					SLOT[j].spriteName = "15_mag_hk";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 16)
				{
					SLOT[j].spriteName = "16_mag_shotgun";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 17)
				{
					SLOT[j].spriteName = "17_canned_open01";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 18)
				{
					SLOT[j].spriteName = "18_canned_open02";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 19)
				{
					SLOT[j].spriteName = "19_canned_open07";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 20)
				{
					SLOT[j].spriteName = "20_canned_open05";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 21)
				{
					SLOT[j].spriteName = "21_mag_mauser";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 22)
				{
					SLOT[j].spriteName = "22_ammo_colt";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 23)
				{
					SLOT[j].spriteName = "23_ammo_ak74";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 24)
				{
					SLOT[j].spriteName = "24_ammo_m4a1";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 25)
				{
					SLOT[j].spriteName = "25_ammo_hk";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 26)
				{
					SLOT[j].spriteName = "26_ammo_glock";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 27)
				{
					SLOT[j].spriteName = "27_scissors";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 28)
				{
					SLOT[j].spriteName = "28_pepsi";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 29)
				{
					SLOT[j].spriteName = "29_limonad";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 30)
				{
					SLOT[j].spriteName = "30_nark_blue";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 31)
				{
					SLOT[j].spriteName = "31_nark_red";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 32)
				{
					SLOT[j].spriteName = "32_nark_new";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 33)
				{
					SLOT[j].spriteName = "33_drova";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 34)
				{
					SLOT[j].spriteName = "34_drova_benz";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 35)
				{
					SLOT[j].spriteName = "35_benz";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 36)
				{
					SLOT[j].spriteName = "36_gas";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 37)
				{
					SLOT[j].spriteName = "37_ammo_mosina";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 38)
				{
					SLOT[j].spriteName = "38_penicillin";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 39)
				{
					SLOT[j].spriteName = "39_zippa";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 40)
				{
					SLOT[j].spriteName = "40_mag_svd";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 41)
				{
					SLOT[j].spriteName = "41_mag_pm";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 42)
				{
					SLOT[j].spriteName = "42_ammo_pm";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 43)
				{
					SLOT[j].spriteName = "43_fitil";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 44)
				{
					SLOT[j].spriteName = "44_bint";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 45)
				{
					SLOT[j].spriteName = "45_ammonal";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 46)
				{
					SLOT[j].spriteName = "46_ammonal2";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 47)
				{
					SLOT[j].spriteName = "47_ammonal3";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 48)
				{
					SLOT[j].spriteName = "48_zapal";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 49)
				{
					SLOT[j].spriteName = "49_vitamins";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 50)
				{
					SLOT[j].spriteName = "50_pervitin";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 51)
				{
					SLOT[j].spriteName = "51_pateto";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 52)
				{
					SLOT[j].spriteName = "52_pateto_cooked";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 53)
				{
					SLOT[j].spriteName = "53_mod_acog";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 54)
				{
					SLOT[j].spriteName = "54_mod_pu35";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 55)
				{
					SLOT[j].spriteName = "55_fish_cooked";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 56)
				{
					SLOT[j].spriteName = "56_meat";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 57)
				{
					SLOT[j].spriteName = "57_meat_cooked";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 58)
				{
					SLOT[j].spriteName = "58_mod_pso";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 59)
				{
					SLOT[j].spriteName = "59_water";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 60)
				{
					SLOT[j].spriteName = "60_juice";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 61)
				{
					SLOT[j].spriteName = "61_bird";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 62)
				{
					SLOT[j].spriteName = "62_bird_cooked";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 63)
				{
					SLOT[j].spriteName = "63_battery";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 64)
				{
					SLOT[j].spriteName = "64_radio_empty";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 65)
				{
					SLOT[j].spriteName = "65_radio_full";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 66)
				{
					SLOT[j].spriteName = "66_mag_pp91";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 67)
				{
					SLOT[j].spriteName = "67_mod_flashlight";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 68)
				{
					SLOT[j].spriteName = "68_mag_pp2000";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 69)
				{
					SLOT[j].spriteName = "69_mag_pecheneg";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 70)
				{
					SLOT[j].spriteName = "70_mag_pps43";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 71)
				{
					SLOT[j].spriteName = "71_mag_thompson";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 72)
				{
					SLOT[j].spriteName = "72_mag_tt";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 73)
				{
					SLOT[j].spriteName = "73_ammo_nagan";
				}
				if (BaseLogic.PLAYER_SLOT_REAL[j] == 74)
				{
					SLOT[j].spriteName = "74_mag_vss";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 75)
				{
					SLOT[j].spriteName = "75_ammo_vss";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 76)
				{
					SLOT[j].spriteName = "76_akb";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 77)
				{
					SLOT[j].spriteName = "77_benz2";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 78)
				{
					SLOT[j].spriteName = "78_otvertka";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 79)
				{
					SLOT[j].spriteName = "79_bomb";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 80)
				{
					SLOT[j].spriteName = "80_at_mine";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 81)
				{
					SLOT[j].spriteName = "81_ap_mine";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 82)
				{
					SLOT[j].spriteName = "82_mag_hksl8";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 83)
				{
					SLOT[j].spriteName = "83_mag_saiga";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 84)
				{
					SLOT[j].spriteName = "84_mod_collimator";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 85)
				{
					SLOT[j].spriteName = "85_mag_trg";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 86)
				{
					SLOT[j].spriteName = "86_mag_rpk74";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 87)
				{
					SLOT[j].spriteName = "87_carrot";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 88)
				{
					SLOT[j].spriteName = "88_tomato";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 89)
				{
					SLOT[j].spriteName = "89_radish";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 90)
				{
					SLOT[j].spriteName = "90_banana";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 91)
				{
					SLOT[j].spriteName = "91_apple";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 92)
				{
					SLOT[j].spriteName = "92_mag_sks";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 93)
				{
					SLOT[j].spriteName = "93_ammo_sks";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 94)
				{
					SLOT[j].spriteName = "94_mag_ak12";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 95)
				{
					SLOT[j].spriteName = "95_mag_akms";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 96)
				{
					SLOT[j].spriteName = "96_mag_mk14";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 97)
				{
					SLOT[j].spriteName = "97_mod_tbo";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 98)
				{
					SLOT[j].spriteName = "98_grenade_rgd";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 99)
				{
					SLOT[j].spriteName = "99_grenade_f1";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 100)
				{
					SLOT[j].spriteName = "100_grenade_rgdm";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 101)
				{
					SLOT[j].spriteName = "101_grenade_f1m";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 102)
				{
					SLOT[j].spriteName = "102_fugas_50mm";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 103)
				{
					SLOT[j].spriteName = "103_fugas_100mm";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 104)
				{
					SLOT[j].spriteName = "104_ammo_30mm";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 105)
				{
					SLOT[j].spriteName = "menu_icon_pnv";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 106)
				{
					SLOT[j].spriteName = "menu_icon_flash";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 107)
				{
					SLOT[j].spriteName = "107_smoke";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 108)
				{
					SLOT[j].spriteName = "108_mag_bizon";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 109)
				{
					SLOT[j].spriteName = "109_mag_delisle";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 110)
				{
					SLOT[j].spriteName = "110_mod";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 111)
				{
					SLOT[j].spriteName = "111_mag_mp40";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 112)
				{
					SLOT[j].spriteName = "112_mag_vector";
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[j] == 113)
				{
					SLOT[j].spriteName = "113_mag_pps";
				}
			}
			else
			{
				SLOT[j].alpha = 0f;
			}
		}
	}
}
