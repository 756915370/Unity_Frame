using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class NetWorkToServer
{
    private Queue<NetMsgBase> recvMsgPool = null;
    private Queue<NetMsgBase> sendMsgPool = null;
    private NetSocket clientSocket;
    private Thread sendThread;

    public NetWorkToServer(string ip, ushort port)
    {
        recvMsgPool = new Queue<NetMsgBase>();
        sendMsgPool = new Queue<NetMsgBase>();
        clientSocket = new NetSocket();
        clientSocket.AsyncConnect(ip, port, AsyncConnectCallBack, AsyncRecvCallBack);
    }
    private void AsyncConnectCallBack(bool success, ErrorSocket error, string exception)
    {
        if (success)
        {
            sendThread = new Thread(LoodSendMsg);
            sendThread.Start();
        }
    }
    #region Send
    /// <summary>
    /// 把要发送给服务端的消息存储起来
    /// </summary>
    /// <param name="msg"></param>
    public void PutSendMsgPool(NetMsgBase msg)
    {
        lock (sendMsgPool)
        {
            sendMsgPool.Enqueue(msg);
        }
    }
    private void CallBackSend(bool success, ErrorSocket error, string exception)
    {
        if (success)
        {

        }
        else
        {

        }
    }
    /// <summary>
    /// 不断发送消息给服务端
    /// </summary>
    private void LoodSendMsg()
    {
        while (clientSocket != null && clientSocket.IsConnect())
        {
            lock (sendMsgPool)
            {
                while (sendMsgPool.Count > 0)
                {
                    NetMsgBase body = sendMsgPool.Dequeue();
                    clientSocket.AsyncSend(body.GetNetBytes(), CallBackSend);
                }
            }
            Thread.Sleep(100);
        }
    }
    #endregion
    #region Recv
    private void AsyncRecvCallBack(bool success, ErrorSocket error, string exception, byte[] byteMessage, string strMessage)
    {

        if (success)
        {
            PutRecvMsgToPool(byteMessage);
        }
        else
        {
            Debug.Log(exception);
        }
    }
    /// <summary>
    /// 接受消息放进队列
    /// </summary>
    /// <param name="recvMsg"></param>
    private void PutRecvMsgToPool(byte[] recvMsg)
    {
        NetMsgBase netMsg = new NetMsgBase(recvMsg);
        recvMsgPool.Enqueue(netMsg);
    }
    /// <summary>
    /// 将客户端接收到的消息发给消息中心处理
    /// </summary>
    public void Update()
    {
        if (recvMsgPool != null )
        {
            while (recvMsgPool.Count > 0)
            {
                NetMsgBase netMsg = recvMsgPool.Dequeue();
                AnalyseData(netMsg);
            }
        }
    }
    private void AnalyseData(NetMsgBase msg)
    {
        MsgCenter.Instance.SendToMsg(msg);
    }
    #endregion
    #region Disconnect

    private void CallBackDisconnect(bool success, ErrorSocket error, string exception)
    {
        if (success)
        {
            sendThread.Abort();
        }
        else
        {
            Debug.Log(exception);
        }
    }
    public void Disconnect()
    {
        if (clientSocket != null && clientSocket.IsConnect())
        {
            clientSocket.AsyncDisconnect(CallBackDisconnect);
        }
    }

    #endregion
}
