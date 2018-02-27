--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetBundleLoader=LAssetBase:New();
LAssetBundleLoader.__index=LAssetBundleLoader;
local this=LAssetBundleLoader;
function LAssetBundleLoader:New(msgid)
    local self={};
    setmetatable(self,LAssetBundleLoader);
    self.msgId=msgid;
    return self;
end

function LAssetBundleLoader.Awake()
    self.msgId[1]=LAssetEvent.GetRes;
    self.msgId[2]=LAssetEvent.ReleaseSingleObj;
    self.msgId[3]=LAssetEvent.ReleaseBundleObjs;

    this:RegisSelf(this,self.msgId);
end
function LAssetBundleLoader:SendMsg()

end

function LAssetBundleLoader:ProcessEvent(msg)
    if(msg.msgId==LAssetEvent.GetRes) then
         LuaLoadReses.Instance:GetResources(msg.sceneName,msg.bundleName,msg.resName,msg.isSingle,msg.callBackFunc)
    elseif(msg.msgId==LAssetEvent.ReleaseSingleObj) then
        LuaLoadReses.Instance:UnLoadResObj(msg.sceneName,msg.bundleName,msg.resName)
    elseif(msg.msgId==LAssetEvent.ReleaseBundleObjs) then
        LuaLoadReses.Instance:UnLoadBundleObjs(msg.sceneName,msg.bundleName)
   
    end

end