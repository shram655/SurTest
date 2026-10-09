using System.Linq;
using UnityEngine;

public class AdminMenu : MonoBehaviour
{
	public GameObject crooshair;

	public GameObject PanelBase;

	public GameObject PanelAdmin;

	public GameObject PanelTab;

	public GameObject ButtonAdmin;

	public GameObject LabelAdmin;

	public GameObject LabelRecon;

	public GameObject ButtonRespawn;

	public UILabel[] TabMenuSide1_UI;

	public UILabel[] TabMenuSide2_UI;

	public GameObject[] AdminButtons;

	private float updateTimer;

	private bool isTabPush;

	private void Start()
	{
		if (BaseLogic.ADMIN != 89)
		{
			Object.Destroy(ButtonAdmin);
			Object.Destroy(LabelAdmin);
		}
	}

	private void ViewMenus()
	{
		PanelTab.SetActive(true);
		for (int i = 0; i < TabMenuSide1_UI.Length; i++)
		{
			TabMenuSide1_UI[i].text = string.Empty;
		}
		for (int j = 0; j < TabMenuSide2_UI.Length; j++)
		{
			TabMenuSide2_UI[j].text = string.Empty;
		}
		HeroBase[] array = Object.FindObjectsOfType(typeof(HeroBase)) as HeroBase[];
		for (int l = 0; l < array.Length; l++)
		{
			if (!array[l].BOT && !array[l].isMyHero)
			{
				array[l].SCHET = (int)array[l].photonView.owner.customProperties["kills"] * 3 - (int)array[l].photonView.owner.customProperties["deaths"] * 2;
			}
		}
		if (array.Length == 0)
		{
			return;
		}
		IOrderedEnumerable<HeroBase> orderedEnumerable = array.OrderByDescending((HeroBase k) => k.SCHET);
		int num = 0;
		foreach (HeroBase item in orderedEnumerable)
		{
			if (item.BOT || item.isMyHero)
			{
				continue;
			}
			if (BaseLogic.GAME_MODE == 1 || BaseLogic.GAME_MODE == 6)
			{
				if (item.SIDE == 1)
				{
					UILabel uILabel = TabMenuSide1_UI[0];
					string text = uILabel.text;
					uILabel.text = text + string.Empty + (num + 1) + "\n";
					UILabel uILabel2 = TabMenuSide1_UI[1];
					uILabel2.text = uILabel2.text + item.SIDE + "\n";
					UILabel uILabel3 = TabMenuSide1_UI[2];
					uILabel3.text = uILabel3.text + string.Empty + item.photonView.owner.name + "\n";
					UILabel uILabel4 = TabMenuSide1_UI[3];
					uILabel4.text = uILabel4.text + item.SCHET + "\n";
					UILabel uILabel5 = TabMenuSide1_UI[4];
					uILabel5.text = uILabel5.text + (int)item.photonView.owner.customProperties["kills"] + "\n";
					UILabel uILabel6 = TabMenuSide1_UI[5];
					uILabel6.text = uILabel6.text + (int)item.photonView.owner.customProperties["deaths"] + "\n";
				}
				else if (item.SIDE == 2)
				{
					UILabel uILabel7 = TabMenuSide2_UI[0];
					string text2 = uILabel7.text;
					uILabel7.text = text2 + string.Empty + (num + 1) + "\n";
					UILabel uILabel8 = TabMenuSide2_UI[1];
					uILabel8.text = uILabel8.text + item.SIDE + "\n";
					UILabel uILabel9 = TabMenuSide2_UI[2];
					uILabel9.text = uILabel9.text + string.Empty + item.photonView.owner.name + "\n";
					UILabel uILabel10 = TabMenuSide2_UI[3];
					uILabel10.text = uILabel10.text + item.SCHET + "\n";
					UILabel uILabel11 = TabMenuSide2_UI[4];
					uILabel11.text = uILabel11.text + (int)item.photonView.owner.customProperties["kills"] + "\n";
					UILabel uILabel12 = TabMenuSide2_UI[5];
					uILabel12.text = uILabel12.text + (int)item.photonView.owner.customProperties["deaths"] + "\n";
				}
			}
			else if (num <= 18)
			{
				UILabel uILabel13 = TabMenuSide1_UI[0];
				string text3 = uILabel13.text;
				uILabel13.text = text3 + string.Empty + (num + 1) + "\n";
				TabMenuSide1_UI[1].text += "-\n";
				UILabel uILabel14 = TabMenuSide1_UI[2];
				uILabel14.text = uILabel14.text + string.Empty + item.photonView.owner.name + "\n";
				UILabel uILabel15 = TabMenuSide1_UI[3];
				uILabel15.text = uILabel15.text + item.SCHET + "\n";
				UILabel uILabel16 = TabMenuSide1_UI[4];
				uILabel16.text = uILabel16.text + (int)item.photonView.owner.customProperties["kills"] + "\n";
				UILabel uILabel17 = TabMenuSide1_UI[5];
				uILabel17.text = uILabel17.text + (int)item.photonView.owner.customProperties["deaths"] + "\n";
			}
			else
			{
				UILabel uILabel18 = TabMenuSide2_UI[0];
				string text4 = uILabel18.text;
				uILabel18.text = text4 + string.Empty + (num + 1) + "\n";
				TabMenuSide2_UI[1].text += "-\n";
				UILabel uILabel19 = TabMenuSide2_UI[2];
				uILabel19.text = uILabel19.text + string.Empty + item.photonView.owner.name + "\n";
				UILabel uILabel20 = TabMenuSide2_UI[3];
				uILabel20.text = uILabel20.text + item.SCHET + "\n";
				UILabel uILabel21 = TabMenuSide2_UI[4];
				uILabel21.text = uILabel21.text + (int)item.photonView.owner.customProperties["kills"] + "\n";
				UILabel uILabel22 = TabMenuSide2_UI[5];
				uILabel22.text = uILabel22.text + (int)item.photonView.owner.customProperties["deaths"] + "\n";
			}
			num++;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.X) && BaseLogic.ADMIN == 89)
		{
			BaseLogic._player.GetComponent<AdminNoclip>().Switch();
		}
		if (!Network.PLAYER)
		{
			return;
		}
		if (Input.GetKey(KeyCode.Tab))
		{
			if (!isTabPush)
			{
				ViewMenus();
				isTabPush = true;
			}
		}
		else if (isTabPush)
		{
			PanelTab.SetActive(false);
			isTabPush = false;
		}
	}

	private void ShowAdminPanel()
	{
		PanelBase.SetActive(false);
		PanelAdmin.SetActive(true);
		for (int i = 0; i < AdminButtons.Length - 1; i++)
		{
			AdminButtons[i].SetActive(true);
		}
	}

	private void ShowReconPanel()
	{
		PanelBase.SetActive(false);
		PanelAdmin.SetActive(true);
		if (BaseLogic.ADMIN != 89)
		{
			for (int i = 0; i < AdminButtons.Length - 1; i++)
			{
				AdminButtons[i].SetActive(false);
			}
		}
	}

	private void ShowBasePanel()
	{
		PanelBase.SetActive(true);
		PanelAdmin.SetActive(false);
	}
}
