using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMonoBase : MonoBehaviour {
    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="msg"></param>
    public abstract void ProcessEvent(MsgBase msg);
}
