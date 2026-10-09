using UnityEngine;

public class AnimalCreate : MonoBehaviour
{
	public int animalType;

	public bool Moved;

	private void Awake()
	{
		if (base.transform.parent == null)
		{
			GameObject gameObject = GameObject.Find("_BASEPARENTSYNC");
			base.transform.parent = gameObject.transform;
		}
		if (Moved)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (animalType == 5)
		{
			GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("_ANIMAL_VAMPIRE"));
			if ((bool)base.transform.parent)
			{
				gameObject2.transform.parent = base.transform.parent.transform;
			}
			gameObject2.name = string.Empty + base.name;
			gameObject2.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.1f, base.transform.localPosition.z);
			gameObject2.transform.rotation = base.transform.rotation;
			if (Moved)
			{
				gameObject2.GetComponent<AnimalAI>().Moved = true;
			}
		}
		else if (animalType == 6)
		{
			GameObject gameObject3 = (GameObject)Object.Instantiate(Resources.Load("_ANIMAL_GHOUL"));
			if ((bool)base.transform.parent)
			{
				gameObject3.transform.parent = base.transform.parent.transform;
			}
			gameObject3.name = string.Empty + base.name;
			gameObject3.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.1f, base.transform.localPosition.z);
			gameObject3.transform.rotation = base.transform.rotation;
			if (Moved)
			{
				gameObject3.GetComponent<AnimalAI>().Moved = true;
			}
		}
		else if (animalType == 1)
		{
			GameObject gameObject4 = (GameObject)Object.Instantiate(Resources.Load("_ANIMAL_WOLF"));
			if ((bool)base.transform.parent)
			{
				gameObject4.transform.parent = base.transform.parent.transform;
			}
			gameObject4.name = string.Empty + base.name;
			gameObject4.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.1f, base.transform.localPosition.z);
			gameObject4.transform.rotation = base.transform.rotation;
			if (Moved)
			{
				gameObject4.GetComponent<AnimalAI>().Moved = true;
			}
			if (BaseLogic.GAME_MODE == 2 && base.name == "_ANIMAL_WOLF_03")
			{
				gameObject4.AddComponent<MissionObject>().QUEST_ID = 6;
			}
		}
		else if (animalType == 2)
		{
			GameObject gameObject5 = (GameObject)Object.Instantiate(Resources.Load("_ANIMAL_BOAR"));
			if ((bool)base.transform.parent)
			{
				gameObject5.transform.parent = base.transform.parent.transform;
			}
			gameObject5.name = string.Empty + base.name;
			gameObject5.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.1f, base.transform.localPosition.z);
			gameObject5.transform.rotation = base.transform.rotation;
			if (Moved)
			{
				gameObject5.GetComponent<AnimalAI>().Moved = true;
			}
		}
		else if (animalType == 3)
		{
			GameObject gameObject6 = (GameObject)Object.Instantiate(Resources.Load("_ANIMAL_BEAR"));
			if ((bool)base.transform.parent)
			{
				gameObject6.transform.parent = base.transform.parent.transform;
			}
			gameObject6.name = string.Empty + base.name;
			gameObject6.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.1f, base.transform.localPosition.z);
			gameObject6.transform.rotation = base.transform.rotation;
			if (Moved)
			{
				gameObject6.GetComponent<AnimalAI>().Moved = true;
			}
			if (BaseLogic.GAME_MODE == 2)
			{
				gameObject6.AddComponent<MissionObject>().QUEST_ID = 16;
			}
		}
		Object.Destroy(base.gameObject);
	}
}
