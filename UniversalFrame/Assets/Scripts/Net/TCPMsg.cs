using System;
using System.Collections.Generic;


public class TCPMsg:MsgBase
{
    public NetMsgBase netMsg;
    public TCPMsg(ushort id,NetMsgBase msgBase)
    {
        this.msgId = id;
        netMsg = msgBase;
    }

}

