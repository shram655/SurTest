using UnityEngine;

public class BackPack : MonoBehaviour
{
	public bool isMyHero;

	public Renderer[] BACKPACK;

	private int oldBack;

	private void Awake()
	{
		HideAllBackpack();
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

	private void HideAllBackpack()
	{
		for (int i = 0; i < BACKPACK.Length; i++)
		{
			BACKPACK[i].enabled = false;
		}
	}

	private void ShowBackpack(int helm)
	{
		HideAllBackpack();
		if (helm >= 1)
		{
			BACKPACK[helm - 1].enabled = true;
		}
	}

	private void Update()
	{
		if (isMyHero && BaseLogic._backpack != oldBack)
		{
			ShowBackpack(BaseLogic._backpack);
			oldBack = BaseLogic._backpack;
		}
	}
}
