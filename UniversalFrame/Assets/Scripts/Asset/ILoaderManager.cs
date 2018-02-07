using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ILoaderManager : MonoBehaviour
{
    private Dictionary<string, IABSceneManager> mABManagerDict = new Dictionary<string, IABSceneManager>();
    private static ILoaderManager instance;
    public static ILoaderManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
        //第一步加载 IABManifest
        StartCoroutine(IABManifestLoader.Instance.LoadManifest());

    }
    /// <summary>
    /// 读取配置文件
    /// </summary>
    /// <param name="sceneName"></param>
    public void ReadConfig(string sceneName)
    {
        if (!mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager iABSceneManager = new IABSceneManager(sceneName);
            iABSceneManager.ReadConfigByName(sceneName);
            mABManagerDict.Add(sceneName, iABSceneManager);
        }
    }
    public void LoadCallBack(string sceneName,string bundleName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            StartCoroutine(sceneManager.LoadAssetSys(bundleName));
        }
        else
        {
            Debug.LogWarning("不存在Bundle名:" + bundleName);
        }
    }
    /// <summary>
    /// 提供加载功能
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundelName"></param>
    /// <param name="progress"></param>
    public void LoadAsset(string sceneName,string bundelName,LoaderProgress progress)
    {
        if (!mABManagerDict.ContainsKey(sceneName))
        {
            ReadConfig(sceneName);
        }
        IABSceneManager iABSceneManager = mABManagerDict[sceneName];
        iABSceneManager.LoadAsset(bundelName, progress, LoadCallBack);
    }

    #region 由下层提供的功能
    public string GetBundleRelativeName(string sceneName,string bundleName)
    {
        IABSceneManager sceneManager = mABManagerDict[sceneName];
        if (sceneManager != null)
        {
            return sceneManager.GetBundleRelativeName(bundleName);
        }
        else
        {
            return null;
        }
    }
    public UnityEngine.Object GetSingleResource(string sceneName,string bundleName,string resName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            return sceneManager.GetSingleResource(bundleName, resName);
        }
        else
        {
            Debug.LogWarning("没有包含该场景名:" + sceneName + " 包名:" + bundleName + " 资源名:" + resName);
            return null;
        }
    }
    public UnityEngine.Object[] GetResources(string sceneName,string bundleName,string resName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            return sceneManager.GetResources(bundleName, resName);
        }
        else
        {
            Debug.LogWarning("没有包含该场景名:" + sceneName + " 包名:" + bundleName + " 资源名:" + resName);
            return null;
        }
    }
    /// <summary>
    /// 释放一个AB包里的单个资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    public void DisposeResObj(string sceneName,string bundleName,string resName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            sceneManager.DisposeResObj(bundleName, resName);
        }
    }
    /// <summary>
    /// 释放一个AB包里的所有资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundelName"></param>
    public void DisposeBundleRes(string sceneName,string bundelName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            sceneManager.DisposeBundleRes(bundelName);
        }
    }
    /// <summary>
    /// 释放整个场景的资源
    /// </summary>
    /// <param name="sceneName"></param>
    public  void DisposeAllResObjs(string sceneName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            sceneManager.DisposeAllRes();
        }
    }
    /// <summary>
    /// 释放一个AB包
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    public void DisposeAssetBundle(string sceneName,string bundleName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            sceneManager.DisposeBundle(bundleName);
        }
    }
    /// <summary>
    /// 释放场景所有AB包
    /// </summary>
    /// <param name="sceneName"></param>
    public void DisposeAllAssetBundle(string sceneName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            sceneManager.DisposeAllBundle();
            System.GC.Collect();
        }
    }
    /// <summary>
    /// 释放场景所有AB包和资源
    /// </summary>
    /// <param name="sceneName"></param>
    public void DisposeAllAssetBundleAndRes(string sceneName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            sceneManager.DisposeAllBundleAndRes();
            System.GC.Collect();
        }
    }
    public void DebugAllAssetBundle(string sceneName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            sceneManager.DebugAllAsset();
        }
    }
    #endregion
    /// <summary>
    /// 正在加载的AB包是否加载完了
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool IsLoadingBundleFinish(string sceneName,string bundleName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            return sceneManager.IsLoadingFinish(bundleName);
        }
        return false;
    }
    /// <summary>
    /// 是否加载了AB包
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool IsLoadedAssetBundle(string sceneName,string bundleName)
    {
        if (mABManagerDict.ContainsKey(sceneName))
        {
            IABSceneManager sceneManager = mABManagerDict[sceneName];
            return sceneManager.IsLoadingAssetBundle(bundleName);
        }
        return false;
    }
    private void OnDestroy()
    {
        mABManagerDict.Clear();
        System.GC.Collect();
    }
}
