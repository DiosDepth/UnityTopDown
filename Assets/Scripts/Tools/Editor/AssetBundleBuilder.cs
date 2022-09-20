using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBundleBuilder : Editor
{ 
    private string path;
    [MenuItem("TDGameTools/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string path = System.IO.Directory.GetCurrentDirectory() + "\\Assets\\Resources\\AssetBundles";
        AssetBundleManifest temp_manifest =  BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
        Debug.Log("AssetBundles was build in : " + path);
        Debug.Log("ManiFestInfo count of bundles : [" + temp_manifest.GetAllAssetBundles().Length + "]");
    }
}
