using UnityEngine;

public class SFS : SingletonProtected<SFS>
{
	[SerializeField]
	private string m_IP = string.Empty;

	[SerializeField]
	private int m_Port;

	[SerializeField]
	private string m_Zone = string.Empty;

	[SerializeField]
	private string m_Key = string.Empty;

	public static SFSOperationConnect Connect()
	{
		return SingletonProtected<SFS>.Instance._Connect();
	}

	private SFSOperationConnect _Connect()
	{
		return new SFSOperationConnect(m_IP, m_Port, m_Zone, m_Key);
	}
}
