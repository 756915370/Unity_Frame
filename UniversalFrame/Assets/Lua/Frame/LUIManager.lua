--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

LUIManager=LManagerBase:New();
LUIManager.__index=LUIManager;
local this=LUIManager;
function LUIManager:New()
    local self={}
    setmetatable(self,LUIManager);
    return self;
end

function LUIManager.GetInstance()
    return this;
end

function LUIManager:SendMsg(msg)
    if msg:GetManager() == LManagerID.UIManager then
        self:ProcessEvent(msg);
    else 
        LMsgCenter.SendToMsg(msg);
    end
end

