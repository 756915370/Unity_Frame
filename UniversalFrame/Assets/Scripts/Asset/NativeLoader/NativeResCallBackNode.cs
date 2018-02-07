using System;
using System.Collections.Generic;


public class NativeResCallBackNode
{
    public string SceneName { get; set; }
    public string BundleName { get; set; }
    public string ResName { get; set; }
    public ushort BackMsgId { get; set; }
    public bool IsSingle { get; set; }//是否获取单个资源
    public NativeResCallBackNode NextNode { get; set; }
    public NativeResCallBack CallBack { get; set; }
    public NativeResCallBackNode(bool isSingle, string sceneName, string bundleName, string resName
        , ushort backId, NativeResCallBack callBack, NativeResCallBackNode backNode)
    {
        IsSingle = isSingle;
        SceneName = sceneName;
        BundleName = bundleName;
        ResName = resName;
        BackMsgId = backId;
        NextNode = backNode;
        CallBack = callBack;
    }
    public void Dispose()
    {
        CallBack = null;
        NextNode = null;
        SceneName = null;
        BundleName = null;
        ResName = null;
    }
}

