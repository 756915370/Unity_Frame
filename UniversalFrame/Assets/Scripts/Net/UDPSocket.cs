using System;
using System.Collections.Generic;

public enum UDPEvent
{
    Initial=TCPEvent.MaxValue+1,
    SendTo,
    MaxValue
}

public class UDPSocket:INetBase
{
    private UDPSocketBase udpSocketBase;
    public override void ProcessEvent(MsgBase msg)
    {
        switch ((UDPEvent)msg.msgId)
        {
            case UDPEvent.Initial:
                UDPMsg udpMsg = (UDPMsg)msg;
                udpSocketBase = new UDPSocketBase();
                udpSocketBase.BindSocket(udpMsg.port, udpMsg.recvBufferLength, udpMsg.recvDelegate);

                break;
            case UDPEvent.SendTo:
                UDPSendMsg sendMsg = (UDPSendMsg)msg;
                udpSocketBase.SendData(sendMsg.ip, sendMsg.data, sendMsg.port);

                break;
          
            default:
                break;
        }
    }

    private void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)UDPEvent.Initial,
            (ushort)UDPEvent.SendTo
        };
        RegisterSelf(this, msgIds);
    }
   
}

