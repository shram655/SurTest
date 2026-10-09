using CodeStage.AntiCheat.Detectors;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MelonDetector : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ObscuredCheatingDetector.StartDetection(MAC_DET);
    }

    void MAC_DET()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            int FilesCount = Directory.GetFiles(Directory.GetCurrentDirectory()).Length;
            int CountDirectories = Directory.GetDirectories(Directory.GetCurrentDirectory()).Length;

            if (FilesCount > 6 || CountDirectories > 3)
            {
                MAC_DET();
            }
            if (File.Exists("version.dll"))
            {
                MAC_DET();
            }
            else if (File.Exists("dobby.dll"))
            {
                MAC_DET();
            }
        }
    }
}
