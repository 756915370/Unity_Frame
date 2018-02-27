using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNode
{
    //当前数据
    public IMonoBase data;
    //下一个节点
    public EventNode next;
    public EventNode(IMonoBase monoBase)
    {
        data = monoBase;
        next = null;
    }
}
public class ManagerBase : IMonoBase
{
    //存储消息
    public Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();

    public override void ProcessEvent(MsgBase msg)
    {
        if (!eventTree.ContainsKey(msg.msgId))
        {
            Debug.LogWarning("没有这个消息 ID: "+msg.msgId+"Msg Manager:"+msg.GetManagerID());
            return;
        }
        else
        {
            EventNode headNode = eventTree[msg.msgId];
            EventNode temp = headNode;
            //通过key找到链表通知这个链表上的所有节点
            do
            {
                temp.data.ProcessEvent(msg);
                temp = temp.next;
            } while (temp != null);
        }
    }

    /// <summary>
    /// 存储一个消息
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="msgs"></param>
    public void RegisterMsg(IMonoBase mono,params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            EventNode eventNode = new EventNode(mono);
            RegisterMsg(msgs[i], eventNode);
        }
    }
    /// <summary>
    /// 存储一个节点
    /// </summary>
    /// <param name="id"></param>
    /// <param name="eventNode"></param>
    public void RegisterMsg(ushort id,EventNode eventNode)
    {
        if (!eventTree.ContainsKey(id))
        {
            eventTree.Add(id, eventNode);
        }
        else
        {
            EventNode node = eventTree[id];
            while (node.next != null)
            {
                node = node.next;
            }
            node.next = eventNode;
        }
    }
    /// <summary>
    /// 根据一组id去掉一个消息
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="msgs"></param>
    public void UnRegisterMsg(IMonoBase mono,params ushort[] msgs)
    {
        for (int i = 0; i < msgs.Length; i++)
        {
            UnRegisterMsg(msgs[i], mono);
        }
    }
    /// <summary>
    /// 根据一个id去掉一个消息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="mono"></param>
    public void UnRegisterMsg(ushort id,IMonoBase mono)
    {
        if (!eventTree.ContainsKey(id))
        {
            Debug.Log("不存在这个消息ID: " + id);
        }
        else
        {
            //释放消息分三种情况，头部、中部、尾部释放
            EventNode head = eventTree[id];
            if (head.data == mono)//头部
            {
                //头部后面还有节点
                if (head.next != null)
                {
                    eventTree[id] = head.next;
                    head.next = null;
                }
                else//头部后面没有节点
                {
                    eventTree.Remove(id);
                }
            }
            else//去掉尾部和中间
            {
                EventNode temp = head;
                while (temp.next != null && temp.next.data != mono)
                {
                    temp = temp.next;
                }
                //表示已经找到目标节点temp.next
                if (temp.next.next != null)//目标节点是中部节点
                {
                    EventNode targetNode = temp.next;
                    temp.next = targetNode.next;
                    targetNode.next = null;
                }
                else//目标节点是尾部节点
                {
                    temp.next = null;
                }
            }
        }
    }
    private void OnDestroy()
    {
        List<ushort> tempKeys = new List<ushort>(eventTree.Keys);
        for (int i = 0; i < tempKeys.Count; i++)
        {
            eventTree[tempKeys[i]] = null;
        }
        eventTree.Clear();
        System.GC.Collect();
    }
}
