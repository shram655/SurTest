using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Ionic.Zlib;
using JsonFx.Json;
using UnityEngine;

public static class Utils
{
	public static WWW WWWPostJsonGZip(string url, string key, Dictionary<string, object> data)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Content-Type", "application/json");
		dictionary.Add("Content-Encoding", "gzip");
		if (data == null)
		{
			data = new Dictionary<string, object>();
		}
		data["tst"] = GetTimeStamp();
		data["sig"] = Md5Sum(GetDictionaryString(data) + key);
		string s = string.Empty;
		try
		{
			s = JsonWriter.Serialize(data);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return new WWW(url, ZlibStream.CompressString(s), dictionary);
	}

	public static WWW WWWPostJson(string url, string key, Dictionary<string, object> data)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Content-Type", "application/json");
		dictionary.Add("Content-Encoding", "utf-8");
		if (data == null)
		{
			data = new Dictionary<string, object>();
		}
		data["tst"] = GetTimeStamp();
		data["sig"] = Md5Sum(GetDictionaryString(data) + key);
		string s = string.Empty;
		try
		{
			s = JsonWriter.Serialize(data);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return new WWW(url, Encoding.UTF8.GetBytes(s), dictionary);
	}

	public static int GetTimeStamp()
	{
		return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
	}

	public static SortedDictionary<object, object> ConvertToSortedDictionary(IDictionary data)
	{
		SortedDictionary<object, object> sortedDictionary = new SortedDictionary<object, object>();
		foreach (DictionaryEntry datum in data)
		{
			sortedDictionary.Add(datum.Key, datum.Value);
		}
		return sortedDictionary;
	}

	public static string GetDictionaryString(IDictionary data)
	{
		string text = string.Empty;
		foreach (KeyValuePair<object, object> item in ConvertToSortedDictionary(data))
		{
			text = ((!(item.Value is string)) ? ((!(item.Value is bool)) ? ((!(item.Value is IDictionary)) ? ((!(item.Value is IEnumerable)) ? (text + Convert.ToString(item.Key) + Convert.ToString(item.Value)) : (text + Convert.ToString(item.Key) + GetArrayString((IEnumerable)item.Value))) : (text + Convert.ToString(item.Key) + GetDictionaryString((IDictionary)item.Value))) : (text + Convert.ToString(item.Key) + ((!(bool)item.Value) ? "0" : "1"))) : (text + Convert.ToString(item.Key) + (string)item.Value));
		}
		return text;
	}

	public static string GetArrayString(IEnumerable data)
	{
		string text = string.Empty;
		foreach (object datum in data)
		{
			text = ((!(datum is string)) ? ((!(datum is bool)) ? ((!(datum is IDictionary)) ? ((!(datum is IEnumerable)) ? (text + Convert.ToString(datum)) : (text + GetArrayString((IEnumerable)datum))) : (text + GetDictionaryString((IDictionary)datum))) : (text + ((!(bool)datum) ? "0" : "1"))) : (text + (string)datum));
		}
		return text;
	}

	public static string Md5Sum(string strToEncrypt)
	{
		return Md5Sum(Encoding.UTF8.GetBytes(strToEncrypt));
	}

	public static string Md5Sum(byte[] bytesToEncrypt)
	{
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] array = mD5CryptoServiceProvider.ComputeHash(bytesToEncrypt);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}
}
