--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetManager=LManagerBase:New();
LAssetManager.__index=LAssetManager;
local this=LAssetManager;
function LAssetManager:New()
    local self={}
    setmetatable(self,LAssetManager);
    return self;
end

function LAssetManager.GetInstance()
    return this;
end

function LAssetManager:SendMsg(msg)
    if msg:GetManager() == LManagerID.AssetManager then
        self:ProcessEvent(msg);
    else 
        LMsgCenter.SendToMsg(msg);
    end
end