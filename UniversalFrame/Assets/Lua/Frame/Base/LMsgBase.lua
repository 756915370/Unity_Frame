--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

--负责消息路由

--endregion

LMsgBase={msgId=0}
LMsgBase.__index=LMsgBase;
function LMsgBase:New(msgid)
    local self={};
    setmetatable(self,LMsgBase);
    self.msgId=msgid;
    return self;
end

function LMsgBase:GetManager()
    tempId=math.floor(self.msgid/MsgSpan)*MsgSpan;
    return math.ceil(tempId);
end
