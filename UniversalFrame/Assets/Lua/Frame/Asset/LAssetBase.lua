--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

LAssetBase={msgId=0}
LAssetBase.__index=LAssetBase;

function LAssetBase:New(msgid)
    local self={}
    setmetatable(self,LAssetBase);
    self.msgId=msgid;
    return self;
end

function LAssetBase:SendMsg(msg)
    LAssetManager.GetInstance().SendMsg(msg)
end

function LAssetBase:RegisSelf(script,msgs)
    LAssetManager.GetInstance().RegisMsgs(script,msgs)
end
function LAssetBase:UnRegisSelf(script,msgs)
    LAssetManager.GetInstance().UnRegisMsgs(script,msgs)
end

function LAssetBase:Destroy()
    self:UnRegisMsgs(self,self.msgId)
end

