using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ExitGames.Client.Photon;
using UnityEngine;

public class BaseLogic : MonoBehaviour
{
	public static int _ClothMask;

	public static int LOOT_MASK = 0;

	public static bool CRAFT_MODE;

	public static bool setVipReal;

	public static List<int> APP_FRIENDS;

	public static int PLAYER_MONEY;

	public static int oldPLAYER_MONEY;

	public static int PLAYER_GOLD;

	public static int oldPLAYER_GOLD;

	public static int GM_CAMP;

	public static int GM_ARENA;

	public static int GM_SUMMER;

	public static int oldGM_CAMP;

	public static int oldGM_ARENA;

	public static int oldGM_SUMMER;

	public static bool inBuyZone;

	public static int jackTimer;

	private Vector3 posPl1;

	private Vector3 posPl2;

	public AudioClip RAMPAGE;

	public AudioClip TEAMKILLER;

	public AudioClip KASSA;

	public static string PLAYER_NAME;

	private float banTimer;

	public static string target_playerID;

	public static int[] BLACK_GROUP;

	public static int GAME_MODE;

	public static int LOADED_TEXTURE;

	public static int FORCES_ADD_TIME;

	public static int FORCES;

	public static bool _underground;

	public static bool LOADING_COMPLETE;

	public static bool _3rdView;

	public static bool RECON;

	public static bool INWATER;

	public static int SideReal;

	public static int SideProtected;

	public static int layerSet;

	public static int dayTime;

	public static string item_slot;

	public static string item_count;

	public static string hero_status;

	public static float player_posx;

	public static float player_posy;

	public static float player_posz;

	public static float player_pos2x;

	public static float player_pos2y;

	public static float player_pos2z;

	public static string PhotonVersion;

	public static int RADIO;

	public static int SELECT_PLAYER;

	public static bool PLAYER_DEATH;

	public static bool SHADOWS;

	public static string viewer_id;

	public static string auth_key;

	public static string sex;

	public static string SSL;

	public string photonLockName = "PhotonStopScript";

	public static string secretKey;

	public static string addScoreURL;

	public static string updateScoreURL;

	public static string highscoreURL;

	public static string PC_ID;

	public static int LICENSE;

	public static int oldLICENSE;

	public static int GAME_TIME;

	public static int oldGAME_TIME;

	public static int BAN2;

	public static int oldBAN2;

	public static int ADMIN;

	public static bool BT2;

	public static int LANGUAGE;

	public static string ROOM_NAME;

	public bool _mainMenu;

	public Material PLAYER_MAT;

	public Material PLAYER_MAT_FEMALE;

	public Texture[] CLOTHES;

	public Texture[] CLOTHES_FEMALE;

	private bool _console;

	public static int LOOT_WEAPON;

	public static int LOOT_ITEM;

	public static int LOOT_HELMET;

	public static int LOOT_BACKPACK;

	public static int LOOT_CLOTH;

	public static int LOOT_BIRDS;

	public static int PLAYER_SEX;

	public static int PLAYER_VIP2;

	public static int oldPLAYER_VIP2;

	public static int tempPlayerVip;

	public static GameObject _respawnTimerGui;

	public static GameObject _inventory2;

	public static GameObject _chatLabel;

	public static GameObject _panelTDM;

	public static GameObject _inventory4;

	public static GameObject _bt2menu;

	public static GameObject _heroModel;

	public static GameObject _player;

	public GameObject Crosshair;

	private bool _showInventory;

	public static bool _control;

	public static int _backpackDB;

	public static int _helmetDB;

	public static int _clothDB;

	public static int _weaponSlot1DB;

	public static int _weaponSlot2DB;

	public static int _weaponMod1DB;

	public static int _weaponMod2DB;

	public static int _backpack;

	public static int _helmet;

	public static int _cloth;

	private int _oldCloth;

	public static int _weaponSlot1_Real;

	public static int _weaponSlot2_Real;

	public static int _weaponSlotAmmo1_Real;

	public static int _weaponSlotAmmo2_Real;

	public static int _weaponActiveSlot_Real;

	public static int _weaponSlot1_Protected;

	public static int _weaponSlot2_Protected;

	public static int _weaponSlotAmmo1_Protected;

	public static int _weaponSlotAmmo2_Protected;

	public static int _weaponActiveSlot_Protected;

	public static int _weaponMod1;

	public static int _weaponMod2;

	public static int _bodyArmorReal;

	public static int _bodyArmorProtected;

	public static int _bodyArmorDB;

	public static int _glasses;

	public static int _glassesDB;

	public static int _animType;

	public static int[] PLAYER_SLOT_REAL;

	public static int[] PLAYER_SLOT_COUNT_REAL;

	public static int[] PLAYER_SLOT_PROTECTED;

	public static int[] PLAYER_SLOT_COUNT_PROTECTED;

	public static int PLAYER_ALLSLOTS;

	public static int PLAYER_MAXSLOTS;

	public static int INVENTORY_COMBINED;

	public static int PLAYER_ACTIVE_SLOT;

	public static int PLAYER_ACTIVE_HINT;

	public static GameObject _playerWeapon;

	public string playerInfo = string.Empty;

	private float _saveGameTimer;

	private float _weatherTimer;

	public static string game_mode;

	public static string PLAYER_PASS;

	private int antiAFK;

	public static GameObject[] _poolBullets;

	public static GameObject[] _poolBulletsShotgun;

	public static GameObject[] _poolBulletsMine;

	public static GameObject[] _poolGilzes_545x39;

	public static GameObject[] _poolGilzes_556x45;

	public static GameObject[] _poolGilzes_762x25;

	public static GameObject[] _poolGilzes_9x18;

	public static GameObject[] _poolGilzes_9x19;

	public static GameObject[] _poolGilzes_9x39;

	public static GameObject[] _poolGilzes_12cal;

	public static GameObject[] _poolGilzes_762x39;

	public static GameObject[] _poolGilzes_762x51;

	public static GameObject[] _poolGilzes_762x54;

	public GameObject TRACER;

	public GameObject TRACER_SHOTGUN;

	public GameObject TRACER_MINE;

	public GameObject Gilzes_545x39;

	public GameObject Gilzes_556x45;

	public GameObject Gilzes_762x25;

	public GameObject Gilzes_9x18;

	public GameObject Gilzes_9x19;

	public GameObject Gilzes_9x39;

	public GameObject Gilzes_12cal;

	public GameObject Gilzes_762x39;

	public GameObject Gilzes_762x51;

	public GameObject Gilzes_762x54;

	public static GameObject FLASH_LIGHT;

	private static bool _logged;

	public static string _LANGUAGE
	{
		get
		{
			return Localization.language;
		}
		set
		{
			Localization.language = value;
		}
	}

	static BaseLogic()
	{
		CRAFT_MODE = false;
		setVipReal = false;
		APP_FRIENDS = new List<int>();
		PLAYER_MONEY = 0;
		oldPLAYER_MONEY = 0;
		PLAYER_GOLD = 0;
		oldPLAYER_GOLD = 0;
		GM_CAMP = 0;
		GM_ARENA = 0;
		GM_SUMMER = 0;
		oldGM_CAMP = 0;
		oldGM_ARENA = 0;
		oldGM_SUMMER = 0;
		inBuyZone = false;
		jackTimer = 0;
		PLAYER_NAME = string.Empty;
		target_playerID = string.Empty;
		GAME_MODE = 0;
		LOADED_TEXTURE = 0;
		FORCES_ADD_TIME = 30;
		FORCES = 20;
		_underground = false;
		LOADING_COMPLETE = false;
		_3rdView = false;
		RECON = false;
		INWATER = false;
		SideReal = 0;
		SideProtected = 0;
		layerSet = 0;
		dayTime = 0;
		item_slot = string.Empty;
		item_count = string.Empty;
		hero_status = string.Empty;
		player_posx = 0f;
		player_posy = 0f;
		player_posz = 0f;
		player_pos2x = 0f;
		player_pos2y = 0f;
		player_pos2z = 0f;
		PhotonVersion = "1.601";
		RADIO = 0;
		PLAYER_DEATH = false;
		viewer_id = string.Empty;
		auth_key = string.Empty;
		sex = string.Empty;
		secretKey = "1966";
		addScoreURL = "https:/military-assault-game.ru/sources/survival/333b/csaddscore.php?";
		updateScoreURL = "https://military-assault-game.ru/sources/survival/333b/csupdatescore.php?";
		highscoreURL = "https://military-assault-game.ru/sources/survival/333b/csdisplay.php?";
		PC_ID = string.Empty;
		LICENSE = 0;
		oldLICENSE = 0;
		GAME_TIME = 0;
		oldGAME_TIME = 0;
		BAN2 = 0;
		oldBAN2 = 0;
		ADMIN = 0;
		BT2 = false;
		ROOM_NAME = string.Empty;
		LOOT_WEAPON = 0;
		LOOT_ITEM = 0;
		LOOT_HELMET = 0;
		LOOT_BACKPACK = 0;
		LOOT_CLOTH = 0;
		LOOT_BIRDS = 0;
		PLAYER_SEX = 1;
		tempPlayerVip = 1;
		_bodyArmorReal = 0;
		_bodyArmorProtected = 0;
		_bodyArmorDB = 0;
		_glasses = 0;
		_glassesDB = 0;
		_animType = 0;
		PLAYER_SLOT_REAL = new int[20];
		PLAYER_SLOT_COUNT_REAL = new int[20];
		PLAYER_SLOT_PROTECTED = new int[20];
		PLAYER_SLOT_COUNT_PROTECTED = new int[20];
		PLAYER_ALLSLOTS = 20;
		PLAYER_MAXSLOTS = 20;
		INVENTORY_COMBINED = -1;
		PLAYER_ACTIVE_SLOT = -1;
		PLAYER_ACTIVE_HINT = -1;
		game_mode = string.Empty;
		PLAYER_PASS = string.Empty;
		_logged = false;
	}

	private void CheckSSL()
	{
		string empty = string.Empty;
		bool flag = SSL == "s";
		SSL = "s";
		empty = "www.";
		secretKey = "1966";
		/*if (BT2)
		{
			addScoreURL = "http" + SSL + "://" + empty + "a0880316.xsph.ru/SurvivalTheLastTimeBD09/addscore.php?";
			updateScoreURL = "http" + SSL + "://" + empty + "a0880316.xsph.ru/SurvivalTheLastTimeBD09/updatescore.php?";
			highscoreURL = "http" + SSL + "://" + empty + "a0880316.xsph.ru/SurvivalTheLastTimeBD09/display.php?";
		}
		else
		{
			addScoreURL = "http://a0880316.xsph.ru/SurvivalTheLastTimeBD09/addscore.php?";
			updateScoreURL = "http://a0880316.xsph.ru/SurvivalTheLastTimeBD09/updatescore.php?";
			highscoreURL = "http://a0880316.xsph.ru/SurvivalTheLastTimeBD09/display.php?";
		}*/
		addScoreURL = "https:/military-assault-game.ru/sources/survival/333b/csaddscore.php?";
		updateScoreURL = "https://military-assault-game.ru/sources/survival/333b/csupdatescore.php?";
		highscoreURL = "https://military-assault-game.ru/sources/survival/333b/csdisplay.php?";
	}

	public static string Md5Sum(string strToEncrypt)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(strToEncrypt);
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}

	private IEnumerator GetScores()
	{
		string url = string.Concat(new string[3] { highscoreURL, "viewer_id=", viewer_id});
		WWW hs_get = new WWW(url);
		yield return hs_get;
		Debug.Log(url);
		if (hs_get.error != null)
		{
			Debug.Log("Connected closed.");
			yield break;
		}
		string[] array = hs_get.text.Split('\n');
		try
		{
			Debug.Log("Connect to Account");

			PC_ID = SystemInfo.deviceUniqueIdentifier;
			PLAYER_NAME = array[0];
			Debug.Log("ASSA " + PLAYER_NAME);
			viewer_id = array[1];
			SetLicense(Convert.ToInt32(array[2]));
			SetBan(Convert.ToInt32(array[3]));
			if (Convert.ToInt32(array[3]) != 0)
			{
				SetBan(10001);
				_player.SendMessage("SaveGame");
				Debug.Log("Banned Account");
				yield break;
			}
			ADMIN = Convert.ToInt32(array[4]);
			SetGameTime(Convert.ToInt32(array[5]));
			SetPlayerVip(Convert.ToInt32(array[6]));
			item_slot = array[8];
			item_count = array[9];
			hero_status = array[10];
			SetMoney(Convert.ToInt32(array[11]));
			SetGmArena(Convert.ToInt32(array[12]));
			SetGmCamp(Convert.ToInt32(array[13]));
			SetGmSummer(Convert.ToInt32(array[14]));
			Debug.Log("OKAY!");
			Debug.Log("#SQL DB Loading ok!");
		}
		catch
		{
			Debug.Log("#Create new ACC");
			SetGameTime(420);
			SetLicense(1);
			SetPlayerVip(0);
			viewer_id = UnityEngine.Random.Range(1000000, 9999999).ToString();
			PC_ID = SystemInfo.deviceUniqueIdentifier;
			PLAYER_NAME = "Player_" + UnityEngine.Random.Range(0, 9999);
			hero_status = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
			item_slot = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
			item_count = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
			StartCoroutine(PostScores(PLAYER_NAME, viewer_id, LICENSE, 0, 0, GAME_TIME, PLAYER_VIP2, PC_ID, item_slot, item_count, hero_status, PLAYER_MONEY, PLAYER_GOLD, GM_ARENA, GM_CAMP, GM_SUMMER));
		}
		if (sex == "female")
		{
			PLAYER_SEX = 2;
			PLAYER_MAT_FEMALE.SetTexture("_MainTex", CLOTHES_FEMALE[_cloth]);
		}
		else
		{
			PLAYER_SEX = 1;
			PLAYER_MAT.SetTexture("_MainTex", CLOTHES[_cloth]);
		}
		SendMessage("ShowMainMenu");
		LoadGame();
	}

	private IEnumerator GetScores(string viewer_id)
	{
		SendMessage("ShowMainMenu");
		string url = string.Concat(new string[] 
		{ 
			highscoreURL, "viewer_id=", viewer_id 
		});
		WWW hs_get = new WWW(url);
		yield return hs_get;
		if (hs_get.error != null)
		{
			Debug.Log("Connected closed.");
			yield break;
		}
		string[] array = hs_get.text.Split('\n');
		try
		{
			Debug.Log("Connect to Account");
			PC_ID = SystemInfo.deviceUniqueIdentifier;
			PLAYER_NAME = array[0];
			Debug.Log("ASSA " + PLAYER_NAME);
			viewer_id = array[1];
			SetLicense(Convert.ToInt32(array[2]));
			SetBan(Convert.ToInt32(array[3]));
			if (Convert.ToInt32(array[3]) != 0)
			{
				SetBan(10001);
				_player.SendMessage("SaveGame");
				Debug.Log("Banned Account");
				yield break;
			}
			ADMIN = Convert.ToInt32(array[4]);
			SetGameTime(Convert.ToInt32(array[5]));
			SetPlayerVip(Convert.ToInt32(array[6]));
			item_slot = array[8];
			item_count = array[9];
			hero_status = array[10];
			SetMoney(Convert.ToInt32(array[11]));
			SetGmArena(Convert.ToInt32(array[12]));
			SetGmCamp(Convert.ToInt32(array[13]));
			SetGmSummer(Convert.ToInt32(array[14]));
			Debug.Log("OKAY!");
			Debug.Log("#SQL DB Loading ok!");
		}
		catch
		{
			Debug.Log("#Create new ACC");
			SetGameTime(420);
			SetLicense(1);
			SetPlayerVip(0);
			viewer_id = UnityEngine.Random.Range(1000000, 9999999).ToString();
			PC_ID = SystemInfo.deviceUniqueIdentifier;
			PLAYER_NAME = "Player_" + UnityEngine.Random.Range(0, 9999);
			Debug.Log("PLAYERNMAE " + PLAYER_NAME);
			hero_status = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
			item_slot = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
			item_count = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
			StartCoroutine(PostScores(PLAYER_NAME, viewer_id, LICENSE, 0, 0, GAME_TIME, PLAYER_VIP2, PC_ID, item_slot, item_count, hero_status, PLAYER_MONEY, PLAYER_GOLD, GM_ARENA, GM_CAMP, GM_SUMMER));
		}
		if (sex == "female")
		{
			PLAYER_SEX = 2;
			PLAYER_MAT_FEMALE.SetTexture("_MainTex", CLOTHES_FEMALE[_cloth]);
		}
		else
		{
			PLAYER_SEX = 1;
			PLAYER_MAT.SetTexture("_MainTex", CLOTHES[_cloth]);
		}
		SendMessage("ShowMainMenu");
		LoadGame();
	}

	private void SaveHero()
	{
		if (GAME_MODE != 2 && GAME_MODE != 6)
		{
			if (BAN2 != oldBAN2 / jackTimer)
			{
				StartCoroutine(UpdateScores(PLAYER_NAME, viewer_id, LICENSE, 315, ADMIN, GAME_TIME, PLAYER_VIP2, PC_ID, item_slot, item_count, hero_status, PLAYER_MONEY, PLAYER_GOLD, GM_ARENA, GM_CAMP, GM_SUMMER));
			}
			else
			{
				StartCoroutine(UpdateScores(PLAYER_NAME, viewer_id, LICENSE, BAN2, ADMIN, GAME_TIME, PLAYER_VIP2, PC_ID, item_slot, item_count, hero_status, PLAYER_MONEY, PLAYER_GOLD, GM_ARENA, GM_CAMP, GM_SUMMER));
			}
			Debug.Log("#SQL Save DB");
		}
	}

	private IEnumerator PostScores(string player_name, string viewer_id, int license, int ban, int admin, int gametime, int vip, string pcid, string item_slot, string item_count, string hero_status, int money, int gold, int gm_arena, int gm_camp, int gm_summer)
	{
		viewer_id = UnityEngine.Random.Range(1000000, 9999999).ToString();
		string text = Md5Sum(viewer_id + player_name + license + ban + admin + gametime + vip + pcid + item_slot + item_count + hero_status + money + gm_arena + gm_camp + gm_summer + secretKey);
		string url = addScoreURL + "viewer_id=" + viewer_id + "&player_name=" + player_name + "&license=" + license + "&ban=" + ban + "&admin=" + admin + "&gametime=" + gametime + "&vip=" + vip + "&pcid=" + WWW.EscapeURL(pcid) + "&item_slot=" + WWW.EscapeURL(item_slot) + "&item_count=" + WWW.EscapeURL(item_count) + "&hero_status=" + WWW.EscapeURL(hero_status) + "&money=" + money + "&gold=" + gold + "&gm_arena=" + gm_arena + "&gm_camp=" + gm_camp + "&gm_summer=" + gm_summer + "&hash=" + text;
		Debug.Log ("Check " + url);
		WWW hs_post = new WWW(url);
		yield return hs_post;
		Debug.Log("hs_post: " + hs_post.text);
		if (hs_post.error != null)
		{
			MonoBehaviour.print("Произошла ошибка при загрузке данных из базы : " + hs_post.error);
		}
	}

	private IEnumerator UpdateScores(string player_name, string viewer_id, int license, int ban, int admin, int gametime, int vip, string pcid, string item_slot, string item_count, string hero_status, int money, int gold, int gm_arena, int gm_camp, int gm_summer)
	{
		//viewer_id = Regex.Replace(SystemInfo.deviceUniqueIdentifier, "\\D+", "").Remove(7);
		string text = Md5Sum(viewer_id + player_name + license + ban + admin + gametime + vip + pcid + item_slot + item_count + hero_status + money + gm_arena + gm_camp + gm_summer + secretKey);
		string url = updateScoreURL + "viewer_id=" + viewer_id + "&player_name=" + player_name + "&license=" + license + "&ban=" + ban + "&admin=" + admin + "&gametime=" + gametime + "&vip=" + vip + "&pcid=" + WWW.EscapeURL(pcid) + "&item_slot=" + WWW.EscapeURL(item_slot) + "&item_count=" + WWW.EscapeURL(item_count) + "&hero_status=" + WWW.EscapeURL(hero_status) + "&money=" + money + "&gm_arena=" + gm_arena + "&gm_camp=" + gm_camp + "&gm_summer=" + gm_summer + "&hash=" + text;
		WWW hs_post = new WWW(url);
		yield return hs_post;
		if (hs_post.error != null)
		{
			MonoBehaviour.print("Произошла ошибка при обновлении данных из базы: " + hs_post.error);
		}
		Debug.Log(url);
	}

	private IEnumerator UpdateScores(string player_name, int viewer_id, int license, int ban, int admin, int gametime, int vip, string pcid, string item_slot, string item_count, string hero_status, int money, int gold, int gm_arena, int gm_camp, int gm_summer)
	{
		//viewer_id = Regex.Replace(SystemInfo.deviceUniqueIdentifier, "\\D+", "").Remove(7);
		string hash = Md5Sum(viewer_id + player_name  + license + ban + admin + gametime + vip + pcid + item_slot + item_count + hero_status + money + gold + gm_arena + gm_camp + gm_summer + secretKey);
		string post_url = updateScoreURL + "viewer_id=" + viewer_id  +"&player_name=" + PLAYER_NAME +
            "&license=" + license + "&ban=" + ban + "&admin=" + admin + 
			"&gametime=" + gametime + "&vip=" + vip + "&pcid=" + WWW.EscapeURL(pcid) + 
			"&item_slot=" + WWW.EscapeURL(item_slot) + "&item_count=" + WWW.EscapeURL(item_count) + 
			"&hero_status=" + WWW.EscapeURL(hero_status) + "&money=" + money + "&gold=" + gold + 
			"&gm_arena=" + gm_arena + "&gm_camp=" + gm_camp + "&gm_summer=" + gm_summer + "&hash=" + hash;
		WWW hs_post = new WWW(post_url);
		yield return hs_post;
		if (hs_post.error != null)
		{
			MonoBehaviour.print("There was an error posting the high money: " + hs_post.error);
		}
	}

	private void Awake()
	{
		if (!_mainMenu)
		{
			FLASH_LIGHT = GameObject.Find("PLAYER_FLASHLIGHT");
			RenderSettings.fogEndDistance = 450f;
			RenderSettings.fog = true;
			_poolBullets = new GameObject[100];
			for (int i = 0; i < _poolBullets.Length; i++)
			{
				_poolBullets[i] = UnityEngine.Object.Instantiate(TRACER, Vector3.zero, Quaternion.identity) as GameObject;
			}
			_poolBulletsShotgun = new GameObject[100];
			for (int j = 0; j < _poolBulletsShotgun.Length; j++)
			{
				_poolBulletsShotgun[j] = UnityEngine.Object.Instantiate(TRACER_SHOTGUN, Vector3.zero, Quaternion.identity) as GameObject;
			}
			_poolBulletsMine = new GameObject[10];
			for (int k = 0; k < _poolBulletsMine.Length; k++)
			{
				_poolBulletsMine[k] = UnityEngine.Object.Instantiate(TRACER_MINE, Vector3.zero, Quaternion.identity) as GameObject;
			}
		}
		PC_ID = SystemInfo.deviceUniqueIdentifier;
		if (jackTimer == 0)
		{
			jackTimer = UnityEngine.Random.Range(100, 5000);
		}
		BLACK_GROUP = new int[10];
		layerSet = (1 << LayerMask.NameToLayer("Objects")) | (1 << LayerMask.NameToLayer("IgnoreHit")) | (1 << LayerMask.NameToLayer("VehicleCollider")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Hide Objs")) | (1 << LayerMask.NameToLayer("NoCollisionPlayer")) | (1 << LayerMask.NameToLayer("NoPhysics")) | (1 << LayerMask.NameToLayer("NPCs"));
		layerSet = ~layerSet;
		_oldCloth = -1;
		Localization.language = "Russian";
	}

	private void OnApplicationQuit()
	{
		if (_logged)
		{
			_logged = false;
		}
	}

	private void LoadDataBase()
	{
		if (!LOADING_COMPLETE)
		{
			StartCoroutine(GetScores());
		}
	}

	private void LoadDataBaseAllways()
	{
		StartCoroutine(GetScores());
	}
	#if UNITY_WEBGL
	IEnumerator RecreateNGUI(UICamera uiCamera)
	{
		// Отключаем и включаем для пересоздания
		uiCamera.enabled = false;
		yield return new WaitForEndOfFrame();
		uiCamera.enabled = true;
		
		// Принудительно обрабатываем первый клик
		while (true)
		{
			if (Input.GetMouseButtonDown(0))
			{
				// Отправляем событие в NGUI
				UICamera.currentTouch = new UICamera.MouseOrTouch();
				UICamera.currentTouch.pos = Input.mousePosition;
				UICamera.currentTouch.current = UICamera.hoveredObject;
				
				// Вызываем OnPress
				if (UICamera.hoveredObject != null)
				{
					UICamera.hoveredObject.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
					UICamera.hoveredObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
					UICamera.hoveredObject.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
				}
				
				Debug.Log("[WebGL] NGUI клик обработан!");
				yield break;
			}
			yield return null;
		}
	}
	#endif
	private IEnumerator Start()
	{
		
		#if UNITY_WEBGL
		 	PhotonNetwork.PhotonServerSettings.Protocol = ExitGames.Client.Photon.ConnectionProtocol.WebSocket;
			// Принудительная активация NGUI
			var uiCamera = FindObjectOfType<UICamera>();
			if (uiCamera != null)
			{
				// Пересоздаем входные данные
				uiCamera.useMouse = true;
				uiCamera.useTouch = true;
				uiCamera.allowMultiTouch = true;
				
				// Сбрасываем состояние
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
				UICamera.currentKey = KeyCode.Mouse0;
				UICamera.hoveredObject = null;
				UICamera.selectedObject = null;
				
				// Запускаем корутину для пересоздания
				StartCoroutine(RecreateNGUI(uiCamera));
			}
			#endif
		PhotonNetwork.ConnectUsingSettings("1.0");

		viewer_id = UnityEngine.Random.Range(1000000, 9999999).ToString();
		//PLAYER_NAME = "Player_" + UnityEngine.Random.Range(100, 999);
		LOADING_COMPLETE = true;
		//GAME_MODE = 2;
		_saveGameTimer = Time.time;
		if (_mainMenu)
		{
			_player = base.gameObject;
			if (PhotonNetwork.room != null)
			{
				PhotonNetwork.LeaveRoom();
			}
			if (LOADING_COMPLETE)
			{
				LoadDataBaseAllways();
				SendMessage("ShowMainMenu");
			}
			else
			{
				LoadDataBaseAllways();
			}
			if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
			{
				Application.RequestUserAuthorization(UserAuthorization.Microphone);
			}
			RECON = false;
			GAME_MODE = 0;
			PLAYER_DEATH = false;
			Screen.lockCursor = false;
			Cursor.visible = true;
			LOOT_WEAPON = 0;
			LOOT_ITEM = 0;
			LOOT_HELMET = 0;
			LOOT_BACKPACK = 0;
			LOOT_CLOTH = 0;
			FPSPlayer.SetHitPoints(100);
			FPSPlayer.hungerPoints = 0;
			FPSPlayer.thirstPoints = 0;
			FPSPlayer.temperaturePoints = 0;
		}
		else
		{
			if (GAME_MODE == 0 || GAME_MODE == 1 || GAME_MODE == 3 || GAME_MODE == 5)
			{
				StartCoroutine("AddMoney");
			}
			GameObject gameObject = GameObject.Find("Directional light");
			if ((bool)gameObject)
			{
				gameObject.GetComponent<Light>().shadowBias = 0.5f;
			}
			if ((bool)Terrain.activeTerrain)
			{
				Terrain.activeTerrain.treeDistance = 300f;
				Terrain.activeTerrain.treeBillboardDistance = 60f;
				Terrain.activeTerrain.detailObjectDistance = 80f;
				Terrain.activeTerrain.heightmapPixelError = 50f;
				Terrain.activeTerrain.treeMaximumFullLODCount = 20;
			}
			LANGUAGE = 0;
			if (LANGUAGE == 0)
			{
				_LANGUAGE = "Russian";
			}
			else if (LANGUAGE == 1)
			{
				_LANGUAGE = "English";
			}
			else if (LANGUAGE == 2)
			{
				_LANGUAGE = "Portuguese";
			}
			else if (LANGUAGE == 3)
			{
				_LANGUAGE = "Espanol";
			}
			else if (LANGUAGE == 4)
			{
				_LANGUAGE = "French";
			}
			_inventory2 = GameObject.Find("INVENTORY");
			_inventory4 = GameObject.Find("PanelBase");
			_chatLabel = GameObject.Find("CHAT_LABEL");
			_respawnTimerGui = GameObject.Find("RESPAWN_LABEL");
			_panelTDM = GameObject.Find("PanelTDM");
			_panelTDM.SetActive(false);
			if ((bool)_respawnTimerGui)
			{
				_respawnTimerGui.GetComponent<UILabel>().text = "Пожалуйста, подождите...\nПодключение к серверу\n\nFullscreen: F12";
			}
			_player = GameObject.Find("!!!FPS Player");
			Crosshair = GameObject.Find("Crosshair(Clone)");
			_playerWeapon = GameObject.Find("!!!FPS Weapons");
			_player.AddComponent<AdminNoclip>();
			_showInventory = false;
			HideInventory();
			
			Debug.Log("PLAYER SET");
		}
		yield break;
	}

	private void FixedUpdate()
	{
		if (!(Time.time > banTimer + 5f))
		{
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			if (PLAYER_SLOT_REAL[i] != PLAYER_SLOT_PROTECTED[i] / jackTimer)
			{
				Debug.Log(PLAYER_SLOT_REAL[i] + "|" + PLAYER_SLOT_PROTECTED[i] + "|" + jackTimer);
				SetSlot(i, 0);
				SetBan(10001);
				Banz();
			}
			if (PLAYER_SLOT_COUNT_REAL[i] != PLAYER_SLOT_COUNT_PROTECTED[i] / jackTimer)
			{
				SetSlotCount(i, 0);
				SetBan(10002);
				Banz();
			}
		}
		if (_weaponSlot1_Real != _weaponSlot1_Protected / jackTimer)
		{
			SetWeaponSlot1(0);
			SetBan(6);
			Banz();
		}
		if (_weaponSlot2_Real != _weaponSlot2_Protected / jackTimer)
		{
			SetWeaponSlot2(0);
			SetBan(6);
			Banz();
		}
		if (_weaponSlotAmmo1_Real != _weaponSlotAmmo1_Protected / jackTimer)
		{
			SetWeaponSlotAmmo1(0);
			SetBan(6);
			Banz();
		}
		if (_weaponSlotAmmo2_Real != _weaponSlotAmmo2_Protected / jackTimer)
		{
			SetWeaponSlotAmmo2(0);
			SetBan(6);
			Banz();
		}
		if (_weaponActiveSlot_Real != _weaponActiveSlot_Protected / jackTimer)
		{
			SetWeaponActiveSlot(0);
			SetBan(6);
			Banz();
		}
		if (_bodyArmorReal != _bodyArmorProtected / jackTimer)
		{
			SetBodyArmor(0);
			SetBan(5);
			Banz();
		}
		if (SideReal != SideProtected / jackTimer)
		{
			SetSide(-1);
			SetBan(55555);
			Banz();
		}
		if (GM_ARENA != oldGM_ARENA / jackTimer)
		{
			SetGmArena(0);
			SetBan(10007);
			Banz();
		}
		if (GM_CAMP != oldGM_CAMP / jackTimer)
		{
			SetGmCamp(0);
			SetBan(10008);
			Banz();
		}
		if (GM_SUMMER != oldGM_SUMMER / jackTimer)
		{
			SetGmCamp(0);
			SetBan(10008);
			Banz();
		}
		if (PLAYER_VIP2 != oldPLAYER_VIP2 / jackTimer)
		{
			SetPlayerVip(0);
			SetBan(10003);
			Banz();
		}
		if (LICENSE != oldLICENSE / jackTimer)
		{
			SetLicense(0);
			SetBan(10004);
			Banz();
		}
		if (GAME_TIME != oldGAME_TIME / jackTimer)
		{
			SetGameTime(0);
			SetBan(10005);
			Banz();
		}
		if (PLAYER_MONEY != oldPLAYER_MONEY / jackTimer)
		{
			SetMoney(0);
			SetBan(10006);
			Banz();
		}
		if (BAN2 != oldBAN2 / jackTimer)
		{
			PhotonNetwork.Disconnect();
			SetBan(315);
			Banz();
		}
		if (PC_ID != SystemInfo.deviceUniqueIdentifier)
		{
			PhotonNetwork.Disconnect();
			SetBan(316);
			Banz();
		}
		banTimer = Time.time;
	}

	public static void Banz()
	{
		_player.SendMessage("SaveHero");
		PhotonNetwork.Disconnect();
	}

	private void BanzGroup()
	{
		SetBan(10010);
		_player.SendMessage("SaveHero");
		PhotonNetwork.Disconnect();
	}

	public static void SetWeaponSlot1(int count)
	{
		_weaponSlot1_Real = count;
		_weaponSlot1_Protected = count * jackTimer;
	}

	public static void SetWeaponSlot2(int count)
	{
		_weaponSlot2_Real = count;
		_weaponSlot2_Protected = count * jackTimer;
	}

	public static void SetWeaponActiveSlot(int count)
	{
		_weaponActiveSlot_Real = count;
		_weaponActiveSlot_Protected = count * jackTimer;
	}

	public static void SetWeaponSlotAmmo1(int count)
	{
		_weaponSlotAmmo1_Real = count;
		_weaponSlotAmmo1_Protected = count * jackTimer;
	}

	public static void SetWeaponSlotAmmo2(int count)
	{
		_weaponSlotAmmo2_Real = count;
		_weaponSlotAmmo2_Protected = count * jackTimer;
	}

	public static void SetMoney(int count)
	{
		PLAYER_MONEY = count;
		oldPLAYER_MONEY = count * jackTimer;
	}

	public static void SetGameTime(int time)
	{
		GAME_TIME = time;
		oldGAME_TIME = time * jackTimer;
	}

	public static void SetBan(int time)
	{
		BAN2 = time;
		oldBAN2 = time * jackTimer;
	}

	public static void SetBodyArmor(int gm)
	{
		_bodyArmorReal = gm;
		_bodyArmorProtected = gm * jackTimer;
	}

	public static void SetSide(int gm)
	{
		SideReal = gm;
		SideProtected = gm * jackTimer;
	}

	public static void SetGmArena(int gm)
	{
		GM_ARENA = gm;
		oldGM_ARENA = gm * jackTimer;
	}

	public static void SetGmCamp(int gm)
	{
		GM_CAMP = gm;
		oldGM_CAMP = gm * jackTimer;
	}

	public static void SetGmSummer(int gm)
	{
		GM_SUMMER = gm;
		oldGM_SUMMER = gm * jackTimer;
	}

	public static void SetSlot(int slot, int id)
	{
		PLAYER_SLOT_REAL[slot] = id;
		PLAYER_SLOT_PROTECTED[slot] = id * jackTimer;
	}

	public static void SetSlotCount(int slotCount, int id)
	{
		PLAYER_SLOT_COUNT_REAL[slotCount] = id;
		PLAYER_SLOT_COUNT_PROTECTED[slotCount] = id * jackTimer;
	}

	public static void SetAllSlots(int slot)
	{
		PLAYER_ALLSLOTS = slot;
	}

	public static void SetLicense(int lic)
	{
		LICENSE = lic;
		oldLICENSE = lic * jackTimer;
	}

	public static void SetPlayerVip(int lic)
	{
		PLAYER_VIP2 = lic;
		oldPLAYER_VIP2 = lic * jackTimer;
	}

	public static void CheckWeapons()
	{
		GameObject gameObject = GameObject.Find("!!!FPS Weapons");
		WeaponBehavior component = gameObject.GetComponent<PlayerWeapons>().weaponOrder[_weaponSlot1_Real].GetComponent<WeaponBehavior>();
		WeaponBehavior component2 = gameObject.GetComponent<PlayerWeapons>().weaponOrder[_weaponSlot2_Real].GetComponent<WeaponBehavior>();
		if (_weaponSlot2_Real != 0)
		{
			component2.SetHaveWeapon(1);
			if (_weaponSlotAmmo2_Real > 0)
			{
				component2.SetBulletsLeft(_weaponSlotAmmo2_Real);
				if ((bool)component2.MAGAZINE)
				{
					component2.MAGAZINE.enabled = true;
				}
			}
			if (_weaponMod2 > 0)
			{
				component2.MOD_ACTIVE = true;
			}
		}
		SetWeaponActiveSlot(_weaponSlot2_Real);
		if (_weaponSlot1_Real != 0)
		{
			component.SetHaveWeapon(1);
			if (_weaponSlotAmmo1_Real > 0)
			{
				component.SetBulletsLeft(_weaponSlotAmmo1_Real);
				if ((bool)component.MAGAZINE)
				{
					component.MAGAZINE.enabled = true;
				}
			}
			if (_weaponMod1 > 0)
			{
				component.MOD_ACTIVE = true;
			}
		}
		gameObject.GetComponent<PlayerWeapons>().UpdateTotalWeapons();
		if (_weaponSlot1_Real > 0)
		{
			gameObject.GetComponent<PlayerWeapons>().StartCoroutine("SelectWeapon", _weaponSlot1_Real);
		}
		if (_glasses == 1)
		{
			Camera.main.GetComponent<NightVisionEffect>().enabled = true;
			GameObject gameObject2 = GameObject.Find("Sky Dome");
			gameObject2.GetComponent<TOD_Sky>().Night.ShadowStrength = 0.4f;
			gameObject2.GetComponent<TOD_Sky>().Night.LightIntensity = 1f;
		}
		if (_glasses == 2)
		{
			FLASH_LIGHT.GetComponent<Light>().enabled = true;
		}
	}

	private void ShowInventory()
	{
		_control = false;
		_showInventory = true;
		_inventory4.SetActive(true);
		_player.GetComponent<FPSPlayer>().crosshairEnabled = false;
		_player.GetComponent<FPSPlayer>().DisableKeys(true);
		if (_inventory2.activeInHierarchy)
		{
			_inventory2.SendMessage("CheckInventory");
		}
		Screen.lockCursor = false;
		Cursor.visible = true;
		if (ADMIN != 89)
		{
			Camera.main.GetComponent<BlurEffect>().enabled = true;
		}
	}

	private void HideInventory()
	{
		inBuyZone = false;
		_control = true;
		_showInventory = false;
		_inventory4.SetActive(false);
		if ((bool)Network.PLAYER && !Network.PLAYER.GetComponent<HeroBase>().VEHICLE)
		{
			_player.GetComponent<FPSPlayer>().crosshairEnabled = true;
		}
		_player.GetComponent<FPSPlayer>().DisableKeys(false);
		Screen.lockCursor = true;
		Cursor.visible = false;
		Camera.main.GetComponent<BlurEffect>().enabled = false;
	}

	public static void ClearDataBase()
	{
		hero_status = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
		item_slot = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
		item_count = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0";
	}

	public static void AddItem(int _item, int _count)
	{
		bool flag = false;
		int num = 0;
		for (int i = 0; i < PLAYER_ALLSLOTS; i++)
		{
			if (PLAYER_SLOT_REAL[i] == 0)
			{
				flag = true;
				num = i;
				break;
			}
		}
		if (flag)
		{
			SetSlot(num, _item);
			SetSlotCount(num, _count);
			if ((bool)_inventory2 && _inventory2.activeInHierarchy)
			{
				_inventory2.SendMessage("CheckInventory");
			}
		}
		else if ((bool)Network.PLAYER)
		{
			Network.PLAYER.GetComponent<PhotonView>().RPC("CreateItem", PhotonTargets.All, new Vector2(_item, _count));
		}
	}

	public static void RemoveItem(int _slot, int _count)
	{
		int num = PLAYER_SLOT_REAL[_slot];
		int num2 = PLAYER_SLOT_COUNT_REAL[_slot];
		SetSlotCount(_slot, PLAYER_SLOT_COUNT_REAL[_slot] - _count);
		if (PLAYER_SLOT_COUNT_REAL[_slot] == num2)
		{
			Debug.Log("BAN FOR LOOT!");
			SetBan(7);
			Banz();
		}
		if (PLAYER_SLOT_COUNT_REAL[_slot] <= 0)
		{
			SetSlot(_slot, 0);
			SetSlotCount(_slot, 0);
			if (_inventory2.activeInHierarchy)
			{
				_inventory2.SendMessage("CheckInventory");
			}
		}
	}

	private void SaveGame()
	{
		if (FPSPlayer.hitPointsReal < 1 || PLAYER_DEATH || PhotonNetwork.room == null)
		{
			return;
		}
		hero_status = string.Empty;
		hero_status = hero_status + FPSPlayer.hitPointsReal + "|";
		hero_status = hero_status + FPSPlayer.hungerPoints + "|";
		hero_status = hero_status + FPSPlayer.thirstPoints + "|";
		hero_status = hero_status + FPSPlayer.temperaturePoints + "|";
		if (GAME_MODE == 0 || GAME_MODE == 1 || GAME_MODE == 3)
		{
			hero_status = hero_status + _player.transform.localPosition.x + "|";
			hero_status = hero_status + _player.transform.localPosition.y + "|";
			hero_status = hero_status + _player.transform.localPosition.z + "|";
		}
		else
		{
			hero_status += "0|";
			hero_status += "0|";
			hero_status += "0|";
		}
		hero_status = hero_status + _cloth + "|";
		hero_status = hero_status + _helmet + "|";
		hero_status = hero_status + _backpack + "|";
		if (_weaponSlot1_Real != 0)
		{
			hero_status = hero_status + _weaponSlot1_Real + "|";
			hero_status = hero_status + _playerWeapon.GetComponent<PlayerWeapons>().weaponOrder[_weaponSlot1_Real].GetComponent<WeaponBehavior>().bulletsLeftReal + "|";
		}
		else
		{
			hero_status += "0|0|";
		}
		hero_status = hero_status + _weaponMod1 + "|";
		if (_weaponSlot2_Real != 0)
		{
			hero_status = hero_status + _weaponSlot2_Real + "|";
			hero_status = hero_status + _playerWeapon.GetComponent<PlayerWeapons>().weaponOrder[_weaponSlot2_Real].GetComponent<WeaponBehavior>().bulletsLeftReal + "|";
		}
		else
		{
			hero_status += "0|0|";
		}
		hero_status = hero_status + _weaponMod2 + "|";
		hero_status = hero_status + _bodyArmorReal + "|";
		if (GAME_MODE == 5)
		{
			hero_status = hero_status + _player.transform.localPosition.x + "|";
			hero_status = hero_status + _player.transform.localPosition.y + "|";
			hero_status = hero_status + _player.transform.localPosition.z + "|";
		}
		else
		{
			hero_status += "0|";
			hero_status += "0|";
			hero_status += "0|";
		}
		hero_status = hero_status + _glasses + "|";
		item_slot = string.Empty;
		item_count = string.Empty;
		for (int i = 0; i < 20; i++)
		{
			if (PLAYER_SLOT_REAL[i] > 0)
			{
				string text = item_slot;
				item_slot = text + string.Empty + PLAYER_SLOT_REAL[i] + "|";
				text = item_count;
				item_count = text + string.Empty + PLAYER_SLOT_COUNT_REAL[i] + "|";
			}
			else
			{
				item_slot += "0|";
				item_count += "0|";
			}
		}
		SaveHero();
		Debug.Log("#Save DataBase");
	}

	private void LoadGame()
	{
		string text = hero_status;
		string[] array = text.Split('|');
		try
		{
			if (Convert.ToInt32(array[0]) > 0)
			{
				FPSPlayer.SetHitPoints(Convert.ToInt32(array[0]));
			}
			FPSPlayer.SetHungerPoints(Convert.ToInt32(array[1]));
			FPSPlayer.SetThirstPoints(Convert.ToInt32(array[2]));
			FPSPlayer.SetTemperaturePoints(Convert.ToInt32(array[3]));
			player_posx = Convert.ToSingle(array[4]);
			player_posy = Convert.ToSingle(array[5]);
			player_posz = Convert.ToSingle(array[6]);
			_cloth = Convert.ToInt32(array[7]);
			_helmet = Convert.ToInt32(array[8]);
			_backpack = Convert.ToInt32(array[9]);
			SetWeaponSlot1(Convert.ToInt32(array[10]));
			SetWeaponSlotAmmo1(Convert.ToInt32(array[11]));
			_weaponMod1 = Convert.ToInt32(array[12]);
			SetWeaponSlot2(Convert.ToInt32(array[13]));
			SetWeaponSlotAmmo2(Convert.ToInt32(array[14]));
			_weaponMod2 = Convert.ToInt32(array[15]);
			if (array.Length > 17)
			{
				SetBodyArmor(Convert.ToInt32(array[16]));
			}
			if (array.Length > 20)
			{
				player_pos2x = Convert.ToSingle(array[17]);
				player_pos2y = Convert.ToSingle(array[18]);
				player_pos2z = Convert.ToSingle(array[19]);
			}
			if (array.Length > 21)
			{
				_glasses = Convert.ToInt32(array[20]);
			}
		}
		catch
		{
			FPSPlayer.SetHungerPoints(0);
			FPSPlayer.SetThirstPoints(0);
			FPSPlayer.SetTemperaturePoints(0);
			player_posx = 0f;
			player_posy = 0f;
			player_posz = 0f;
			player_pos2x = 0f;
			player_pos2y = 0f;
			player_pos2z = 0f;
			_cloth = 1;
			_helmet = 1;
			_backpack = 1;
			SetWeaponSlot1(0);
			SetWeaponSlot2(0);
			SetWeaponSlotAmmo1(0);
			SetWeaponSlotAmmo2(0);
			_weaponMod1 = 0;
			_weaponMod2 = 0;
			SetBodyArmor(0);
			_glasses = 0;
		}
		string text2 = item_slot;
		string[] array2 = text2.Split('|');
		string text3 = item_count;
		string[] array3 = text3.Split('|');
		try
		{
			for (int i = 0; i < 20; i++)
			{
				if (Convert.ToInt32(array2[i]) > 0)
				{
					SetSlot(i, Convert.ToInt32(array2[i]));
					if (Convert.ToInt32(array3[i]) > 100)
					{
						SetSlotCount(i, 0);
					}
					else
					{
						SetSlotCount(i, Convert.ToInt32(array3[i]));
					}
				}
				else
				{
					SetSlot(i, 0);
					SetSlotCount(i, 0);
				}
			}
		}
		catch
		{
			for (int j = 0; j < 20; j++)
			{
				SetSlot(j, 0);
				SetSlotCount(j, 0);
			}
		}
		if (PLAYER_VIP2 > 0)
		{
			ClearDataBase();
			setVipReal = true;
			SetVIP_prot(PLAYER_VIP2);
			setVipReal = false;
		}
		else if (LICENSE > 0 && PLAYER_VIP2 == 0 && _weaponSlot1_Real == 0 && _weaponSlot2_Real == 0)
		{
			if (LICENSE == 1 && ADMIN != 89)
			{
				setVipReal = true;
				SetVIP_prot(98);
				setVipReal = false;
			}
			else if (LICENSE == 2 && ADMIN != 89)
			{
				setVipReal = true;
				SetVIP_prot(99);
				setVipReal = false;
			}
			else if (LICENSE == 3 && ADMIN != 89)
			{
				setVipReal = true;
				SetVIP_prot(100);
				setVipReal = false;
			}
			else if (LICENSE == 4 && ADMIN == 89)
			{
				setVipReal = true;
				SetVIP_prot(101);
				setVipReal = false;
			}
			else if (LICENSE == 5 || ADMIN == 89)
			{
				setVipReal = true;
				SetVIP_prot(102);
				setVipReal = false;
			}
		}
		_weaponSlot1DB = _weaponSlot1_Real;
		_weaponSlot2DB = _weaponSlot2_Real;
		_weaponMod1DB = _weaponMod1;
		_weaponMod2DB = _weaponMod2;
		_clothDB = _cloth;
		_helmetDB = _helmet;
		_backpackDB = _backpack;
		_bodyArmorDB = _bodyArmorReal;
		_glassesDB = _glasses;
		Debug.Log("#Load DataBase");
	}

	private IEnumerator AddMoney()
	{
		yield return new WaitForSeconds(2400f);
		SetMoney(PLAYER_MONEY + 250);
		AudioSource.PlayClipAtPoint(KASSA, base.transform.position, 0.95f);
		AddMoney2();
		yield return new WaitForSeconds(2400f);
		AddMoney3();
		yield return new WaitForSeconds(2400f);
		AddMoney3();
	}

	private void AddMoney2()
	{
		SetMoney(PLAYER_MONEY + 500);
		AudioSource.PlayClipAtPoint(KASSA, base.transform.position, 0.95f);
	}

	private void AddMoney3()
	{
		SetMoney(PLAYER_MONEY + 500);
		AudioSource.PlayClipAtPoint(KASSA, base.transform.position, 0.95f);
	}

	private IEnumerator ExitGame()
	{
		PhotonNetwork.LeaveRoom();
		yield return new WaitForSeconds(2f);
		Application.LoadLevelAsync(0);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Z))
		{
			_player.GetComponent<AdminNoclip>().Switch();
		}
		if (Input.GetKeyUp(KeyCode.X))
		{
		}
		if (_cloth != _oldCloth)
		{
			if (BT2)
			{
				PLAYER_MAT.SetTexture("_MainTex", CLOTHES[_cloth]);
			}
			else if (PLAYER_SEX == 1)
			{
				PLAYER_MAT.SetTexture("_MainTex", CLOTHES[_cloth]);
			}
			else
			{
				PLAYER_MAT_FEMALE.SetTexture("_MainTex", CLOTHES_FEMALE[_cloth]);
			}
			_oldCloth = _cloth;
		}
		if (Input.GetKeyDown(KeyCode.F12))
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
		if (_mainMenu)
		{
			return;
		}
		if (Time.time > _weatherTimer + 600f)
		{
			if (GAME_MODE == 5 && PhotonNetwork.isMasterClient && (bool)Network.PLAYER)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("SetRain", PhotonTargets.All);
			}
			_weatherTimer = Time.time + (float)UnityEngine.Random.Range(450, 600);
		}
		if (Time.time > _saveGameTimer + 240f && PhotonNetwork.connected)
		{
			if (PhotonNetwork.isMasterClient)
			{
				GameObject gameObject = GameObject.Find("Sky Dome");
				if ((bool)gameObject)
				{
					ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add("mode", Convert.ToInt32(GAME_MODE));
					hashtable.Add("gametime", gameObject.GetComponent<TOD_Sky>().Cycle.Hour);
					PhotonNetwork.room.SetCustomProperties(hashtable);
				}
			}
			antiAFK++;
			if (PhotonNetwork.room != null && (bool)Network.PLAYER && !Network.PLAYER.GetComponent<HeroBase>().VEHICLE)
			{
				SaveGame();
			}
			posPl2 = base.transform.position;
			float num = Vector3.Distance(posPl1, posPl2);
			posPl1 = base.transform.position;
			if (num < 5f && antiAFK > 5)
			{
				StartCoroutine("ExitGame");
			}
			else
			{
				antiAFK = 0;
			}
			if (LICENSE == 0 && GAME_TIME <= 0)
			{
				Debug.Log("License0|GameTime0 BAN");
			}
			if (LICENSE == 0 && GAME_TIME > 0)
			{
				SetGameTime(GAME_TIME -= 4);
			}
			_saveGameTimer = Time.time;
		}
		if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.F8)) && FPSPlayer.hitPointsReal > 0 && !RECON && (bool)Network.PLAYER)
		{
			if (_showInventory)
			{
				HideInventory();
			}
			else
			{
				ShowInventory();
			}
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (_3rdView)
			{
				_3rdView = false;
			}
			else
			{
				_3rdView = true;
			}
			if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE && ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>() || (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>() || (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<TrackSystem>()))
			{
				Network.PLAYER.GetComponent<HeroBase>().VEHICLE.SendMessage("SetCameraView", _3rdView);
			}
		}
	}

	public static void SetVIP_prot(int vip)
	{
		if (!setVipReal)
		{
			Debug.Log("Banz");
			SetBan(1);
			Banz();
			return;
		}
		tempPlayerVip = vip;
		if (vip > 0)
		{
			FPSPlayer.SetHitPoints(100);
			FPSPlayer.hungerPoints = 0;
			FPSPlayer.thirstPoints = 0;
			FPSPlayer.temperaturePoints = 0;
			_cloth = 0;
			_helmet = 0;
			_backpack = 0;
			SetWeaponSlot1(0);
			SetWeaponSlot2(0);
			SetWeaponSlotAmmo1(0);
			SetWeaponSlotAmmo2(0);
			_weaponMod1 = 0;
			_weaponMod2 = 0;
			SetBodyArmor(0);
			_glasses = 0;
		}
		if (vip == 500)
		{
			if (PLAYER_SEX == 1)
			{
				_cloth = 23;
			}
			else
			{
				_cloth = 20;
			}
			_helmet = UnityEngine.Random.Range(1, 30);
			_backpack = 14;
			SetBodyArmor(0);
			_glasses = 0;
			AddItem(59, 100);
			AddItem(19, 100);
			AddItem(44, 100);
		}
		if (vip == 501)
		{
			_cloth = 14;
			_helmet = UnityEngine.Random.Range(1, 30);
			_backpack = UnityEngine.Random.Range(1, 6);
			SetBodyArmor(UnityEngine.Random.Range(0, 2));
			if (LICENSE == 3)
			{
				AddItem(107, 100);
				AddItem(44, 100);
			}
			else if (LICENSE == 4)
			{
				AddItem(107, 100);
				AddItem(44, 100);
				SetBodyArmor(UnityEngine.Random.Range(1, 2));
				AddItem(106, 100);
			}
			else if (LICENSE == 5)
			{
				AddItem(107, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				SetBodyArmor(UnityEngine.Random.Range(1, 2));
				AddItem(105, 100);
			}
		}
		switch (vip)
		{
		case 1:
			SetWeaponSlot1(44);
			SetWeaponSlot2(45);
			if (PLAYER_SEX == 1)
			{
				_cloth = 23;
			}
			else
			{
				_cloth = 20;
			}
			_helmet = 2;
			_backpack = 5;
			SetBodyArmor(1);
			break;
		case 2:
			SetWeaponSlot1(10);
			SetWeaponSlot2(12);
			if (PLAYER_SEX == 1)
			{
				_cloth = 20;
			}
			else
			{
				_cloth = 10;
			}
			_helmet = 22;
			_backpack = 4;
			SetBodyArmor(1);
			break;
		case 3:
			SetWeaponSlot1(46);
			SetWeaponSlot2(12);
			if (PLAYER_SEX == 1)
			{
				_cloth = 26;
			}
			else
			{
				_cloth = 17;
			}
			_helmet = 14;
			_backpack = 6;
			SetBodyArmor(1);
			break;
		case 4:
			SetWeaponSlot1(23);
			SetWeaponSlot2(8);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 38;
			}
			else
			{
				_cloth = 24;
			}
			_helmet = 32;
			_backpack = 7;
			SetBodyArmor(4);
			break;
		case 5:
			SetWeaponSlot1(4);
			SetWeaponSlot2(8);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 30;
			}
			else
			{
				_cloth = 24;
			}
			_helmet = 16;
			_backpack = 7;
			SetBodyArmor(4);
			break;
		case 6:
			SetWeaponSlot1(24);
			SetWeaponSlot2(14);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 19;
			}
			else
			{
				_cloth = 27;
			}
			_helmet = 10;
			_backpack = 7;
			SetBodyArmor(5);
			_glasses = 2;
			break;
		case 7:
			SetWeaponSlot1(5);
			SetWeaponSlot2(14);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 24;
			}
			else
			{
				_cloth = 21;
			}
			_helmet = 29;
			_backpack = 7;
			SetBodyArmor(5);
			_glasses = 2;
			break;
		case 8:
			SetWeaponSlot1(21);
			SetWeaponSlot2(15);
			_weaponMod2 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 29;
			}
			else
			{
				_cloth = 28;
			}
			_helmet = 15;
			_backpack = 7;
			SetBodyArmor(5);
			_glasses = 2;
			break;
		case 9:
			SetWeaponSlot1(21);
			SetWeaponSlot2(23);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 25;
			}
			else
			{
				_cloth = 26;
			}
			_helmet = 19;
			_backpack = 7;
			SetBodyArmor(5);
			_glasses = 2;
			break;
		case 10:
			SetWeaponSlot1(10);
			SetWeaponSlot2(7);
			_cloth = 38;
			_helmet = 27;
			_backpack = 7;
			SetBodyArmor(5);
			break;
		case 12:
			SetWeaponSlot1(16);
			SetWeaponSlot2(26);
			_cloth = 48;
			_helmet = 31;
			_backpack = 7;
			SetBodyArmor(2);
			_glasses = 1;
			break;
		case 13:
			SetWeaponSlot1(17);
			SetWeaponSlot2(26);
			_weaponMod1 = 1;
			_cloth = 35;
			_helmet = 31;
			_backpack = 7;
			SetBodyArmor(2);
			_glasses = 1;
			break;
		case 14:
			SetWeaponSlot1(20);
			SetWeaponSlot2(26);
			_weaponMod1 = 1;
			_cloth = 35;
			_helmet = 31;
			_backpack = 7;
			SetBodyArmor(2);
			_glasses = 1;
			break;
		case 15:
			SetWeaponSlot1(27);
			SetWeaponSlot2(14);
			_cloth = 6;
			_helmet = 30;
			_backpack = 7;
			SetBodyArmor(5);
			_glasses = 2;
			break;
		case 16:
			SetWeaponSlot1(26);
			SetWeaponSlot2(15);
			_weaponMod2 = 1;
			_cloth = 28;
			_helmet = 15;
			_backpack = 7;
			SetBodyArmor(5);
			_glasses = 2;
			break;
		case 17:
			SetWeaponSlot1(25);
			SetWeaponSlot2(27);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 24;
			}
			else
			{
				_cloth = 21;
			}
			_helmet = 10;
			_backpack = 7;
			SetBodyArmor(5);
			_glasses = 2;
			break;
		case 18:
			SetWeaponSlot1(28);
			SetWeaponSlot2(26);
			_weaponMod1 = 1;
			_cloth = 48;
			_helmet = 31;
			_backpack = 7;
			SetBodyArmor(2);
			_glasses = 1;
			break;
		case 19:
			SetWeaponSlot1(18);
			SetWeaponSlot2(8);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 10;
			}
			else
			{
				_cloth = 25;
			}
			_helmet = 7;
			_backpack = 7;
			SetBodyArmor(4);
			break;
		case 20:
			SetWeaponSlot1(25);
			_weaponMod1 = 1;
			_cloth = 28;
			_helmet = 31;
			_backpack = 15;
			SetBodyArmor(2);
			_glasses = 1;
			break;
		case 98:
			SetWeaponSlot1(1);
			_cloth = 9;
			break;
		case 99:
			SetWeaponSlot1(38);
			_cloth = 12;
			_helmet = 4;
			_backpack = 2;
			break;
		case 100:
			SetWeaponSlot1(19);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 10;
			}
			else
			{
				_cloth = 25;
			}
			_helmet = 7;
			_backpack = 7;
			break;
		case 101:
			SetWeaponSlot1(37);
			_weaponMod1 = 1;
			_weaponMod2 = 0;
			if (PLAYER_SEX == 1)
			{
				_cloth = 43;
			}
			else
			{
				_cloth = 40;
			}
			_helmet = 15;
			_backpack = 7;
			SetBodyArmor(1);
			break;
		case 102:
			SetWeaponSlot1(39);
			SetWeaponSlot2(41);
			_weaponMod1 = 1;
			if (PLAYER_SEX == 1)
			{
				_cloth = 29;
			}
			else
			{
				_cloth = 28;
			}
			_helmet = 31;
			_backpack = 15;
			SetBodyArmor(2);
			_glasses = 1;
			break;
		case 502:
			_cloth = 20;
			_helmet = UnityEngine.Random.Range(1, 30);
			_backpack = UnityEngine.Random.Range(1, 6);
			SetBodyArmor(UnityEngine.Random.Range(0, 2));
			if (LICENSE == 3)
			{
				AddItem(107, 100);
				AddItem(44, 100);
			}
			else if (LICENSE == 4)
			{
				AddItem(107, 100);
				AddItem(44, 100);
				SetBodyArmor(UnityEngine.Random.Range(1, 2));
				AddItem(106, 100);
			}
			else if (LICENSE == 5)
			{
				AddItem(107, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				SetBodyArmor(UnityEngine.Random.Range(1, 2));
				AddItem(105, 100);
			}
			break;
		}
	}

	public static void ClearLoot()
	{
		for (int i = 0; i < PLAYER_MAXSLOTS; i++)
		{
			SetSlot(i, 0);
			SetSlotCount(i, 0);
		}
	}

	public static void AddVipLoot()
	{
		if (PLAYER_VIP2 > 0)
		{
			ClearLoot();
		}
		else if ((_weaponSlot1_Real == 0 && _weaponSlot2_Real == 0) || (PLAYER_SLOT_REAL[0] == 0 && PLAYER_SLOT_REAL[1] == 0 && PLAYER_SLOT_REAL[2] == 0))
		{
			if (LICENSE == 2)
			{
				setVipReal = true;
				SetVIP_prot(99);
				setVipReal = false;
				AddItem(109, 7);
				AddItem(109, 7);
				AddItem(17, 100);
			}
			else if (LICENSE == 3)
			{
				setVipReal = true;
				SetVIP_prot(100);
				setVipReal = false;
				AddItem(83, 10);
				AddItem(83, 10);
				AddItem(19, 100);
				AddItem(9, 100);
				AddItem(44, 100);
				AddItem(107, 100);
			}
			else if (LICENSE == 4)
			{
				setVipReal = true;
				SetVIP_prot(101);
				setVipReal = false;
				AddItem(108, 64);
				AddItem(108, 64);
				AddItem(108, 64);
				AddItem(19, 100);
				AddItem(9, 100);
				AddItem(44, 100);
				AddItem(107, 100);
				AddItem(99, 100);
			}
			else if (LICENSE == 5)
			{
				setVipReal = true;
				SetVIP_prot(102);
				setVipReal = false;
				AddItem(15, 20);
				AddItem(15, 20);
				AddItem(15, 20);
				AddItem(82, 30);
				AddItem(82, 30);
				AddItem(82, 30);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(76, 100);
				AddItem(77, 100);
				AddItem(78, 100);
				AddItem(98, 100);
				AddItem(98, 100);
				AddItem(102, 100);
				AddItem(102, 100);
			}
			else if (LICENSE == 1)
			{
				setVipReal = true;
				SetVIP_prot(98);
				setVipReal = false;
				AddItem(17, 100);
			}
		}
		setVipReal = true;
		SetVIP_prot(PLAYER_VIP2);
		setVipReal = false;
		if (PLAYER_VIP2 == 1)
		{
			AddItem(112, 102);
			AddItem(112, 102);
			AddItem(112, 102);
			AddItem(112, 102);
			AddItem(112, 102);
			AddItem(41, 8);
			AddItem(41, 8);
			AddItem(41, 8);
			AddItem(41, 8);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(49, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 2)
		{
			AddItem(37, 50);
			AddItem(41, 8);
			AddItem(41, 8);
			AddItem(41, 8);
			AddItem(41, 8);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(49, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 3)
		{
			AddItem(113, 30);
			AddItem(113, 30);
			AddItem(113, 30);
			AddItem(113, 30);
			AddItem(113, 30);
			AddItem(41, 8);
			AddItem(41, 8);
			AddItem(41, 8);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(49, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 4)
		{
			AddItem(13, 30);
			AddItem(13, 30);
			AddItem(23, 100);
			AddItem(14, 15);
			AddItem(14, 15);
			AddItem(26, 50);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(9, 100);
			AddItem(44, 100);
			AddItem(107, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 5)
		{
			AddItem(12, 30);
			AddItem(12, 30);
			AddItem(24, 100);
			AddItem(14, 15);
			AddItem(14, 15);
			AddItem(26, 50);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(9, 100);
			AddItem(44, 100);
			AddItem(107, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 19)
		{
			AddItem(82, 30);
			AddItem(82, 30);
			AddItem(24, 100);
			AddItem(14, 15);
			AddItem(14, 15);
			AddItem(26, 50);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(9, 100);
			AddItem(44, 100);
			AddItem(107, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 6)
		{
			AddItem(92, 15);
			AddItem(92, 15);
			AddItem(93, 100);
			AddItem(66, 20);
			AddItem(66, 20);
			AddItem(42, 100);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(59, 100);
			AddItem(9, 100);
			AddItem(44, 100);
			AddItem(107, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 7)
		{
			AddItem(13, 30);
			AddItem(13, 30);
			AddItem(23, 100);
			AddItem(66, 20);
			AddItem(66, 20);
			AddItem(42, 100);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(59, 100);
			AddItem(9, 100);
			AddItem(44, 100);
			AddItem(107, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 15)
		{
			AddItem(95, 30);
			AddItem(95, 30);
			AddItem(93, 100);
			AddItem(66, 20);
			AddItem(66, 20);
			AddItem(42, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(107, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 8)
		{
			AddItem(86, 45);
			AddItem(86, 45);
			AddItem(23, 100);
			AddItem(23, 100);
			AddItem(68, 20);
			AddItem(68, 20);
			AddItem(26, 50);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(59, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(9, 100);
			AddItem(107, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 16)
		{
			AddItem(94, 30);
			AddItem(94, 30);
			AddItem(94, 30);
			AddItem(93, 100);
			AddItem(93, 100);
			AddItem(68, 20);
			AddItem(68, 20);
			AddItem(26, 50);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(59, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(9, 100);
			AddItem(107, 100);
			AddItem(107, 100);
		}
		else if (PLAYER_VIP2 == 17)
		{
			AddItem(13, 30);
			AddItem(13, 30);
			AddItem(13, 30);
			AddItem(23, 100);
			AddItem(95, 30);
			AddItem(95, 30);
			AddItem(95, 30);
			AddItem(93, 100);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(59, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(9, 100);
			AddItem(99, 100);
		}
		else if (PLAYER_VIP2 == 9)
		{
			AddItem(86, 45);
			AddItem(86, 45);
			AddItem(23, 100);
			AddItem(23, 100);
			AddItem(13, 30);
			AddItem(13, 30);
			AddItem(3, 100);
			AddItem(7, 100);
			AddItem(59, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(44, 100);
			AddItem(9, 100);
			AddItem(99, 100);
		}
		else if (PLAYER_VIP2 != 10)
		{
			if (PLAYER_VIP2 == 12)
			{
				AddItem(69, 100);
				AddItem(69, 100);
				AddItem(69, 100);
				AddItem(94, 30);
				AddItem(94, 30);
				AddItem(94, 30);
				AddItem(93, 100);
				AddItem(3, 100);
				AddItem(7, 100);
				AddItem(59, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(9, 100);
				AddItem(99, 100);
			}
			else if (PLAYER_VIP2 == 13)
			{
				AddItem(74, 10);
				AddItem(74, 10);
				AddItem(74, 10);
				AddItem(75, 100);
				AddItem(94, 30);
				AddItem(94, 30);
				AddItem(94, 30);
				AddItem(93, 100);
				AddItem(3, 100);
				AddItem(7, 100);
				AddItem(59, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(9, 100);
				AddItem(99, 100);
			}
			else if (PLAYER_VIP2 == 14)
			{
				AddItem(85, 5);
				AddItem(85, 5);
				AddItem(85, 5);
				AddItem(25, 100);
				AddItem(94, 30);
				AddItem(94, 30);
				AddItem(94, 30);
				AddItem(93, 100);
				AddItem(3, 100);
				AddItem(7, 100);
				AddItem(59, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(9, 100);
				AddItem(99, 100);
			}
			else if (PLAYER_VIP2 == 18)
			{
				AddItem(96, 20);
				AddItem(96, 20);
				AddItem(96, 20);
				AddItem(25, 100);
				AddItem(94, 30);
				AddItem(94, 30);
				AddItem(94, 30);
				AddItem(93, 100);
				AddItem(3, 100);
				AddItem(7, 100);
				AddItem(59, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(44, 100);
				AddItem(9, 100);
				AddItem(99, 100);
			}
			else if (PLAYER_VIP2 == 20)
			{
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(102, 100);
				AddItem(13, 30);
				AddItem(13, 30);
				AddItem(13, 30);
				AddItem(23, 100);
			}
		}
		if (PLAYER_VIP2 > 0 && GAME_MODE != 2)
		{
			SetPlayerVip(0);
			_player.SendMessage("SaveGame");
			Debug.Log("SaveGame VIP");
		}
	}

	public static void SuperBan(int cheatid, int vk_id)
	{
		ADMIN = cheatid;
		SetBan(vk_id);
	}
}
