using System;
using UnityEngine;

public class AntiSH : MonoBehaviour
{
	private DateTime olddt;

	private long oldTick;

	private int errorCount;

	private void Start()
	{
		olddt = DateTime.Now;
		oldTick = Environment.TickCount;
		InvokeRepeating("invSpeedHack", 5f, 5f);
	}

	private void invSpeedHack()
	{
		TimeSpan timeSpan = DateTime.Now - olddt;
		olddt = DateTime.Now;
		long num = Environment.TickCount - oldTick;
		oldTick = Environment.TickCount;
		if (timeSpan.TotalMilliseconds * 1.2999999523162842 < (double)num)
		{
			errorCount++;
		}
		if (errorCount > 5)
		{
			BaseLogic.SetBan(9000);
			BaseLogic._player.SendMessage("SaveHero");
			if (PhotonNetwork.room != null)
			{
				PhotonNetwork.LeaveRoom();
			}
		}
	}
}
