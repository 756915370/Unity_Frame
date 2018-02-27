using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LoadAssetBundleCallBack(string sceneName, string bundlePath);
//单个存取
public class AssetObj
{
    public List<UnityEngine.Object> mObjList;
    public AssetObj(params UnityEngine.Object[] objs)
    {
        mObjList = new List<UnityEngine.Object>();
        mObjList.AddRange(objs);
    }
    public void ReleaseObj()
    {
        for (int i = 0; i < mObjList.Count; i++)
        {
            if (mObjList.GetType() != typeof(GameObject))
            {
                //只能卸载非实例化出来的物体
                Resources.UnloadAsset(mObjList[i]);
            }
        }
    }
}
//存储一个AB包里的资源
public class AssetBundleObjs
{
    public Dictionary<string, AssetObj> mResObjDict;
    public AssetBundleObjs(string name,AssetObj obj)
    {
        mResObjDict = new Dictionary<string, AssetObj>();
        mResObjDict.Add(name, obj);
    }
    public void AddResObj(string name,AssetObj obj)
    {
        mResObjDict.Add(name, obj);
    }
    /// <summary>
    /// 释放全部资源
    /// </summary>
    public void ReleaseAllResObjs()
    {
        List<string> keys = new List<string>();
        keys.AddRange(mResObjDict.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            ReleaseResObj(keys[i]);
        }
    }
    /// <summary>
    /// 释放单个资源
    /// </summary>
    /// <param name="name"></param>
    public void ReleaseResObj(string name)
    {
        if (mResObjDict.ContainsKey(name))
        {
            AssetObj assetObj = mResObjDict[name];
            assetObj.ReleaseObj();
        }
        else
        {
            Debug.Log("要释放的资源不存在" + name);
        }
    }
    public List<UnityEngine.Object> GetResObj(string name)
    {
        if (mResObjDict.ContainsKey(name))
        {
            AssetObj assetObj = mResObjDict[name];
            return assetObj.mObjList;
        }
        else
        {
            Debug.Log("要获取的资源不存在" + name);
            return null;
        }
    }
}
/// <summary>
/// 对一个场景所有包的管理
/// </summary>
public class IABManager
{
    private string mSceneName;
    //存储每个包
    private Dictionary<string, IABRelationManager> mABRelationDict = new Dictionary<string, IABRelationManager>();
    private Dictionary<string, AssetBundleObjs> mAssetObjsDict = new Dictionary<string, AssetBundleObjs>();
    public IABManager(string sceneName)
    {
        mSceneName = sceneName;
    }
    public bool IsLoadingAssetBundle(string bundlePath)
    {
        if (mABRelationDict.ContainsKey(bundlePath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #region 释放资源
    /// <summary>
    /// 释放一个Bundle包里的一个资源
    /// </summary>
    /// <param name="bundlePath"></param>
    /// <param name="resName"></param>
    public void DisposeResObj(string bundlePath, string resName)
    {
        if (mAssetObjsDict.ContainsKey(bundlePath))
        {
            AssetBundleObjs assetBundleObjs = mAssetObjsDict[bundlePath];
            assetBundleObjs.ReleaseResObj(resName);
        }
    }
    /// <summary>
    /// 循环地处理依赖关系，如果没有包依赖自己则释放
    /// </summary>
    /// <param name="bundlePath"></param>
    public void DisposeBundle(string bundlePath)
    {
        if (mABRelationDict.ContainsKey(bundlePath))
        {
            IABRelationManager iABRelationManager = mABRelationDict[bundlePath];
            List<string> dependenceList = iABRelationManager.GetDependences();
            for (int i = 0; i < dependenceList.Count; i++)
            {
                if (mABRelationDict.ContainsKey(dependenceList[i]))
                {
                    IABRelationManager dependence = mABRelationDict[dependenceList[i]];
                    if (dependence.RemoveReference(bundlePath))
                    {
                        DisposeBundle(dependence.BundlePath);
                    }
                }
            }
            if (iABRelationManager.GetReferences().Count <= 0)
            {
                iABRelationManager.Dispose();
                mABRelationDict.Remove(bundlePath);
            }
        }
    }

    public void DisposeBundleAndObjs(string bundlePath)
    {
        if (mABRelationDict.ContainsKey(bundlePath))
        {
            IABRelationManager iABRelationManager = mABRelationDict[bundlePath];
            List<string> dependenceList = iABRelationManager.GetDependences();
            for (int i = 0; i < dependenceList.Count; i++)
            {
                if (mABRelationDict.ContainsKey(dependenceList[i]))
                {
                    IABRelationManager dependence = mABRelationDict[dependenceList[i]];
                    if (dependence.RemoveReference(bundlePath))
                    {
                        DisposeBundle(dependence.BundlePath);
                    }
                }
            }
            if (iABRelationManager.GetReferences().Count <= 0)
            {
                iABRelationManager.Dispose();
                mABRelationDict.Remove(bundlePath);
                DisposeResObjs(bundlePath);
            }
        }
    }
    /// <summary>
    /// 卸载所有Bundle包
    /// </summary>
    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(mABRelationDict.Keys);
        for (int i = 0; i < mABRelationDict.Count; i++)
        {
            IABRelationManager iABRelationManager = mABRelationDict[keys[i]];
            iABRelationManager.Dispose();
        }
        mABRelationDict.Clear();
    }
    /// <summary>
    /// 释放所有Bundle包的资源和卸载所有包
    /// </summary>
    public void DisposeAllBundleAndRes()
    {
        DisposeAllBundleObjs();
        DisposeAllBundle();
    }
    /// <summary>
    /// 释放一个Bundle包里的所有资源
    /// </summary>
    /// <param name="bundlePath"></param>
    public void DisposeResObjs(string bundlePath)
    {
        if (mAssetObjsDict.ContainsKey(bundlePath))
        {
            AssetBundleObjs assetBundleObjs = mAssetObjsDict[bundlePath];
            assetBundleObjs.ReleaseAllResObjs();
        }
        Resources.UnloadUnusedAssets();
    }
    /// <summary>
    /// 释放当前存储的所有Bundle包的资源
    /// </summary>
    public void DisposeAllBundleObjs()
    {
        List<string> keys = new List<string>();
        keys.AddRange(mAssetObjsDict.Keys);
        for (int i = 0; i < mAssetObjsDict.Count; i++)
        {
            DisposeResObjs(keys[i]);
        }
        mAssetObjsDict.Clear();
    }
    #endregion
    public void LoadAssetBundle(string bundlePath,LoaderProgress progress,LoadAssetBundleCallBack callBack)
    {
        if (!mABRelationDict.ContainsKey(bundlePath))
        {
            IABRelationManager iABRelation = new IABRelationManager();
            iABRelation.Initial(bundlePath, progress);
            mABRelationDict.Add(bundlePath, iABRelation);
            callBack.Invoke(mSceneName, bundlePath);
        }
        else
        {
            Debug.Log("已经包含BundlePath:" + bundlePath);
        }

    }
    private string[] GetDependences(string bundlePath)
    {
       return IABManifestLoader.Instance.GetDependences(bundlePath);
    }
    /// <summary>
    /// 加载assetbundle必须先加载manifest，然后加载依赖项
    /// </summary>
    /// <param name="bundlePath"></param>
    /// <returns></returns>
    public IEnumerator LoadAssetBundles(string bundlePath)
    {
        while (!IABManifestLoader.Instance.IsLoadFinish())
        {
            yield return null;
        }
        IABRelationManager iABRelationManager = mABRelationDict[bundlePath];
        string[] dependences = GetDependences(bundlePath);
        iABRelationManager.AddDependences(dependences);
        for (int i = 0; i < dependences.Length; i++)
        {
            yield return LoadAssetBundleDependences(dependences[i], bundlePath, iABRelationManager.GetLoaderProgress());
        }
        yield return iABRelationManager.LoadAssetBundle();
    }
    public IEnumerator LoadAssetBundleDependences(string bundlePath,string refName,LoaderProgress progress)
    {
        if (!mABRelationDict.ContainsKey(bundlePath))
        {
            IABRelationManager iABRelation = new IABRelationManager();
            iABRelation.Initial(bundlePath, progress);
            //记录被依赖对象
            if (refName != null)
            {
                iABRelation.AddReference(refName);
            }

            mABRelationDict.Add(bundlePath, iABRelation);
            yield return LoadAssetBundles(bundlePath);
        }
        else
        {
            if (refName != null)
            {
                IABRelationManager iABRelationManager = mABRelationDict[bundlePath];
                iABRelationManager.AddReference(bundlePath);
            }
        }
    }
    #region 由下层提供的功能
    public void DebugAssetBundle(string bundlePath)
    {
        if (mABRelationDict.ContainsKey(bundlePath))
        {
            IABRelationManager loader = mABRelationDict[bundlePath];
            loader.DebuggerAsset();
        }
    }

    public bool IsLoadingFinish(string bundlePath)
    {
        if (mABRelationDict.ContainsKey(bundlePath))
        {
           return mABRelationDict[bundlePath].IsBundleLoadFinish();
        }
        else
        {
            Debug.Log("没有包含对应的包:" + bundlePath);
            return false;
        }
    }
    public UnityEngine.Object GetSingleResource(string bundlePath,string resName)
    {
        //表示是否缓存了该物体
        if (mAssetObjsDict.ContainsKey(bundlePath))
        {
            AssetBundleObjs abObjs = mAssetObjsDict[bundlePath];
            List<UnityEngine.Object> objs = abObjs.GetResObj(resName);
            if (objs != null)
            {
                return objs[0];
            }
        }
        //表示已经加载过bundle
        if (mABRelationDict.ContainsKey(bundlePath))
        {
            IABRelationManager iABRelation = mABRelationDict[bundlePath];
            UnityEngine.Object obj = iABRelation.GetSingleResource(resName);
            AssetObj assetObj = new AssetObj(obj);
            if (mAssetObjsDict.ContainsKey(bundlePath))
            {
                AssetBundleObjs assetBundleObjs = mAssetObjsDict[bundlePath];
                mAssetObjsDict.Add(bundlePath, assetBundleObjs);
            }
            else
            {
                AssetBundleObjs assetBundleObjs = new AssetBundleObjs(resName, assetObj);
                mAssetObjsDict.Add(bundlePath, assetBundleObjs);
            }
            return obj;
        }
        return null;
    }
    public UnityEngine.Object[] GetResources(string bundlePath,string resName)
    {
        //表示是否缓存了该物体
        if (mAssetObjsDict.ContainsKey(bundlePath))
        {
            AssetBundleObjs abObjs = mAssetObjsDict[bundlePath];
            List<UnityEngine.Object> objs = abObjs.GetResObj(resName);
            if (objs != null)
            {
                return objs.ToArray();
            }
        }
        //表示已经加载过bundle
        if (mABRelationDict.ContainsKey(bundlePath))
        {
            IABRelationManager iABRelation = mABRelationDict[bundlePath];
            UnityEngine.Object[] objs = iABRelation.GetResources(resName);
            AssetObj assetObj = new AssetObj(objs);
            if (mAssetObjsDict.ContainsKey(bundlePath))
            {
                AssetBundleObjs assetBundleObjs = mAssetObjsDict[bundlePath];
                mAssetObjsDict.Add(bundlePath, assetBundleObjs);
            }
            else
            {
                AssetBundleObjs assetBundleObjs = new AssetBundleObjs(resName, assetObj);
                mAssetObjsDict.Add(bundlePath, assetBundleObjs);
            }
            return objs;
        }
        return null;
    }
    #endregion
}

