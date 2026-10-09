using System;
using UnityEngine;

public class MainMenuNetwork : MonoBehaviour
{
	private float connectTimer;

	public string roomName;

	public UIAtlas _atlas;

	public Font _font;

	public GameObject _parent;

	private GameObject[] _goButton;

	private int roomID;

	private int _rooms;

	private int _roomScroll;

	public int timer;

	private int roomID2;

	public GameObject _selectMapElement;

	public GameObject _selectModeElement;

	private void Awake()
	{
		_goButton = new GameObject[0];
		PhotonNetwork.autoJoinLobby = true;
	}

	private void Start()
	{
		if (!PhotonNetwork.connected && PhotonNetwork.connectionState != ConnectionState.Connecting)
		{
			PhotonNetwork.ConnectUsingSettings(BaseLogic.PhotonVersion);
		}
	}

	private void ClearServerList()
	{
		for (int i = 0; i < _goButton.Length; i++)
		{
			UnityEngine.Object.DestroyObject(_goButton[i]);
		}
	}

	private void UpdateServerList()
	{
		roomID2 = 0;
		roomID = 0;
		GameObject parent = _parent;
		if (PhotonNetwork.GetRoomList().Length == 0)
		{
			return;
		}
		for (int i = 0; i < _goButton.Length; i++)
		{
			UnityEngine.Object.DestroyObject(_goButton[i]);
		}
		_goButton = new GameObject[PhotonNetwork.GetRoomList().Length];
		int depth = NGUITools.CalculateNextDepth(parent);
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		Array.Sort(roomList, (RoomInfo r1, RoomInfo r2) => -r1.playerCount.CompareTo(r2.playerCount));
		RoomInfo[] array = roomList;
		RoomInfo[] array2 = array;
		foreach (RoomInfo roomInfo in array2)
		{
			int num = Convert.ToInt32(roomInfo.customProperties["mode"]);
			if (roomID2 <= 17 && roomInfo.playerCount < roomInfo.maxPlayers - 3 && num == BaseLogic.GAME_MODE)
			{
				roomID2++;
				parent = NGUITools.AddChild(parent);
				parent.name = "MyButton";
				_goButton[roomID] = parent;
				UISprite uISprite = NGUITools.AddWidget<UISprite>(parent);
				uISprite.name = "Background";
				uISprite.depth = depth;
				uISprite.atlas = _atlas;
				uISprite.spriteName = "box2";
				uISprite.transform.localScale = new Vector3(1f, 1f, 0f);
				uISprite.width = 250;
				uISprite.height = 20;
				_goButton[roomID].AddComponent<UIButton>();
				_goButton[roomID].GetComponent<UIButton>().tweenTarget = uISprite.gameObject;
				_goButton[roomID].GetComponent<UIButton>().enabled = true;
				_goButton[roomID].GetComponent<UIButton>().hover = new Color(0.5882353f, 32f / 51f, 28f / 51f, 0.5f);
				NGUITools.AddWidgetCollider(parent);
				_goButton[roomID].AddComponent<MainMenuButton>();
				_goButton[roomID].GetComponent<MainMenuButton>().ID_BUTTON = 500;
				_goButton[roomID].GetComponent<MainMenuButton>().roomName = string.Empty + roomInfo.name;
				EventDelegate.Add(_goButton[roomID].GetComponent<UIButton>().onClick, _goButton[roomID].GetComponent<MainMenuButton>().ClickButton);
				_goButton[roomID].GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
				_goButton[roomID].transform.localPosition = new Vector3(-67f, 101f - (float)(roomID * 15), 0f);
				_goButton[roomID].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
				UILabel uILabel = NGUITools.AddWidget<UILabel>(parent);
				uILabel.trueTypeFont = _font;
				uILabel.text = string.Format(Localization.Get("Games_name"), roomID + 1, roomInfo.name, roomInfo.playerCount, roomInfo.maxPlayers);
				uILabel.MakePixelPerfect();
				uILabel.transform.localScale = new Vector3(1f, 1f, 0f);
				uILabel.transform.localPosition = new Vector3(0f, 0f, 0f);
				uILabel.width = 250;
				uILabel.height = 20;
				parent = _parent;
				uISprite.color = new Color(0f, 0f, 0f, 0f);
				roomID++;
			}
		}
	}
}
