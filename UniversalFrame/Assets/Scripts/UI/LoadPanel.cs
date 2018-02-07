using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPanel : IUIPanelBase
{
    public override void ProcessEvent(MsgBase msg)
    {
        switch (msg.msgId)
        {
            case (ushort)UIEventID.Load:

                break;
            case (ushort)UIEventID.Register:

                break;
        }

    }
    private void Awake()
    {
        msgIds = new ushort[]
       {
           (ushort)UIEventID.Load,
           (ushort)UIEventID.Register
       };
        RegisterSelf(this, msgIds);
    }
    private void Start()
    {
        UIManager.Instance.GetGameObjectByName("LoadButton").GetComponent<UIBehavior>().AddButtonListener(ButtonClick);
    }
    public void ButtonClick()
    {
        Debug.Log("Button事件");
        MsgBase tempBase = new MsgBase((ushort)UIEventID.Load);
        SendMsg(tempBase);
    }
}
