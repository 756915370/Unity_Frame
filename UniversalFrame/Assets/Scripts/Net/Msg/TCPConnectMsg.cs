using System;
using System.Collections.Generic;


public class TCPConnectMsg:MsgBase
{
    public string ip;
    public ushort port;
    public TCPConnectMsg(ushort id,string ip,ushort port)
    {
        this.msgId = id;
        this.ip = ip;
        this.port = port;
    }
}

