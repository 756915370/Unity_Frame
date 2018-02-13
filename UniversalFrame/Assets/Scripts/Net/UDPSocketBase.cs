using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;

public class UDPSocketBase
{
    public delegate void UDPSocketDelegate(byte[] pBuf, int dwCount, string ip, ushort port);
    private UDPSocketDelegate udpSocketDelegate;
    private IPEndPoint udpIp;
    private Socket udpSocket;
    private byte[] recvData;
    private Thread recvThread;
    private bool isRunningThread = true;
    public bool BindSocket(ushort port, int bufferLength,UDPSocketDelegate socketDelegate)
    {
        udpIp = new IPEndPoint(IPAddress.Any, port);
        UDPConnect();
        udpSocketDelegate = socketDelegate;
        recvData = new byte[bufferLength];
        recvThread = new Thread(RecvDataThread);
        recvThread.Start();
        return true;
    }
    public void UDPConnect()
    {
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //带有服务器功能的都有Bind
        udpSocket.Bind(udpIp);
    }
    public void RecvDataThread()
    {
        while (isRunningThread)
        {
            if (udpSocket == null||udpSocket.Available<1)
            {
                Thread.Sleep(100);
                continue;
            }
            lock (this)
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)sender;
                int myCount = udpSocket.ReceiveFrom(recvData, ref remote);
                if (udpSocketDelegate != null)
                {
                    udpSocketDelegate.Invoke(recvData, myCount, remote.AddressFamily.ToString(), (ushort)sender.Port);
                }
            }
        }
    }
    public int SendData(string ip,byte[] data,ushort uport)
    {
        IPEndPoint sendToIp = new IPEndPoint(IPAddress.Parse(ip), uport);
        if(!udpSocket.Connected)
        {
            UDPConnect();
        }
        //已发送的字节数
        int mySend = udpSocket.SendTo(data, data.Length, SocketFlags.None, sendToIp);
        return mySend;
    }
}
