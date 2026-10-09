using System.Collections;
using UnityEngine;

public class MissionScenario : MonoBehaviour
{
	public int ID_MISSION;

	public int ID_QUEST;

	public GameObject[] QUEST_TARGET;

	public GameObject[] QUEST_TARGET2;

	public GameObject HINT_LABEL;

	private bool hintShow;

	public bool showChatHint;

	public TargetGUI targetGUI;

	public TargetGUI targetGUI2;

	private void Start()
	{
		HINT_LABEL = GameObject.Find("HINT_LABEL");
		if ((bool)HINT_LABEL)
		{
			HINT_LABEL.SetActive(false);
		}
		StartMission();
	}

	private void CheckMission()
	{
	}

	private void StartMission()
	{
		if (ID_MISSION == -1)
		{
			targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
			targetGUI.target = QUEST_TARGET[0];
		}
		else if (ID_MISSION == 0)
		{
			if (BaseLogic.SideReal == 1)
			{
				targetGUI.TARGET_TEXT = "СБОР";
				targetGUI.target = QUEST_TARGET[0];
			}
			else
			{
				targetGUI.TARGET_TEXT = "СБОР";
				targetGUI.target = QUEST_TARGET2[0];
			}
		}
		else if (ID_MISSION == 1)
		{
			targetGUI.TARGET_TEXT = "POINT А";
			targetGUI.target = QUEST_TARGET[0];
			targetGUI2.TARGET_TEXT = "POINT B";
			targetGUI2.target = QUEST_TARGET[1];
		}
		else
		{
			targetGUI2.TARGET_TEXT = "POINT B";
			targetGUI2.target = QUEST_TARGET[0];
		}
	}

	private void Update()
	{
		if (hintShow)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				StartCoroutine("noFireOnMouseClick");
				BaseLogic._player.GetComponent<FPSPlayer>().DisableKeys(false);
				HINT_LABEL.SetActive(false);
				hintShow = false;
			}
			if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
			{
				BaseLogic._player.GetComponent<FPSPlayer>().DisableKeys(false);
				HINT_LABEL.SetActive(false);
				hintShow = false;
				BaseLogic._control = true;
			}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				HINT_LABEL.SetActive(false);
				hintShow = false;
				BaseLogic._control = true;
			}
		}
	}

	private IEnumerator noFireOnMouseClick()
	{
		yield return new WaitForSeconds(0.3f);
		BaseLogic._control = true;
	}

	private void ShowHint(string texz)
	{
		BaseLogic._player.SendMessage("HideInventory");
		BaseLogic._control = false;
		BaseLogic._player.GetComponent<FPSPlayer>().DisableKeys(true);
		hintShow = true;
		HINT_LABEL.SetActive(true);
		HINT_LABEL.GetComponent<UILabel>().text = texz;
	}

	private void CompleteQuest()
	{
		ID_QUEST++;
		if (ID_MISSION == -1)
		{
			if (ID_QUEST == 1)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nУправление персонажем производится с помощью клавиш WASD (движение), SPACE - прыжок, CTRL - сесть. Быстрый бег - клавиша SHIFT.\n\nЧтобы продолжить, нажмите левую кнопку мыши.");
			}
			if (ID_QUEST == 2)
			{
				targetGUI.TARGET_TEXT = "ВЗЯТЬ ПРЕДМЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nДля того, чтобы взять оружие, боеприпасы и любые другие предметы, а также сесть в автомобиль, наведите на него и нажмите клавишу E.");
			}
			if (ID_QUEST == 3)
			{
				targetGUI.TARGET_TEXT = "ВЗЯТЬ ПРЕДМЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 4)
			{
				targetGUI.TARGET_TEXT = "ВЗЯТЬ ПРЕДМЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nЗарядите оружие, зажав клавишу R. Удерживайте клавишу до полной перезарядки оружия.");
			}
			if (ID_QUEST == 5)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nУстановите тактический фонарь, зайдя в инвентарь на клавишу Q, выберав его и нажав 'Использовать'.");
			}
			if (ID_QUEST == 8 || ID_QUEST == 9 || ID_QUEST == 10)
			{
				targetGUI.TARGET_TEXT = "ВЗЯТЬ ПРЕДМЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 6)
			{
				targetGUI.TARGET_TEXT = "УНИЧТОЖИТЬ";
				GameObject gameObject = GameObject.Find("_ANIMAL_WOLF_03");
				if ((bool)gameObject && !gameObject.GetComponent<AnimalAI>().isDead)
				{
					targetGUI.target = gameObject;
				}
				else
				{
					CompleteQuest();
				}
				ShowHint("Подсказка:\n\nПри стрельбе учитывйте баллистику, расстояние до цели, скорость полета пули. Не забывайте перезаряжать оружие на клавишу R.\n\nЧтобы продолжить, прицельтесь, нажав на правую кнопку мыши.");
			}
			if (ID_QUEST == 7)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 11)
			{
				targetGUI.TARGET_TEXT = "НАЙТИ ТРАНСПОРТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 12)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nИспользуйте аккумулятор, бензин и отвертку, чтобы завести автомобиль. Нажмите на клавишу Q, поочередно используйте аккумулятор, бензин, отвертку. Чтобы включить вид от 3 лица, нажмите на V. Фары включаются на клавишу L.");
			}
			if (ID_QUEST == 13)
			{
				targetGUI.TARGET_TEXT = "ВЗЯТЬ ПРЕДМЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 14)
			{
				targetGUI.TARGET_TEXT = "ВЗЯТЬ ПРЕДМЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nВы взяли автоматический пистолет 'GLOCK'. Чтобы переключить режим ведения огня с одиночного на автоматический, нажмите клавишу F.");
			}
			if (ID_QUEST == 15)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nВставьте магазин в оружие, нажав клавишу R.");
			}
			if (ID_QUEST == 16)
			{
				targetGUI.TARGET_TEXT = "УНИЧТОЖИТЬ";
				GameObject gameObject2 = GameObject.Find("_ANIMAL_BEAR_01");
				if ((bool)gameObject2 && !gameObject2.GetComponent<AnimalAI>().isDead)
				{
					targetGUI.target = gameObject2;
				}
				else
				{
					CompleteQuest();
				}
				targetGUI.target = gameObject2;
				ShowHint("Подсказка:\n\nБудьте осторожны, впереди опасный враг. Цельтесь в голову, чтобы добиться максимального урона.");
			}
			if (ID_QUEST == 17 || ID_QUEST == 18)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 19 || ID_QUEST == 20)
			{
				targetGUI.TARGET_TEXT = "ВЗЯТЬ ПРЕДМЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 21)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nРасчистите завал на дороге с помощью аммонала. Комбинируйте заправленную зажигалку с аммоналом, чтобы поджечь фитиль.");
			}
			if (ID_QUEST == 22 || ID_QUEST == 23 || ID_QUEST == 24)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 25 || ID_QUEST == 26)
			{
				targetGUI.TARGET_TEXT = "ВЗЯТЬ ПРЕДМЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
			}
			if (ID_QUEST == 27)
			{
				targetGUI.TARGET_TEXT = "СЕСТЬ В ВЕРТОЛЕТ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nИспользуйте аккумулятор, бензин и отвертку, чтобы завести вертолет.");
			}
			if (ID_QUEST == 28)
			{
				targetGUI.TARGET_TEXT = "ДВИЖЕНИЕ";
				targetGUI.target = QUEST_TARGET[ID_QUEST];
				ShowHint("Подсказка:\n\nУправление в вертолете.\nSHIFT - увеличить тягу.\nCTRL - уменьшить тягу.\nЧтобы включить вид от 3 лица, нажмите на V. Фары включаются на клавишу L.\n\nЛетите к точке эвакуации.");
			}
			if (ID_QUEST == 29)
			{
				BaseLogic._player.SendMessage("HideInventory");
				BaseLogic._inventory4.SetActive(false);
				GameObject gameObject3 = GameObject.Find("RESPAWN_LABEL");
				if ((bool)gameObject3)
				{
					gameObject3.GetComponent<UILabel>().text = "ПОЗДРАВЛЯЕМ!\nВЫ УСПЕШНО ПРОШЛИ ОБУЧЕНИЕ";
				}
				GameObject gameObject4 = Object.Instantiate(BaseLogic._player.GetComponent<FPSPlayer>().levelLoadFadeObj) as GameObject;
				gameObject4.GetComponent<LevelLoadFade>().FadeAndLoadLevel(Color.black, 500f, false);
				StartCoroutine("ExitGame");
			}
		}
		if (ID_MISSION == 0 && ID_QUEST == 1)
		{
			if (BaseLogic.SideReal == 1)
			{
				targetGUI.TARGET_TEXT = "УДЕРЖАТЬ";
			}
			else
			{
				targetGUI.TARGET_TEXT = "УНИЧТОЖИТЬ";
			}
			targetGUI.target = QUEST_TARGET[ID_QUEST];
		}
	}

	private IEnumerator ExitGame()
	{
		yield return new WaitForSeconds(5f);
		BaseLogic.PLAYER_DEATH = false;
		if (PhotonNetwork.room != null)
		{
			PhotonNetwork.LeaveRoom();
		}
		Application.LoadLevel(0);
	}

	private IEnumerator HideHint()
	{
		yield return new WaitForSeconds(8f);
		BaseLogic._respawnTimerGui.GetComponent<UILabel>().text = string.Empty;
	}
}
