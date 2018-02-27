--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetMsg=LMsgBase:New(1);
LAssetMsg.__index=LAssetMsg;
function LAssetMsg:New(msgid,single,bundle,res,scene,backFunc)
    local self={};
    setmetatable(self,LAssetMsg);
    self.msgId=msgid;
    self.sceneName=scene;
    self.bundleName=bundle;
    self.resName=res;
    self.isSingle=single;
    self.callBackFunc=backFunc;
    return self;
end

