using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TCPEvent
{
    TcpConnect = ManagerID.NetManager + 1,
    TcpSendMsg,
    
    MaxValue
}

public class TcoSocket : INetBase {

    private NetWorkToServer netWorkToServer = null;
    public override void ProcessEvent(MsgBase msg)
    {
        switch ((TCPEvent)msg.msgId)
        {
            case TCPEvent.TcpConnect:
                TCPConnectMsg connectMsg = (TCPConnectMsg)msg;
                netWorkToServer = new NetWorkToServer(connectMsg.ip, connectMsg.port);
                break;
            case TCPEvent.TcpSendMsg:
                TCPMsg sendMsg = (TCPMsg)msg;
                netWorkToServer.PutSendMsgPool(sendMsg.netMsg);

                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        msgIds = new ushort[]
        {
            (ushort)TCPEvent.TcpSendMsg,
            (ushort)TCPEvent.TcpConnect
        };
        RegisterSelf(this, msgIds);
    }
    private void Update()
    {
        if (netWorkToServer != null)
        {
            netWorkToServer.Update();
        }
    }
}
