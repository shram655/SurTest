using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreWWW : SingletonProtected<ScoreWWW>
{
	[SerializeField]
	private string m_Url = string.Empty;

	[SerializeField]
	private string m_UrlHttps = string.Empty;

	[SerializeField]
	private bool m_OnlyHttps;

	[SerializeField]
	private string m_Key = string.Empty;

	private bool m_UrlCheck;

	private bool m_UseHttps;

	private string UrlReal
	{
		get
		{
			if (!m_UrlCheck)
			{
				if (m_OnlyHttps || Application.absoluteURL.IndexOf("https://", StringComparison.Ordinal) == 0)
				{
					m_UseHttps = true;
				}
				else
				{
					m_UseHttps = false;
				}
				m_UrlCheck = true;
			}
			return (!m_UseHttps) ? m_Url : m_UrlHttps;
		}
	}

	public static ScoreWWWOperation Get()
	{
		return SingletonProtected<ScoreWWW>.Instance._Get(false);
	}

	public static ScoreWWWOperation Get(bool isGzip)
	{
		return SingletonProtected<ScoreWWW>.Instance._Get(isGzip);
	}

	public static ScoreWWWOperation Set(Dictionary<string, object> fields)
	{
		return SingletonProtected<ScoreWWW>.Instance._Set(fields, false);
	}

	public static ScoreWWWOperation Set(Dictionary<string, object> fields, bool isGzip)
	{
		return SingletonProtected<ScoreWWW>.Instance._Set(fields, isGzip);
	}

	private ScoreWWWOperation _Get(bool isGzip)
	{
		return new ScoreWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id);
	}

	private ScoreWWWOperation _Set(Dictionary<string, object> fields, bool isGzip)
	{
		if (isGzip)
		{
			return new ScoreWWWOperationPostGzip(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, fields);
		}
		return new ScoreWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, fields);
	}
}
