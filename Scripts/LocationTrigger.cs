using System.Collections;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
	public int ExitID;

	public int location_id;

	public WWW download;

	public bool _startGUI;

	public bool _vkVersion;

	private float enterTimer;

	private void Start()
	{
		base.gameObject.layer = 2;
	}

	private void Update()
	{
		if (_startGUI && Time.time > enterTimer + 1f)
		{
			_startGUI = false;
//			BaseLogic.LOCATION_ID = location_id;
//			BaseLogic.idExitLocation = ExitID;
			UnityEngine.Debug.Log("Load Location!");
			StartCoroutine("LoadLocation");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!_startGUI && other.tag == "Player"/* && BaseLogic.LOCATION_ID != location_id && !BaseLogic.SURVIVAL_DM*/)
		{
			UnityEngine.Debug.Log("OnTriggerEnter exitLocation");
			if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE)
			{
				if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>())
				{
					if (Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>()._passenger)
					{
						//BaseLogic._passengerVehicle = Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name;
					}
				}
				else if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>() && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>()._passenger)
				{
					//BaseLogic._passengerVehicle = Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name;
				}
			}
			_startGUI = true;
			enterTimer = Time.time;
			BaseLogic._player.SendMessage("FadeIn", 9999);
		}
		else
		{
			if (!other.GetComponent<DamageZone>())
			{
				return;
			}
			if ((bool)other.GetComponent<DamageZone>().BaseVehicle.GetComponent<CarSystem>())
			{
				if (other.GetComponent<DamageZone>().BaseVehicle.GetComponent<CarSystem>()._control || other.GetComponent<DamageZone>().BaseVehicle.GetComponent<CarSystem>()._passenger)
				{
					_startGUI = true;
					enterTimer = Time.time;
					BaseLogic._player.SendMessage("FadeIn", 9999);
				}
			}
			else if ((bool)other.GetComponent<DamageZone>().BaseVehicle.GetComponent<HeliSystem>() && (other.GetComponent<DamageZone>().BaseVehicle.GetComponent<HeliSystem>()._control || other.GetComponent<DamageZone>().BaseVehicle.GetComponent<HeliSystem>()._passenger))
			{
				_startGUI = true;
				enterTimer = Time.time;
				BaseLogic._player.SendMessage("FadeIn", 9999);
			}
		}
	}

	private IEnumerator LoadLocation()
	{
		_startGUI = false;
		if ((bool)Network.PLAYER && (bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE)
		{
			if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>() && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<CarSystem>()._control)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name);
			}
			else if ((bool)Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>() && Network.PLAYER.GetComponent<HeroBase>().VEHICLE.GetComponent<HeliSystem>()._control)
			{
				Network.PLAYER.GetComponent<PhotonView>().RPC("DestroyObject", PhotonTargets.Others, Network.PLAYER.GetComponent<HeroBase>().VEHICLE.name);
			}
		}
		PhotonNetwork.Disconnect();
		BaseLogic._player.GetComponent<Network>().enabled = false;
		yield return new WaitForSeconds(1f);
		BaseLogic.INWATER = false;
		//BaseLogic.SaveBulletsLeft();
		GameObject BaseGUI = GameObject.Find("!!!FPS Gui");
		BaseGUI.SetActive(value: false);
		if (_vkVersion)
		{
			if (location_id == 1)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("!!FPS Base");
			}
			else if (location_id == 2)
			{
				StartCoroutine("LoadlevelZ", 2);
			}
			else if (location_id == 101)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("!!FPS Bunker");
			}
		}
		else if (location_id == 1)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_1");
		}
		else if (location_id == 2)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_2");
		}
		else if (location_id == 3)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_3");
		}
		else if (location_id == 4)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_4");
		}
		else if (location_id == 5)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_5");
		}
		else if (location_id == 6)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_6");
		}
		else if (location_id == 7)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_7");
		}
		else if (location_id == 8)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_8");
		}
		else if (location_id == 9)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_9");
		}
		else if (location_id == 10)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_10");
		}
		else if (location_id == 12)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_12");
		}
		else if (location_id == 101)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_101");
		}
		else if (location_id == 102)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_102");
		}
		else if (location_id >= 200)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("high_200");
		}
	}
}
