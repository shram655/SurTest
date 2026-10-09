//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2019 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;

/// <summary>
/// Turns the popup list it's attached to into a language selection list.
/// </summary>

[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Language Selection")]
public class LanguageSelection : MonoBehaviour
{
	UIPopupList mList;
	bool mStarted = false;

	void Awake () { mList = GetComponent<UIPopupList>(); }

	void Start ()
	{
		mStarted = true;
		//Refresh();
		EventDelegate.Add(mList.onChange, delegate() {
			/*Localization.language = UIPopupList.current.value; 

			if (UIPopupList.current.value == "Russian")
				BaseLogic.LANGUAGE = 0;
			else if (UIPopupList.current.value == "English")
				BaseLogic.LANGUAGE = 1;
			else if (UIPopupList.current.value == "Portuguese")
				BaseLogic.LANGUAGE = 2;
			else if (UIPopupList.current.value == "Espanol")
				BaseLogic.LANGUAGE = 3;
			else if (UIPopupList.current.value == "French")
				BaseLogic.LANGUAGE = 4;*/

			Localization.language = "Russian";

			BaseLogic.LANGUAGE = 0;

			Debug.Log("BaseLogic.LANGUAGE: " + BaseLogic.LANGUAGE);
		});
	}

	void OnEnable () { if (mStarted) Refresh(); }

	/// <summary>
	/// Immediately refresh the list of known languages.
	/// </summary>

	public void Refresh ()
	{
		if (mList != null && Localization.knownLanguages != null)
		{
			mList.Clear();

			for (int i = 0, imax = Localization.knownLanguages.Length; i < imax; ++i)
				mList.items.Add(Localization.knownLanguages[i]);

			mList.value = Localization.language;
		}
	}

	void OnLocalize () { Refresh(); } 
}
