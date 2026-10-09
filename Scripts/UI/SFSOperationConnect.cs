using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using UnityEngine;

public sealed class SFSOperationConnect : SFSOperation<SFSConnectReponse>
{
	private bool m_IsDone;

	private float m_Progress;

	private string m_Error;

	private SmartFox m_SfxConn;

	private string m_Zone;

	private string m_Key;

	private SFSConnectReponse m_Result;

	public override SFSConnectReponse result
	{
		get
		{
			return m_Result;
		}
	}

	public override bool isDone
	{
		get
		{
			return m_IsDone;
		}
	}

	public override float progress
	{
		get
		{
			return (!m_IsDone) ? m_Progress : 1f;
		}
	}

	public override string error
	{
		get
		{
			return m_Error;
		}
	}

	public SFSOperationConnect(string ip, int port, string zone, string key)
	{
		m_Zone = zone;
		m_Key = key;
		if (SmartfoxConnection.IsInitialized)
		{
			m_SfxConn = SmartfoxConnection.Connection;
		}
		else
		{
			m_SfxConn = new SmartFox(false);
			SmartfoxConnection.Connection = m_SfxConn;
		}
		if (!m_SfxConn.IsConnected)
		{
			if (Application.isEditor)
			{
				Debug.LogWarning("Security Exception. Policy file load failed!");
			}
			m_SfxConn.AddEventListener(SFSEvent.CONNECTION, OnConnection);
			m_SfxConn.AddEventListener(SFSEvent.LOGIN, OnLogin);
			m_SfxConn.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
			m_SfxConn.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
			m_SfxConn.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
			m_SfxConn.Connect(ip, port);
		}
		else
		{
			m_Result = new SFSConnectReponse();
			m_Result.online = true;
			m_IsDone = true;
		}
	}

	private void OnConnection(BaseEvent evt)
	{
		bool flag = (bool)evt.Params["success"];
		string text = (string)evt.Params["error"];
		if (flag)
		{
			m_Progress = 0.33f;
			m_SfxConn.Send(new LoginRequest(BaseLogic.game_mode.Remove(2) + ":" + BaseLogic.viewer_id, m_Key, m_Zone));
			return;
		}
		m_Result = new SFSConnectReponse();
		m_Result.online = false;
		m_Error = text;
		m_IsDone = true;
		RemoveEventListeners();
	}

	private void OnLogin(BaseEvent evt)
	{
		m_Progress = 0.66f;
	}

	private void OnLoginError(BaseEvent evt)
	{
		string text = (string)evt.Params["errorMessage"];
		m_Result = new SFSConnectReponse();
		m_Result.online = false;
		m_Error = text;
		m_IsDone = true;
		RemoveEventListeners();
	}

	private void OnRoomJoin(BaseEvent evt)
	{
		m_Result = new SFSConnectReponse();
		m_Result.online = true;
		m_IsDone = true;
		RemoveEventListeners();
	}

	private void OnRoomJoinError(BaseEvent evt)
	{
		string text = (string)evt.Params["errorMessage"];
		m_Result = new SFSConnectReponse();
		m_Result.online = false;
		m_Error = text;
		m_IsDone = true;
		RemoveEventListeners();
	}

	private void RemoveEventListeners()
	{
		m_SfxConn.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
		m_SfxConn.RemoveEventListener(SFSEvent.LOGIN, OnLogin);
		m_SfxConn.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		m_SfxConn.RemoveEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		m_SfxConn.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
	}
}
