using UnityEngine;

// Заглушка для Microphone, чтобы избежать ошибок компиляции
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBGL && !UNITY_IOS && !UNITY_ANDROID
namespace UnityEngine
{
    public static class Microphone
    {
        public static string[] devices
        {
            get { return new string[] { "Default Device" }; }
        }
        
        public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency)
        {
            Debug.LogWarning("Microphone.Start вызван, но микрофон не доступен на этой платформе");
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
}
#endif