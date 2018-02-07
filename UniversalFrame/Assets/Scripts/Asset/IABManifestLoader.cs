using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABManifestLoader
{
    private static IABManifestLoader instance = null;
    public static IABManifestLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new IABManifestLoader();
            }
            return instance;
        }
    }
    public AssetBundleManifest ABManifest { get; set; }
    public string ManifestPath { get; set; }
    public AssetBundle ManifestLoader { get; set; }
    private bool mIsLoadFinish;
    private IABManifestLoader()
    {
        ABManifest = null;
        ManifestLoader = null;
        mIsLoadFinish = false;
        ManifestPath = IPathTools.GetWWWAssetBundlePath() + "/" + IPathTools.GetPlatformFolderName(Application.platform);
    }
    public void SetManifestPath(string path)
    {
        ManifestPath = path;
    }
    public bool IsLoadFinish()
    {
        return mIsLoadFinish;
    }
    public IEnumerator LoadManifest()
    {
        WWW manifest = new WWW(ManifestPath);
        Debug.Log(ManifestPath);
        yield return manifest;
        if (!string.IsNullOrEmpty(manifest.error))
        {
            Debug.Log(manifest.error);
        }
        else
        {
            if (manifest.progress >= 1.0f)
            {
                ManifestLoader = manifest.assetBundle;
                ABManifest = ManifestLoader.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                mIsLoadFinish = true;
            }
        }
    }
    public string[] GetDependences(string name)
    {
        return ABManifest.GetAllDependencies(name);
    }
    /// <summary>
    /// 把AB包内存镜像和加载出的Asset都卸载
    /// </summary>
    public void UnLoadManifest()
    {
        ManifestLoader.Unload(true);
    }
}

