using System;
using System.Collections.Generic;
using System.IO;

public delegate void CallBackRecvOver(byte[] allData);
/// <summary>
/// 消息head  == 前四个字节表示消息的长度 (一般不包含头部的长度) + msgid 命令
/// 数据body == protobuffer /json/xml
/// 消息的长度 ：只表示你 后面数据的长度
/// </summary>
public class SocketBuffer
{
    //定义消息头
    private byte[] headByte;//存储一个int32类型，正数转换公式int32 x=b0+b1*256+b2*256^2+b3*256^3
    private byte headLength = 6;
    private byte[] allRecvData;//接受到的数据
    private CallBackRecvOver callBackRecvOver;
    private int curRecvLength;//当前接受到的数据长度，一开始等于0
    private int allDataLength;//总共接受到的数据长度
    public SocketBuffer(byte tHeadLength,CallBackRecvOver callBack)
    {
        headLength = tHeadLength;
        headByte = new byte[headLength];
        callBackRecvOver = callBack;
    }
    public void RecvByte(byte[] recvByte,int recvLength)
    {
        if (recvLength == 0)
        {
            return;
        }

        if (curRecvLength < headByte.Length)
        {
            RecvHead(recvByte, recvLength);
        }
        else
        {
            int allLength = curRecvLength + recvLength;
            if (allLength == allDataLength)
            {
                //刚好相等
                RecvOneAll(recvByte, recvLength);
            }
            //接受的数据比这个消息长
            else if (allLength > allDataLength)
            {
                RecvLarger(recvByte, recvLength);
            }
            else
            {
                RecvSmaller(recvByte, recvLength);
            }
        }
    }
    private void RecvOneAll(byte[] recvByte,int recvLength)
    {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, recvLength);
        curRecvLength += recvLength;
        RecvOneMsgOver();
    }
    private void RecvLarger(byte[] recvByte,int recvLength)
    {
        //拼成一条消息还需要多少数据
        int bodyRemainLength = allDataLength - curRecvLength;
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, bodyRemainLength);
        curRecvLength += bodyRemainLength;
        RecvOneMsgOver();
        //取完一条消息还剩下多少字节
        int remainLength = recvLength - bodyRemainLength;
        byte[] remainByte = new byte[remainLength];
        Buffer.BlockCopy(recvByte, bodyRemainLength, remainByte, 0, remainLength);
        RecvByte(remainByte, remainLength);
    }
    private void RecvSmaller(byte[] recvByte,int recvLength)
    {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, recvLength);
        curRecvLength += recvLength;
    }
    /// <summary>
    /// 计算头部
    /// </summary>
    /// <param name="recvByte">接受到的数据</param>
    /// <param name="recvLength">头部后面的数据的长度</param>
    private void RecvHead(byte[] recvByte,int recvLength)
    {
        //差多少个字节才能组成一个头 
        int remain = headByte.Length - curRecvLength;
        //现在接受的和已经接受的总长度
        int allNewlength = curRecvLength + recvLength;
        if (allNewlength < headByte.Length)
        {
            //直接拷贝缓存区
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, recvLength);
            curRecvLength += recvLength;
        }
        else
        {
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, remain);
            curRecvLength += remain;
            //头部已经凑齐
            //取出4个字节，转换int
            allDataLength = BitConverter.ToInt32(headByte, 0) + headLength;
            allRecvData = new byte[allDataLength];//head+body
            //头部数据拷入allRecvData
            Buffer.BlockCopy(headByte, 0, allRecvData, 0, headLength);
            int bodyRemain = recvLength - remain;
            //表示recvByte去掉头部所需数据后是否还有数据
            if (bodyRemain > 0)
            {
                //凑齐头部后还剩下的byte
                byte[] remainByte = new byte[bodyRemain];
                //将剩下的字节送入remainByte
                Buffer.BlockCopy(recvByte, remain, remainByte, 0, bodyRemain);
                RecvByte(remainByte, bodyRemain);
            }
            else
            {
                RecvOneMsgOver();
            }
        }
    }
    
    
    private void RecvOneMsgOver()
    {
        if (callBackRecvOver != null)
        {
            callBackRecvOver(allRecvData);
        }
        curRecvLength = 0;
        allDataLength = 0;
        allRecvData = null;
    }
}

