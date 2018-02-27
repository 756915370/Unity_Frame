--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

--负责消息的存储和处理

--endregion
LManagerBase={ eventTree={}}
LManagerBase.__index=LManagerBase;
local this=LManagerBase;
function LManagerBase:New()
    local self={};
    setmetatable(self,LManagerBase);
    return self;
end

function LManagerBase.GetInstance()
    return this;
end

function LManagerBase:FindKey(dict,key)
    for k,v in pairs(dict) do
    if k ==key then
    return true;
    end
    return false;
end
--注册单个
function LManagerBase:RegisMsg(id,eventNode)
    if not this:FindKey(self.eventTree,id) then 
        self.eventTree[id]=eventNode;
    else
        tempNode=self.eventTree[id];
        while tempNode.next==nil  do
            tempNode=tempNode.next;
        end
        tempNode.next=eventNode;
    end
end
--注册一个脚本里若干点消息
function LManagerBase:RegisMsgs(script,msgs)
    for i,v in pairs(msgs) do
        eventNode=LEventNode:New(script);
        self:RegisMsg(v,eventNode);
    end
end

function LManagerBase:UnRegisMsg(script,id)
    if this:FindKey(self.eventTree,id) then
        tempNode=self.eventTree[id];
        --释放的是头部节点
        if tempNode.value==script then
            --头部后面还有节点
            if tempNode.next~=nil then
            self.eventTree[id]=tempNode.next;
            tempNode.next=nil;
            --头部后面没有节点
            else 
            self.eventTree[id]=nil
            end
        else
            while tempNode.next~=nil and tempNode.next.value ~=script do 
                tempNode=tempNode.next;
            end
            --释放的是中间节点
            if tempNode.next.next~=nil then
                curNode=tempNode.next;
                tempNode.next=curNode.next;
                curNode.next=nil;
            else
                tempNode.next=nil;
            end
        end
end

function LManagerBase:UnRegisMsgs(script,...)
    if arg==nil then
        return
    end
    for i in arg do
        self:UnRegisMsg(script,i);
    end
end

function LManagerBase:Destroy()
    keys={}
    keyCount=0;
    for k,v in pairs(self.eventTree) do
        keys[keyCount]=k;
        keyCount=keyCount+1;

    end
     
    for i=1, keyCount do
        self.eventTree[keys[i]]=nil
    end
end

function LManagerBase:ProcessEvent(msg)
    if this:FindKey(self.eventTree,msg.msgId) then
        local tempNode=self.eventTree[msg.msgId];

        while tempNode ~=nil do
            tempNode.value:ProcessEvent(msg);
            if tempNode.next~=nil then
            tempNode=tempNode.next;
            else 
            tempNode=nil;
            end
        end
   else
        print("Msg not contain msg=="..msg.msgId);
   end

end