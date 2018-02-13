using System;
using System.Collections.Generic;
using UnityEngine;

public class NetManager:ManagerBase
{
    public static NetManager Instance
    {
        get { return instance; }
    }
    private static NetManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManagerID() == ManagerID.NetManager)
        {
            //ManagerBase 本模块自己
            Debug.Log("Net模块处理自己的消息");
            ProcessEvent(msg);
        }
        else //MsgCenter
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }
}

