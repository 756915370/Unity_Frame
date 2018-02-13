using System;
using System.Collections.Generic;


public abstract class INetBase:IMonoBase
{
    public ushort[] msgIds;
    public void RegisterSelf(IMonoBase mono, params ushort[] msgs)
    {
        NetManager.Instance.RegisterMsg(mono, msgs);
    }
    public void UnRegisterSelf(IMonoBase mono, params ushort[] msgs)
    {
        NetManager.Instance.UnRegisterMsg(mono, msgs);
    }

    public void SendMsg(MsgBase msg)
    {
        NetManager.Instance.SendMsg(msg);
    }
    private void OnDestroy()
    {
        if (msgIds != null)
        {
            UnRegisterSelf(this, msgIds);
        }
    }
}

