using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
	public int ID_BUTTON;

	public GameObject TARGET;

	public GameObject TARGET2;

	public string roomName = string.Empty;

	public GameObject fpsGUI;

	private float _updateTimer;

	private void Start()
	{
		fpsGUI = GameObject.Find("!!!FPS MainMenu GUI");
	}

	private void Update()
	{
	}

	public void ClickButton()
	{
		if (string.IsNullOrEmpty(BaseLogic.viewer_id))
		{
			Debug.Log("No viewer ID!");
		}
		else if (BaseLogic.BAN2 != 0)
		{
			Debug.Log("Banned! " + BaseLogic.BAN2);
		}
		else if (ID_BUTTON == 1)
		{
			if (BaseLogic.GAME_MODE == 2) {
				TARGET.SendMessage ("LoadLevelFunc", 2);
				return;
			} else {
				BaseLogic.GAME_MODE = 0;
				TARGET.SendMessage ("LoadLevelFunc", 1);
			}
		}
		else if (ID_BUTTON == 100)
		{
			BaseLogic.GAME_MODE = 2;
			TARGET.SendMessage("LoadLevelFunc", 2);
		}
		else if (ID_BUTTON == 101)
		{
			BaseLogic.GAME_MODE = 6;
			TARGET.SendMessage("LoadLevelFunc", 4);
		}
		else if (ID_BUTTON == 2)
		{
			fpsGUI.SendMessage("ShowServerMenu");
			TARGET.SendMessage("UpdateServerList");
		}
		else if (ID_BUTTON == 3)
		{
			fpsGUI.SendMessage("ShowShopMenu");
			if (TARGET2.GetComponent<MainMenuButton>().roomName == "order2")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(1);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order3")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(2);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order4")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(3);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order5")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(4);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order6")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(5);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order7")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(6);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order8")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(7);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order9")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(8);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order10")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(9);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order11")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(12);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order12")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(13);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order13")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(14);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order14")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(15);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order15")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(16);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order16")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(17);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order17")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(18);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order18")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(19);
				BaseLogic.setVipReal = false;
			}
			else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order19")
			{
				BaseLogic.setVipReal = true;
				BaseLogic.SetVIP_prot(20);
				BaseLogic.setVipReal = false;
			}
		}
		else if (ID_BUTTON == 4)
		{
			if (BaseLogic.BT2)
			{
				Application.ExternalCall("OpenVKURL", "http://vk.com/battalion2");
			}
			else if (BaseLogic.game_mode == "vk.com")
			{
				Application.ExternalCall("OpenVKURL", "http://vk.com/survival3d");
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				Application.ExternalCall("OpenVKURL", "http://ok.ru/survival3d");
			}
			else
			{
				Application.ExternalCall("OpenVKURL", "https://www.facebook.com/Survival3DdCommunity");
			}
		}
		else if (ID_BUTTON == 5)
		{
			if (BaseLogic.game_mode == "vk.com")
			{
				Application.ExternalCall("OpenVKURL", "http://vk.com/topic-67389128_30008579");
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				Application.ExternalCall("OpenVKURL", "http://ok.ru/survival3d/topic/62538814991144");
			}
			else
			{
				Application.ExternalCall("OpenVKURL", "https://www.facebook.com/notes/survival/help/1650337378524906");
			}
		}
		else if (ID_BUTTON == 6)
		{
			fpsGUI.SendMessage("ShowClanMenu");
		}
		else if (ID_BUTTON == 501)
		{
			TARGET.SendMessage("ClearServerList");
			BaseLogic.ROOM_NAME = "Server_" + Random.Range(0, 9999);
			fpsGUI.SendMessage("LoadLevelFunc", 1);
		}
		else if (ID_BUTTON == 502)
		{
			if (Time.time > _updateTimer + 6f)
			{
				TARGET.SendMessage("UpdateServerList");
				_updateTimer = Time.time;
			}
		}
		else if (ID_BUTTON == 505)
		{
			if (Time.time > _updateTimer + 0.2f)
			{
				BaseLogic.GAME_MODE--;
				SelectGameMode(true);
				TARGET.SendMessage("UpdateServerList");
				_updateTimer = Time.time;
			}
		}
		else if (ID_BUTTON == 504)
		{
			if (Time.time > _updateTimer + 0.2f)
			{
				BaseLogic.GAME_MODE++;
				SelectGameMode(false);
				TARGET.SendMessage("UpdateServerList");
				_updateTimer = Time.time;
			}
		}
		else if (ID_BUTTON == 503)
		{
			TARGET.SendMessage("ClearServerList");
			fpsGUI.SendMessage("ShowMainMenu");
			BaseLogic.SetWeaponSlot1(BaseLogic._weaponSlot1DB);
			BaseLogic.SetWeaponSlot2(BaseLogic._weaponSlot2DB);
			BaseLogic._weaponMod1 = BaseLogic._weaponMod1DB;
			BaseLogic._weaponMod2 = BaseLogic._weaponMod2DB;
			BaseLogic._cloth = BaseLogic._clothDB;
			BaseLogic._helmet = BaseLogic._helmetDB;
			BaseLogic._backpack = BaseLogic._backpackDB;
			BaseLogic.SetBodyArmor(BaseLogic._bodyArmorDB);
			BaseLogic._glasses = BaseLogic._glassesDB;
		}
		else if (ID_BUTTON == 508)
		{
			TARGET.SendMessage("ClearServerList");
			fpsGUI.SendMessage("ShowShopMenu");
			BaseLogic.SetWeaponSlot1(BaseLogic._weaponSlot1DB);
			BaseLogic.SetWeaponSlot2(BaseLogic._weaponSlot2DB);
			BaseLogic._weaponMod1 = BaseLogic._weaponMod1DB;
			BaseLogic._weaponMod2 = BaseLogic._weaponMod2DB;
			BaseLogic._cloth = BaseLogic._clothDB;
			BaseLogic._helmet = BaseLogic._helmetDB;
			BaseLogic._backpack = BaseLogic._backpackDB;
			BaseLogic.SetBodyArmor(BaseLogic._bodyArmorDB);
			BaseLogic._glasses = BaseLogic._glassesDB;
		}
		else if (ID_BUTTON == 500)
		{
			GameObject gameObject = GameObject.Find("!!!Network");
			gameObject.SendMessage("ClearServerList");
			BaseLogic.ROOM_NAME = roomName;
			fpsGUI.SendMessage("LoadLevelFunc", 1);
		}
		else if (ID_BUTTON == 600)
		{
			TARGET.SendMessage("SetShadows");
		}
		else if (ID_BUTTON == 601)
		{
			TARGET.SendMessage("SetMusic");
		}
		else if (ID_BUTTON == 1000)
		{
			Screen.SetResolution(760, 665, false);
			TARGET.SendMessage("ShowDonateBackMenu");
			Application.ExternalCall("order1");
		}
		else if (ID_BUTTON == 1001)
		{
			if (BaseLogic.ADMIN == 89)
			{
				BaseLogic.SetPlayerVip(BaseLogic.tempPlayerVip);
				BaseLogic.AddVipLoot();
				fpsGUI.SendMessage("ShowMainMenu");
			}
			else
			{
				Screen.SetResolution(760, 665, false);
				TARGET.SendMessage("ShowDonateBackMenu");
				Application.ExternalCall(roomName);
			}
		}
		else if (ID_BUTTON == 8888)
		{
			Screen.SetResolution(760, 665, false);
			TARGET.SendMessage("ShowDonateBackMenu");
			Application.ExternalCall("order100");
		}
		else if (ID_BUTTON == 8889)
		{
			Screen.SetResolution(760, 665, false);
			TARGET.SendMessage("ShowDonateBackMenu");
			Application.ExternalCall("order101");
		}
		else if (ID_BUTTON == 8890)
		{
			Screen.SetResolution(760, 665, false);
			TARGET.SendMessage("ShowDonateBackMenu");
			Application.ExternalCall("order102");
		}
		else if (ID_BUTTON == 8891)
		{
			Screen.SetResolution(760, 665, false);
			TARGET.SendMessage("ShowDonateBackMenu");
			Application.ExternalCall("order103");
		}
		else if (ID_BUTTON == 1600)
		{
			Screen.SetResolution(760, 665, false);
			TARGET.SendMessage("ShowDonateBackMenu");
			Application.ExternalCall("orderCamp");
		}
		else if (ID_BUTTON == 1601)
		{
			Screen.SetResolution(760, 665, false);
			TARGET.SendMessage("ShowDonateBackMenu");
			Application.ExternalCall("orderArena");
		}
		else if (ID_BUTTON == 1602)
		{
			Screen.SetResolution(760, 665, false);
			TARGET.SendMessage("ShowDonateBackMenu");
			Application.ExternalCall("orderSummer");
		}
		else if (ID_BUTTON == 1111)
		{
			TARGET.SendMessage("ShowMarketMenu");
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(9);
			BaseLogic.setVipReal = false;
		}
		else if (ID_BUTTON == 1112)
		{
			TARGET.SendMessage("ShowMarketMenu2");
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(99);
			BaseLogic.setVipReal = false;
		}
		else if (ID_BUTTON == 1113)
		{
			TARGET.SendMessage("ShowMarketMenu3");
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(100);
			BaseLogic.setVipReal = false;
		}
		else if (ID_BUTTON == 1114)
		{
			TARGET.SendMessage("ShowMarketMenu4");
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(101);
			BaseLogic.setVipReal = false;
		}
		else if (ID_BUTTON == 1118)
		{
			TARGET.SendMessage("ShowMarketMenu5");
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(102);
			BaseLogic.setVipReal = false;
		}
		else if (ID_BUTTON == 1115)
		{
			TARGET.SendMessage("ShowGameMode1Menu");
		}
		else if (ID_BUTTON == 1116)
		{
			TARGET.SendMessage("ShowGameMode2Menu");
		}
		else if (ID_BUTTON == 1117)
		{
			TARGET.SendMessage("ShowGameMode3Menu");
		}
		else if (ID_BUTTON == 1002)
		{
			DonateNext();
		}
		else if (ID_BUTTON == 1003)
		{
			DonateBack();
		}
		else if (ID_BUTTON == 5000)
		{
			fpsGUI.SendMessage("HideAllMenus");
			TARGET.SendMessage("LoadDataBaseAllways");
		}
	}

	private void DonateBack()
	{
		if (TARGET2.GetComponent<MainMenuButton>().roomName == "order3")
		{
			SetPrice(1);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order2";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(1);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order4")
		{
			SetPrice(1);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order3";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(2);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order5")
		{
			SetPrice(1);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order4";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(3);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order18")
		{
			SetPrice(2);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order5";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(4);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order6")
		{
			SetPrice(2);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order18";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(5);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order7")
		{
			SetPrice(2);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order6";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(19);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order8")
		{
			SetPrice(3);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order7";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(6);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order14")
		{
			SetPrice(3);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order8";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(7);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order9")
		{
			SetPrice(3);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order14";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(15);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order15")
		{
			SetPrice(4);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order9";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(8);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order16")
		{
			SetPrice(4);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order15";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(16);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order10")
		{
			SetPrice(5);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order16";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(17);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order11")
		{
			SetPrice(5);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order10";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(9);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order12")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order11";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(12);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order13")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order12";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(13);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order17")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order13";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(14);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order19")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order17";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(18);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order2")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order19";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(20);
			BaseLogic.setVipReal = false;
		}
	}

	private void DonateNext()
	{
		if (TARGET2.GetComponent<MainMenuButton>().roomName == "order2")
		{
			SetPrice(1);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order3";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(2);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order3")
		{
			SetPrice(1);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order4";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(3);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order4")
		{
			SetPrice(2);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order5";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(4);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order5")
		{
			SetPrice(2);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order18";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(5);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order18")
		{
			SetPrice(2);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order6";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(19);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order6")
		{
			SetPrice(3);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order7";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(6);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order7")
		{
			SetPrice(3);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order8";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(7);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order8")
		{
			SetPrice(3);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order14";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(15);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order14")
		{
			SetPrice(4);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order9";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(8);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order9")
		{
			SetPrice(4);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order15";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(16);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order15")
		{
			SetPrice(5);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order16";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(17);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order16")
		{
			SetPrice(5);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order10";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(9);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order10")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order11";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(12);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order11")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order12";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(13);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order12")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order13";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(14);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order13")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order17";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(18);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order17")
		{
			SetPrice(10);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order19";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(20);
			BaseLogic.setVipReal = false;
		}
		else if (TARGET2.GetComponent<MainMenuButton>().roomName == "order19")
		{
			SetPrice(1);
			TARGET2.GetComponent<MainMenuButton>().roomName = "order2";
			BaseLogic.setVipReal = true;
			BaseLogic.SetVIP_prot(1);
			BaseLogic.setVipReal = false;
		}
	}

	private void SetPrice(int price)
	{
		switch (price)
		{
		case 1:
			if (BaseLogic.game_mode == "vk.com")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "1", "Голос");
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "7", "ОК");
			}
			else
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "0.2", "USD");
			}
			break;
		case 2:
			if (BaseLogic.game_mode == "vk.com")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "2", "Голоса");
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "14", "ОК");
			}
			else
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "0.4", "USD");
			}
			break;
		case 3:
			if (BaseLogic.game_mode == "vk.com")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "3", "Голоса");
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "21", "ОК");
			}
			else
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "0.6", "USD");
			}
			break;
		case 4:
			if (BaseLogic.game_mode == "vk.com")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "4", "Голоса");
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "28", "ОК");
			}
			else
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "0.8", "USD");
			}
			break;
		case 5:
			if (BaseLogic.game_mode == "vk.com")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "5", "Голосов");
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "35", "ОК");
			}
			else
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "1", "USD");
			}
			break;
		case 10:
			if (BaseLogic.game_mode == "vk.com")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "10", "Голосов");
			}
			else if (BaseLogic.game_mode == "ok.ru")
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "70", "ОК");
			}
			else
			{
				TARGET.GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "2", "USD");
			}
			break;
		case 6:
		case 7:
		case 8:
		case 9:
			break;
		}
	}

	private void SelectGameMode(bool minus)
	{
		if (BaseLogic.GAME_MODE == 0)
		{
			string[] array = TARGET2.GetComponent<UILabel>().text.Split(':');
			TARGET2.GetComponent<UILabel>().text = array[0] + ": DM Classic";
		}
		else if (BaseLogic.GAME_MODE == 1)
		{
			string[] array2 = TARGET2.GetComponent<UILabel>().text.Split(':');
			TARGET2.GetComponent<UILabel>().text = array2[0] + ": TDM Classic";
		}
		else if (BaseLogic.GAME_MODE == 3)
		{
			string[] array3 = TARGET2.GetComponent<UILabel>().text.Split(':');
			TARGET2.GetComponent<UILabel>().text = array3[0] + ": DM Classic (NO VIP)";
		}
		else if (BaseLogic.GAME_MODE == 4 && BaseLogic.GM_CAMP == 1)
		{
			string[] array4 = TARGET2.GetComponent<UILabel>().text.Split(':');
			TARGET2.GetComponent<UILabel>().text = array4[0] + ": Camp";
		}
		else if (BaseLogic.GAME_MODE == 5 && BaseLogic.GM_SUMMER == 1)
		{
			string[] array5 = TARGET2.GetComponent<UILabel>().text.Split(':');
			TARGET2.GetComponent<UILabel>().text = array5[0] + ": Summer Island";
		}
		else if (BaseLogic.GAME_MODE == 6 && BaseLogic.GM_ARENA == 1)
		{
			string[] array6 = TARGET2.GetComponent<UILabel>().text.Split(':');
			TARGET2.GetComponent<UILabel>().text = array6[0] + ": Arena";
		}
		else if (minus)
		{
			BaseLogic.GAME_MODE--;
			if (BaseLogic.GAME_MODE < 0)
			{
				BaseLogic.GAME_MODE = 6;
			}
			SelectGameMode(true);
		}
		else
		{
			BaseLogic.GAME_MODE++;
			if (BaseLogic.GAME_MODE > 6)
			{
				BaseLogic.GAME_MODE = 0;
			}
			SelectGameMode(false);
		}
	}
}
