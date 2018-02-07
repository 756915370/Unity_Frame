using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager:ManagerBase
{
    public static AssetManager Instance
    {
        get { return instance; }
    }
    private static AssetManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManagerID() == ManagerID.AssetManager)
        {
            //ManagerBase 本模块自己
            Debug.Log("Asset模块处理自己的消息");
            ProcessEvent(msg);
        }
        else //MsgCenter
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }

}

