using System.Collections;
using UnityEngine;

public class LoadLevelBundle : MonoBehaviour
{
	public GameObject[] _Elements;

	public GameObject[] _ClanMenuElements;

	public GameObject[] _BaseMenuElements;

	public GameObject[] _ServerMenuElements;

	public GameObject[] _BlackGroupElements;

	public GameObject[] _SettingsMenuElements;

	public GameObject[] _MarketMenuElements;

	public GameObject[] _MarketMenuElements2;

	public GameObject[] _MarketMenuElements3;

	public GameObject[] _MarketMenuElements4;

	public GameObject[] _MarketMenuElements5;

	public GameObject[] _DonateBackMenuElements;

	public GameObject[] _ShopMenuElements;

	public GameObject[] _GameMode1Elements;

	public GameObject[] _GameMode2Elements;

	public GameObject[] _GameMode3Elements;

	public WWW download;

	public bool _startGUI;

	public AudioClip LoadingMusic;

	private bool firstMusic;

	private void Awake()
	{
		HideAllMenus();
	}

	private void SetShadows()
	{
		if (BaseLogic.SHADOWS)
		{
			BaseLogic.SHADOWS = false;
			GameObject gameObject = GameObject.Find("Directional light");
			gameObject.GetComponent<Light>().shadows = LightShadows.None;
		}
		else
		{
			BaseLogic.SHADOWS = true;
			GameObject gameObject2 = GameObject.Find("Directional light");
			gameObject2.GetComponent<Light>().shadows = LightShadows.Soft;
		}
	}

	private void SetMusic()
	{
		if (!firstMusic)
		{
			firstMusic = true;
		}
		else if (Camera.main.GetComponent<AudioSource>().isPlaying)
		{
			Camera.main.GetComponent<AudioSource>().Stop();
		}
		else
		{
			Camera.main.GetComponent<AudioSource>().Play();
		}
	}

	private void ShowGameMode1Menu()
	{
		HideAllMenus();
		for (int i = 0; i < _GameMode1Elements.Length; i++)
		{
			_GameMode1Elements[i].SetActive(true);
		}
	}

	private void ShowGameMode2Menu()
	{
		HideAllMenus();
		for (int i = 0; i < _GameMode2Elements.Length; i++)
		{
			_GameMode2Elements[i].SetActive(true);
		}
	}

	private void ShowGameMode3Menu()
	{
		HideAllMenus();
		for (int i = 0; i < _GameMode3Elements.Length; i++)
		{
			_GameMode3Elements[i].SetActive(true);
		}
	}

	private void ShowDonateBackMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _DonateBackMenuElements.Length; i++)
		{
			_DonateBackMenuElements[i].SetActive(true);
		}
	}

	private void ShowMainMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _BaseMenuElements.Length; i++)
		{
			_BaseMenuElements[i].SetActive(true);
		}
	}

	private void ShowBlackGroupMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _BlackGroupElements.Length; i++)
		{
			_BlackGroupElements[i].SetActive(true);
		}
		string[] array = new string[BaseLogic.BLACK_GROUP.Length];
		for (int j = 0; j < BaseLogic.BLACK_GROUP.Length; j++)
		{
			if (BaseLogic.BLACK_GROUP[j] == 0)
			{
				array[j] = string.Empty;
				continue;
			}
			switch (j)
			{
			case 0:
				array[j] = "\nhttp://vk.com/cffteam_ru";
				break;
			case 1:
				array[j] = "\nhttp://vk.com/survival3";
				break;
			case 2:
				array[j] = "\nhttp://vk.com/xakpro4634";
				break;
			case 3:
				array[j] = "\nhttp://vk.com/cheats_games_vk39";
				break;
			case 4:
				array[j] = "\nhttp://vk.com/cheat_by_survival";
				break;
			case 5:
				array[j] = "\nhttp://vk.com/clubsurvival_cheats";
				break;
			}
		}
		_BlackGroupElements[0].GetComponent<UILabel>().text = "Внимание!\nЧтобы продолжить игру, Вам необходимо отписаться от следующих сообществ:\n" + array[0] + array[1] + array[2] + array[3] + array[4] + array[5] + array[6] + array[7] + array[8] + array[9];
	}

	private void ShowServerMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _ServerMenuElements.Length; i++)
		{
			_ServerMenuElements[i].SetActive(true);
		}
		BaseLogic.GAME_MODE = 0;
	}

	private void ShowSettingsMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _SettingsMenuElements.Length; i++)
		{
			_SettingsMenuElements[i].SetActive(true);
		}
	}

	private void ShowMarketMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _MarketMenuElements.Length; i++)
		{
			_MarketMenuElements[i].SetActive(true);
		}
	}

	private void ShowMarketMenu2()
	{
		HideAllMenus();
		for (int i = 0; i < _MarketMenuElements2.Length; i++)
		{
			_MarketMenuElements2[i].SetActive(true);
		}
	}

	private void ShowMarketMenu3()
	{
		HideAllMenus();
		for (int i = 0; i < _MarketMenuElements3.Length; i++)
		{
			_MarketMenuElements3[i].SetActive(true);
		}
	}

	private void ShowMarketMenu4()
	{
		HideAllMenus();
		for (int i = 0; i < _MarketMenuElements4.Length; i++)
		{
			_MarketMenuElements4[i].SetActive(true);
		}
	}

	private void ShowMarketMenu5()
	{
		HideAllMenus();
		for (int i = 0; i < _MarketMenuElements5.Length; i++)
		{
			_MarketMenuElements5[i].SetActive(true);
		}
	}

	private void ShowClanMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _ClanMenuElements.Length; i++)
		{
			_ClanMenuElements[i].SetActive(true);
		}
	}

	private void CheckGameModes()
	{
		if (BaseLogic.GM_CAMP > 0)
		{
			_ShopMenuElements[15].SetActive(false);
		}
		else
		{
			_ShopMenuElements[15].SetActive(true);
		}
		if (BaseLogic.GM_ARENA > 0)
		{
			_ShopMenuElements[14].SetActive(false);
		}
		else
		{
			_ShopMenuElements[14].SetActive(true);
		}
		if (BaseLogic.GM_SUMMER > 0)
		{
			_ShopMenuElements[16].SetActive(false);
		}
		else
		{
			_ShopMenuElements[16].SetActive(true);
		}
	}

	private void ShowShopMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _ShopMenuElements.Length; i++)
		{
			_ShopMenuElements[i].SetActive(true);
		}
		if (BaseLogic.LICENSE == 1)
		{
			_ShopMenuElements[6].SetActive(false);
			_ShopMenuElements[7].SetActive(true);
			_ShopMenuElements[8].SetActive(true);
			_ShopMenuElements[9].SetActive(true);
			_ShopMenuElements[17].SetActive(true);
			_ShopMenuElements[12].SetActive(true);
			_ShopMenuElements[13].SetActive(false);
			CheckGameModes();
		}
		else if (BaseLogic.LICENSE == 2)
		{
			_ShopMenuElements[6].SetActive(false);
			_ShopMenuElements[7].SetActive(false);
			_ShopMenuElements[8].SetActive(true);
			_ShopMenuElements[9].SetActive(true);
			_ShopMenuElements[17].SetActive(true);
			_ShopMenuElements[12].SetActive(true);
			_ShopMenuElements[13].SetActive(false);
			CheckGameModes();
		}
		else if (BaseLogic.LICENSE == 3)
		{
			_ShopMenuElements[6].SetActive(false);
			_ShopMenuElements[7].SetActive(false);
			_ShopMenuElements[8].SetActive(false);
			_ShopMenuElements[9].SetActive(true);
			_ShopMenuElements[17].SetActive(true);
			_ShopMenuElements[12].SetActive(true);
			_ShopMenuElements[13].SetActive(false);
			CheckGameModes();
		}
		else if (BaseLogic.LICENSE == 4)
		{
			_ShopMenuElements[6].SetActive(false);
			_ShopMenuElements[7].SetActive(false);
			_ShopMenuElements[8].SetActive(false);
			_ShopMenuElements[9].SetActive(false);
			_ShopMenuElements[17].SetActive(true);
			_ShopMenuElements[12].SetActive(true);
			_ShopMenuElements[13].SetActive(false);
			CheckGameModes();
		}
		else if (BaseLogic.LICENSE == 0)
		{
			_ShopMenuElements[6].SetActive(true);
			_ShopMenuElements[7].SetActive(false);
			_ShopMenuElements[8].SetActive(false);
			_ShopMenuElements[9].SetActive(false);
			_ShopMenuElements[17].SetActive(false);
			_ShopMenuElements[12].SetActive(false);
			_ShopMenuElements[13].SetActive(true);
			_ShopMenuElements[14].SetActive(false);
			_ShopMenuElements[15].SetActive(false);
			_ShopMenuElements[16].SetActive(false);
		}
		else
		{
			_ShopMenuElements[6].SetActive(false);
			_ShopMenuElements[7].SetActive(false);
			_ShopMenuElements[8].SetActive(false);
			_ShopMenuElements[9].SetActive(false);
			_ShopMenuElements[17].SetActive(false);
			_ShopMenuElements[12].SetActive(true);
			_ShopMenuElements[13].SetActive(false);
			CheckGameModes();
		}
		if (BaseLogic.game_mode == "vk.com")
		{
			_ShopMenuElements[2].GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "1", "Голос");
		}
		else if (BaseLogic.game_mode == "ok.ru")
		{
			_ShopMenuElements[2].GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "1", "OK");
		}
		else
		{
			_ShopMenuElements[2].GetComponent<UILabel>().text = string.Format(Localization.Get("Special_offer"), "0.2", "USD");
		}
	}

	private void HideAllMenus()
	{
		for (int i = 0; i < _BaseMenuElements.Length; i++)
		{
			_BaseMenuElements[i].SetActive(false);
		}
		for (int j = 0; j < _BaseMenuElements.Length; j++)
		{
			_BaseMenuElements[j].SetActive(false);
		}
		for (int k = 0; k < _SettingsMenuElements.Length; k++)
		{
			_SettingsMenuElements[k].SetActive(false);
		}
		for (int l = 0; l < _ServerMenuElements.Length; l++)
		{
			_ServerMenuElements[l].SetActive(false);
		}
		for (int m = 0; m < _MarketMenuElements.Length; m++)
		{
			_MarketMenuElements[m].SetActive(false);
		}
		for (int n = 0; n < _MarketMenuElements2.Length; n++)
		{
			_MarketMenuElements2[n].SetActive(false);
		}
		for (int num = 0; num < _MarketMenuElements3.Length; num++)
		{
			_MarketMenuElements3[num].SetActive(false);
		}
		for (int num2 = 0; num2 < _MarketMenuElements4.Length; num2++)
		{
			_MarketMenuElements4[num2].SetActive(false);
		}
		for (int num3 = 0; num3 < _MarketMenuElements5.Length; num3++)
		{
			_MarketMenuElements5[num3].SetActive(false);
		}
		for (int num4 = 0; num4 < _DonateBackMenuElements.Length; num4++)
		{
			_DonateBackMenuElements[num4].SetActive(false);
		}
		for (int num5 = 0; num5 < _ShopMenuElements.Length; num5++)
		{
			_ShopMenuElements[num5].SetActive(false);
		}
		for (int num6 = 0; num6 < _ClanMenuElements.Length; num6++)
		{
			_ClanMenuElements[num6].SetActive(false);
		}
		for (int num7 = 0; num7 < _BlackGroupElements.Length; num7++)
		{
			_BlackGroupElements[num7].SetActive(false);
		}
		for (int num8 = 0; num8 < _GameMode1Elements.Length; num8++)
		{
			_GameMode1Elements[num8].SetActive(false);
		}
		for (int num9 = 0; num9 < _GameMode2Elements.Length; num9++)
		{
			_GameMode2Elements[num9].SetActive(false);
		}
		for (int num10 = 0; num10 < _GameMode3Elements.Length; num10++)
		{
			_GameMode3Elements[num10].SetActive(false);
		}
	}

	private void ShowMenu()
	{
		HideAllMenus();
		for (int i = 0; i < _Elements.Length; i++)
		{
			_Elements[i].SetActive(true);
		}
	}

	public void LoadLevelFunc(int level)
	{
		if (BaseLogic.GAME_MODE != 2)
		{
			level = ((BaseLogic.GAME_MODE == 4) ? 3 : ((BaseLogic.GAME_MODE == 6) ? 4 : ((BaseLogic.GAME_MODE != 5) ? 1 : 5)));
		}
		else
		{
			level = 2;
			BaseLogic.ClearLoot();
		}
		ShowMenu();
		_Elements[2].GetComponent<UILabel>().text = string.Empty;
		int num = Random.Range(0, 4);
		_Elements[2].GetComponent<UILabel>().text = Localization.Get("Advice_" + (num + 1));
		StartCoroutine(Loadlevel(level));
	}

	private IEnumerator Loadlevel(int _level)
	{
		_startGUI = true;
		for (int i = 0; i < _Elements.Length; i++)
		{
			_Elements[i].SetActive(true);
		}

		switch (_level)
		{
		case 1:
			Application.LoadLevel("buildplayer-!!fps base");
			//Application.LoadLevel("high_101");
			break;
		case 4:
			Application.LoadLevel("buildplayer-high_44");
			break;
		case 5:
			Application.LoadLevel("buildplayer-high_46");
			break;
		case 3:
			Application.LoadLevel("buildplayer-demo valley");
			break;
		case 2:
			Application.LoadLevel("high_101");
			break;
		case 6:
			Application.LoadLevel("high_101");
			break;
		}
		yield return null;
	}

	private void FixedUpdate()
	{
		if (_startGUI)
		{
			//_Elements[0].GetComponent<UILabel>().text = string.Format(Localization.Get("Loading"), (int)(download.progress * 100f));
			//_Elements[3].GetComponent<UISprite>().fillAmount = download.progress;
		}
	}
}
