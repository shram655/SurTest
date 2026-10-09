using UnityEngine;

public class Glasses : MonoBehaviour
{
	public bool isMyHero;

	public Renderer[] HELMET;

	private int oldHelmet;

	public Light _flash;

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
		if (!isMyHero)
		{
			if (helm == 2)
			{
				_flash.enabled = true;
			}
			else
			{
				_flash.enabled = false;
			}
		}
	}

	private void Update()
	{
		if (isMyHero && BaseLogic._glasses != oldHelmet)
		{
			ShowHelmet(BaseLogic._glasses);
			oldHelmet = BaseLogic._glasses;
		}
	}
}
