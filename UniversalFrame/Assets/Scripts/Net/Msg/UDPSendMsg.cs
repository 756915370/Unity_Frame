using System;
using System.Collections.Generic;


public class UDPSendMsg:MsgBase
{
    public string ip;
    public byte[] data;
    public ushort port;
    public UDPSendMsg(string ip,byte[] data,ushort port)
    {
        this.ip = ip;
        this.data = data;
        this.port = port;
    }
}

