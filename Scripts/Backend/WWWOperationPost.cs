using System;
using System.Collections.Generic;
using System.Text;
using Ionic.Zlib;
using JsonFx.Json;
using UnityEngine;

public class WWWOperationPost<T> : WWWOperation<T>
{
	protected bool m_Disposed;

	protected WWW m_WWW;

	protected string m_Error;

	protected T m_Result;

	public override T result
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
			if (m_Result != null || !string.IsNullOrEmpty(m_Error))
			{
				return true;
			}
			if (m_WWW.isDone)
			{
				ParseWWWData();
				return true;
			}
			return false;
		}
	}

	public override float progress
	{
		get
		{
			if (!string.IsNullOrEmpty(m_Error))
			{
				return 1f;
			}
			return m_WWW.progress;
		}
	}

	public override string error
	{
		get
		{
			return m_Error;
		}
	}

	protected void InitWWW(string url, string key, Dictionary<string, object> val)
	{
		m_WWW = Utils.WWWPostJson(url, key, val);
	}

	protected void InitWWWGzip(string url, string key, Dictionary<string, object> val)
	{
		m_WWW = Utils.WWWPostJsonGZip(url, key, val);
	}

	protected override void Dispose(bool disposing)
	{
		if (!m_Disposed)
		{
			if (disposing)
			{
				m_WWW.Dispose();
			}
			m_Disposed = true;
		}
	}

	protected virtual void ParseJsonData(string jsonString)
	{
		try
		{
			m_Result = JsonReader.Deserialize<T>(jsonString);
		}
		catch (Exception ex)
		{
			m_Error = ex.Message;
			Debug.LogException(ex);
		}
	}

	protected void ParseWWWData()
	{
		if (string.IsNullOrEmpty(m_WWW.error))
		{
			string value = null;
			if (m_WWW.responseHeaders.TryGetValue("CONTENT-TYPE", out value))
			{
				if (value.Equals("application/json", StringComparison.Ordinal))
				{
					if (m_WWW.responseHeaders.TryGetValue("CONTENT-ENCODING", out value))
					{
						string text = null;
						if (value.Equals("gzip", StringComparison.Ordinal))
						{
							byte[] array = null;
							try
							{
								array = ZlibStream.UncompressBuffer(m_WWW.bytes);
							}
							catch (Exception ex)
							{
								Debug.LogException(ex);
								m_Error = ex.Message;
							}
							if (array != null)
							{
								text = Encoding.UTF8.GetString(array);
							}
						}
						else if (value.Equals("utf-8", StringComparison.Ordinal))
						{
							text = Encoding.UTF8.GetString(m_WWW.bytes);
						}
						if (!string.IsNullOrEmpty(text))
						{
							ParseJsonData(text);
						}
						else if (string.IsNullOrEmpty(m_Error))
						{
							m_Error = "Content-Encoding mismatch type";
						}
					}
					else
					{
						m_Error = "Content-Encoding not found";
					}
				}
				else
				{
					m_Error = "Content-Type mismatch type";
				}
			}
			else
			{
				m_Error = "Content-Type not found";
			}
		}
		else
		{
			m_Error = m_WWW.error;
		}
	}
}
