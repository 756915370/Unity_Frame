using System;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class LuaAndCMsgCenter : IMonoBase
{
    LuaFunction luaFunctionCallBack = null;
    private static LuaAndCMsgCenter instance;
    public LuaAndCMsgCenter Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    public override void ProcessEvent(MsgBase msg)
    {
        //从网络来的msg和从框架来的数据不一样
        if (luaFunctionCallBack != null)
        {
            //从网络来的msg
            if (msg.GetState() != 127)
            {
                NetMsgBase netMsgBase = msg as NetMsgBase;
                byte[] proto = netMsgBase.GetProtoBuffer();
                LuaByteBuffer buffer = new LuaByteBuffer(proto);
                luaFunctionCallBack.Call(true, netMsgBase.msgId, netMsgBase.GetState(), buffer);
            }
            else
            {
                luaFunctionCallBack.Call(false, msg);
            }
        }

    }
    /// <summary>
    /// 注册回调函数
    /// </summary>
    /// <param name="luaFunction"></param>
    public void SettingLuaCallBack(LuaFunction luaFunction)
    {
        luaFunctionCallBack = luaFunction;
    }
}

