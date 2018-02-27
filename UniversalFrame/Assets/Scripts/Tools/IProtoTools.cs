using System;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;

public class IProtoTools
{
    /// <summary>
    /// 序列化出来给Socket
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static  byte[] Serialize(IExtensible msg)
    {
        byte[] result;
        using(var stream=new MemoryStream())
        {
            Serializer.Serialize(stream, msg);
            result = stream.ToArray();
        }
        return result;
    }
    /// <summary>
    /// 主要从Socket接收数据，反序列，返回值可以是泛型
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static IExtensible Deserialize<IExtensible>(byte[] message)
    {
        IExtensible result;
        using(var stream=new MemoryStream(message))
        {
            result = Serializer.Deserialize<IExtensible>(stream);
        }
        return result;
    }
}

