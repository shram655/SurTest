using System.Collections.Generic;

public sealed class ScoreWWWOperationPostGzip : ScoreWWWOperation
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

	public ScoreWWWOperationPostGzip(string url, string key, string social, string viewer_id)
	{
		InitWWWGzip(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "get" }
		});
	}

	public ScoreWWWOperationPostGzip(string url, string key, string social, string viewer_id, Dictionary<string, object> fields)
	{
		InitWWWGzip(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "get" },
			{ "fields", fields }
		});
	}
}
