using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
/// <summary>
/// 包依赖关系的管理
/// </summary>
public class IABRelationManager
{
    /// <summary>
    /// A依赖B,C被D,E依赖 mDependenceBundles={B,C},mReferBundles={D,E}
    /// </summary>
    //存储依赖关系
    private List<string> mDependenceBundles = null;
    //存储被依赖关系
    private List<string> mReferBundles = null;
    private bool mIsLoadFinish;
    private IABLoader mABLoader;
    private string mbundlePath;
    private LoaderProgress mLoaderProgress;
    public string BundlePath
    {
        get { return mbundlePath; }
    }
    public LoaderProgress GetLoaderProgress()
    {
        return mLoaderProgress;
    }
    public IABRelationManager()
    {
        mDependenceBundles = new List<string>();
        mReferBundles = new List<string>();
    }
    public void BundleLoadFinish(string bundlePath)
    {
        mIsLoadFinish = true;
    }
    public bool IsBundleLoadFinish()
    {
        return mIsLoadFinish;
    }
    public void Initial(string bundlePath, LoaderProgress progress)
    {
        mIsLoadFinish = false;
        mbundlePath = bundlePath;
        mLoaderProgress = progress;
        mABLoader = new IABLoader(progress, BundleLoadFinish);
        mABLoader.SetBundleName(bundlePath);
        string fullPath = IPathTools.GetWWWAssetBundlePath() + "/" + bundlePath;
        mABLoader.LoadResources(fullPath);
    }
    public void AddReference(string bundlePath)
    {
        mReferBundles.Add(bundlePath);
    }
    public void AddDependences(string[] depence)
    {
        if (depence.Length > 0)
        {
            mDependenceBundles.AddRange(depence);
        }
    }
    public List<string> GetReferences()
    {
        return mReferBundles;
    }
  
    public List<string> GetDependences()
    {
        return mDependenceBundles;
    }
    public void RemoveDependence(string bundlePath)
    {
        mDependenceBundles.Remove(bundlePath);
    }
    /// <summary>
    /// 移除被依赖AB包名称
    /// </summary>
    /// <param name="bundlePath"></param>
    /// <returns>返回true表示没有任何包依赖自己然后释放了自己</returns>
    public bool RemoveReference(string bundlePath)
    {
        mReferBundles.Remove(bundlePath);
        if (mReferBundles.Count <= 0)
        {
            Dispose();
            return true;
        }
        return false;
    }
    #region 由下层提供的功能

    public void DebuggerAsset()
    {
        if (mABLoader != null)
        {
            mABLoader.DebugLoader();
        }
        else
        {
            Debug.Log("资源不存在");
        }
    }
    public IEnumerator LoadAssetBundle()
    {
        yield return mABLoader.LoadAB();
    }
    /// <summary>
    /// 卸载内存镜像
    /// </summary>
    public void Dispose()
    {
        mABLoader.Dispose();
    }

    public UnityEngine.Object GetSingleResource(string resName)
    {
        return mABLoader.GetResource(resName);
    }
    public UnityEngine.Object[] GetResources(string resName)
    {
        return mABLoader.GetResources(resName);
    }
    #endregion
}

