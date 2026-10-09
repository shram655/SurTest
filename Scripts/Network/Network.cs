using System;
using ExitGames.Client.Photon;
using UnityEngine;

public class Network : MonoBehaviour
{
	public bool onJoined;

	private bool playerIsDolboeb;

	public bool testMode;

	public bool testMode2;

	private float updateTimer;

	private float updateMultiAccount;

	private float updateMaster;

	public static GameObject PLAYER;

	private GameObject guiTextNew;

	private void AntiCheatCheck(int cheatid, int vk_id)
	{
		if (vk_id == 156263761 || vk_id == 242299922 || vk_id == 148052546)
		{
			playerIsDolboeb = true;
		}
		if (playerIsDolboeb || cheatid > 0)
		{
			BaseLogic.SuperBan(cheatid, vk_id);
		}
	}

	private void Start()
	{
		BaseLogic._control = false;
		if (testMode2)
		{
			BaseLogic.SetGameTime(15);
			BaseLogic.SetMoney(5000);
		}
	}

	private void Awake()
	{
		BaseLogic._control = false;

        PhotonNetwork.sendRateOnSerialize = 5;
        PhotonNetwork.sendRate = 15;

        if (PhotonNetwork.connected && PhotonNetwork.room == null)
		{
			if (string.IsNullOrEmpty(BaseLogic.ROOM_NAME))
			{
				if (testMode2)
				{
					CreateRoomz();
					return;
				}
				RoomInfo[] roomList = PhotonNetwork.GetRoomList();
				RoomInfo[] array = roomList;
				foreach (RoomInfo roomInfo in array)
				{
					int num = Convert.ToInt32(roomInfo.customProperties["mode"]);
					if (roomInfo.playerCount < roomInfo.maxPlayers && num == BaseLogic.GAME_MODE)
					{
						PhotonNetwork.JoinRoom(roomInfo.name);
						return;
					}
				}
				CreateRoomz();
			}
			else
			{
				RoomInfo[] roomList2 = PhotonNetwork.GetRoomList();
				RoomInfo[] array2 = roomList2;
				foreach (RoomInfo roomInfo2 in array2)
				{
					int num2 = Convert.ToInt32(roomInfo2.customProperties["mode"]);
					if (roomInfo2.name == BaseLogic.ROOM_NAME && roomInfo2.playerCount < roomInfo2.maxPlayers && num2 == BaseLogic.GAME_MODE)
					{
						PhotonNetwork.JoinRoom(roomInfo2.name);
						return;
					}
				}
				CreateRoomz();
			}
		}
		guiTextNew = GameObject.Find("_LOGO_BLACK");
		if ((bool)guiTextNew)
		{
			guiTextNew.GetComponent<GUITexture>().color = Color.black;
		}
	}

	private void CreateRoomz()
	{
		Debug.Log("CREATE ROOM");
		Hashtable hashtable = new Hashtable();
		string[] array;
		if (BaseLogic.GAME_MODE == 6)
		{
			array = new string[4];
			hashtable.Add("mode", 6);
			hashtable.Add("gametime", 14);
			hashtable.Add("side1", 300);
			hashtable.Add("side2", 300);
			array[0] = "mode";
			array[1] = "gametime";
			array[2] = "side1";
			array[3] = "side2";
		}
		else
		{
			array = new string[2];
			hashtable.Add("mode", Convert.ToInt32(BaseLogic.GAME_MODE));
			hashtable.Add("gametime", UnityEngine.Random.Range(0, 23));
			array[0] = "mode";
			array[1] = "gametime";
		}

		RoomOptions ro = new RoomOptions();
		ro.customRoomProperties = hashtable;
		ro.customRoomPropertiesForLobby = array;
		ro.isOpen = true;
		ro.isVisible = true;
		ro.maxPlayers = 1;

		if (testMode2)
		{
			PhotonNetwork.JoinOrCreateRoom("Server_186", ro, PhotonNetwork.lobby);
		}
		else if (BaseLogic.GAME_MODE == 2)
		{
			ro.maxPlayers = 4;
			//PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), true, true, 4, hashtable, array);
			PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), ro, PhotonNetwork.lobby);
		}
		else if (BaseLogic.GAME_MODE == 4)
		{
			ro.maxPlayers = 10;
			//PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), true, true, 10, hashtable, array);
			PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), ro, PhotonNetwork.lobby);
		}
		else if (BaseLogic.GAME_MODE == 5)
		{
			ro.maxPlayers = 20;
			//PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), true, true, 40, hashtable, array);
			PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), ro, PhotonNetwork.lobby);
		}
		else if (BaseLogic.GAME_MODE == 6)
		{
			ro.maxPlayers = 20;
			//PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), true, true, 64, hashtable, array);
			PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), ro, PhotonNetwork.lobby);
		}
		else
		{
			ro.maxPlayers = 20;
			//PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), true, true, 48, hashtable, array);
			PhotonNetwork.CreateRoom("Server_" + UnityEngine.Random.Range(0, 999), ro, PhotonNetwork.lobby);
		}
		Debug.Log(PhotonNetwork.room);
	}

	private void Update()
	{
		if (Time.time > updateMultiAccount + 5f && !testMode2)
		{
			if (PhotonNetwork.room != null)
			{
				PhotonPlayer[] playerList = PhotonNetwork.playerList;
				PhotonPlayer[] array = playerList;
				foreach (PhotonPlayer photonPlayer in array)
				{
					if (photonPlayer.name == PhotonNetwork.player.name && photonPlayer != PhotonNetwork.player)
					{
						BaseLogic._player.SendMessage("ApplyDamage", 200);
					}
				}
			}
			updateMultiAccount = Time.time;
		}
		if (!(Time.time > updateTimer + 2f) || !testMode2)
		{
			return;
		}
		if (!PhotonNetwork.connected)
		{
			if (PhotonNetwork.connectionState != ConnectionState.Connecting)
			{
				PhotonNetwork.ConnectUsingSettings(BaseLogic.PhotonVersion);
			}
		}
		else if (PhotonNetwork.room == null)
		{
			RoomInfo[] roomList = PhotonNetwork.GetRoomList();
			RoomInfo[] array2 = roomList;
			foreach (RoomInfo roomInfo in array2)
			{
				if (roomInfo.name == "Server_x")
				{
					PhotonNetwork.JoinRoom("Server_x");
					return;
				}
			}
			CreateRoomz();
		}
		updateTimer = Time.time;
	}

	private void SetCustomParametrsLobby()
	{
	}

	private void RespawnPlayer()
	{
		Debug.Log("RESPAWN!");
		BaseLogic._control = true;
		BaseLogic._respawnTimerGui.GetComponent<UILabel>().text = string.Empty;
		BaseLogic._panelTDM.SetActive(false);
		if ((bool)guiTextNew)
		{
			guiTextNew.GetComponent<GUITexture>().enabled = false;
		}
		GameObject gameObject = GameObject.Find("_BLACKBOX");
		if ((bool)gameObject)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		GameObject gameObject2 = GameObject.Find("_LOGO_BLACK");
		if ((bool)gameObject2)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		GameObject gameObject3 = GameObject.Find("_LOGO_BLACK(Clone)");
		if ((bool)gameObject3)
		{
			UnityEngine.Object.Destroy(gameObject3);
		}
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.playerName = BaseLogic.PLAYER_NAME;
		if (BaseLogic.PLAYER_SEX == 1)
		{
			PLAYER = PhotonNetwork.Instantiate("HeroMale", base.transform.position, base.transform.rotation, 0);
		}
		else
		{
			PLAYER = PhotonNetwork.Instantiate("HeroFemale", base.transform.position, base.transform.rotation, 0);
		}
		PLAYER.transform.parent = BaseLogic._player.transform;
		PLAYER.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		PLAYER.transform.localPosition = new Vector3(0f, -1.088882f, 0f);
		if ((BaseLogic.PC_ID == "c29ec9c61bae91b227e2876327003504c74bc20f" || BaseLogic.PC_ID == "393881d5acd10e1eee223ad221d7ce4e43072e25" || BaseLogic.PC_ID == "611f0565daa1d5cbff4313a60e6e3219a70c987d" || BaseLogic.PC_ID == "4cc3d19d3e0406add2870d6ff181414f151869a2" || BaseLogic.PC_ID == "7a8e4dc10848c448bc17727dd66ff60bfbebf368" || BaseLogic.PC_ID == "57b3fce6bdfbda61d9e5ea03271a943c852cff83" || BaseLogic.PC_ID == "6beaf6f71bbf7b3ce952035701de564d17307b08" || BaseLogic.PC_ID == "26bb716a3145325c990e5f9afed4790f3b845d83" || BaseLogic.PC_ID == "62599038177e7a4276dd27736e2a3d3acbafbe40" || BaseLogic.PC_ID == "2443423528737d96c186c6b589f28cd5d987a452" || BaseLogic.PC_ID == "ba9495440877dc0945f09fee785d69421497d583" || BaseLogic.PC_ID == "0dbd055b5697bb0f42bb3f1ecab1351eff1876d5") && (bool)PLAYER)
		{
			if ((bool)PLAYER.GetComponent<HeroBase>().hahaha)
			{
				AudioSource.PlayClipAtPoint(PLAYER.GetComponent<HeroBase>().hahaha, base.transform.position);
			}
			BaseLogic.SetBan(1);
			BaseLogic._player.SendMessage("SaveHero");
			if (PhotonNetwork.room != null)
			{
				PhotonNetwork.LeaveRoom();
			}
		}
		switch (BaseLogic.GAME_MODE = Convert.ToInt32(PhotonNetwork.room.customProperties["mode"]))
		{
		case 1:
		{
			GameObject gameObject5 = GameObject.Find("!!!FPS Network");
			gameObject5.GetComponent<CreateBasePrefabs>().SPAWNS_STANDART.SetActive(false);
			gameObject5.GetComponent<CreateBasePrefabs>().SPAWNS_DM.SetActive(true);
			PLAYER.GetComponent<HeroBase>().SIDE = BaseLogic.SideReal;
			PLAYER.GetComponent<PhotonView>().RPC("SetSide", PhotonTargets.OthersBuffered, BaseLogic.SideReal);
			PLAYER.SendMessage("SetRandomPosition");
			break;
		}
		case 2:
		case 4:
			BaseLogic.SetSide(1);
			PLAYER.GetComponent<HeroBase>().SIDE = BaseLogic.SideReal;
			PLAYER.GetComponent<PhotonView>().RPC("SetSide", PhotonTargets.OthersBuffered, BaseLogic.SideReal);
			PLAYER.SendMessage("SetRandomPosition");
			break;
		case 0:
		case 3:
		case 5:
		{
			BaseLogic.SetSide(UnityEngine.Random.Range(10, 1000));
			GameObject gameObject4 = GameObject.Find("!!!FPS Network");
			if ((bool)gameObject4 && (bool)gameObject4.GetComponent<CreateBasePrefabs>().SPAWNS_STANDART && (bool)gameObject4.GetComponent<CreateBasePrefabs>().SPAWNS_DM)
			{
				gameObject4.GetComponent<CreateBasePrefabs>().SPAWNS_STANDART.SetActive(true);
				gameObject4.GetComponent<CreateBasePrefabs>().SPAWNS_DM.SetActive(false);
			}
			LoadPositions();
			break;
		}
		case 6:
			PLAYER.GetComponent<HeroBase>().SIDE = BaseLogic.SideReal;
			PLAYER.GetComponent<PhotonView>().RPC("SetSide", PhotonTargets.OthersBuffered, BaseLogic.SideReal);
			PLAYER.SendMessage("SetRandomPosition");
			break;
		case 7:
			BaseLogic.SetSide(UnityEngine.Random.Range(10, 1000));
			PLAYER.SendMessage("SetRandomPosition");
			break;
		}
		MonoBehaviour.print ("GAME " + BaseLogic.GAME_MODE);
		GameObject gameObject6 = GameObject.Find("!Water Zone Trigger");
		if ((bool)gameObject6)
		{
			gameObject6.SendMessage("OutWater");
		}
		BaseLogic.CheckWeapons();
	}

	private void OnJoinedRoom()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("kills", 0);
		hashtable.Add("deaths", 0);
		hashtable.Add("id", BaseLogic.viewer_id);
		PhotonNetwork.player.SetCustomProperties(hashtable);

		Debug.Log("JOIN ROOM! mode: " + BaseLogic.GAME_MODE);
		if (!onJoined && (BaseLogic.GAME_MODE == 1 ||/* BaseLogic.GAME_MODE == 3 || */BaseLogic.GAME_MODE == 6))
		{
			BaseLogic._respawnTimerGui.GetComponent<UILabel>().text = "Выберите команду:\n\n";
			BaseLogic._control = false;
			BaseLogic._panelTDM.SetActive(true);
		}
		else
		{
			RespawnPlayer();
		}
		onJoined = true;
		GameObject gameObject = GameObject.Find("Sky Dome");
		if ((bool)gameObject)
		{
			gameObject.GetComponent<TOD_Sky>().Cycle.Hour = Convert.ToSingle(PhotonNetwork.room.customProperties["gametime"]);
		}
		else
		{
			Debug.Log("No dynamic time!");
		}
	}

	private void LoadPositions()
	{
		try
		{
			string text = Convert.ToString(BaseLogic.hero_status);
			string[] array = text.Split('|');
			float num;
			float num2;
			float num3;
			if (BaseLogic.GAME_MODE == 5)
			{
				num = Convert.ToSingle(array[17]);
				num2 = Convert.ToSingle(array[18]);
				num3 = Convert.ToSingle(array[19]);
			}
			else
			{
				num = Convert.ToSingle(array[4]);
				num2 = Convert.ToSingle(array[5]);
				num3 = Convert.ToSingle(array[6]);
			}
			if (num < 15f && num2 < 15f && num3 < 15f && num > -15f && num > -15f && num > -15f)
			{
				PLAYER.SendMessage("SetRandomPosition");
				return;
			}
			Debug.Log("Set Position: " + new Vector3(num, num2, num3).ToString());
			BaseLogic._player.transform.localPosition = new Vector3(num, num2, num3);
		}
		catch
		{
			PLAYER.SendMessage("SetRandomPosition");
		}
	}
}
