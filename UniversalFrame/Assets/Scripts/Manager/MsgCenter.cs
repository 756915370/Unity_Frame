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
    }
   
    public void SendToMsg(MsgBase msg)
    {
        AnalysisMsg(msg);
    }
    private void AnalysisMsg(MsgBase msg)
    {
        ManagerID managerID = msg.GetManagerID();
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
