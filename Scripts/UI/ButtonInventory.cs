using System.Collections;
using UnityEngine;

public class ButtonInventory : MonoBehaviour
{
	public int ID_BUTTON;

	private float mortarTimer;

	private float _antiLag;

	public GameObject TARGET;

	public UILabel TARGET2;

	private bool EXIT_GAME;

	private float doubleClickTimer;

	private BaseInventory inventoryBase;

	private float updateTimer;

	private int side1;

	private int side2;

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("INVENTORY");
		if ((bool)gameObject)
		{
			inventoryBase = gameObject.GetComponent<BaseInventory>();
		}
	}

	private void Update()
	{
		if ((ID_BUTTON != 1012 && ID_BUTTON != 1013) || !(Time.time > updateTimer + 2f))
		{
			return;
		}
		side1 = 0;
		side2 = 0;
		HeroBase[] array = Object.FindObjectsOfType(typeof(HeroBase)) as HeroBase[];
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].isMyHero && !array[i].BOT)
			{
				if (array[i].SIDE == 1)
				{
					side1++;
				}
				else if (array[i].SIDE == 2)
				{
					side2++;
				}
			}
		}
		if (PhotonNetwork.room != null)
		{
			if (ID_BUTTON == 1012)
			{
				TARGET.GetComponent<UILabel>().text = "Команда А: " + side1 + "/" + PhotonNetwork.room.maxPlayers / 2;
			}
			else if (ID_BUTTON == 1013)
			{
				TARGET.GetComponent<UILabel>().text = "Команда B: " + side2 + "/" + PhotonNetwork.room.maxPlayers / 2;
			}
		}
		if (ID_BUTTON == 1012 && side1 > side2)
		{
			TARGET.GetComponent<UILabel>().color = Color.gray;
		}
		else
		{
			TARGET.GetComponent<UILabel>().color = Color.white;
		}
		if (ID_BUTTON == 1013 && side2 > side1)
		{
			TARGET.GetComponent<UILabel>().color = Color.gray;
		}
		else
		{
			TARGET.GetComponent<UILabel>().color = Color.white;
		}
		updateTimer = Time.time;
	}

	private IEnumerator ExitGame()
	{
		PhotonNetwork.LeaveRoom();
		yield return new WaitForSeconds(2f);
		Application.LoadLevelAsync(0);
	}

	private void ActivationItem()
	{
		_antiLag = Time.time + 0.3f;
		BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
		BaseLogic.PLAYER_ACTIVE_SLOT = -1;
		inventoryBase.SendMessage("CheckInventory");
		inventoryBase.NAME.text = string.Empty;
	}

	private void RemoveActivationItem(int count)
	{
		_antiLag = Time.time + 0.3f;
		BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, count);
		if (BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] <= 0)
		{
			BaseLogic.PLAYER_ACTIVE_SLOT = -1;
			inventoryBase.NAME.text = string.Empty;
		}
		inventoryBase.SendMessage("CheckInventory");
	}

	public void ClickButton()
	{
		if (ID_BUTTON == 5000)
		{
			BaseLogic.CRAFT_MODE = false;
			TARGET.SetActive(true);
			TARGET2.text = string.Empty;
			inventoryBase.CheckInventory();
			return;
		}
		if (ID_BUTTON == 5001)
		{
			BaseLogic.CRAFT_MODE = true;
			TARGET.SetActive(true);
			TARGET2.text = string.Empty;
			inventoryBase.CheckInventory();
			return;
		}
		if (ID_BUTTON == 5002)
		{
			TARGET.SetActive(false);
			TARGET2.text = string.Empty;
			if (FPSPlayer.hitPointsReal < 20)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hits20_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.hitPointsReal < 40)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hits40_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.hitPointsReal < 60)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hits60_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.hitPointsReal < 80)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hits80_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.hitPointsReal < 100)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hits100_0" + Random.Range(1, 6)));
			}
			else
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hitsAll_0" + Random.Range(1, 6)));
			}
			if (FPSPlayer.hungerPoints > 80)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hungry80_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.hungerPoints > 60)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hungry60_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.hungerPoints > 40)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hungry40_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.hungerPoints > 20)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hungry20_0" + Random.Range(1, 6)));
			}
			else
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_hungry0_0" + Random.Range(1, 6)));
			}
			if (FPSPlayer.thirstPoints > 80)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_thirst80_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.thirstPoints > 60)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_thirst60_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.thirstPoints > 40)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_thirst40_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.thirstPoints > 20)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_thirst20_0" + Random.Range(1, 6)));
			}
			else
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_thirst0_0" + Random.Range(1, 6)));
			}
			if (FPSPlayer.temperaturePoints > 80)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_temp80_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.temperaturePoints > 60)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_temp60_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.temperaturePoints > 40)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_temp40_0" + Random.Range(1, 6)));
			}
			else if (FPSPlayer.temperaturePoints > 20)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_temp20_0" + Random.Range(1, 6)));
			}
			else
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_temp0_0" + Random.Range(1, 6)));
			}
			if (BaseLogic._weaponSlot1_Real == 0 && BaseLogic._weaponSlot2_Real == 0)
			{
				TARGET2.text += string.Format(Localization.Get("PersInfo_weap_0" + Random.Range(1, 6)));
			}
			return;
		}
		if (ID_BUTTON == 1003)
		{
			if (TARGET.GetComponent<UILabel>().text != "54539")
			{
				return;
			}
			BaseLogic.RECON = true;
			BaseLogic._playerWeapon.SetActive(false);
			GameObject gameObject = GameObject.Find("!!!FPS Gui");
			gameObject.SendMessage("ShowAdminPanel");
			Object[] array = Object.FindObjectsOfType(typeof(HeroBase));
			if (array.Length == 0)
			{
				Debug.Log("#HeroBase is not in Scene!");
				return;
			}
			Object[] array2 = array;
			int num = 0;
			if (num < array2.Length)
			{
				HeroBase heroBase = (HeroBase)array2[num];
				Debug.Log("#Find HeroBase Component!" + heroBase.name);
				Camera.main.GetComponent<CameraKick>().enabled = false;
				Camera.main.transform.parent.GetComponent<SmoothMouseLook>().enabled = false;
				Camera.main.GetComponent<MouseOrb>().enabled = true;
				Camera.main.GetComponent<MouseOrb>().target = heroBase.transform;
				Camera.main.GetComponent<MouseOrb>()._HeroOsmotr = true;
			}
		}
		else if (ID_BUTTON == 1011)
		{
			string text = string.Empty;
			if (BaseLogic.game_mode == "vk.com")
			{
				text = "http://vk.com/id" + BaseLogic.target_playerID;
			}
			else if (BaseLogic.game_mode == "fb.com")
			{
				text = "http://facebook.com/" + BaseLogic.target_playerID;
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				text = "http://www.odnoklassniki.ru/profile/" + BaseLogic.target_playerID;
			}
			Application.ExternalCall("OpenVKURL", string.Empty + text);
		}
		else if (ID_BUTTON == 1004)
		{
			BaseLogic._playerWeapon.SetActive(true);
			GameObject gameObject2 = GameObject.Find("!!!FPS Gui");
			gameObject2.SendMessage("ShowBasePanel");
			Camera.main.GetComponent<CameraKick>().enabled = true;
			Camera.main.transform.parent.GetComponent<SmoothMouseLook>().enabled = true;
			Camera.main.GetComponent<MouseOrb>()._HeroOsmotr = false;
			Camera.main.GetComponent<MouseOrb>().enabled = false;
			BaseLogic.RECON = false;
		}
		else if (ID_BUTTON == 1010)
		{
			BaseLogic._playerWeapon.SetActive(true);
			GameObject gameObject3 = GameObject.Find("!!!FPS Gui");
			gameObject3.SendMessage("ShowBasePanel");
			Camera.main.GetComponent<CameraKick>().enabled = true;
			Camera.main.transform.parent.GetComponent<SmoothMouseLook>().enabled = true;
			Camera.main.GetComponent<MouseOrb>().enabled = false;
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.SendMessage("AllRespawn");
				Network.PLAYER.SendMessage("Bt2Respawn");
			}
			base.gameObject.SetActive(false);
		}
		else if (ID_BUTTON == 1005 || ID_BUTTON == 1006)
		{
			HeroBase[] array3 = Object.FindObjectsOfType(typeof(HeroBase)) as HeroBase[];
			if (ID_BUTTON == 1005)
			{
				BaseLogic.SELECT_PLAYER--;
				if (BaseLogic.SELECT_PLAYER < 0)
				{
					BaseLogic.SELECT_PLAYER = array3.Length;
				}
			}
			else if (ID_BUTTON == 1006)
			{
				BaseLogic.SELECT_PLAYER++;
				if (BaseLogic.SELECT_PLAYER > array3.Length)
				{
					BaseLogic.SELECT_PLAYER = 0;
				}
			}
			for (int i = 0; i < array3.Length; i++)
			{
				if (i == BaseLogic.SELECT_PLAYER && !array3[i].isMyHero && !array3[i].BOT && array3[i].GetComponent<PhotonView>().owner.ID != PhotonNetwork.player.ID)
				{
					UILabel component = TARGET.GetComponent<UILabel>();
					string[] obj = new string[6]
					{
						string.Empty,
						array3[i].GetComponent<PhotonView>().owner.name,
						" / HP: ",
						array3[i].hitPointsReal.ToString(),
						"\nID: ",
						null
					};
					object obj2 = array3[i].GetComponent<PhotonView>().owner.customProperties["id"];
					obj[5] = ((obj2 != null) ? obj2.ToString() : null);
					component.text = string.Concat(obj);
					BaseLogic.target_playerID = string.Empty + array3[i].GetComponent<PhotonView>().owner.name;
					Camera.main.GetComponent<CameraKick>().enabled = false;
					Camera.main.transform.parent.GetComponent<SmoothMouseLook>().enabled = false;
					Camera.main.GetComponent<MouseOrb>().enabled = true;
					Camera.main.GetComponent<MouseOrb>().target = array3[i].transform;
					array3[i].GetComponent<HeroBase>()._lodSysTimer = Time.time - 8f;
					break;
				}
				if (i != BaseLogic.SELECT_PLAYER || (!array3[i].isMyHero && !array3[i].BOT && array3[i].GetComponent<PhotonView>().owner.ID != PhotonNetwork.player.ID))
				{
					continue;
				}
				if (ID_BUTTON == 1005)
				{
					BaseLogic.SELECT_PLAYER--;
					if (BaseLogic.SELECT_PLAYER < 0)
					{
						BaseLogic.SELECT_PLAYER = array3.Length;
					}
				}
				else if (ID_BUTTON == 1006)
				{
					BaseLogic.SELECT_PLAYER++;
					if (BaseLogic.SELECT_PLAYER > array3.Length)
					{
						BaseLogic.SELECT_PLAYER = 0;
					}
				}
			}
		}
		else if (ID_BUTTON != 1007)
		{
			if (ID_BUTTON == 1008)
			{
				if ((bool)Camera.main.GetComponent<MouseOrb>().target)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("Ban", PhotonTargets.All, string.Empty + Camera.main.GetComponent<MouseOrb>().target.GetComponent<PhotonView>().owner.ID + "\n" + BaseLogic.viewer_id);
				}
			}
			else if (ID_BUTTON == 1012)
			{
				if (side1 <= side2)
				{
					BaseLogic.SetSide(1);
					BaseLogic._player.SendMessage("RespawnPlayer");
				}
			}
			else if (ID_BUTTON == 1013)
			{
				if (side2 <= side1)
				{
					BaseLogic.SetSide(2);
					BaseLogic._player.SendMessage("RespawnPlayer");
				}
			}
			else if (ID_BUTTON == 1009)
			{
				switch (QualitySettings.GetQualityLevel())
				{
				case 0:
					QualitySettings.SetQualityLevel(1);
					TARGET.GetComponent<UILabel>().text = "Video: 20%";
					if ((bool)Terrain.activeTerrain)
					{
						Terrain.activeTerrain.detailObjectDistance = 40f;
						Terrain.activeTerrain.heightmapPixelError = 75f;
						Terrain.activeTerrain.treeMaximumFullLODCount = 8;
					}
					break;
				case 1:
					QualitySettings.SetQualityLevel(2);
					TARGET.GetComponent<UILabel>().text = "Video: 30%";
					if ((bool)Terrain.activeTerrain)
					{
						Terrain.activeTerrain.detailObjectDistance = 80f;
						Terrain.activeTerrain.heightmapPixelError = 65f;
						Terrain.activeTerrain.treeMaximumFullLODCount = 10;
					}
					break;
				case 2:
					QualitySettings.SetQualityLevel(3);
					TARGET.GetComponent<UILabel>().text = "Video: 50%";
					if ((bool)Terrain.activeTerrain)
					{
						Terrain.activeTerrain.detailObjectDistance = 100f;
						Terrain.activeTerrain.heightmapPixelError = 50f;
						Terrain.activeTerrain.treeMaximumFullLODCount = 20;
					}
					break;
				case 3:
					QualitySettings.SetQualityLevel(4);
					TARGET.GetComponent<UILabel>().text = "Video: 75%";
					if ((bool)Terrain.activeTerrain)
					{
						Terrain.activeTerrain.detailObjectDistance = 100f;
						Terrain.activeTerrain.heightmapPixelError = 30f;
						Terrain.activeTerrain.treeMaximumFullLODCount = 25;
					}
					break;
				case 4:
					QualitySettings.SetQualityLevel(5);
					TARGET.GetComponent<UILabel>().text = "Video: 100%";
					if ((bool)Terrain.activeTerrain)
					{
						Terrain.activeTerrain.detailObjectDistance = 100f;
						Terrain.activeTerrain.heightmapPixelError = 15f;
						Terrain.activeTerrain.treeMaximumFullLODCount = 30;
					}
					break;
				default:
					QualitySettings.SetQualityLevel(0);
					TARGET.GetComponent<UILabel>().text = "Video: 10%";
					if ((bool)Terrain.activeTerrain)
					{
						Terrain.activeTerrain.detailObjectDistance = 30f;
						Terrain.activeTerrain.heightmapPixelError = 120f;
						Terrain.activeTerrain.treeMaximumFullLODCount = 5;
					}
					break;
				}
			}
		}
		if (Time.time > _antiLag + 0.3f)
		{
			_antiLag = Time.time;
			if (ID_BUTTON == 900)
			{
				CombineItem();
			}
			else if (ID_BUTTON == 1000)
			{
				if (!EXIT_GAME)
				{
					BaseLogic._player.SendMessage("SaveGame");
					StartCoroutine("ExitGame");
					EXIT_GAME = true;
				}
			}
			else if (ID_BUTTON == 1001)
			{
				int qualityLevel = QualitySettings.GetQualityLevel();
				if (qualityLevel >= 3)
				{
					if (!Screen.fullScreen)
					{
						Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
					}
					else
					{
						Screen.SetResolution(760, 665, false);
					}
				}
				else
				{
					Screen.fullScreen = !Screen.fullScreen;
				}
			}
			else if (ID_BUTTON == 1002)
			{
				if (FPSPlayer.hitPointsReal > 0)
				{
					BaseLogic._player.SendMessage("ApplyDamage", 200);
				}
			}
			else if (ID_BUTTON == 901)
			{
				if (BaseLogic.PLAYER_DEATH || BaseLogic.INVENTORY_COMBINED != -1)
				{
					return;
				}
				if (BaseLogic.PLAYER_ACTIVE_SLOT >= 0 && BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] > 0 && (bool)Network.PLAYER)
				{
					if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 65)
					{
						BaseLogic.RADIO = 0;
					}
					Network.PLAYER.GetComponent<PhotonView>().RPC("CreateItem", PhotonTargets.All, new Vector2(BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT], BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]));
					ActivationItem();
				}
			}
			else if (ID_BUTTON == 905)
			{
				if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands)
				{
					Ironsights component2 = BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().playerObj.GetComponent<Ironsights>();
					if (component2.reloading)
					{
						return;
					}
					if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.name == "P_PP91")
					{
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().weaponMesh.GetComponent<Animation>().Play("Idle", PlayMode.StopAll);
					}
					if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MAGAZINE && BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MAGAZINE.enabled)
					{
						BaseLogic.AddItem(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().magazine_id, BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal);
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetBulletsLeft(0);
						AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
						if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MAGAZINE)
						{
							BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MAGAZINE.enabled = false;
						}
					}
					else if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal > 0)
					{
						BaseLogic.AddItem(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().magazine_id, BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal);
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetBulletsLeft(0);
						AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					}
				}
			}
			else if (ID_BUTTON == 906)
			{
				if (BaseLogic.INVENTORY_COMBINED != -1)
				{
					return;
				}
				if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands && BaseLogic._weaponMod1 == 1 && BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.name != "R_TRG")
				{
					BaseLogic._weaponMod1 = 0;
					BaseLogic.AddItem(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().mod_id, 100);
					BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MOD_ACTIVE = false;
					BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MOD.SetActive(false);
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					if ((bool)Network.PLAYER)
					{
						Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetWeapons", PhotonTargets.Others, new Quaternion(BaseLogic._weaponSlot1_Real, BaseLogic._weaponSlot2_Real, BaseLogic._weaponMod1, BaseLogic._weaponMod2));
					}
				}
			}
			else if (ID_BUTTON == 908)
			{
				if (BaseLogic.INVENTORY_COMBINED != -1)
				{
					return;
				}
				if (BaseLogic._glasses == 1)
				{
					BaseLogic.AddItem(105, 100);
					Camera.main.GetComponent<NightVisionEffect>().enabled = false;
					GameObject gameObject4 = GameObject.Find("Sky Dome");
					gameObject4.GetComponent<TOD_Sky>().Night.ShadowStrength = 1f;
					gameObject4.GetComponent<TOD_Sky>().Night.LightIntensity = 0.3f;
					AudioSource.PlayClipAtPoint(inventoryBase.PNV_OFF, Camera.main.transform.position, 1f);
				}
				else if (BaseLogic._glasses == 2)
				{
					BaseLogic.AddItem(106, 100);
					BaseLogic.FLASH_LIGHT.GetComponent<Light>().enabled = false;
					AudioSource.PlayClipAtPoint(inventoryBase.FLASH_LIGHT, Camera.main.transform.position, 1f);
				}
				BaseLogic._glasses = 0;
				base.gameObject.SetActive(false);
			}
			else if (ID_BUTTON == 907)
			{
				if (Time.time > mortarTimer + 5f)
				{
					if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>() && !Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>().vehicleGun)
					{
						BaseLogic._backpack = 15;
						string text2 = Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name;
						Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>().ExitCar2();
						Network.PLAYER.GetComponent<PhotonView>().RPC("MortarDelete", PhotonTargets.Others, text2);
						BaseLogic._player.SendMessage("HideInventory");
					}
					else if (Network.PLAYER.GetComponent<HeroBase>().VEHICLE == null)
					{
						BaseLogic._backpack = 0;
						GameObject gameObject5 = (GameObject)Object.Instantiate(Resources.Load("_STATIC_GUN_MORTAR"));
						gameObject5.transform.position = new Vector3(Network.PLAYER.transform.position.x, Network.PLAYER.transform.position.y + 0.85f, Network.PLAYER.transform.position.z);
						gameObject5.transform.rotation = Network.PLAYER.transform.rotation;
						gameObject5.name = "_MORTAR_" + PhotonNetwork.player.ID + "_" + Random.Range(0, 5000);
						base.gameObject.SetActive(false);
						Network.PLAYER.GetComponent<PhotonView>().RPC("MortarCreate", PhotonTargets.Others, gameObject5.name);
						BaseLogic._player.SendMessage("HideInventory");
					}
					mortarTimer = Time.time;
				}
			}
			else if (ID_BUTTON == 903 && BaseLogic.PLAYER_ACTIVE_SLOT >= 0)
			{
				int num2 = BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
				if (BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] > 0)
				{
					switch (num2)
					{
					default:
						return;
					case 12:
					case 82:
						BaseLogic.AddItem(24, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 13:
					case 86:
						BaseLogic.AddItem(23, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 14:
					case 68:
					case 109:
					case 111:
					case 112:
					case 113:
						BaseLogic.AddItem(26, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 15:
					case 85:
					case 96:
						BaseLogic.AddItem(25, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 40:
					case 69:
						BaseLogic.AddItem(37, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 41:
					case 108:
						BaseLogic.AddItem(42, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 66:
						BaseLogic.AddItem(42, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 74:
						BaseLogic.AddItem(75, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 83:
						BaseLogic.AddItem(16, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					case 92:
					case 94:
					case 95:
						BaseLogic.AddItem(93, BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic.AddItem(num2, 0);
						break;
					}
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 1f);
				}
			}
			else if (ID_BUTTON == 902)
			{
				UseItem();
			}
			else if (ID_BUTTON == 999)
			{
				BaseLogic.inBuyZone = false;
				BaseLogic._player.SendMessage("HideInventory");
			}
			else if (ID_BUTTON == 1100)
			{
				if (inventoryBase.WEAPONS_PACK > 0)
				{
					inventoryBase.WEAPONS_PACK--;
				}
				else if (inventoryBase.WEAPONS_PACK == 0)
				{
					inventoryBase.WEAPONS_PACK = 2;
				}
				inventoryBase.CheckInventory();
			}
			else if (ID_BUTTON == 1101)
			{
				if (inventoryBase.WEAPONS_PACK < 2)
				{
					inventoryBase.WEAPONS_PACK++;
				}
				else if (inventoryBase.WEAPONS_PACK == 2)
				{
					inventoryBase.WEAPONS_PACK = 0;
				}
				inventoryBase.CheckInventory();
			}
		}
		if (ID_BUTTON >= 900)
		{
			return;
		}
		if (BaseLogic.INVENTORY_COMBINED != -1)
		{
			if (BaseLogic.INVENTORY_COMBINED == 16 || BaseLogic.INVENTORY_COMBINED == 22 || BaseLogic.INVENTORY_COMBINED == 23 || BaseLogic.INVENTORY_COMBINED == 24 || BaseLogic.INVENTORY_COMBINED == 25 || BaseLogic.INVENTORY_COMBINED == 26 || BaseLogic.INVENTORY_COMBINED == 37 || BaseLogic.INVENTORY_COMBINED == 42 || BaseLogic.INVENTORY_COMBINED == 75 || BaseLogic.INVENTORY_COMBINED == 93)
			{
				int num3 = BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
				int num4 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == BaseLogic.INVENTORY_COMBINED && num3 + num4 <= 100 && BaseLogic.PLAYER_ACTIVE_SLOT != ID_BUTTON)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.AddItem(BaseLogic.INVENTORY_COMBINED, num3 + num4);
					CombineComplete();
					return;
				}
			}
			if (BaseLogic.INVENTORY_COMBINED == 23)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 13 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 30)
				{
					int num5 = 30 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num6 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num5 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num5);
						BaseLogic.AddItem(item, 30);
					}
					else
					{
						int count = num6 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item, count);
					}
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 86 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 45)
				{
					int num7 = 45 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num8 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item2 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num7 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num7);
						BaseLogic.AddItem(item2, 45);
					}
					else
					{
						int count2 = num8 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item2, count2);
					}
					CombineComplete();
					return;
				}
			}
			if (BaseLogic.INVENTORY_COMBINED == 24 && (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 12 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 82) && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 30)
			{
				int num9 = 30 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
				int num10 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
				int item3 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
				AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
				BaseLogic.RemoveItem(ID_BUTTON, 990);
				if (num9 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
				{
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num9);
					BaseLogic.AddItem(item3, 30);
				}
				else
				{
					int count3 = num10 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
					BaseLogic.AddItem(item3, count3);
				}
				CombineComplete();
				return;
			}
			if (BaseLogic.INVENTORY_COMBINED == 93)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 92 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 15)
				{
					int num11 = 15 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num12 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item4 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num11 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num11);
						BaseLogic.AddItem(item4, 15);
					}
					else
					{
						int count4 = num12 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item4, count4);
					}
					CombineComplete();
					return;
				}
				if ((BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 94 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 95) && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 30)
				{
					int num13 = 30 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num14 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item5 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num13 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num13);
						BaseLogic.AddItem(item5, 30);
					}
					else
					{
						int count5 = num14 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item5, count5);
					}
					CombineComplete();
					return;
				}
			}
			if (BaseLogic.INVENTORY_COMBINED == 37)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 40 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 10)
				{
					int num15 = 10 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num16 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item6 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num15 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num15);
						BaseLogic.AddItem(item6, 10);
					}
					else
					{
						int count6 = num16 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item6, count6);
					}
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 69 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 100)
				{
					int num17 = 100 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num18 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item7 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num17 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num17);
						BaseLogic.AddItem(item7, 100);
					}
					else
					{
						int count7 = num18 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item7, count7);
					}
					CombineComplete();
					return;
				}
			}
			if (BaseLogic.INVENTORY_COMBINED == 25)
			{
				if ((BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 15 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 96) && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 20)
				{
					int num19 = 20 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num20 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item8 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num19 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num19);
						BaseLogic.AddItem(item8, 20);
					}
					else
					{
						int count8 = num20 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item8, count8);
					}
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 85 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 5)
				{
					int num21 = 5 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num22 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item9 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num21 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num21);
						BaseLogic.AddItem(item9, 5);
					}
					else
					{
						int count9 = num22 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item9, count9);
					}
					CombineComplete();
					return;
				}
			}
			if (BaseLogic.INVENTORY_COMBINED == 16 && BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 83 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 10)
			{
				int num23 = 10 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
				int num24 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
				int item10 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
				AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
				BaseLogic.RemoveItem(ID_BUTTON, 990);
				if (num23 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
				{
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num23);
					BaseLogic.AddItem(item10, 10);
				}
				else
				{
					int count10 = num24 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
					BaseLogic.AddItem(item10, count10);
				}
				CombineComplete();
				return;
			}
			if (BaseLogic.INVENTORY_COMBINED == 75 && BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 74 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 10)
			{
				int num25 = 10 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
				int num26 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
				int item11 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
				AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
				BaseLogic.RemoveItem(ID_BUTTON, 990);
				if (num25 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
				{
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num25);
					BaseLogic.AddItem(item11, 10);
				}
				else
				{
					int count11 = num26 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
					BaseLogic.AddItem(item11, count11);
				}
				CombineComplete();
				return;
			}
			if (BaseLogic.INVENTORY_COMBINED == 26)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 14 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 15)
				{
					int num27 = 15 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num28 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item12 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num27 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num27);
						BaseLogic.AddItem(item12, 15);
					}
					else
					{
						int count12 = num28 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item12, count12);
					}
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 68 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 20)
				{
					int num29 = 20 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num30 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item13 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num29 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num29);
						BaseLogic.AddItem(item13, 20);
					}
					else
					{
						int count13 = num30 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item13, count13);
					}
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 109 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 7)
				{
					int num31 = 7 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num32 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item14 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num31 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num31);
						BaseLogic.AddItem(item14, 7);
					}
					else
					{
						int count14 = num32 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item14, count14);
					}
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 111 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 32)
				{
					int num33 = 32 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num34 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item15 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num33 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num33);
						BaseLogic.AddItem(item15, 32);
					}
					else
					{
						int count15 = num34 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item15, count15);
					}
					CombineComplete();
					return;
				}
				if ((BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 112 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 113) && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 30)
				{
					int num35 = 30 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num36 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item16 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num35 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num35);
						BaseLogic.AddItem(item16, 30);
					}
					else
					{
						int count16 = num36 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item16, count16);
					}
					CombineComplete();
					return;
				}
			}
			if (BaseLogic.INVENTORY_COMBINED == 42)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 41 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 8)
				{
					int num37 = 8 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num38 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item17 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num37 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num37);
						BaseLogic.AddItem(item17, 8);
					}
					else
					{
						int count17 = num38 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item17, count17);
					}
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 66 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 20)
				{
					int num39 = 20 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num40 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item18 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num39 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num39);
						BaseLogic.AddItem(item18, 20);
					}
					else
					{
						int count18 = num40 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item18, count18);
					}
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 108 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 64)
				{
					int num41 = 64 - BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int num42 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					int item19 = BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON];
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 990);
					if (num41 <= BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT])
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, num41);
						BaseLogic.AddItem(item19, 64);
					}
					else
					{
						int count19 = num42 + BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 990);
						BaseLogic.AddItem(item19, count19);
					}
					CombineComplete();
					return;
				}
			}
			else if (BaseLogic.INVENTORY_COMBINED == 35 || BaseLogic.INVENTORY_COMBINED == 77)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 33)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.BENZIN, Camera.main.transform.position, 1.25f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					if (BaseLogic.INVENTORY_COMBINED == 35)
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 50);
					}
					else if (BaseLogic.INVENTORY_COMBINED == 77)
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 10);
					}
					BaseLogic.AddItem(34, 100);
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 39)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.BENZIN, Camera.main.transform.position, 0.85f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					if (BaseLogic.INVENTORY_COMBINED == 35)
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 30);
					}
					else if (BaseLogic.INVENTORY_COMBINED == 77)
					{
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 5);
					}
					BaseLogic.AddItem(39, 100);
					BaseLogic.PLAYER_ACTIVE_SLOT = -1;
					BaseLogic.INVENTORY_COMBINED = -1;
					inventoryBase.SendMessage("CheckInventory");
					inventoryBase.NAME.text = "Ok!";
					return;
				}
				if (BaseLogic.INVENTORY_COMBINED == 35 && BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 77 && BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON] < 100)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.BENZIN, Camera.main.transform.position, 0.85f);
					int num43 = BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
					int num44 = BaseLogic.PLAYER_SLOT_COUNT_REAL[ID_BUTTON];
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					if (num44 + num43 / 6 < 100)
					{
						BaseLogic.AddItem(77, num44 + num43 / 6);
					}
					else
					{
						BaseLogic.AddItem(77, 100);
					}
					CombineComplete();
					return;
				}
			}
			else if (BaseLogic.INVENTORY_COMBINED == 32)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 38)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.BENZIN, Camera.main.transform.position, 0.85f);
					BaseLogic.RemoveItem(ID_BUTTON, 50);
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					BaseLogic.AddItem(30, 100);
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 50)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.BENZIN, Camera.main.transform.position, 0.85f);
					BaseLogic.RemoveItem(ID_BUTTON, 50);
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					BaseLogic.AddItem(31, 100);
					CombineComplete();
					return;
				}
			}
			else if (BaseLogic.INVENTORY_COMBINED == 63)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 64)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.85f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					BaseLogic.AddItem(65, 100);
					CombineComplete();
					return;
				}
			}
			else if (BaseLogic.INVENTORY_COMBINED == 48)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 45)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.85f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					BaseLogic.AddItem(46, 100);
					CombineComplete();
					return;
				}
			}
			else if (BaseLogic.INVENTORY_COMBINED == 43)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 46)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.85f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					BaseLogic.AddItem(47, 100);
					CombineComplete();
					return;
				}
			}
			else if (BaseLogic.INVENTORY_COMBINED == 36)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 6)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.GAS, Camera.main.transform.position, 0.85f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 20);
					BaseLogic.AddItem(6, 100);
					CombineComplete();
					return;
				}
			}
			else if (BaseLogic.INVENTORY_COMBINED == 6 || BaseLogic.INVENTORY_COMBINED == 39)
			{
				if (BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] >= 11)
				{
					if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 34)
					{
						AudioSource.PlayClipAtPoint(inventoryBase.ZAJIGALKA, Camera.main.transform.position, 1f);
						BaseLogic.RemoveItem(ID_BUTTON, 100);
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 10);
						CombineComplete();
						if ((bool)Network.PLAYER)
						{
							Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, 5000);
						}
						return;
					}
					if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 47 && BaseLogic.GAME_MODE != 4)
					{
						AudioSource.PlayClipAtPoint(inventoryBase.ZAJIGALKA, Camera.main.transform.position, 1f);
						BaseLogic.RemoveItem(ID_BUTTON, 100);
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 10);
						CombineComplete();
						if ((bool)Network.PLAYER)
						{
							Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, 5001);
						}
						return;
					}
				}
			}
			else if (BaseLogic.INVENTORY_COMBINED == 3 || BaseLogic.INVENTORY_COMBINED == 27 || BaseLogic.INVENTORY_COMBINED == 78)
			{
				int min = 45;
				int max = 75;
				if (BaseLogic.INVENTORY_COMBINED == 3)
				{
					min = 90;
					max = 99;
				}
				else if (BaseLogic.INVENTORY_COMBINED == 27)
				{
					min = 50;
					max = 80;
				}
				else if (BaseLogic.INVENTORY_COMBINED == 78)
				{
					min = 40;
					max = 70;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 1)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.CANOPEN, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.AddItem(17, Random.Range(min, max));
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 2)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.CANOPEN, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.AddItem(18, Random.Range(min, max));
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 5)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.CANOPEN, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.AddItem(20, Random.Range(min, max));
					CombineComplete();
					return;
				}
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 7)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.CANOPEN, Camera.main.transform.position, 0.75f);
					BaseLogic.RemoveItem(ID_BUTTON, 100);
					BaseLogic.AddItem(19, Random.Range(min, max));
					CombineComplete();
					return;
				}
			}
			AudioSource.PlayClipAtPoint(inventoryBase.BEEP, Camera.main.transform.position, 1f);
			BaseLogic.PLAYER_ACTIVE_SLOT = -1;
			BaseLogic.INVENTORY_COMBINED = -1;
			inventoryBase.SendMessage("CheckInventory");
		}
		else if (BaseLogic.inBuyZone)
		{
			if (ID_BUTTON == BaseLogic.PLAYER_ACTIVE_SLOT && Time.time < doubleClickTimer + 0.5f)
			{
				BuyWeapon();
				return;
			}
			doubleClickTimer = Time.time;
			BaseLogic.PLAYER_ACTIVE_SLOT = ID_BUTTON;
			inventoryBase.SendMessage("CheckInventory");
		}
		else
		{
			if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] <= 0)
			{
				return;
			}
			if (ID_BUTTON == BaseLogic.PLAYER_ACTIVE_SLOT && Time.time < doubleClickTimer + 0.5f)
			{
				if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 3 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 23 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 24 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 25 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 26 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 42 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 75 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 93 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 6 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 32 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 35 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 36 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 39 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 43 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 48 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 63)
				{
					CombineItem();
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 27 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 76 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 77 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 78)
				{
					if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE)
					{
						UseItem();
					}
					else
					{
						CombineItem();
					}
				}
				else if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 16 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 22 || BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 37)
				{
					GameObject gameObject6 = GameObject.Find("!!!FPS Weapons");
					WeaponBehavior component3 = gameObject6.GetComponent<PlayerWeapons>().weaponOrder[BaseLogic._weaponSlot1_Real].GetComponent<WeaponBehavior>();
					if ((BaseLogic._weaponSlot1_Real == 3 || BaseLogic._weaponSlot1_Real == 13) && BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 16 && component3.bulletsLeftReal < component3.bulletsPerClipReal)
					{
						UseItem();
					}
					else if (BaseLogic._weaponSlot1_Real == 2 && BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 22 && component3.bulletsLeftReal < component3.bulletsPerClipReal)
					{
						UseItem();
					}
					else if (BaseLogic._weaponSlot1_Real == 10 && BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] == 37 && component3.bulletsLeftReal < component3.bulletsPerClipReal)
					{
						UseItem();
					}
					else
					{
						CombineItem();
					}
				}
				else
				{
					UseItem();
				}
				doubleClickTimer = 0f;
			}
			else
			{
				doubleClickTimer = Time.time;
				BaseLogic.PLAYER_ACTIVE_SLOT = ID_BUTTON;
				inventoryBase.SendMessage("CheckInventory");
			}
		}
	}

	private void BuyAmmo()
	{
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 2)
		{
			BaseLogic.AddItem(22, 50);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 4)
		{
			BaseLogic.AddItem(12, 30);
			BaseLogic.AddItem(12, 30);
			BaseLogic.AddItem(12, 30);
			BaseLogic.AddItem(12, 30);
			BaseLogic.AddItem(12, 30);
			BaseLogic.AddItem(24, 90);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 5 || inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 23 || inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 25)
		{
			BaseLogic.AddItem(13, 30);
			BaseLogic.AddItem(13, 30);
			BaseLogic.AddItem(13, 30);
			BaseLogic.AddItem(13, 30);
			BaseLogic.AddItem(13, 30);
			BaseLogic.AddItem(23, 90);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 6)
		{
			BaseLogic.AddItem(15, 20);
			BaseLogic.AddItem(15, 20);
			BaseLogic.AddItem(15, 20);
			BaseLogic.AddItem(15, 20);
			BaseLogic.AddItem(15, 20);
			BaseLogic.AddItem(25, 80);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 7)
		{
			BaseLogic.AddItem(21, 10);
			BaseLogic.AddItem(21, 10);
			BaseLogic.AddItem(21, 10);
			BaseLogic.AddItem(21, 10);
			BaseLogic.AddItem(21, 10);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 8)
		{
			BaseLogic.AddItem(14, 15);
			BaseLogic.AddItem(14, 15);
			BaseLogic.AddItem(14, 15);
			BaseLogic.AddItem(14, 15);
			BaseLogic.AddItem(26, 45);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 10)
		{
			BaseLogic.AddItem(37, 50);
			BaseLogic.AddItem(54, 100);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 11)
		{
			BaseLogic.AddItem(40, 10);
			BaseLogic.AddItem(40, 10);
			BaseLogic.AddItem(40, 10);
			BaseLogic.AddItem(40, 10);
			BaseLogic.AddItem(40, 10);
			BaseLogic.AddItem(37, 50);
			BaseLogic.AddItem(58, 100);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 12)
		{
			BaseLogic.AddItem(41, 8);
			BaseLogic.AddItem(41, 8);
			BaseLogic.AddItem(41, 8);
			BaseLogic.AddItem(41, 8);
			BaseLogic.AddItem(41, 8);
			BaseLogic.AddItem(41, 8);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 14)
		{
			BaseLogic.AddItem(66, 20);
			BaseLogic.AddItem(66, 20);
			BaseLogic.AddItem(66, 20);
			BaseLogic.AddItem(66, 20);
			BaseLogic.AddItem(66, 20);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 15)
		{
			BaseLogic.AddItem(68, 20);
			BaseLogic.AddItem(68, 20);
			BaseLogic.AddItem(68, 20);
			BaseLogic.AddItem(68, 20);
			BaseLogic.AddItem(68, 20);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 16)
		{
			BaseLogic.AddItem(69, 100);
			BaseLogic.AddItem(69, 100);
			BaseLogic.AddItem(69, 100);
			BaseLogic.AddItem(69, 100);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 17)
		{
			BaseLogic.AddItem(74, 10);
			BaseLogic.AddItem(74, 10);
			BaseLogic.AddItem(74, 10);
			BaseLogic.AddItem(74, 10);
			BaseLogic.AddItem(74, 10);
			BaseLogic.AddItem(75, 50);
			BaseLogic.AddItem(58, 100);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 18)
		{
			BaseLogic.AddItem(82, 30);
			BaseLogic.AddItem(82, 30);
			BaseLogic.AddItem(82, 30);
			BaseLogic.AddItem(82, 30);
			BaseLogic.AddItem(82, 30);
			BaseLogic.AddItem(24, 90);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 19)
		{
			BaseLogic.AddItem(83, 10);
			BaseLogic.AddItem(83, 10);
			BaseLogic.AddItem(83, 10);
			BaseLogic.AddItem(83, 10);
			BaseLogic.AddItem(83, 10);
			BaseLogic.AddItem(16, 50);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 20)
		{
			BaseLogic.AddItem(85, 5);
			BaseLogic.AddItem(85, 5);
			BaseLogic.AddItem(85, 5);
			BaseLogic.AddItem(85, 5);
			BaseLogic.AddItem(85, 5);
			BaseLogic.AddItem(85, 5);
			BaseLogic.AddItem(25, 50);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 21)
		{
			BaseLogic.AddItem(86, 45);
			BaseLogic.AddItem(86, 45);
			BaseLogic.AddItem(86, 45);
			BaseLogic.AddItem(86, 45);
			BaseLogic.AddItem(86, 45);
			BaseLogic.AddItem(23, 90);
			BaseLogic.AddItem(23, 90);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 24)
		{
			BaseLogic.AddItem(92, 15);
			BaseLogic.AddItem(92, 15);
			BaseLogic.AddItem(92, 15);
			BaseLogic.AddItem(92, 15);
			BaseLogic.AddItem(92, 15);
			BaseLogic.AddItem(93, 90);
			BaseLogic.AddItem(54, 100);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 26)
		{
			BaseLogic.AddItem(94, 30);
			BaseLogic.AddItem(94, 30);
			BaseLogic.AddItem(94, 30);
			BaseLogic.AddItem(94, 30);
			BaseLogic.AddItem(94, 30);
			BaseLogic.AddItem(93, 90);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 27)
		{
			BaseLogic.AddItem(95, 30);
			BaseLogic.AddItem(95, 30);
			BaseLogic.AddItem(95, 30);
			BaseLogic.AddItem(95, 30);
			BaseLogic.AddItem(95, 30);
			BaseLogic.AddItem(93, 90);
		}
		if (inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] == 28)
		{
			BaseLogic.AddItem(96, 20);
			BaseLogic.AddItem(96, 20);
			BaseLogic.AddItem(96, 20);
			BaseLogic.AddItem(96, 20);
			BaseLogic.AddItem(96, 20);
			BaseLogic.AddItem(25, 80);
			BaseLogic.AddItem(97, 100);
		}
	}

	private void BuyWeapon()
	{
		if (BaseLogic.PLAYER_MONEY < inventoryBase.WEAPON_PRICE[inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT]])
		{
			inventoryBase.SendMessage("CheckInventory");
			return;
		}
		if (BaseLogic._weaponSlot1_Real == 0)
		{
			BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - inventoryBase.WEAPON_PRICE[inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT]]);
			BaseLogic.SetWeaponSlot1(inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT]);
			BaseLogic.CheckWeapons();
			AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().KASSA, base.transform.position);
			BuyAmmo();
		}
		else if (BaseLogic._weaponSlot2_Real == 0 && inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT] != BaseLogic._weaponSlot1_Real)
		{
			BaseLogic.SetMoney(BaseLogic.PLAYER_MONEY - inventoryBase.WEAPON_PRICE[inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT]]);
			BaseLogic.SetWeaponSlot2(inventoryBase.WEAPON_SLOT[BaseLogic.PLAYER_ACTIVE_SLOT]);
			BaseLogic.CheckWeapons();
			AudioSource.PlayClipAtPoint(BaseLogic._player.GetComponent<BaseLogic>().KASSA, base.transform.position);
			BuyAmmo();
		}
		inventoryBase.SendMessage("CheckInventory");
	}

	private void ShowHint(int buttonID)
	{
		if (ID_BUTTON == 901)
		{
			TARGET.SetActive(true);
			TARGET.GetComponent<UILabel>().text = "Подсказка:\nВыкинуть предмет";
		}
		else if (ID_BUTTON == 907)
		{
			if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>())
			{
				TARGET.SetActive(true);
				TARGET.GetComponent<UILabel>().text = "Подсказка:\nРазобрать миномет";
			}
			else
			{
				TARGET.SetActive(true);
				TARGET.GetComponent<UILabel>().text = "Подсказка:\nСобрать миномет";
			}
		}
		else if (ID_BUTTON == 903)
		{
			TARGET.SetActive(true);
			TARGET.GetComponent<UILabel>().text = "Подсказка:\nРазрядить магазин";
		}
		else if (ID_BUTTON == 906)
		{
			TARGET.SetActive(true);
			TARGET.GetComponent<UILabel>().text = "Подсказка:\nСнять прицел";
		}
		else if (ID_BUTTON == 908)
		{
			TARGET.SetActive(true);
			TARGET.GetComponent<UILabel>().text = "Подсказка:\nСнять ПНВ";
		}
		else if (ID_BUTTON == 905)
		{
			TARGET.SetActive(true);
			TARGET.GetComponent<UILabel>().text = "Подсказка:\nРазрядить оружие";
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[ID_BUTTON] > 0)
		{
			if (BaseLogic.INVENTORY_COMBINED == -1)
			{
				TARGET.SetActive(true);
				BaseLogic.PLAYER_ACTIVE_HINT = ID_BUTTON;
				inventoryBase.SendMessage("CheckInventory");
			}
		}
		else if (BaseLogic.inBuyZone)
		{
			TARGET.SetActive(true);
			BaseLogic.PLAYER_ACTIVE_HINT = ID_BUTTON;
			inventoryBase.SendMessage("CheckInventory");
		}
	}

	private void HideHint()
	{
		if (BaseLogic.INVENTORY_COMBINED == -1)
		{
			TARGET.SetActive(false);
		}
	}

	private void OnHover(bool isOver)
	{
		if ((ID_BUTTON >= 0 && ID_BUTTON <= 19) || ID_BUTTON == 901 || ID_BUTTON == 903 || ID_BUTTON == 907 || ID_BUTTON == 908 || ID_BUTTON == 906 || ID_BUTTON == 905)
		{
			if (isOver)
			{
				ShowHint(ID_BUTTON);
			}
			else
			{
				HideHint();
			}
		}
	}

	private void CombineComplete()
	{
		BaseLogic.PLAYER_ACTIVE_SLOT = -1;
		BaseLogic.INVENTORY_COMBINED = -1;
		inventoryBase.SendMessage("CheckInventory");
	}

	private void CombineItem()
	{
		if (BaseLogic.PLAYER_ACTIVE_SLOT != -1)
		{
			if (BaseLogic.INVENTORY_COMBINED == -1)
			{
				inventoryBase.NAME.text = Localization.Get("Select_item");
				BaseLogic.INVENTORY_COMBINED = BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
			}
			else
			{
				BaseLogic.INVENTORY_COMBINED = -1;
				BaseLogic.PLAYER_ACTIVE_SLOT = -1;
				inventoryBase.NAME.text = string.Empty;
			}
		}
	}

	private void UseItem()
	{
		if (BaseLogic.PLAYER_ACTIVE_SLOT == -1 || (BaseLogic.PLAYER_ACTIVE_SLOT >= 0 && BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] <= 0))
		{
			return;
		}
		int num = BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
		if ((num >= 12 && num <= 15) || num == 21 || num == 40 || num == 41 || num == 66 || num == 68 || num == 69 || num == 70 || num == 71 || num == 72 || num == 74 || num == 82 || num == 83 || num == 85 || num == 86 || num == 92 || num == 94 || num == 95 || num == 96 || num == 108 || num == 109 || num == 111 || num == 112 || num == 113)
		{
			if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands && BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().magazine_id == num)
			{
				if (BaseLogic._player.GetComponent<Ironsights>().reloading)
				{
					return;
				}
				BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetAmmo(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
				ActivationItem();
				if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal > 0)
				{
					BaseLogic.AddItem(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().magazine_id, BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal);
					BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetBulletsLeft(0);
				}
				else if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MAGAZINE && BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MAGAZINE.enabled)
				{
					BaseLogic.AddItem(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().magazine_id, 0);
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 102)
		{
			if ((bool)Network.PLAYER && BaseLogic.GAME_MODE != 4 && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>() && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>()._control && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>().ammo < 15)
			{
				AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 1f);
				Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<MortarSystem>().ammo++;
				Network.PLAYER.GetComponent<HeroBase>().VEHICLE.SendMessage("UpdateAmmo");
				BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 105)
		{
			if (BaseLogic._glasses == 0)
			{
				BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
				Camera.main.GetComponent<NightVisionEffect>().enabled = true;
				GameObject gameObject = GameObject.Find("Sky Dome");
				gameObject.GetComponent<TOD_Sky>().Night.ShadowStrength = 0.4f;
				gameObject.GetComponent<TOD_Sky>().Night.LightIntensity = 1f;
				BaseLogic._glasses = 1;
				AudioSource.PlayClipAtPoint(inventoryBase.PNV_ON, Camera.main.transform.position, 1f);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 106)
		{
			if (BaseLogic._glasses == 0)
			{
				BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
				BaseLogic.FLASH_LIGHT.GetComponent<Light>().enabled = true;
				BaseLogic._glasses = 2;
				AudioSource.PlayClipAtPoint(inventoryBase.FLASH_LIGHT, Camera.main.transform.position, 1f);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 76)
		{
			if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE)
			{
				if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>())
				{
					if (Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>()._control && !Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().detail_accuma)
					{
						AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 1f);
						Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().detail_accuma = true;
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
						Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetAccuma", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name);
					}
				}
				else if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>() && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>()._control && !Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().detail_accuma)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 1f);
					Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().detail_accuma = true;
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetAccuma", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name);
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 77)
		{
			if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE)
			{
				if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>())
				{
					if (Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>()._control && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().FUEL < 100)
					{
						AudioSource.PlayClipAtPoint(inventoryBase.BENZIN, Camera.main.transform.position, 1f);
						Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().FUEL += BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
						BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
						Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetVehicleFuel", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().FUEL);
					}
				}
				else if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>() && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>()._control && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().FUEL < 100)
				{
					AudioSource.PlayClipAtPoint(inventoryBase.BENZIN, Camera.main.transform.position, 1f);
					Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().FUEL += BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT];
					BaseLogic.RemoveItem(BaseLogic.PLAYER_ACTIVE_SLOT, 100);
					Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetVehicleFuel", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().FUEL);
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 78 || BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 27)
		{
			if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE)
			{
				if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>())
				{
					if (Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>()._control && !Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().engine_start && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().detail_accuma)
					{
						if (Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().FUEL > 0)
						{
							AudioSource.PlayClipAtPoint(inventoryBase.ENGINE_START, Camera.main.transform.position, 0.6f);
							Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().engine_start = true;
							Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().GetComponent<AudioSource>().Play();
							Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetVehicleEngine", PhotonTargets.Others);
							Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetVehicleFuel", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name + "\n" + Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>().FUEL);
							Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetAccuma", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name);
						}
						else
						{
							AudioSource.PlayClipAtPoint(inventoryBase.ENGINE_START_NoFuel, Camera.main.transform.position, 0.6f);
						}
					}
				}
				else if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>() && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>()._control && !Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().engine_start && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().detail_accuma)
				{
					if (Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().FUEL > 0)
					{
						AudioSource.PlayClipAtPoint(inventoryBase.ENGINE_START, Camera.main.transform.position, 0.6f);
						Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().thrustInput = 0.3f;
						Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().POWER.localEulerAngles = new Vector3(0f, 90f, 310f);
						Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().engine_start = true;
						Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetVehicleFuel", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name + "\n" + Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>().FUEL);
						Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetAccuma", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name);
					}
					else
					{
						AudioSource.PlayClipAtPoint(inventoryBase.ENGINE_START_NoFuel, Camera.main.transform.position, 0.6f);
					}
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 16)
		{
			if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands)
			{
				if (BaseLogic._player.GetComponent<Ironsights>().reloading)
				{
					return;
				}
				if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.name == "R_SHOTGUN")
				{
					if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal < 5)
					{
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetAmmo(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().ammoReal + 1);
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().StartCoroutine("Reload");
						RemoveActivationItem(1);
					}
				}
				else if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.name == "R_DOUBLESHOTGUN")
				{
					if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal == 0 && BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] >= 2)
					{
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetAmmo(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().ammoReal + 2);
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().StartCoroutine("Reload");
						RemoveActivationItem(2);
					}
					else
					{
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetAmmo(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().ammoReal + 1);
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().StartCoroutine("Reload");
						RemoveActivationItem(1);
					}
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 37)
		{
			if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands)
			{
				if (BaseLogic._player.GetComponent<Ironsights>().reloading)
				{
					return;
				}
				if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.name == "R_MOSINA" && BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal < 5)
				{
					BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetAmmo(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().ammoReal + 1);
					BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().StartCoroutine("Reload");
					RemoveActivationItem(1);
				}
			}
		}
		else if (num == 22)
		{
			if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands)
			{
				if (BaseLogic._player.GetComponent<Ironsights>().reloading)
				{
					return;
				}
				if (BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().magazine_id == num && BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsLeftReal == 0)
				{
					if (BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] >= BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsPerClipReal)
					{
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetAmmo(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsPerClipReal);
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().StartCoroutine("Reload");
						RemoveActivationItem(BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().bulletsPerClipReal);
					}
					else
					{
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().SetAmmo(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
						BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().StartCoroutine("Reload");
						RemoveActivationItem(BaseLogic.PLAYER_SLOT_COUNT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT]);
					}
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().mod_id)
		{
			if ((bool)BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands && !BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MOD_ACTIVE)
			{
				BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MOD_ACTIVE = true;
				BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().weaponInHands.GetComponent<WeaponBehavior>().MOD.SetActive(true);
				BaseLogic._weaponMod1 = 1;
				ActivationItem();
				AudioSource.PlayClipAtPoint(inventoryBase.RELOAD_MAG, Camera.main.transform.position, 0.75f);
				if ((bool)Network.PLAYER)
				{
					Network.PLAYER.GetComponent<HeroBase>().photonView.RPC("SetWeapons", PhotonTargets.Others, new Quaternion(BaseLogic._weaponSlot1_Real, BaseLogic._weaponSlot2_Real, BaseLogic._weaponMod1, BaseLogic._weaponMod2));
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 17)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(25);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 15);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 5);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 18)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(25);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 15);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 2);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 19)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(20);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 20);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 2);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 20)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(30);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 10);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 5);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 56)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(35);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 10);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 57)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(35);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 15);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 51)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(100);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 10);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 52)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(100);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 20);
			}
			if (FPSPlayer.thirstPoints < 99)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints + 1);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 4)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(35);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 10);
			}
			if (FPSPlayer.thirstPoints < 99)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints + 1);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 55)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(35);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 25);
			}
			if (FPSPlayer.thirstPoints < 99)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints + 1);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 8)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(20);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 2);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 10);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 9)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(10);
			if (FPSPlayer.temperaturePoints > 0)
			{
				FPSPlayer.SetTemperaturePoints(FPSPlayer.temperaturePoints - 5);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 10)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(30);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 2);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 15);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 28)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(40);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 2);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 15);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 29)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(40);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 2);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 15);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 59)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(30);
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 25);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 60)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(30);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 2);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 20);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] >= 87 && BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] <= 91)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(35);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 5);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 61)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(50);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 15);
			}
			if (FPSPlayer.thirstPoints > 0)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints - 2);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 62)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(50);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 25);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 11)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(15);
			if (FPSPlayer.hungerPoints > 0)
			{
				FPSPlayer.SetHungerPoints(FPSPlayer.hungerPoints - 10);
			}
			if (FPSPlayer.thirstPoints < 99)
			{
				FPSPlayer.SetThirstPoints(FPSPlayer.thirstPoints + 2);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 38)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(100);
			BaseLogic._player.SendMessage("HealPlayer", 20);
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 44)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.BINT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(50);
			BaseLogic._player.SendMessage("HealPlayer", 20);
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 49)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(40);
			BaseLogic._player.SendMessage("HealPlayer", 10);
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 30)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(100);
			BaseLogic._player.SendMessage("HealPlayer", 60);
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 50)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.DRINK, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(100);
			BaseLogic._player.SendMessage("HealPlayer", 10);
			BaseLogic._player.GetComponent<FPSRigidBodyWalker>().staminaForSprint = 80f;
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 31)
		{
			AudioSource.PlayClipAtPoint(inventoryBase.EAT, Camera.main.transform.position, 0.75f);
			RemoveActivationItem(100);
			BaseLogic._player.SendMessage("HealPlayer", 20);
			BaseLogic._player.GetComponent<FPSRigidBodyWalker>().staminaForSprint = 120f;
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 79)
		{
			RemoveActivationItem(100);
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, 5002);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 80)
		{
			RemoveActivationItem(100);
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, Random.Range(5004, 5100));
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 81)
		{
			RemoveActivationItem(100);
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, Random.Range(5101, 5199));
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 98)
		{
			if (BaseLogic.GAME_MODE != 4)
			{
				RemoveActivationItem(100);
				if ((bool)Network.PLAYER)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, 5201);
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 99)
		{
			if (BaseLogic.GAME_MODE != 4)
			{
				RemoveActivationItem(100);
				if ((bool)Network.PLAYER)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, 5202);
				}
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 107)
		{
			RemoveActivationItem(100);
			if ((bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("CreateWeapon", PhotonTargets.All, 5203);
			}
		}
		else if (BaseLogic.PLAYER_SLOT_REAL[BaseLogic.PLAYER_ACTIVE_SLOT] == 65)
		{
			if (BaseLogic.RADIO == 0)
			{
				BaseLogic.RADIO = 1;
				inventoryBase.NAME.text = string.Format(Localization.Get("Portable_radio_set_101"));
			}
			else
			{
				BaseLogic.RADIO = 0;
				inventoryBase.NAME.text = string.Format(Localization.Get("Portable_radio_set_100"));
			}
		}
		inventoryBase.SendMessage("CheckInventory");
	}
}
