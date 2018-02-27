--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

local lAssetBegin=LManagerID.LAssetManager;
LAssetEvent={
  GetRes = lAssetBegin+1,
    ReleaseSingleObj = lAssetBegin+2,
    ReleaseBundleObjs= lAssetBegin+3,
    ReleaseSceneObj= lAssetBegin+4,
    ReleaseSingleBundle= lAssetBegin+5,
    ReleaseSceneBundle= lAssetBegin+6,
    ReleaseAll= lAssetBegin+7,
    InitObj = lAssetBegin+8
}