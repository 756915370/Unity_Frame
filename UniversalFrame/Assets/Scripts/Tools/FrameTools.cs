using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTools
{
    public const int MsgSpan = 3000;
    

}
public enum ManagerID
{
    GameManager = FrameTools.MsgSpan * 11,
    NetManager = FrameTools.MsgSpan * 12,
    UIManager = FrameTools.MsgSpan * 13,
    NPCManager = FrameTools.MsgSpan * 14,
    CharactorManager = FrameTools.MsgSpan * 15,
    AssetManager = FrameTools.MsgSpan * 16,
    DataManager = FrameTools.MsgSpan * 17,
    AudioManager = FrameTools.MsgSpan * 18
    
   
}
