using UnityEngine;

public class Helmet : MonoBehaviour
{
	public bool isMyHero;

	public Renderer[] HELMET;

	private int oldHelmet;

	private void Awake()
	{
		HideAllHelmets();
	}

	private void Start()
	{
		if (isMyHero)
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			Renderer[] array2 = array;
			foreach (Renderer renderer in array2)
			{
				renderer.gameObject.layer = 5;
			}
		}
	}

	private void HideAllHelmets()
	{
		for (int i = 0; i < HELMET.Length; i++)
		{
			HELMET[i].enabled = false;
		}
	}

	private void ShowHelmet(int helm)
	{
		HideAllHelmets();
		if (helm >= 1)
		{
			HELMET[helm - 1].enabled = true;
		}
	}

	private void Update()
	{
		if (isMyHero && BaseLogic._helmet != oldHelmet)
		{
			ShowHelmet(BaseLogic._helmet);
			oldHelmet = BaseLogic._helmet;
		}
	}
}
