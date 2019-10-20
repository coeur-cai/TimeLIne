using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public class BatchMode {
    static List<string> levels = new List<string>();
  
    public static void BuildTimeDemoApk()
    {
        foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
        {
            if (!item.enabled)
            {
                continue;
            }
            levels.Add(item.path);
        }
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        string res = BuildPipeline.BuildPlayer(levels.ToArray(), "TimeLine.apk", BuildTarget.Android, BuildOptions.None);
        if (res.Length>0)
        {
            throw new Exception("BuildPlayer failure:" + res);
        }
    }
	
}
