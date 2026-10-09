using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Logging;
using Sfs2X.Requests;
using UnityEngine;

public class SmartfoxConnection : SingletonProtected<SmartfoxConnection>
{
	private SmartFox sfxConn;

	public static SmartFox Connection
	{
		get
		{
			return SingletonProtected<SmartfoxConnection>.Instance.sfxConn;
		}
		set
		{
			if (SingletonProtected<SmartfoxConnection>.Instance.sfxConn == null)
			{
				SingletonProtected<SmartfoxConnection>.Instance.sfxConn = value;
				SingletonProtected<SmartfoxConnection>.Instance.sfxConn.AddLogListener(LogLevel.DEBUG, OnDebugMessage);
			}
		}
	}

	public static bool IsInitialized
	{
		get
		{
			return SingletonProtected<SmartfoxConnection>.Instance.sfxConn != null;
		}
	}

	public static bool IsConnected
	{
		get
		{
			return SingletonProtected<SmartfoxConnection>.Instance.sfxConn != null && SingletonProtected<SmartfoxConnection>.Instance.sfxConn.IsConnected;
		}
	}

	public static void StartSync()
	{
		SingletonProtected<SmartfoxConnection>.Instance.StartCoroutine(SingletonProtected<SmartfoxConnection>.Instance.TimeSyncRequest());
	}

	public static void StopSync()
	{
		SingletonProtected<SmartfoxConnection>.Instance.StopCoroutine(SingletonProtected<SmartfoxConnection>.Instance.TimeSyncRequest());
	}

	private static void OnDebugMessage(BaseEvent evt)
	{
		string text = (string)evt.Params["message"];
		Debug.Log("[SFS DEBUG] " + text);
	}

	protected override void OnDestroy()
	{
		if (sfxConn != null)
		{
			sfxConn.RemoveLogListener(LogLevel.DEBUG, OnDebugMessage);
			sfxConn.RemoveAllEventListeners();
			sfxConn.Disconnect();
		}
		base.OnDestroy();
	}

	private void FixedUpdate()
	{
		if (sfxConn != null)
		{
			sfxConn.ProcessEvents();
		}
	}

	private IEnumerator TimeSyncRequest()
	{
		while (true)
		{
			sfxConn.Send(new ExtensionRequest("t", new SFSObject()));
			yield return new WaitForSeconds(30f);
		}
	}
}
