using System;
using UnityEditor;
using UnityEngine;

public class GenCfgAudio
{
    static string dataOutputDir = "audioinfo/";

    [MenuItem("DevTool/GenCfg/Audio/Base")]
    public static void AudioBase ()
    {
        Debug.Log("gen audio base");
    }
}