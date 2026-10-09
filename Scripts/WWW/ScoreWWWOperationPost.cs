using System.Collections.Generic;

public sealed class ScoreWWWOperationPost : ScoreWWWOperation
{
	public override ScoreResponse score
	{
		get
		{
			if (m_Result != null && m_Result.response != null && m_Result.response.Length != 0)
			{
				return m_Result.response[0];
			}
			return null;
		}
	}

	public ScoreWWWOperationPost(string url, string key, string social, string viewer_id)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "get" }
		});
	}

	public ScoreWWWOperationPost(string url, string key, string social, string viewer_id, Dictionary<string, object> fields)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "set" },
			{ "fields", fields }
		});
	}
}
