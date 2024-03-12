using System;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundle
{
    [MenuItem("Assets/Create Asset Bundles")]
    private static void CreateAllAssetBundles()
    {
        string assetBundlerDirPath = Application.dataPath + "/AssetBundles";
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundlerDirPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
        }

    }
}
