using UnityEngine;

public class CreateVehiclePrefab : MonoBehaviour
{
	public Transform[] VehsPos;

	private void Awake()
	{
		GameObject standartPrefab = (GameObject)Object.Instantiate(Resources.Load("_VEHICLES_ALL"));
		if (BaseLogic.GAME_MODE == 2)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_JEEP"), VehsPos[0].position, VehsPos[0].rotation);
			GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_SPORT"), VehsPos[1].position, VehsPos[1].rotation);
			gameObject.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject2.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject.name = VehsPos[0].name + "_VEH";
			gameObject2.name = VehsPos[1].name + "_VEH";
			GameObject gameObject3 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_NIVA"), VehsPos[2].position, VehsPos[2].rotation);
			gameObject3.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject3.name = VehsPos[2].name + "_VEH";
			GameObject gameObject4 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_URAL"), VehsPos[3].position, VehsPos[3].rotation);
			gameObject4.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject4.name = VehsPos[3].name + "_VEH";
			GameObject gameObject5 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_MI8"), VehsPos[4].position, VehsPos[4].rotation);
			gameObject5.GetComponent<HeliSystem>().StandartPrefab = standartPrefab;
			gameObject5.name = VehsPos[4].name + "_VEH";
			GameObject gameObject6 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_MI8"), VehsPos[5].position, VehsPos[5].rotation);
			gameObject6.GetComponent<HeliSystem>().StandartPrefab = standartPrefab;
			gameObject6.name = VehsPos[5].name + "_VEH";
			GameObject gameObject7 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_MI8"), VehsPos[6].position, VehsPos[6].rotation);
			gameObject7.GetComponent<HeliSystem>().StandartPrefab = standartPrefab;
			gameObject7.name = VehsPos[6].name + "_VEH";
			GameObject obj = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_MI8"), VehsPos[7].position, VehsPos[7].rotation);
			obj.GetComponent<HeliSystem>().StandartPrefab = standartPrefab;
			obj.name = VehsPos[7].name + "_VEH";
			gameObject.AddComponent<MissionObject>();
			gameObject.GetComponent<MissionObject>().QUEST_ID = 11;
			gameObject2.AddComponent<MissionObject>();
			gameObject2.GetComponent<MissionObject>().QUEST_ID = 11;
			gameObject3.AddComponent<MissionObject>();
			gameObject3.GetComponent<MissionObject>().QUEST_ID = 11;
			gameObject4.AddComponent<MissionObject>();
			gameObject4.GetComponent<MissionObject>().QUEST_ID = 11;
			gameObject5.AddComponent<MissionObject>();
			gameObject5.GetComponent<MissionObject>().QUEST_ID = 27;
			gameObject6.AddComponent<MissionObject>();
			gameObject6.GetComponent<MissionObject>().QUEST_ID = 27;
			gameObject7.AddComponent<MissionObject>();
			gameObject7.GetComponent<MissionObject>().QUEST_ID = 27;
			obj.AddComponent<MissionObject>();
			obj.GetComponent<MissionObject>().QUEST_ID = 27;
		}
		else if (BaseLogic.GAME_MODE == 5)
		{
			GameObject gameObject8 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_SPORT"), VehsPos[0].position, VehsPos[0].rotation);
			GameObject obj2 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_ARMBUS"), VehsPos[1].position, VehsPos[1].rotation);
			gameObject8.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			obj2.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject8.name = VehsPos[0].name + "_VEH";
			obj2.name = VehsPos[1].name + "_VEH";
			GameObject obj3 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_MI8_HAKI"), VehsPos[2].position, VehsPos[2].rotation);
			obj3.GetComponent<HeliSystem>().StandartPrefab = standartPrefab;
			obj3.name = VehsPos[2].name + "_VEH";
			GameObject gameObject9 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_HUMVEE"), VehsPos[3].position, VehsPos[3].rotation);
			GameObject gameObject10 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_SEDAN"), VehsPos[4].position, VehsPos[4].rotation);
			GameObject obj4 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_SEDAN"), VehsPos[5].position, VehsPos[5].rotation);
			gameObject9.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject10.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			obj4.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject9.name = VehsPos[3].name + "_VEH";
			gameObject10.name = VehsPos[4].name + "_VEH";
			obj4.name = VehsPos[5].name + "_VEH";
			GameObject obj5 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_SPORT"), VehsPos[6].position, VehsPos[6].rotation);
			obj5.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			obj5.name = VehsPos[6].name + "_VEH";
		}
		else if (BaseLogic.BT2)
		{
			GameObject gameObject11 = null;
			for (int i = 0; i < 10; i++)
			{
				if (i == 0)
				{
					gameObject11 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_JEEP"), VehsPos[i].position, VehsPos[i].rotation);
				}
				gameObject11.name = VehsPos[i].name + "_VEH";
			}
		}
		else
		{
			GameObject gameObject12 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_JEEP"), VehsPos[0].position, VehsPos[0].rotation);
			GameObject obj6 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_HUMVEE"), VehsPos[1].position, VehsPos[1].rotation);
			gameObject12.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			obj6.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject12.name = VehsPos[0].name + "_VEH";
			obj6.name = VehsPos[1].name + "_VEH";
			GameObject obj7 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_MI8"), VehsPos[2].position, VehsPos[2].rotation);
			obj7.GetComponent<HeliSystem>().StandartPrefab = standartPrefab;
			obj7.name = VehsPos[2].name + "_VEH";
			GameObject gameObject13 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_ARMBUS"), VehsPos[3].position, VehsPos[3].rotation);
			GameObject gameObject14 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_SEDAN"), VehsPos[4].position, VehsPos[4].rotation);
			GameObject obj8 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_NIVA"), VehsPos[5].position, VehsPos[5].rotation);
			gameObject13.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject14.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			obj8.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			gameObject13.name = VehsPos[3].name + "_VEH";
			gameObject14.name = VehsPos[4].name + "_VEH";
			obj8.name = VehsPos[5].name + "_VEH";
			GameObject obj9 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_URAL"), VehsPos[6].position, VehsPos[6].rotation);
			obj9.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			obj9.name = VehsPos[6].name + "_VEH";
			GameObject obj10 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_SPORT"), VehsPos[7].position, VehsPos[7].rotation);
			obj10.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			obj10.name = VehsPos[7].name + "_VEH";
			GameObject obj11 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_MI8_HAKI"), VehsPos[8].position, VehsPos[8].rotation);
			obj11.GetComponent<HeliSystem>().StandartPrefab = standartPrefab;
			obj11.name = VehsPos[8].name + "_VEH";
			GameObject obj12 = (GameObject)Object.Instantiate(Resources.Load("_VEHICLE_BTR"), VehsPos[9].position, VehsPos[9].rotation);
			obj12.GetComponent<CarSystem>().StandartPrefab = standartPrefab;
			obj12.name = VehsPos[9].name + "_VEH";
		}
	}

	private void Update()
	{
	}
}
