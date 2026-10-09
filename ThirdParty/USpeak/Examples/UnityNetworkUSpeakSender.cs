/* Copyright (c) 2012 MoPho' Games
 * All Rights Reserved
 * 
 * Please see the included 'LICENSE.TXT' for usage rights
 * If this asset was downloaded from the Unity Asset Store,
 * you may instead refer to the Unity Asset Store Customer EULA
 * If the asset was NOT purchased or downloaded from the Unity
 * Asset Store and no such 'LICENSE.TXT' is present, you may
 * assume that the software has been pirated.
 * */

using UnityEngine;
using System.Collections;

using MoPhoGames.USpeak.Interface;
using ExitGames.Client.Photon;

/// <summary>
/// Sends data received from USpeak over RPC with Unity Networking
/// </summary>
public class UnityNetworkUSpeakSender : Photon.MonoBehaviour, ISpeechDataHandler
{
	void Start()
	{
		if( !photonView.isMine )
			USpeaker.Get( this ).SpeakerMode = SpeakerMode.Remote;
	}

	#region ISpeechDataHandler Members

	/// <summary>
	/// Calls an RPC which passes the data back to USpeaker
	/// </summary>
	public void USpeakOnSerializeAudio( byte[] data )
	{
		photonView.RPC( "vc", PhotonTargets.All, data );
	}

	/// <summary>
	/// Calls a buffered RPC which passes the settings data to USpeaker
	/// </summary>
	public void USpeakInitializeSettings( int data )
	{
		photonView.RPC( "init", PhotonTargets.AllBuffered, data );
	}

	#endregion

	[PunRPC]
	void vc( byte[] data )
	{
		USpeaker.Get( this ).ReceiveAudio( data );
	}

	[PunRPC]
	void init( int data )
	{
		USpeaker.Get( this ).InitializeSettings( data );
	}
}