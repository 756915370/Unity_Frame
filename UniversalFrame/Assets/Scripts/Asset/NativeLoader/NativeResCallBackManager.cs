using System;
using System.Collections.Generic;
using UnityEngine;

public class NativeResCallBackManager
{
    private Dictionary<string, NativeResCallBackNode> mNodeManager = null;
    public NativeResCallBackManager()
    {
        mNodeManager = new Dictionary<string, NativeResCallBackNode>();

    }
    /// <summary>
    /// 添加节点
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="callBackNode"></param>
    public void AddBundle(string bundle, NativeResCallBackNode callBackNode)
    {
        if (mNodeManager.ContainsKey(bundle))
        {
            NativeResCallBackNode headNode = mNodeManager[bundle];
            while (headNode != null)
            {
                headNode = headNode.NextNode;
            }
            headNode.NextNode = callBackNode;
        }
        else
        {
            mNodeManager.Add(bundle, callBackNode);
        }
    }
    /// <summary>
    /// 加载完成后且向上传递消息，传递完后调用
    /// </summary>
    /// <param name="bundle"></param>
    public void Dispose(string bundle)
    {
        if (mNodeManager.ContainsKey(bundle))
        {
            NativeResCallBackNode headNode = mNodeManager[bundle];
            while (headNode.NextNode != null)
            {
                NativeResCallBackNode curNode = headNode;
                headNode = headNode.NextNode;
                curNode.Dispose();
            }
            headNode.Dispose();
            mNodeManager.Remove(bundle);
        }
    }
    public void CallBackRes(string bundle)
    {
        if (mNodeManager.ContainsKey(bundle))
        {
            Debug.Log("调用了回调+bundleName: " + bundle);
            NativeResCallBackNode headNode = mNodeManager[bundle];
            do
            {
                headNode.CallBack.Invoke(headNode);
                headNode = headNode.NextNode;
            } while (headNode != null);
        }
    }
}

