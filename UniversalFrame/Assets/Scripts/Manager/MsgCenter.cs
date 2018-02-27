using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgCenter : MonoBehaviour
{
    public static MsgCenter Instance
    {
        get { return instance; }
    }
    private static MsgCenter instance;
    public void Awake()
    {
        instance = this;
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<AssetManager>();
        gameObject.AddComponent<ILoaderManager>();
        gameObject.AddComponent<NetManager>();
        gameObject.AddComponent<LuaEventProcess>();
        LuaAndCMsgCenter luaAndCMsgCenter = gameObject.AddComponent<LuaAndCMsgCenter>();
        LuaEventProcess.Instance.SettingChild(luaAndCMsgCenter);
    }
   
    public void SendToMsg(MsgBase msg)
    {
        AnalysisMsg(msg);
    }
    private void AnalysisMsg(MsgBase msg)
    {
        ManagerID managerID = msg.GetManagerID();
        if ((ushort)managerID < (ushort)ManagerID.GameManager)
        {
            LuaEventProcess.Instance.ProcessEvent(msg);
        }
        switch (managerID)
        {
            case ManagerID.GameManager:
                break;
            case ManagerID.UIManager:
                break;
            case ManagerID.AudioManager:
                break;
            case ManagerID.NPCManager:
                break;
            case ManagerID.CharactorManager:
                break;
            case ManagerID.AssetManager:
                break;
            case ManagerID.NetManager:
                break;
            default:
                break;
        }
    }
}
