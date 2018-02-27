using ProtoBuf;
using System;
using System.Collections.Generic;


public class TCPNetMsg<T> : NetMsgBase where T : IExtensible
{
    public TCPNetMsg(T temp, ushort id, byte[] arr) : base(arr)
    {
        ChangeMsgData(temp, id);
    }
    /// <summary>
    /// 从Socket接受过来的数据转化成制定类
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <returns></returns>
    public U GetPBClass<U>() where U : IExtensible
    {
        byte[] tempType = new byte[this.buffer.Length - 6];
        Buffer.BlockCopy(buffer, 6, tempType, 0, buffer.Length - 6);
        return IProtoTools.Deserialize<U>(tempType);
    }
    public U GetPBClass<U>(byte[] tempArr) where U : IExtensible
    {
        this.buffer = tempArr;
        return GetPBClass<U>();
    }
    public void ChangeMsgData<V>(V temp) where V : IExtensible
    {
        byte[] tempByte = IProtoTools.Serialize(temp);
        buffer = new byte[tempByte.Length + 6];
        //添加长度
        byte[] dataLength = BitConverter.GetBytes(tempByte.Length);
        Buffer.BlockCopy(dataLength, 0, buffer, 0, 4);
        //添加命令
        //byte[] eventId = BitConverter.GetBytes(msgId);
        //Buffer.BlockCopy(eventId, 0, buffer, 4, 2);
        //添加data
        Buffer.BlockCopy(tempByte, 0, buffer, 6, tempByte.Length);
    }
    public void ChangeMsgData(ushort msgId)
    {
        this.msgId = msgId;
        byte[] eventId = BitConverter.GetBytes(msgId);
        Buffer.BlockCopy(eventId, 0, buffer, 4, 2);
    }
    public void ChangeMsgData<V>(V temp ,ushort msgId) where V : IExtensible
    {
        ChangeMsgData(temp);
        ChangeMsgData(msgId);
    }
}

