using UnityEngine;

public class MissionObject : MonoBehaviour
{
	private GameObject missionLogic;

	public int QUEST_ID;

	private void Start()
	{
		missionLogic = GameObject.Find("_missionLogic");
	}

	private void Update()
	{
	}

	private void DestroyObject()
	{
		if (QUEST_ID == missionLogic.GetComponent<MissionScenario>().ID_QUEST)
		{
			missionLogic.SendMessage("CompleteQuest");
		}
	}

	private void PickUpItem()
	{
		if (QUEST_ID == missionLogic.GetComponent<MissionScenario>().ID_QUEST && (!GetComponent<WeaponPickup>() || BaseLogic._playerWeapon.GetComponent<PlayerWeapons>().totalWeapons < 2))
		{
			missionLogic.SendMessage("CompleteQuest");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (QUEST_ID == missionLogic.GetComponent<MissionScenario>().ID_QUEST && (other.tag == "Player" || ((bool)other.GetComponent<CarSystem>() && other.GetComponent<CarSystem>()._control) || ((bool)other.GetComponent<CarSystem>() && other.GetComponent<CarSystem>()._passenger) || ((bool)other.GetComponent<HeliSystem>() && other.GetComponent<HeliSystem>()._control) || ((bool)other.GetComponent<HeliSystem>() && other.GetComponent<HeliSystem>()._passenger) || ((bool)other.GetComponent<TrackSystem>() && other.GetComponent<TrackSystem>()._passenger) || ((bool)other.GetComponent<TrackSystem>() && other.GetComponent<TrackSystem>()._GunControl) || ((bool)other.GetComponent<TrackSystem>() && other.GetComponent<TrackSystem>()._control)))
		{
			Debug.Log("Mission Complete, OnTriggerEnter");
			missionLogic.SendMessage("CompleteQuest");
		}
	}
}
