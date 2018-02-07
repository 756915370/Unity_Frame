using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase
{
    private Dictionary<string, GameObject> childMemberDict = new Dictionary<string, GameObject>();
    public static UIManager Instance
    {
        get { return instance; }
    }
    private static UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManagerID() == ManagerID.UIManager)
        {
            //ManagerBase 本模块自己
            ProcessEvent(msg);
        }
        else //MsgCenter
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
    }
    public void RegisterGo(string name,GameObject go)
    {
        if (!childMemberDict.ContainsKey(name))
        {
            childMemberDict.Add(name, go);
        }
    }
    public void UnRegisterGo(string name)
    {
        if (childMemberDict.ContainsKey(name))
        {
            childMemberDict.Remove(name);
        }
    }
    public GameObject GetGameObjectByName(string name)
    {
        if (childMemberDict.ContainsKey(name))
        {
            return childMemberDict[name];
        }
        else
        {
            return null;
        }
    }
}
