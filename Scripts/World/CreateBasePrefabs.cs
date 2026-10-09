using UnityEngine;

public class CreateBasePrefabs : MonoBehaviour
{
	public string PlayerPrefab = "!!!FPS Player Main";

	public string GuiPrefab = "!!!FPS Gui";

	public GameObject SPAWNS_STANDART;

	public GameObject SPAWNS_DM;

	public GameObject SPAWNS_ARENA;

	public GameObject[] SPAWNS_BASES;

	public bool noCreate;

	private void Awake()
	{
		if (!noCreate)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Sky Dome"));
			gameObject.name = "Sky Dome";
			GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("_BLACKBOX"));
			GameObject gameObject3 = (GameObject)Object.Instantiate(Resources.Load("_LOGO_BLACK"));
			gameObject3.transform.position = new Vector3(0.5f, 0.5f, 0f);
			gameObject3.name = "_LOGO_BLACK";
			gameObject2.name = "_BLACKBOX";
			Object.Instantiate(Resources.Load("Localization"));
			GameObject gameObject4 = (GameObject)Object.Instantiate(Resources.Load("AntiCeathPalatke"));
			gameObject4.transform.position = new Vector3(0f, -500f, 0f);
			GameObject gameObject5 = (GameObject)Object.Instantiate(Resources.Load(PlayerPrefab));
			gameObject5.name = "!!!FPS Player Main";
			gameObject5.transform.position = new Vector3(0f, 0f, 0f);
			GameObject gameObject6 = (GameObject)Object.Instantiate(Resources.Load(GuiPrefab));
			gameObject6.name = "!!!FPS Gui";
			gameObject6.transform.position = new Vector3(-6f, 0f, 0f);
		}
	}
}
