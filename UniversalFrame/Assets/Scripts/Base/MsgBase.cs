using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgBase
{
    public ushort msgId;
    public MsgBase() { }
    
    public MsgBase(ushort id)
    {
        msgId = id;
    }
    public ManagerID GetManagerID()
    {
        int tempiD = msgId / FrameTools.MsgSpan;
        return (ManagerID)(tempiD * 3000);
    }
    

}
