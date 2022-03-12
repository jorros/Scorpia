using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildNumber : IPreprocessBuildWithReport
{
    public int callbackOrder => 1;

    public void OnPreprocessBuild(BuildReport report)
    {
        VersionScriptableObject so = ScriptableObject.CreateInstance<VersionScriptableObject>();

        so.BuildNumber = Application.version + DateTime.Now.ToString("yyyyMMdd");

        AssetDatabase.DeleteAsset("Assets/Resources/Build.asset");
        AssetDatabase.CreateAsset(so, "Assets/Resources/Build.asset");
        AssetDatabase.SaveAssets();
    }
}
