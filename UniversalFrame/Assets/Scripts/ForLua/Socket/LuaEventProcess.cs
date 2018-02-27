using System;
using System.Collections.Generic;
using UnityEngine;

public class LuaEventProcess : IMonoBase
{
    private IMonoBase monoChild;
    private static LuaEventProcess instance;
    public static LuaEventProcess Instance
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
    public void SettingChild(IMonoBase child)
    {
        monoChild = child;
    }
    public override void ProcessEvent(MsgBase msg)
    {
        if (monoChild != null)
        {
            monoChild.ProcessEvent(msg);
        }
    }
}

