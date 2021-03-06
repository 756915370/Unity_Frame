﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaAndCMsgCenterWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaAndCMsgCenter), typeof(IMonoBase));
		L.RegFunction("ProcessEvent", ProcessEvent);
		L.RegFunction("SettingLuaCallBack", SettingLuaCallBack);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Instance", get_Instance, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			LuaAndCMsgCenter obj = (LuaAndCMsgCenter)ToLua.CheckObject<LuaAndCMsgCenter>(L, 1);
			MsgBase arg0 = (MsgBase)ToLua.CheckObject<MsgBase>(L, 2);
			obj.ProcessEvent(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SettingLuaCallBack(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			LuaAndCMsgCenter obj = (LuaAndCMsgCenter)ToLua.CheckObject<LuaAndCMsgCenter>(L, 1);
			LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
			obj.SettingLuaCallBack(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			LuaAndCMsgCenter obj = (LuaAndCMsgCenter)o;
			LuaAndCMsgCenter ret = obj.Instance;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Instance on a nil value");
		}
	}
}

