using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public enum ErrorSocket
{
    Success = 0,
    TimeOut,
    SocketNull,
    SocketUnConnect,
    ConnectUnSuccessUnknown,
    ConnectError,
    SendUnSuccessUnknown,
    SendSuccess,
    RecvUnSuccessUnknown,
    DisConnectUnknown,
    DisConnectSuccess
}
public class NetSocket
{
    public delegate void CallBackNormal(bool success, ErrorSocket error, string exception);
    //public delegate void CallBackSend(bool success, ErrorSocket error, string exception);
    public delegate void CallBackRecv(bool success, ErrorSocket error, string exception, byte[] byteMessage, string strMessage);
    //public delegate void CallBackDisConnect(bool success, ErrorSocket error, string exception);
    private CallBackNormal callBackConnect;
    private CallBackNormal callBackSend;
    private CallBackNormal callBackDisconnect;
    private CallBackRecv callBackRecv;
    private byte[] recvByte;
    private ErrorSocket errorSocket;
    private Socket clientSocket;
    private string addressIP;
    private ushort port;
    private SocketBuffer recvBuffer;
    public NetSocket()
    {
        recvBuffer = new SocketBuffer(6, RecvMsgOver);
        recvByte = new byte[1024];
    }

    #region Connect
    public bool IsConnect()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            return true;
        }
        return false;
    }
    public void AsyncConnect(string ip, ushort port, CallBackNormal connectBack, CallBackRecv callBackRecv)
    {
        errorSocket = ErrorSocket.Success;
        this.callBackConnect = connectBack;
        this.callBackRecv = callBackRecv;
        if (clientSocket != null && clientSocket.Connected)
        {
            this.callBackConnect(false, ErrorSocket.ConnectError, "重复发送");
        }
        else if (clientSocket == null || !clientSocket.Connected)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress iPAddress = IPAddress.Parse(ip);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            IAsyncResult connect = clientSocket.BeginConnect(iPEndPoint, ConnectCallBack, clientSocket);
            if (!WriteDot(connect))
            {
                connectBack(false, errorSocket, "连接超时");
            }

        }
    }
    private void ConnectCallBack(IAsyncResult ar)
    {
        try
        {
            clientSocket.EndConnect(ar);
            if (clientSocket.Connected == false)
            {
                errorSocket = ErrorSocket.ConnectUnSuccessUnknown;
                callBackConnect(false, errorSocket, "连接超时");
                return;
            }
            else
            {
                //接受消息
                errorSocket = ErrorSocket.Success;
                this.callBackConnect(true, errorSocket, "连接成功");
            }
        }
        catch (Exception ec)
        {
            this.callBackConnect(false, errorSocket, errorSocket.ToString());
        }
    }
    #endregion
    #region Recv

    public void Receive()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            IAsyncResult async = clientSocket.BeginReceive(recvByte, 0, recvByte.Length, SocketFlags.None, RecvCallBack, clientSocket);
            if (WriteDot(async) == false)
            {
                callBackRecv(false, ErrorSocket.RecvUnSuccessUnknown, "接收失败", null, "");
            }
        }
    }
    private void RecvCallBack(IAsyncResult ar)
    {
        try
        {
            if (!clientSocket.Connected)
            {
                callBackRecv(false, ErrorSocket.RecvUnSuccessUnknown, "连接失败", null, "");
                return;

            }
            int length = clientSocket.EndReceive(ar);
            if (length == 0)
            {
                return;
            }
            recvBuffer.RecvByte(recvByte, length);
        }
        catch (Exception ec)
        {
            callBackRecv(false, ErrorSocket.RecvUnSuccessUnknown, ec.ToString(), null, "");
        }
        Receive();
    }
    #endregion
    #region RecvMsgOver

    public void RecvMsgOver(byte[] allByte)
    {
        callBackRecv(true, ErrorSocket.Success, "", null, "recv back success");
    }
    #endregion
    #region Send
    public void SendCallBack(IAsyncResult ar)
    {
        try
        {
            int byteSend = clientSocket.EndSend(ar);
            if (byteSend > 0)
            {
                callBackSend(true, ErrorSocket.SendSuccess, "发送成功");
            }
            else
            {
                callBackSend(false, ErrorSocket.SendUnSuccessUnknown, "发送失败");
            }
        }
        catch (Exception ee)
        {
            callBackSend(false, ErrorSocket.SendUnSuccessUnknown, "发送失败");
        }

    }

    public void AsyncSend(byte[] sendBuffer, CallBackNormal sendBack)
    {
        errorSocket = ErrorSocket.Success;
        this.callBackSend = sendBack;
        if (clientSocket == null)
        {
            this.callBackSend(false, ErrorSocket.SocketNull, "socket为空");
        }
        else if (!clientSocket.Connected)
        {
            callBackSend(false, ErrorSocket.SocketUnConnect, "没有连接");
        }
        else
        {
            IAsyncResult async = clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, SendCallBack, clientSocket);
            if (WriteDot(async) == false)
            {
                callBackSend(false, ErrorSocket.SendUnSuccessUnknown, "发送失败");
            }
        }
    }
    #endregion
    #region TimeOut Check
    /// <summary>
    /// 判断是否超时
    /// </summary>
    /// <param name="ar"></param>
    /// <returns>true表示可以写入读取，false表示超时</returns>
    private bool WriteDot(IAsyncResult ar)
    {
        int i = 0;
        while (ar.IsCompleted == false)
        {
            i++;
            if (i > 20)
            {
                errorSocket = ErrorSocket.TimeOut;
                return false;
            }
            Thread.Sleep(100);
        }
        return true;
    }

    #endregion
    #region Disconnect
    public void AsyncDisconnect(CallBackNormal disconnectBack)
    {
        try
        {
            errorSocket = ErrorSocket.Success;
            this.callBackDisconnect = disconnectBack;
            if (clientSocket == null)
            {
                callBackDisconnect(false, ErrorSocket.DisConnectUnknown, "socket为空");
            }
            else if (!clientSocket.Connected)
            {
                callBackDisconnect(false, ErrorSocket.DisConnectUnknown, "断开连接");
            }
            else
            {
                IAsyncResult async = clientSocket.BeginDisconnect(false, DisconnectCallBack, clientSocket);
                if (WriteDot(async) == false)
                {
                    callBackDisconnect(false, ErrorSocket.DisConnectUnknown, "超时连接");
                }
            }
        }
        catch 
        {
            callBackDisconnect(false, ErrorSocket.DisConnectUnknown, "超时连接");
        }
    }
    public void DisconnectCallBack(IAsyncResult ar)
    {
        try
        {
            clientSocket.EndDisconnect(ar);
            clientSocket.Close();
            clientSocket = null;
            callBackDisconnect(true, ErrorSocket.DisConnectSuccess, "关闭成功");
        }
        catch
        {
            callBackDisconnect(false, ErrorSocket.DisConnectUnknown, "关闭失败");
        }
    }
    #endregion
}

