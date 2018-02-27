using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class NetMsgBase : MsgBase
{
    public byte[] buffer;
    public NetMsgBase() { }
    public NetMsgBase(byte[] arr)
    {
        buffer = arr;
        //字节流从第5个开始表示消息id
        msgId = BitConverter.ToUInt16(arr, 4);

    }
    public override byte GetState()
    {
        return buffer[6];
    }
    public virtual byte[] GetNetBytes()
    {
        return buffer;
    }
	public virtual byte[] GetProtoBuffer()
    {
        byte[] tempByte = new byte[buffer.Length - 7];
        Buffer.BlockCopy(buffer, 7, tempByte, 0, buffer.Length - 7);
        return tempByte;
    }
}
