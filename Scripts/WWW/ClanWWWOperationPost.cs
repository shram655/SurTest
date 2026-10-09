using System.Collections.Generic;

public sealed class ClanWWWOperationPost : ClanWWWOperation
{
	public override ClanResponse clan
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

	public ClanWWWOperationPost(string url, string key, string social, string viewer_id, string cmd)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", cmd }
		});
	}

	public ClanWWWOperationPost(string url, string key, string social, string viewer_id, string cmd, long clan_id)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", cmd },
			{ "clan_id", clan_id }
		});
	}

	public ClanWWWOperationPost(string url, string key, string social, string viewer_id, string cmd, string clan_name)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "create_me_clan" },
			{ "clan_name", clan_name }
		});
	}

	public ClanWWWOperationPost(string url, string key, string social, string viewer_id, string cmd, long user_id, bool is_user_id)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", cmd },
			{ "user_id", user_id }
		});
	}

	public ClanWWWOperationPost(string url, string key, string social, string viewer_id, string cmd, long user_id, int clan_role)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "set_clan_user_role" },
			{ "user_id", user_id },
			{ "clan_role", clan_role }
		});
	}

	public ClanWWWOperationPost(string url, string key, string social, string viewer_id, string cmd, long clan_id, string clan_name)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "set_clan_name" },
			{ "clan_id", clan_id },
			{ "clan_name", clan_name }
		});
	}

	public ClanWWWOperationPost(string url, string key, string social, string viewer_id, string cmd, long user_id, int status, bool invite_request)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "set_clan_invite_request" },
			{ "user_id", user_id },
			{ "status", status }
		});
	}

	public ClanWWWOperationPost(string url, string key, string social, string viewer_id, int count)
	{
		InitWWW(url, key, new Dictionary<string, object>
		{
			{ "sc", social },
			{ "vid", viewer_id },
			{ "cmd", "get_clans_rand" },
			{ "count", count }
		});
	}
}
