using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 加载AssetBundle里的资源
/// </summary>
public class IABResLoader:IDisposable
{
    private AssetBundle mABRes;

    public IABResLoader(AssetBundle assetBundle)
    {
        mABRes = assetBundle;
    }
    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object this[string resName]
    {
        get
        {
            if (this.mABRes == null || !this.mABRes.Contains(resName))
            {
                Debug.LogWarning("AssetBundle资源不存在");
                return null;
            }
            return mABRes.LoadAsset(resName);
            
        }
    }
    /// <summary>
    /// 加载多个资源
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object[] LoadResources(string resName)
    {
        if (this.mABRes == null || !this.mABRes.Contains(resName))
        {
            Debug.LogWarning("AssetBundle资源不存在");
            return null;
        }
        return mABRes.LoadAssetWithSubAssets(resName);
    }
    public void UnLoadRes(UnityEngine.Object resObj)
    {
        Resources.UnloadAsset(resObj);
    }
    /// <summary>
    /// 卸载AssetBundle内存镜像
    /// </summary>
    public void Dispose()
    {
        if (mABRes != null)
        {
            mABRes.Unload(false);
        }
    }

    public void DebugAllRes()
    {
        string[] assetNames = mABRes.GetAllAssetNames();
        for (int i = 0; i < assetNames.Length; i++)
        {
            Debug.Log("AB包里的资源名:" + assetNames[i]);
        }
        
    }
}

