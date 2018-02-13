using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class NetMsgBase : MsgBase
{
    public byte[] buffer;
    public NetMsgBase(byte[] arr)
    {
        buffer = arr;
        //字节流从第5个开始表示消息id
        msgId = BitConverter.ToUInt16(arr, 4);

    }
    public byte[] GetNetBytes()
    {
        return buffer;
    }
	
}
