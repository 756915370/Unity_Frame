using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine;

public class IABSceneManager
{

    private IABManager mABManager;
    public IABSceneManager(string sceneName)
    {
        mABManager = new IABManager(sceneName);
    }
    private IABManager mAbManager;
    private Dictionary<string, string> mAllAssetDict = new Dictionary<string, string>();
    /// <summary>
    /// 通过场景名读取配置文件
    /// </summary>
    /// <param name="fileName">场景名字</param>
    public void ReadConfigByName(string sceneName)
    {
        string textFileName = "Record.txt";
        string path = IPathTools.GetAssetBundlePath() + "/" + sceneName + textFileName;
        mABManager = new IABManager(sceneName);
        ReadConfigByPath(path);
    }
    /// <summary>
    /// 给定路径读取文件，第一行是总行数
    /// </summary>
    /// <param name="path"></param>
    private void ReadConfigByPath(string path)
    {
        FileStream fs = new FileStream(path,FileMode.Open);
        StreamReader br = new StreamReader(fs);
        string line = br.ReadLine();
        int allCount = int.Parse(line);
        for (int i = 0; i < allCount; i++)
        {
            string lineInfo = br.ReadLine();
            string[] infos = lineInfo.Split(" ".ToCharArray());
            mAllAssetDict.Add(infos[0], infos[1]);
        }
        br.Close();
        fs.Close();
        foreach(string key in mAllAssetDict.Keys)
        {
            Debug.Log(mAllAssetDict[key]);
        }
    }
    public void LoadAsset(string bundleName,LoaderProgress progress,LoadAssetBundleCallBack callBack)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            string bundlePath = mAllAssetDict[bundleName];
            mABManager.LoadAssetBundle(bundlePath, progress, callBack);
        }
        else
        {
            Debug.LogWarning("没有包含Bundle名:" + bundleName);
        }
    }
    public string GetBundleRelativeName(string bundleName)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            return mAllAssetDict[bundleName];
        }
        else
        {
            return null;
        }
    }
    #region 由下层提供功能
    public IEnumerator LoadAssetSys(string bundleName)
    {
        yield return mABManager.LoadAssetBundles(bundleName);
    }

    public UnityEngine.Object GetSingleResource(string bundleName,string resName)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            return mABManager.GetSingleResource(mAllAssetDict[bundleName], resName);
        }
        else
        {
            Debug.LogWarning("没有包含Bundle名:" + bundleName);
            return null;
        }
    }
    public UnityEngine.Object[] GetResources(string bundleName,string resName)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            return mABManager.GetResources(mAllAssetDict[bundleName], resName);
        }
        else
        {
            Debug.LogWarning("没有包含Bundle名:" + bundleName);
            return null;
        }
    }
    /// <summary>
    /// 释放单个资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    public void DisposeResObj(string bundleName,string resName)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            mAbManager.DisposeResObj(mAllAssetDict[bundleName], resName);
        }
        else
        {
            Debug.LogWarning("没有包含Bundle名:" + bundleName);
        }
    }
    public void DisposeBundleRes(string bundleName)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            mAbManager.DisposeResObjs(mAllAssetDict[bundleName]);
        }
        else
        {
            Debug.LogWarning("没有包含Bundle名:" + bundleName);
        }
    }
    public void DisposeAllRes()
    {
        mAbManager.DisposeAllBundleObjs();
    }
    public void DisposeBundle(string bundleName)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            mAbManager.DisposeBundle(bundleName);
        }
        else
        {
            Debug.LogWarning("没有包含Bundle名:" + bundleName);
        }
    }
    public void DisposeAllBundleAndRes()
    {
        mAbManager.DisposeAllBundleAndRes();
        mAllAssetDict.Clear();
    }
    public void DisposeAllBundle()
    {
        mAbManager.DisposeAllBundle();
        mAllAssetDict.Clear();
    }
    public void DebugAllAsset()
    {
        List<string> keys = new List<string>();
        keys.AddRange(mAllAssetDict.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            mABManager.DebugAssetBundle(mAllAssetDict[keys[i]]);
        }
    }
    public bool IsLoadingFinish(string bundleName)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            return mABManager.IsLoadingFinish(mAllAssetDict[bundleName]);
        }
        else
        {
            Debug.LogWarning("没有包含Bundle包:" + bundleName);
            return false;
        }
    }
    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (mAllAssetDict.ContainsKey(bundleName))
        {
            return mABManager.IsLoadingAssetBundle(mAllAssetDict[bundleName]);
        }
        else
        {
            Debug.LogWarning("没有包含Bundle包:" + bundleName);
            return false;
        }
    }
    #endregion
}

