using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using UnityEngine;
using Utils;

public class BuildHashGenerator : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    
    public void OnPreprocessBuild(BuildReport report)
    {
        // var hash = System.Guid.NewGuid().ToString();
        // File.WriteAllText("Assets/BuildHash.txt", hash);
        // PlayerSettings.bundleVersion = $"{Parameters.GAME_VERSION}-{hash}";
    }
}