using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class LuaCallBackNode
{
    public string resName;
    public string bundleName;// share/gameObject.ld
    public string sceneName;
    public bool isSingle;
    public LuaCallBackNode nextNode;
    public LuaFunction luaFunction;
    public LuaCallBackNode(string resName,string bundleName,string sceneName,LuaFunction function,bool isSingle,LuaCallBackNode backNode)
    {
        this.resName = resName;
        this.bundleName = bundleName;
        this.sceneName = sceneName;
        this.isSingle = isSingle;
        this.luaFunction = function;
        this.nextNode = backNode;
    }
    public void Dispose()
    {
        this.resName = null;
        this.bundleName = null;
        this.sceneName = null;
        this.luaFunction.Dispose();
        this.nextNode = null;
    }
}
public class LuaCallBackManager
{
    Dictionary<string, LuaCallBackNode> manager = null;
    public LuaCallBackManager()
    {
        manager = new Dictionary<string, LuaCallBackNode>();

    }
    public void AddBundleCallBack(string bundle,LuaCallBackNode node)
    {
        if (manager.ContainsKey(bundle))
        {
            LuaCallBackNode topNode = manager[bundle];
            while (topNode.nextNode != null)
            {
                topNode = topNode.nextNode;
            }
            topNode.nextNode = node;
        }
        else
        {
            manager.Add(bundle, node);
        }
    }
    public void Dispose(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            LuaCallBackNode topNode = manager[bundle];
            while (topNode.nextNode != null)
            {
                LuaCallBackNode curNode = topNode;
                topNode = topNode.nextNode;
                curNode.Dispose();
            }
            topNode.Dispose();
            manager.Remove(bundle);
        }
    }
    public void CallBackLua(string bundle)
    {
        if (manager.ContainsKey(bundle))
        {
            LuaCallBackNode topNpode = manager[bundle];
            do
            {
                if (topNpode.isSingle)
                {
                    UnityEngine.Object tempObj = ILoaderManager.Instance.GetSingleResource(topNpode.sceneName, topNpode.bundleName, topNpode.resName);
                    topNpode.luaFunction.Call(topNpode.sceneName, topNpode.bundleName, topNpode.resName, tempObj);
                }
                else
                {
                    UnityEngine.Object[] tempObjs = ILoaderManager.Instance.GetResources(topNpode.sceneName, topNpode.bundleName, topNpode.resName);
                    topNpode.luaFunction.Call(topNpode.sceneName, topNpode.bundleName, topNpode.resName, tempObjs);
                }
                topNpode = topNpode.nextNode;
            } while (topNpode != null);
        }
    }
}
public class LuaLoadReses
{
    private static LuaLoadReses instance = null;
    public static LuaLoadReses Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LuaLoadReses();
            }
            return instance;
        }
    }
    private LuaCallBackManager luaCallManager = null;
    public LuaCallBackManager LuaCallBackManager
    {
        get
        {
            return luaCallManager;
        }
    }
    private LuaLoadReses()
    {
        luaCallManager = new LuaCallBackManager();
    }
    public void GetResources(string sceneName,string bundleName,string res, bool isSingle,LuaFunction luaFunction)
    {
        // 没有加载
        if (!ILoaderManager.Instance.IsLoadedAssetBundle(sceneName, bundleName))
        {
            Debug.Log("开始加载");
            ILoaderManager.Instance.LoadAsset(sceneName, bundleName, LoaderProgress);
            string bundleFullName = ILoaderManager.Instance.GetBundleRelativeName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                LuaCallBackNode node = new LuaCallBackNode(res,bundleName,sceneName,luaFunction,isSingle,null);
                luaCallManager.AddBundleCallBack(bundleFullName, node);
                Debug.Log("Get Resource==" + bundleFullName);
            }
            else
            {
                Debug.LogWarning("Bundle名为空:" + bundleName);
            }

        }
        //表示已经加载完成
        else if (ILoaderManager.Instance.IsLoadingBundleFinish(sceneName, bundleName))
        {
            if (isSingle)
            {
                Debug.Log("加载完成");
                UnityEngine.Object obj = ILoaderManager.Instance.GetSingleResource(sceneName, bundleName, res);
                luaFunction.Call(sceneName, bundleName, res, obj);
                //ResBackMsg.Changer(backid, obj);
                //SendMsg(ResBackMsg);
            }
            else
            {
                UnityEngine.Object[] objs = ILoaderManager.Instance.GetResources(sceneName, bundleName, res);
                luaFunction.Call(sceneName, bundleName, res, objs);
                //ResBackMsg.Changer(backid, objs);
                //SendMsg(ResBackMsg);
            }
        }
        //已经加载但是没有加载完把命令存起来
        else
        {
            Debug.Log("加载中");
            string bundleFullName = ILoaderManager.Instance.GetBundleRelativeName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                LuaCallBackNode node = new LuaCallBackNode(res, bundleName, sceneName, luaFunction, isSingle, null);
                luaCallManager.AddBundleCallBack(bundleFullName, node);
            }
            else
            {
                Debug.LogWarning("Bundle名为空:" + bundleName);
            }
        }
    }
    private void LoaderProgress(string bundleName, float progress)
    {
        Debug.Log("bundleName:" + bundleName + " bundle name progress==" + progress);
        if (progress >= 1.0f)
        {
            luaCallManager.CallBackLua(bundleName);
            luaCallManager.Dispose(bundleName);
        }
    }
    /// <summary>
    /// 释放单个资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    public void UnLoadResObj(string sceneName,string bundleName,string resName)
    {
        ILoaderManager.Instance.DisposeResObj(sceneName, bundleName, resName);
    }
    /// <summary>
    /// 释放一个AB包的资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="bundleName"></param>
    public void UnLoadBundleObjs(string sceneName,string bundleName)
    {
        ILoaderManager.Instance.DisposeBundleRes(sceneName, bundleName);
    }

    public void UnLoadSingleBundle(string sceneName,string bundleName)
    {
        ILoaderManager.Instance.DisposeAssetBundle(sceneName, bundleName);
    }

    public void UnLoadBundleAndObjs(string sceneName,string bundleName)
    {
        //TODO 卸载AB包加载出的GameObejct和AB包
    }
}

