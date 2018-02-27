--region *.lua
--Date
--此文件由[BabeLua]插件自动生成


--主要是负责各个消息模块的转发
--endregion
LMsgCenter={}
LMsgCenter.__index=LMsgCenter;
local this=LMsgCenter;
function LMsgCenter:New()
    local self={};
    setmetatable(self,LMsgCenter);
    return self;
end
--加.的函数相当于静态函数
function LMsgCenter.GetInstance()
    return this;
end

function LMsgCenter.RecvMsg(fromNet,arg0,arg1,arg2)
    if fromNet==true then
        local tempMsg=LMsgBase:New(arg0);
        tempMsg.state=arg1;
        tempId.data=arg2;
        this.AnasyMsg(tempMsg);
    else
        this.AnasyMsg(arg0);
    end

   

end

function LMsgCenter.Awake()
    LuaAndCMsgCenter.Instance.SettingLuaCallBack(this.RecvMsg)
end

function LMsgCenter.SendToMsg(msg)
    this.AnasyMsg(msg);
end

function LMsgCenter.AnasyMsg(msg)
    managerId=msg:GetManager();
    if managerId==LManagerID.LAssetManager then
     
     elseif managerId==LManagerID.LUIManager then
     LUIManager.GetInstance():SendMsg(msg);
     elseif managerId==LManagerID.LCharactorManager then

     elseif managerId==LManagerID.LDataManager then

     elseif managerId==LManagerID.LuaManager then

     elseif managerId==LManagerID.LGameManager then

     elseif managerId==LManagerID.LNPCManager then

     else
        MsgCenter.Instance().SendToMsg(msg);
     end
end