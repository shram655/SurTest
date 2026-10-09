using UnityEngine;

public class TextReplacer : MonoBehaviour
{
	public UILabel UILABEL;

	public string ODK_TEXT = string.Empty;

	public string ENGLISH_TEXT = string.Empty;

	public string PTBR_TEXT = string.Empty;

	public string ES_TEXT = string.Empty;

	public string FR_TEXT = string.Empty;

	public bool Language = true;

	private int language;

	private void Update()
	{
		if (language == BaseLogic.LANGUAGE)
		{
			return;
		}
		if (!UILABEL)
		{
			UILABEL = GetComponent<UILabel>();
		}
		if (!(BaseLogic.game_mode == "vk.com"))
		{
			if (BaseLogic.LANGUAGE == 1 || BaseLogic.game_mode == "fb.com")
			{
				if ((bool)UILABEL)
				{
					UILABEL.text = ENGLISH_TEXT;
				}
			}
			else if (BaseLogic.LANGUAGE == 2)
			{
				if ((bool)UILABEL)
				{
					UILABEL.text = PTBR_TEXT;
				}
			}
			else if (BaseLogic.LANGUAGE == 3)
			{
				if ((bool)UILABEL)
				{
					UILABEL.text = ES_TEXT;
				}
			}
			else if (BaseLogic.LANGUAGE == 4)
			{
				if ((bool)UILABEL)
				{
					UILABEL.text = FR_TEXT;
				}
			}
			else if (!Language && (bool)UILABEL)
			{
				UILABEL.text = ODK_TEXT;
			}
		}
		language = BaseLogic.LANGUAGE;
	}
}
