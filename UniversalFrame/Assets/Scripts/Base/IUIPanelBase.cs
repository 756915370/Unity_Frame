using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUIPanelBase : IMonoBase
{
    public ushort[] msgIds;
    public void RegisterSelf(IMonoBase mono,params ushort[] msgs)
    {
        UIManager.Instance.RegisterMsg(mono, msgs);
    }
    public void UnRegisterSelf(IMonoBase mono,params ushort[] msgs)
    {
        UIManager.Instance.UnRegisterMsg(mono, msgs);
    }
  
    public void SendMsg(MsgBase msg)
    {
        UIManager.Instance.SendMsg(msg);
    }
    private void OnDestroy()
    {
        if (msgIds != null)
        {
            UnRegisterSelf(this, msgIds);
        }
    }
}
