using UnityEngine;

public class HeroRotate : MonoBehaviour
{
	public GameObject MALE;

	public GameObject FEMALE;

	private float _updateTimer;

	private void Start()
	{
	}

	private void Update()
	{
		if (Time.time > _updateTimer + 0.3f)
		{
			if (BaseLogic.BT2)
			{
				if (BaseLogic.SideReal == 1)
				{
					MALE.SetActive(true);
					FEMALE.SetActive(false);
				}
				else
				{
					MALE.SetActive(false);
					FEMALE.SetActive(true);
				}
			}
			else if (BaseLogic.PLAYER_SEX == 1)
			{
				MALE.SetActive(true);
				FEMALE.SetActive(false);
			}
			else
			{
				MALE.SetActive(false);
				FEMALE.SetActive(true);
			}
			int weaponSlot1_Real = BaseLogic._weaponSlot1_Real;
			if (weaponSlot1_Real == 0 || weaponSlot1_Real == 1 || weaponSlot1_Real == 9 || weaponSlot1_Real == 22 || weaponSlot1_Real == 2 || weaponSlot1_Real == 7 || weaponSlot1_Real == 8 || weaponSlot1_Real == 12 || weaponSlot1_Real == 14 || weaponSlot1_Real == 15)
			{
				if (MALE.activeSelf)
				{
					MALE.GetComponent<Animator>().SetBool("Rifle", false);
				}
				if (FEMALE.activeSelf)
				{
					FEMALE.GetComponent<Animator>().SetBool("Rifle", false);
				}
			}
			else
			{
				if (MALE.activeSelf)
				{
					MALE.GetComponent<Animator>().SetBool("Rifle", true);
				}
				if (FEMALE.activeSelf)
				{
					FEMALE.GetComponent<Animator>().SetBool("Rifle", true);
				}
			}
			_updateTimer = Time.time;
		}
		if (Input.GetKey(KeyCode.Mouse0))
		{
			base.transform.Rotate(0f, (0f - Input.GetAxis("Mouse X")) * 10f, 0f);
		}
	}
}
