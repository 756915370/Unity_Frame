using System;
using System.Collections.Generic;


public class UDPMsg:MsgBase
{
    public ushort port;
    public int recvBufferLength;
    public UDPSocketBase.UDPSocketDelegate recvDelegate;
    public UDPMsg(ushort id,ushort port,int recvLength,UDPSocketBase.UDPSocketDelegate socketDelegate)
    {
        this.msgId = id;
        this.port = port;
        this.recvBufferLength = recvLength;
        recvDelegate = socketDelegate;
    }
}

