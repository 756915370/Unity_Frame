using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void NativeResCallBack(NativeResCallBackNode callBackNode);


public class NativeResLoader : IAssetBase
{
    private AssetResBackMsg mAssetResMsg = null;
    private AssetResBackMsg ResBackMsg
    {
        get
        {
            if (mAssetResMsg == null)
            {
                mAssetResMsg = new AssetResBackMsg();
            }
            return mAssetResMsg;
        }
    }
    private NativeResCallBackManager mCallBackManager = null;
    public NativeResCallBackManager CallBackManager
    {
        get
        {
            if (mCallBackManager == null)
            {
                mCallBackManager = new NativeResCallBackManager();
            }
            return mCallBackManager;
        }
    }

    private void Awake()
    {
        msgIds = new ushort[]
        {
               (ushort)AssetEvent.GetRes ,
               (ushort)AssetEvent.ReleaseSingleObj,
               (ushort)AssetEvent.ReleaseBundleObjs,
               (ushort)AssetEvent.ReleaseSceneObj,
               (ushort)AssetEvent.ReleaseSingleBundle,
               (ushort)AssetEvent.ReleaseSceneBundle,
               (ushort)AssetEvent.ReleaseAll,
               (ushort)AssetEvent.InitObj
        };
        RegisterSelf(this, msgIds);
    }
    private void Start()
    {
        MsgBase msg = new AssetResMsg(true, (ushort)AssetEvent.GetRes, "scene1", "GameObject", "Cube.prefab", (ushort)AssetEvent.InitObj);
        ProcessEvent(msg);
    }
    public override void ProcessEvent(MsgBase msg)
    {
        if (msg is AssetResMsg)
        {
            ProcessAssetResMsg(msg as AssetResMsg);
        }else if(msg is AssetResBackMsg)
        {
            ProcressAssetResBackMsg(msg as AssetResBackMsg);
        }
    }
    /// <summary>
    /// 处理AssetResBackMsg消息
    /// </summary>
    private void ProcressAssetResBackMsg(AssetResBackMsg msg)
    {
        switch ((AssetEvent)msg.msgId)
        {
            case AssetEvent.InitObj:
                for (int i = 0; i < msg.ResObjs.Length; i++)
                {
                    GameObject.Instantiate(msg.ResObjs[i], Vector3.one, Quaternion.identity);
                }
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 处理AssetResMsg消息
    /// </summary>
    /// <param name="assetResMsg"></param>
    private void ProcessAssetResMsg(AssetResMsg assetResMsg)
    {
        switch ((AssetEvent)assetResMsg.msgId)
        {
            case AssetEvent.GetRes:
                GetResources(assetResMsg.SceneName, assetResMsg.BundleName, assetResMsg.ResName, assetResMsg.IsSingle, assetResMsg.BackMsgId);
                break;
            case AssetEvent.ReleaseSingleObj:
                ILoaderManager.Instance.DisposeResObj(assetResMsg.SceneName, assetResMsg.BundleName, assetResMsg.ResName);
                break;
            case AssetEvent.ReleaseBundleObjs:
                ILoaderManager.Instance.DisposeBundleRes(assetResMsg.SceneName, assetResMsg.BundleName);
                break;
            case AssetEvent.ReleaseSceneObj:
                ILoaderManager.Instance.DisposeAllResObjs(assetResMsg.SceneName);
                break;
            case AssetEvent.ReleaseSingleBundle:
                ILoaderManager.Instance.DisposeAssetBundle(assetResMsg.SceneName, assetResMsg.BundleName);
                break;
            case AssetEvent.ReleaseSceneBundle:
                ILoaderManager.Instance.DisposeAllAssetBundle(assetResMsg.SceneName);
                break;
            case AssetEvent.ReleaseAll:
                ILoaderManager.Instance.DisposeAllAssetBundleAndRes(assetResMsg.SceneName);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Node回调，在加载Bundel包后调用
    /// </summary>
    /// <param name="node"></param>
    public void SendToBackMsg(NativeResCallBackNode node)
    {
        if (node.IsSingle)
        {
            UnityEngine.Object obj = ILoaderManager.Instance.GetSingleResource(node.SceneName, node.BundleName, node.ResName);
            this.ResBackMsg.Changer(node.BackMsgId,obj);
            SendMsg(ResBackMsg);
        }
        else
        {
            UnityEngine.Object[] objs = ILoaderManager.Instance.GetResources(node.SceneName, node.BundleName, node.ResName);
            this.ResBackMsg.Changer(node.BackMsgId, objs);
            SendMsg(ResBackMsg);
        }
    }
    private void LoaderProgress(string bundleName,float progress)
    {
        Debug.Log("bundleName:"+bundleName+ " bundle name progress==" + progress);
        if (progress >= 1.0f)
        {
            CallBackManager.CallBackRes(bundleName);
            CallBackManager.Dispose(bundleName);
        }
    }
    private void GetResources(string sceneName,string bundleName,string resName,bool isSingle,ushort backid)
    {
        //没有加载
        if (!ILoaderManager.Instance.IsLoadedAssetBundle(sceneName, bundleName))
        {
            Debug.Log("开始加载");
            ILoaderManager.Instance.LoadAsset(sceneName, bundleName, LoaderProgress);
            string bundleFullName = ILoaderManager.Instance.GetBundleRelativeName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                NativeResCallBackNode node = new NativeResCallBackNode(isSingle, sceneName, bundleName, resName, backid, SendToBackMsg, null);
                CallBackManager.AddBundle(bundleFullName, node);
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
                UnityEngine.Object obj = ILoaderManager.Instance.GetSingleResource(sceneName, bundleName, resName);
                ResBackMsg.Changer(backid, obj);
                SendMsg(ResBackMsg);
            }
            else
            {
                UnityEngine.Object[] objs = ILoaderManager.Instance.GetResources(sceneName, bundleName, resName);
                ResBackMsg.Changer(backid, objs);
                SendMsg(ResBackMsg);
            }
        }
        //已经加载但是没有加载完把命令存起来
        else
        {
            Debug.Log("加载中");
            string bundleFullName = ILoaderManager.Instance.GetBundleRelativeName(sceneName, bundleName);
            if (bundleFullName != null)
            {
                NativeResCallBackNode node = new NativeResCallBackNode(isSingle, sceneName, bundleFullName, resName, backid, SendToBackMsg, null);
                CallBackManager.AddBundle(bundleFullName, node);
            }
            else
            {
                Debug.LogWarning("Bundle名为空:" + bundleName);
            }
        }
    }
}
