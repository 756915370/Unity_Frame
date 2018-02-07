using System;
using System.Collections.Generic;


public abstract class IAssetBase : IMonoBase
{
    public ushort[] msgIds;
    public void RegisterSelf(IMonoBase mono, params ushort[] msgs)
    {
        AssetManager.Instance.RegisterMsg(mono, msgs);
    }
    public void UnRegisterSelf(IMonoBase mono, params ushort[] msgs)
    {
        AssetManager.Instance.UnRegisterMsg(mono, msgs);
    }

    public void SendMsg(MsgBase msg)
    {
        AssetManager.Instance.SendMsg(msg);
    }
    private void OnDestroy()
    {
        if (msgIds != null)
        {
            UnRegisterSelf(this, msgIds);
        }
    }
}

