using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles for UnityEditor")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/UnityEditor";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        Debug.Log("Bundle for UnityEditor Created Successfully");
    }

    [MenuItem("Assets/Build AssetBundles for Android")]
    static void BuildAllAssetBundlesforAndroid()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/Android";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        Debug.Log("Bundle for Android Created Successfully");

    }
    [MenuItem("Assets/Build AssetBundles for iOS")]
    static void BuildAllAssetBundlesIos()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/iOS";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
        Debug.Log("Bundle for iOS Created Successfully");

    }

    [MenuItem("Assets/Build AssetBundles for WebGl")]
    static void BuildAllAssetBundlesWebGl()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/WebGl";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.WebGL);
        Debug.Log("Bundle for WebGl Created Successfully");

    }
    [MenuItem("Assets/Build AssetBundles for Windows")]
    static void BuildAllAssetBundlesWindows()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/Windows";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        Debug.Log("Bundle for Windows Created Successfully");
    }


    [MenuItem("Assets/Build for All")]
    static void BuildForAllAtonce()
    {
        string assetBundleDirectory1 = "Assets/StreamingAssets/Windows";
        string assetBundleDirectory2 = "Assets/StreamingAssets/Android";
        string assetBundleDirectory3 = "Assets/StreamingAssets/iOs";
        string assetBundleDirectory4 = "Assets/StreamingAssets/WebGL";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory1);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory1, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        Debug.Log("Bundle for Windows Created Successfully");

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory2);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory2, BuildAssetBundleOptions.None, BuildTarget.Android);
        Debug.Log("Bundle for Android Created Successfully");

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory3);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory3, BuildAssetBundleOptions.None, BuildTarget.iOS);
        Debug.Log("Bundle for IOS Created Successfully");

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory4);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory4, BuildAssetBundleOptions.None, BuildTarget.WebGL);
        Debug.Log("Bundle for WebGL Created Successfully");

    }
}
