using System;
using UnityEngine;

public class ClanWWW : SingletonProtected<ClanWWW>
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

	public static ClanWWWOperation CreateClan(string clan_name)
	{
		return SingletonProtected<ClanWWW>.Instance._CreateClan(clan_name);
	}

	public static ClanWWWOperation RemoveClan()
	{
		return SingletonProtected<ClanWWW>.Instance._RemoveClan();
	}

	public static ClanWWWOperation GetMeClanInfo()
	{
		return SingletonProtected<ClanWWW>.Instance._GetMeClanInfo();
	}

	public static ClanWWWOperation CreateInviteRequest(long clan_id)
	{
		return SingletonProtected<ClanWWW>.Instance._CreateInviteRequest(clan_id);
	}

	public static ClanWWWOperation RemoveInviteRequest()
	{
		return SingletonProtected<ClanWWW>.Instance._RemoveInviteRequest();
	}

	public static ClanWWWOperation GetInviteRequest()
	{
		return SingletonProtected<ClanWWW>.Instance._GetInviteRequest();
	}

	public static ClanWWWOperation InviteUser(long user_id)
	{
		return SingletonProtected<ClanWWW>.Instance._InviteUser(user_id);
	}

	public static ClanWWWOperation RemoveUser(long user_id)
	{
		return SingletonProtected<ClanWWW>.Instance._RemoveUser(user_id);
	}

	public static ClanWWWOperation SetUserRole(long user_id, int clan_role)
	{
		return SingletonProtected<ClanWWW>.Instance._SetUserRole(user_id, clan_role);
	}

	public static ClanWWWOperation SetClanName(long clan_id, string clan_name)
	{
		return SingletonProtected<ClanWWW>.Instance._SetClanName(clan_id, clan_name);
	}

	public static ClanWWWOperation GetClanInfo(long clan_id)
	{
		return SingletonProtected<ClanWWW>.Instance._GetClanInfo(clan_id);
	}

	public static ClanWWWOperation GetClanInviteRequests(long clan_id)
	{
		return SingletonProtected<ClanWWW>.Instance._GetClanInviteRequests(clan_id);
	}

	public static ClanWWWOperation SetClanInviteRequests(long clan_id, int status)
	{
		return SingletonProtected<ClanWWW>.Instance._SetClanInviteRequests(clan_id, status);
	}

	public static ClanWWWOperation GetClanUsers(long clan_id)
	{
		return SingletonProtected<ClanWWW>.Instance._GetClanUsers(clan_id);
	}

	public static ClanWWWOperation GetRandomClans(int count)
	{
		return SingletonProtected<ClanWWW>.Instance._GetRandomClans(count);
	}

	private ClanWWWOperation _CreateClan(string clan_name)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "create_me_clan", clan_name);
	}

	private ClanWWWOperation _RemoveClan()
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "remove_me_clan");
	}

	private ClanWWWOperation _GetMeClanInfo()
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "get_me_clan_info");
	}

	private ClanWWWOperation _CreateInviteRequest(long clan_id)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "create_me_invite_request", clan_id);
	}

	private ClanWWWOperation _RemoveInviteRequest()
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "remove_me_invite_request");
	}

	private ClanWWWOperation _GetInviteRequest()
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "get_me_invite_request");
	}

	private ClanWWWOperation _InviteUser(long user_id)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "invite_clan_user", user_id, true);
	}

	private ClanWWWOperation _RemoveUser(long user_id)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "remove_clan_user", user_id, true);
	}

	private ClanWWWOperation _SetUserRole(long user_id, int clan_role)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "set_clan_user_role", user_id, clan_role);
	}

	private ClanWWWOperation _SetClanName(long clan_id, string clan_name)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "set_clan_name", clan_id, clan_name);
	}

	private ClanWWWOperation _GetClanInfo(long clan_id)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "get_clan_info", clan_id);
	}

	private ClanWWWOperation _GetClanInviteRequests(long clan_id)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "get_clan_invite_requests", clan_id);
	}

	private ClanWWWOperation _SetClanInviteRequests(long user_id, int status)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "set_clan_invite_request", user_id, status, true);
	}

	private ClanWWWOperation _GetClanUsers(long clan_id)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, "get_clan_users", clan_id);
	}

	private ClanWWWOperation _GetRandomClans(int count)
	{
		return new ClanWWWOperationPost(UrlReal, m_Key, BaseLogic.game_mode.Remove(2), BaseLogic.viewer_id, count);
	}
}
