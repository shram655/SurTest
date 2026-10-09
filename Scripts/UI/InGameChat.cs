using UnityEngine;

public class InGameChat : SingletonProtected<InGameChat>
{
	public UITweener[] chatBack;

	public UITweener chatTextList;

	public UIInput mInput;

	public UITextList textList;

	private bool mIgnoreNextEnter;

	private float timeToHide;

	private bool isHided = true;

	private void Update()
	{
		if (!isHided && !mInput.isSelected && Time.time > timeToHide)
		{
			isHided = true;
			chatTextList.Play(true);
		}
		if (Input.GetKeyUp(KeyCode.Return))
		{
			if (!mIgnoreNextEnter && !mInput.isSelected)
			{
				mInput.label.maxLineCount = 1;
				mInput.isSelected = true;
				BaseLogic._player.GetComponent<FPSPlayer>().DisableKeys(true);
				ShowChat();
			}
				
			mIgnoreNextEnter = false;
		}
	}

	public static void RPCSubmit(string text)
	{
		SingletonProtected<InGameChat>.Instance.Submit(text);
	}

	public void OnSubmit()
	{
		if (textList != null)
		{
			string text = NGUIText.StripSymbols(mInput.value);
			if (!string.IsNullOrEmpty(text))
			{
				textList.Add(BaseLogic.PLAYER_NAME + ": " + text);
				if (PhotonNetwork.room != null)
				{
					Network.PLAYER.GetComponent<PhotonView>().RPC("GameChat", PhotonTargets.Others, BaseLogic.PLAYER_NAME + ": " + text);
				}
			}
			mInput.value = string.Empty;
			mInput.isSelected = false;
			BaseLogic._player.GetComponent<FPSPlayer>().DisableKeys(false);
			isHided = false;
			timeToHide = Time.time + 5f;
			HideChat(false);
		}
		mIgnoreNextEnter = true;
	}

	private void Submit(string text)
	{
		text = NGUIText.StripSymbols(text);
		if (!string.IsNullOrEmpty(text))
		{
			if (isHided)
			{
				chatTextList.Play(false);
			}
			isHided = false;
			timeToHide = Time.time + 5f;
			textList.Add(text);
		}
	}


	private void ShowChat()
	{
		foreach (UITweener item in this.chatBack)
		{
			item.Play(false);
		}
	}

	private void HideChat(bool hideTextList)
	{
		foreach (UITweener item in this.chatBack)
		{
			if (hideTextList || item != chatTextList)
			{
				item.Play(true);
			}
		}
	}
}
