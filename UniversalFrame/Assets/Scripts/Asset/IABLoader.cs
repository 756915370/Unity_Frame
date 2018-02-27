using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public delegate void LoaderProgress(string bundle, float progress);
public delegate void LoadFinish(string bundle);
/// <summary>
/// 加载AssetBundle
/// </summary>
public class IABLoader
{
    private string mBundleName;
    private string mBundleWWWPath;
    private string mBundleFilePath;
    private WWW mLoader;
    private float mLoadProcess;
    private event LoaderProgress LoadProgressEvent;
    private event LoadFinish LoadFinishEvent;
    private IABResLoader mABResLoader;
    public IABLoader(LoaderProgress loaderProgress,LoadFinish loadFinish)
    {
        mBundleName = "";
        mBundleWWWPath = "";
        mLoadProcess = 0;
        LoadProgressEvent = loaderProgress;
        LoadFinishEvent = loadFinish;
        mABResLoader = null;
    }
    public void SetBundleName(string bundleName)
    {
        mBundleName = bundleName;
    }
    /// <summary>
    /// 赋值AB包路径
    /// </summary>
    /// <param name="path">完整路径</param>
    public void LoadResources(string pathWWW,string pathFile)
    {
        mBundleWWWPath = pathWWW;
        mBundleFilePath = pathFile;
    }

    public IEnumerator LoadAB()
    {
        //WWW类加载方法
        //mLoader = new WWW(mBundleWWWPath);
        //while (!mLoader.isDone)
        //{
        //    mLoadProcess = mLoader.progress;
        //    if (LoadProgressEvent != null)
        //    {
        //        LoadProgressEvent.Invoke(mBundleName, mLoadProcess);
        //    }
        //    yield return mLoader.progress;
        //    mLoadProcess = mLoader.progress;
        //}
        ////加载完成
        //if (mLoadProcess >= 1.0f)
        //{
        //    mABResLoader = new IABResLoader(mLoader.assetBundle);
        //    if (LoadProgressEvent != null)
        //    {
        //        LoadProgressEvent.Invoke(mBundleName, mLoadProcess);
        //    }
        //    if (LoadFinishEvent != null)
        //    {
        //        LoadFinishEvent(mBundleName);
        //    }

        //}
        //else
        //{
        //    Debug.LogError("加载Bundle错误" + mBundleName);
        //}
        //mLoader = null;
        //使用LoadFromFileAsync
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromFileAsync(mBundleFilePath);
        yield return createRequest;
        if (createRequest.assetBundle != null)
        {
            mLoadProcess = 1;
            mABResLoader = new IABResLoader(createRequest.assetBundle);
            if (LoadProgressEvent != null)
            {
                LoadProgressEvent.Invoke(mBundleName, mLoadProcess);
            }
            if (LoadFinishEvent != null)
            {
                LoadFinishEvent(mBundleName);
            }
        }
        else
        {
            Debug.LogError("加载Bundle错误" + mBundleName);
        }
        mLoader = null;
    }
    public void DebugLoader()
    {
        if (mLoader != null)
        {
            mABResLoader.DebugAllRes();
        }
    }
    #region 由下层提供的功能
    /// <summary>
    /// 获取单个资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UnityEngine.Object GetResource(string name)
    {
        if (mABResLoader != null)
        {
            return mABResLoader[name];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 获取多个资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UnityEngine.Object[] GetResources(string name)
    {
        if (mABResLoader != null)
        {
            return mABResLoader.LoadResources(name);
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 卸载单个资源
    /// </summary>
    /// <param name="resObj"></param>
    public void UnLoadRes(UnityEngine.Object resObj)
    {
        if (mABResLoader != null)
        {
            mABResLoader.UnLoadRes(resObj);
        }
    }
    /// <summary>
    /// 卸载内存镜像
    /// </summary>
    public void Dispose()
    {
        if (mABResLoader != null)
        {
            mABResLoader.Dispose();
            mABResLoader = null;
        }
    }
    
    #endregion
}

