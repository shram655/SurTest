using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;

public class MissionPoint : MonoBehaviour
{
	private float updateTimer;

	private int soldiers_A;

	private int soldiers_B;

	private Vector3 diff = Vector3.zero;

	private float dist;

	private float zahvatTimer;

	private int zahvatSide;

	private bool isZahvachena;

	private bool _endGame;

	public new GUITexture guiTexture;

	private float colorGUI = 0.2f;

	private bool Zahvacheno;

	private void Start()
	{
	}

	private void Update()
	{
		if (!(Time.time > updateTimer + 5f + Random.Range(-1f, 2f)) || _endGame)
		{
			return;
		}
		if (!_endGame)
		{
			soldiers_A = 0;
			soldiers_B = 0;
			HeroBase[] array = Object.FindObjectsOfType(typeof(HeroBase)) as HeroBase[];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].BOT || array[i]._syncDead)
				{
					continue;
				}
				diff = base.transform.position - array[i].transform.position;
				dist = diff.sqrMagnitude;
				if (dist < 1200f)
				{
					if (array[i].SIDE == 1)
					{
						soldiers_A++;
					}
					else if (array[i].SIDE == 2)
					{
						soldiers_B++;
					}
				}
			}
			if (soldiers_A > soldiers_B && !Zahvacheno)
			{
				if (BaseLogic.SideReal == 1)
				{
					guiTexture.color = new Color(Color.green.r, Color.green.g, Color.green.b, colorGUI);
				}
				else if (BaseLogic.SideReal == 2)
				{
					guiTexture.color = new Color(Color.red.r, Color.red.g, Color.red.b, colorGUI);
				}
				zahvatSide = 1;
				colorGUI += 0.01f;
				zahvatTimer += 1f;
				if (zahvatTimer > 10f)
				{
					Zahvacheno = true;
					if (BaseLogic.SideReal == 1)
					{
						colorGUI = 0.1f;
						guiTexture.color = new Color(Color.green.r, Color.green.g, Color.green.b, colorGUI);
					}
					else if (BaseLogic.SideReal == 2)
					{
						colorGUI = 0.1f;
						guiTexture.color = new Color(Color.red.r, Color.red.g, Color.red.b, colorGUI);
					}
					if (PhotonNetwork.isMasterClient)
					{
						ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
						hashtable.Add("side2", (int)PhotonNetwork.room.customProperties["side2"] - 1);
						PhotonNetwork.room.SetCustomProperties(hashtable);
					}
				}
			}
			else if (soldiers_B > soldiers_A && !Zahvacheno)
			{
				if (BaseLogic.SideReal == 1)
				{
					guiTexture.color = new Color(Color.red.r, Color.red.g, Color.red.b, colorGUI);
				}
				else if (BaseLogic.SideReal == 2)
				{
					guiTexture.color = new Color(Color.green.r, Color.green.g, Color.green.b, colorGUI);
				}
				zahvatSide = 2;
				colorGUI += 0.01f;
				zahvatTimer += 1f;
				if (zahvatTimer > 10f)
				{
					Zahvacheno = true;
					if (BaseLogic.SideReal == 1)
					{
						colorGUI = 0.1f;
						guiTexture.color = new Color(Color.red.r, Color.red.g, Color.red.b, colorGUI);
					}
					else if (BaseLogic.SideReal == 2)
					{
						colorGUI = 0.1f;
						guiTexture.color = new Color(Color.green.r, Color.green.g, Color.green.b, colorGUI);
					}
					if (PhotonNetwork.isMasterClient)
					{
						ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
						hashtable2.Add("side1", (int)PhotonNetwork.room.customProperties["side1"] - 1);
						PhotonNetwork.room.SetCustomProperties(hashtable2);
					}
				}
			}
			else if ((soldiers_A == 0 && soldiers_B == 0) || soldiers_A == soldiers_B)
			{
				if (zahvatTimer > 0f)
				{
					colorGUI -= 0.005f;
					guiTexture.color = new Color(Color.grey.r, Color.grey.g, Color.grey.b, colorGUI);
					zahvatTimer -= 1f;
				}
				else
				{
					colorGUI = 0.1f;
					Zahvacheno = false;
					zahvatSide = 0;
					guiTexture.color = new Color(Color.grey.r, Color.grey.g, Color.grey.b, 0.1f);
				}
			}
		}
		if (PhotonNetwork.room != null && PhotonNetwork.room.customProperties.ContainsKey("side1"))
		{
			int num = (int)PhotonNetwork.room.customProperties["side1"];
			int num2 = (int)PhotonNetwork.room.customProperties["side2"];
			if ((bool)Network.PLAYER && (bool)BaseLogic._chatLabel)
			{
				BaseLogic._chatLabel.GetComponent<UILabel>().text = "Подкрепления\nКоманда А: " + num + "\n[FF8888]Команда B: " + num2;
			}
			if (num <= 0 || num2 <= 0)
			{
				StartCoroutine("EndGame");
			}
		}
		updateTimer = Time.time;
	}

	private IEnumerator EndGame()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.room.open = false;
			PhotonNetwork.room.visible = false;
		}
		_endGame = true;
		BaseLogic._control = false;
		BaseLogic._player.SendMessage("DisableKeys", true);
		GameObject _gui = GameObject.Find("!!!FPS Gui");
		_gui.SendMessage("ViewMenus");
		yield return new WaitForSeconds(15f);
		PhotonNetwork.LeaveRoom();
		Application.LoadLevel(0);
	}
}
