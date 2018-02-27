--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LEventNode={}
LEventNode.__index=LEventNode;
function LEventNode:New(mono)
    local self={};
    setmetatable(self,LEventNode);
    self.value=mono;
    self.next=nil;
    return self;
end

