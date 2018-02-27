using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MyDebugger
{
    private static UDPSocketBase udpSocket = null;
    public static bool EnableLog = true;
    public static UDPSocketBase UDPSocket
    {
        get
        {
            if (udpSocket == null)
            {
                udpSocket = new UDPSocketBase();
                udpSocket.BindSocket(18001, 1024, null);
            }
            return udpSocket;
        }
    }
    public static void LogError(object message,UnityEngine.Object context)
    {
        if (EnableLog)
        {
            if(Application.platform==RuntimePlatform.WindowsEditor|| 
                Application.platform == RuntimePlatform.OSXEditor)
            {
                Debug.LogError(message, context);
            }
            else
            {
                byte[] data = Encoding.Default.GetBytes(message.ToString());
                udpSocket.SendData("255.255.255.255", data, 18001);
            }
        }
    }

}

