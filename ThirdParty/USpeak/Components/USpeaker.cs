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
using System.Collections.Generic;
using System.Linq;

using MoPhoGames.USpeak.Codec;
using MoPhoGames.USpeak.Interface;
using MoPhoGames.USpeak.Core;
using MoPhoGames.USpeak.Core.Utils;

// ============================================
// ЗАГЛУШКА ДЛЯ MICROPHONE - для WebGL и других платформ
// ============================================
public static class MicrophoneHelper
{
    public static string[] devices 
    { 
        get 
        { 
            try
            {
                return new string[] { "Default" };
            }
            catch
            {
                return new string[] { "Default" };
            }
        } 
    }
    
    public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency)
    {
        Debug.Log("[USpeak] MicrophoneHelper.Start вызван (заглушка)");
        return null;
    }
    
    public static void End(string deviceName)
    {
        // Ничего не делаем
    }
    
    public static int GetPosition(string deviceName)
    {
        return 0;
    }
    
    public static bool IsRecording(string deviceName)
    {
        return false;
    }
}
// ============================================

/// <summary>
/// Bandwidth mode used to record
/// </summary>
public enum BandMode
{
    Narrow,
    Wide,
    UltraWide
}

/// <summary>
/// Mode the USpeaker operates in
/// </summary>
public enum SpeakerMode
{
    Local,
    Remote
}

/// <summary>
/// How data is sent
/// </summary>
public enum SendBehavior
{
    Constant,
    RecordThenSend
}

/// <summary>
/// Which 3D Features to use during playback
/// </summary>
public enum ThreeDMode
{
    None,
    SpeakerPan,
    Full3D
}

/// <summary>
/// The core component of USpeak
/// Handles recording and encoding, and decoding and playing, audio data
/// </summary>
[AddComponentMenu( "USpeak/USpeaker" )]
public class USpeaker : MonoBehaviour
{
    #region Static Fields

    public static float RemoteGain = 1.0f;
    public static float LocalGain = 1.0f;
    public static bool MuteAll = false;
    public static List<USpeaker> USpeakerList = new List<USpeaker>();

    private static int InputDeviceID = 0;
    private static string InputDeviceName = "";

    #endregion

    #region Public Fields

    public SpeakerMode SpeakerMode;
    public BandMode BandwidthMode = BandMode.Narrow;
    public float SendRate = 16;
    public SendBehavior SendingMode = SendBehavior.Constant;
    public bool UseVAD = false;

    [System.Obsolete( "Use USpeaker._3DMode instead" )]
    public bool Is3D
    {
        get { return _3DMode == ThreeDMode.SpeakerPan; }
        set { _3DMode = value ? ThreeDMode.SpeakerPan : ThreeDMode.None; }
    }

    public ThreeDMode _3DMode = ThreeDMode.None;
    public bool DebugPlayback = false;
    public bool AskPermission = true;

    public bool IsTalking
    {
        get { return talkTimer > 0.0f; }
    }

    public bool Mute = false;
    public float SpeakerVolume = 1.0f;
    public float VolumeThreshold = 0.01f;
    public int Codec = 0;

    #endregion

    #region Private Fields

    private USpeakCodecManager codecMgr;
    private AudioClip recording;
    private int recFreq;
    private int lastReadPos = 0;
    private float sendTimer = 0.0f;
    private float sendt = 1.0f;
    private string[] micDeviceList;
    private float lastDeviceUpdate = 0f;
    private List<USpeakFrameContainer> sendBuffer = new List<USpeakFrameContainer>();
    private List<byte> tempSendBytes = new List<byte>();
    private ISpeechDataHandler audioHandler;
    private IUSpeakTalkController talkController;
    private int overlap = 0;
    private USpeakSettingsData settings;
    private string currentDeviceName = "";
    private float talkTimer = 0.0f;
    private float vadHangover = 0.5f;
    private float lastVTime = 0.0f;
    private List<float[]> pendingEncode = new List<float[]>();
    private double played = 0;
    private int index = 0;
    private double received = 0;
    private float[] receivedData = null;
    private float playDelay = 0;
    private bool shouldPlay = false;
    private float lastTime = 0f;
    private BandMode lastBandMode;
    private int lastCodec;
    private ThreeDMode last3DMode;
    private int recordedChunkCount = 0;
    private int micFoundDelay = 0;
    private bool waitingToStartRec = false;

    #endregion

    #region Private Properties

    private int audioFrequency
    {
        get
        {
            if( recFreq == 0 )
            {
                switch( BandwidthMode )
                {
                    case BandMode.Narrow: recFreq = 8000; break;
                    case BandMode.Wide: recFreq = 16000; break;
                    case BandMode.UltraWide: recFreq = 32000; break;
                    default: recFreq = 8000; break;
                }
            }
            return recFreq;
        }
    }

    #endregion

    #region Static Methods

    public static void SetInputDevice( int deviceID )
    {
        InputDeviceID = deviceID;
        if( MicrophoneHelper.devices.Length > 0 )
            InputDeviceName = MicrophoneHelper.devices[ Mathf.Min(InputDeviceID, MicrophoneHelper.devices.Length - 1) ];
        else
            InputDeviceName = "Default";
    }

    public static USpeaker Get( Object source )
    {
        if( source is GameObject )
            return ( source as GameObject ).GetComponent<USpeaker>();
        else if( source is Transform )
            return ( source as Transform ).GetComponent<USpeaker>();
        else if( source is Component )
            return ( source as Component ).GetComponent<USpeaker>();
        return null;
    }

    #endregion

    #region Public Methods

    public void GetInputHandler()
    {
        talkController = (IUSpeakTalkController)FindInputHandler();
    }

    public void DrawTalkControllerUI()
    {
        if( talkController != null )
            talkController.OnInspectorGUI();
        else
            GUILayout.Label( "No component available which implements IUSpeakTalkController\nReverting to default behavior - data is always sent" );
    }

    public void ReceiveAudio( byte[] data )
    {
        if( settings == null )
        {
            Debug.LogWarning( "Trying to receive remote audio data without calling InitializeSettings!\nIncoming packet will be ignored" );
            return;
        }

        if( MuteAll || Mute || ( SpeakerMode == SpeakerMode.Local && !DebugPlayback ) )
            return;

        if( SpeakerMode == SpeakerMode.Remote )
            talkTimer = 1.0f;

        int offset = 0;
        while( offset < data.Length )
        {
            int len = System.BitConverter.ToInt32( data, offset );
            byte[] frame = USpeakPoolUtils.GetByte( len + 6 );
            System.Array.Copy( data, offset, frame, 0, frame.Length );

            USpeakFrameContainer cont = default( USpeakFrameContainer );
            cont.LoadFrom( frame );

            USpeakPoolUtils.Return( frame );

            float[] sample = USpeakAudioClipCompressor.DecompressAudio( cont.encodedData, (int)cont.Samples, 1, false, settings.bandMode, codecMgr.Codecs[ Codec ], RemoteGain );

            float sampleTime = ( (float)sample.Length / (float)audioFrequency );
            received += sampleTime;

            System.Array.Copy( sample, 0, receivedData, index, sample.Length );

            USpeakPoolUtils.Return( sample );

            index += sample.Length;

            if( index >= GetComponent<AudioSource>().clip.samples )
                index = 0;

            GetComponent<AudioSource>().clip.SetData( receivedData, 0 );

            if( !GetComponent<AudioSource>().isPlaying )
            {
                shouldPlay = true;
                if( playDelay <= 0 )
                {
                    playDelay = sampleTime * 5f;
                }
            }

            offset += frame.Length;
        }
    }

    public void InitializeSettings( int data )
    {
        print( "Settings changed" );
        settings = new USpeakSettingsData( (byte)data );
        Codec = settings.Codec;
    }

    #endregion

    #region Unity Callbacks

    void Awake()
    {
        USpeakerList.Add( this );

        if( GetComponent<AudioSource>() == null )
            gameObject.AddComponent<AudioSource>();

        GetComponent<AudioSource>().clip = AudioClip.Create( "vc", audioFrequency * 10, 1, audioFrequency, ( _3DMode == ThreeDMode.Full3D ), false );
        GetComponent<AudioSource>().loop = true;
        receivedData = new float[ audioFrequency * 10 ];

        codecMgr = USpeakCodecManager.Instance;

        lastBandMode = BandwidthMode;
        lastCodec = Codec;
        last3DMode = _3DMode;
    }

    void OnDestroy()
    {
        USpeakerList.Remove( this );
    }

    IEnumerator Start()
    {
        yield return null;

        audioHandler = (ISpeechDataHandler)FindSpeechHandler();
        talkController = (IUSpeakTalkController)FindInputHandler();

        if( audioHandler == null )
        {
            Debug.LogError( "USpeaker requires a component which implements the ISpeechDataHandler interface" );
            yield break;
        }

        if( SpeakerMode == SpeakerMode.Remote )
        {
            yield break;
        }

        // Микрофон отключен для WebGL сборки
        #if !UNITY_WEBGL
        if( AskPermission )
        {
            if( !Application.HasUserAuthorization( UserAuthorization.Microphone ) )
            {
                yield return Application.RequestUserAuthorization( UserAuthorization.Microphone );
            }
        }

        if( !Application.HasUserAuthorization( UserAuthorization.Microphone ) )
        {
            Debug.LogError( "Failed to start recording - user has denied microphone access" );
            yield break;
        }

        if( MicrophoneHelper.devices.Length == 0 )
        {
            Debug.LogWarning( "Failed to find a recording device" );
            yield break;
        }

        UpdateSettings();

        sendt = 1.0f / (float)SendRate;

        recording = MicrophoneHelper.Start( currentDeviceName, true, 5, audioFrequency );

        if( MicrophoneHelper.devices.Length > 0 )
        {
            int deviceIdx = Mathf.Min(InputDeviceID, MicrophoneHelper.devices.Length - 1);
            print( MicrophoneHelper.devices[ deviceIdx ] );
            currentDeviceName = MicrophoneHelper.devices[ deviceIdx ];
        }
        else
        {
            currentDeviceName = "Default";
        }

        micDeviceList = MicrophoneHelper.devices;
        #else
        Debug.Log("[USpeak] WebGL: Микрофон отключен");
        #endif
    }

    void Update()
    {
        #if !UNITY_WEBGL
        if( SpeakerMode == SpeakerMode.Local )
        {
            if( Time.time >= lastDeviceUpdate )
            {
                lastDeviceUpdate = Time.time + 2f;
                micDeviceList = MicrophoneHelper.devices;
            }
        }
        #endif

        talkTimer -= Time.deltaTime;

        GetComponent<AudioSource>().volume = SpeakerVolume;

        if( last3DMode != _3DMode )
        {
            last3DMode = _3DMode;

            StopPlaying();
            GetComponent<AudioSource>().clip = AudioClip.Create( "vc", audioFrequency * 10, 1, audioFrequency, ( _3DMode == ThreeDMode.Full3D ), false );
            GetComponent<AudioSource>().loop = true;
        }

        if( _3DMode == ThreeDMode.SpeakerPan )
        {
            Transform listener = Camera.main.transform;
            Vector3 side = Vector3.Cross( listener.up, listener.forward );
            side.Normalize();

            float x = Vector3.Dot( transform.position - listener.position, side );
            float z = Vector3.Dot( transform.position - listener.position, listener.forward );

            float angle = Mathf.Atan2( x, z );
            float pan = Mathf.Sin( angle );

            GetComponent<AudioSource>().panStereo = pan;
        }

        if( GetComponent<AudioSource>().isPlaying )
        {
            if( lastTime > GetComponent<AudioSource>().time )
            {
                played += GetComponent<AudioSource>().clip.length;
            }

            lastTime = GetComponent<AudioSource>().time;

            if( played + GetComponent<AudioSource>().time >= received )
            {
                StopPlaying();
                shouldPlay = false;
            }
        }
        else
        {
            if( shouldPlay )
            {
                playDelay -= Time.deltaTime;

                if( playDelay <= 0 )
                {
                    GetComponent<AudioSource>().Play();
                }
            }
        }

        if( SpeakerMode == SpeakerMode.Remote )
            return;

        if( audioHandler == null )
            return;

        #if !UNITY_WEBGL
        // Весь код с микрофоном только для не-WebGL
        if( micDeviceList.Length == 0 )
        {
            return;
        }
        else
        {
            if( string.IsNullOrEmpty( InputDeviceName ) )
                InputDeviceName = currentDeviceName;

            if( string.IsNullOrEmpty( currentDeviceName ) )
            {
                if( waitingToStartRec )
                {
                    micFoundDelay--;
                    if( micFoundDelay <= 0 )
                    {
                        micFoundDelay = 0;
                        waitingToStartRec = false;

                        print( "New device found: " + currentDeviceName );
                        InputDeviceID = 0;
                        if( micDeviceList.Length > 0 )
                        {
                            InputDeviceName = micDeviceList[ 0 ];
                            currentDeviceName = micDeviceList[ 0 ];
                        }
                        else
                        {
                            InputDeviceName = "Default";
                            currentDeviceName = "Default";
                        }

                        recording = MicrophoneHelper.Start( currentDeviceName, true, 5, audioFrequency );

                        lastReadPos = 0;
                        sendBuffer.Clear();
                        recordedChunkCount = 0;

                        UpdateSettings();
                    }
                }
                else
                {
                    waitingToStartRec = true;
                    micFoundDelay = 5;
                }
            }
            else
            {
                if( InputDeviceName != currentDeviceName )
                {
                    MicrophoneHelper.End( currentDeviceName );
                    print( "Using input device: " + InputDeviceName );
                    currentDeviceName = InputDeviceName;

                    recording = MicrophoneHelper.Start( currentDeviceName, true, 5, audioFrequency );

                    lastReadPos = 0;
                    sendBuffer.Clear();
                    recordedChunkCount = 0;
                }

                if( micDeviceList.Length > 0 && InputDeviceID < micDeviceList.Length )
                {
                    if( micDeviceList[ Mathf.Min( InputDeviceID, micDeviceList.Length - 1 ) ] != currentDeviceName )
                    {
                        bool found = false;
                        for( int i = 0; i < MicrophoneHelper.devices.Length; i++ )
                        {
                            if( i < micDeviceList.Length && micDeviceList[ i ] == currentDeviceName )
                            {
                                InputDeviceID = i;
                                found = true;
                            }
                        }

                        if( !found )
                        {
                            InputDeviceID = 0;
                            if( micDeviceList.Length > 0 )
                            {
                                InputDeviceName = micDeviceList[ 0 ];
                                currentDeviceName = micDeviceList[ 0 ];
                            }
                            else
                            {
                                InputDeviceName = "Default";
                                currentDeviceName = "Default";
                            }

                            print( "Device unplugged, switching to: " + currentDeviceName );

                            recording = MicrophoneHelper.Start( currentDeviceName, true, 5, audioFrequency );

                            lastReadPos = 0;
                            sendBuffer.Clear();
                            recordedChunkCount = 0;
                        }
                    }
                }
            }
        }

        if( lastBandMode != BandwidthMode || lastCodec != Codec )
        {
            UpdateSettings();
            lastBandMode = BandwidthMode;
            lastCodec = Codec;
        }

        if( recording == null )
            return;

        int readPos = MicrophoneHelper.GetPosition( currentDeviceName );

        int realReadPos = readPos + recording.samples * recordedChunkCount;

        if( realReadPos < lastReadPos )
            recordedChunkCount++;

        readPos += recording.samples * recordedChunkCount;

        if( readPos <= overlap )
            return;

        bool talkController_shouldSend = ( talkController == null || talkController.ShouldSend() );

        try
        {
            int sz = readPos - lastReadPos;
            int minSize = codecMgr.Codecs[ Codec ].GetSampleSize( audioFrequency );

            if( minSize == 0 )
                minSize = 100;

            int currentIDX = lastReadPos;
            int numClips = Mathf.FloorToInt( sz / minSize );

            for( int i = 0; i < numClips; i++ )
            {
                float[] d = USpeakPoolUtils.GetFloat( minSize );

                recording.GetData( d, currentIDX % recording.samples );
                if( talkController_shouldSend )
                {
                    talkTimer = 1f;
                    OnAudioAvailable( d );
                }

                USpeakPoolUtils.Return( d );

                currentIDX += minSize;
            }

            lastReadPos = currentIDX;
        }
        catch( System.Exception ) { }

        ProcessPendingEncodeBuffer();

        bool allowSend = true;
        if( SendingMode == SendBehavior.RecordThenSend && talkController != null )
        {
            allowSend = !talkController_shouldSend;
        }

        sendTimer += Time.deltaTime;
        if( sendTimer >= sendt && allowSend )
        {
            sendTimer = 0.0f;

            tempSendBytes.Clear();
            foreach( USpeakFrameContainer frame in sendBuffer )
            {
                tempSendBytes.AddRange( frame.ToByteArray() );
            }
            sendBuffer.Clear();

            if( tempSendBytes.Count > 0 )
            {
                audioHandler.USpeakOnSerializeAudio( tempSendBytes.ToArray() );
            }
        }
        #endif
    }

    #endregion

    #region Private Methods

    void StopPlaying()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().time = 0;
        index = 0;
        played = 0;
        received = 0;
        lastTime = 0;
    }

    void UpdateSettings()
    {
        if( !Application.isPlaying )
            return;

        settings = new USpeakSettingsData();
        settings.bandMode = BandwidthMode;
        settings.Codec = Codec;

        audioHandler.USpeakInitializeSettings( (short)settings.ToByte() );
    }

    Component FindSpeechHandler()
    {
        Component[] comp = GetComponents<Component>();
        foreach( Component c in comp )
        {
            if( c is ISpeechDataHandler )
                return c;
        }
        return null;
    }

    Component FindInputHandler()
    {
        Component[] comp = GetComponents<Component>();
        foreach( Component c in comp )
        {
            if( c is IUSpeakTalkController )
                return c;
        }
        return null;
    }

    void OnAudioAvailable( float[] pcmData )
    {
        if( UseVAD && !CheckVAD( pcmData ) )
            return;

        int chunkSize = 1280;
        List<float[]> audio_chunks = SplitArray( pcmData, chunkSize );

        foreach( float[] chunk in audio_chunks )
            pendingEncode.Add( chunk );
    }

    List<float[]> SplitArray( float[] array, int size )
    {
        List<float[]> chunksList = new List<float[]>();
        int skipCounter = 0;

        while( skipCounter < array.Length )
        {
            float[] chunk = array.Skip( skipCounter ).Take( size ).ToArray<float>();
            chunksList.Add( chunk );
            skipCounter += chunk.Length;
        }
        return chunksList;
    }

    void ProcessPendingEncodeBuffer()
    {
        int budget_ms = 100;
        float budget_s = (float)budget_ms / 1000.0f;

        float t = Time.realtimeSinceStartup;
        while( Time.realtimeSinceStartup <= t + budget_s && pendingEncode.Count > 0 )
        {
            float[] pcm = pendingEncode[ 0 ];
            pendingEncode.RemoveAt( 0 );
            ProcessPendingEncode( pcm );
        }
    }

    void ProcessPendingEncode( float[] pcm )
    {
        int s;
        byte[] b = USpeakAudioClipCompressor.CompressAudioData( pcm, 1, out s, lastBandMode, codecMgr.Codecs[ lastCodec ], LocalGain );

        USpeakFrameContainer cont = default( USpeakFrameContainer );
        cont.Samples = (ushort)s;
        cont.encodedData = b;

        sendBuffer.Add( cont );
    }

    int CalculateSamplesRead( int readPos )
    {
        if( readPos >= lastReadPos )
        {
            return readPos - lastReadPos;
        }
        else
        {
            return ( ( audioFrequency * 10 ) - lastReadPos ) + readPos;
        }
    }

    private float[] normalize( float[] samples, float magnitude )
    {
        float[] newSamples = new float[ samples.Length ];
        for( int i = 0; i < samples.Length; i++ )
            newSamples[ i ] = samples[ i ] / magnitude;
        return newSamples;
    }

    private float amplitude( float[] x )
    {
        float sum = 0.0f;
        for( int i = 0; i < x.Length; i++ )
        {
            sum = Mathf.Max( sum, Mathf.Abs( x[ i ] ) );
        }
        return sum;
    }

    bool CheckVAD( float[] samples )
    {
        if( Time.realtimeSinceStartup < lastVTime + vadHangover )
            return true;

        float max = 0.0f;
        foreach( float f in samples )
            max = Mathf.Max( max, Mathf.Abs( f ) );
        bool val = ( max >= VolumeThreshold );
        if( val )
            lastVTime = Time.realtimeSinceStartup;
        return val;
    }

    #endregion
}