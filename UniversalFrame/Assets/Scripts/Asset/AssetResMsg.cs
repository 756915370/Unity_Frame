using System;
using System.Collections.Generic;

public enum AssetEvent
{
    GetRes = ManagerID.AssetManager + 1,
    ReleaseSingleObj,
    ReleaseBundleObjs,
    ReleaseSceneObj,
    ReleaseSingleBundle,
    ReleaseSceneBundle,
    ReleaseAll,
    InitObj
}
public class AssetResMsg : MsgBase
{
    public string SceneName { get; set; }
    public string BundleName { get; set; }
    public string ResName { get; set; }
    public ushort BackMsgId { get; set; }
    public bool IsSingle { get; set; }//是否获取单个资源
    public AssetResMsg(bool isSingle, ushort id,string sceneName,string bundleName,string resName,ushort backId) : base(id)
    {
        IsSingle = isSingle;
        SceneName = sceneName;
        BundleName = bundleName;
        ResName = resName;
        BackMsgId = backId;
    }
    
}
public class AssetResBackMsg : MsgBase
{
    public UnityEngine.Object[] ResObjs { get; set; }
    public AssetResBackMsg()
    {
        ResObjs = null;
    }
    public void Changer(ushort id,params UnityEngine.Object[] value)
    {
        msgId = id;
        ResObjs = value;
    }
    public void Changer(ushort id)
    {
        msgId = id;
    }
    public void Changer(params UnityEngine.Object[] value)
    {
        ResObjs = value;
    }
}


