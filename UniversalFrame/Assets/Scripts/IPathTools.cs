using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class IPathTools
{
    public static string GetPlatformFolderName(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "IOS";
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            default:
                return null;
        }
    }
    private static string GetAppFilePath()
    {
        string tempPath = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            tempPath = Application.streamingAssetsPath;
        }
        else
        {
            tempPath = Application.persistentDataPath;
        }
        return tempPath;
    }
    public static string GetAssetBundlePath()
    {
        string platFolder = GetPlatformFolderName(Application.platform);
        string allPath = GetAppFilePath() + "/" + platFolder;
        return allPath;
    }
    public static string GetWWWAssetBundlePath()
    {
        string result = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            result = "file:///" + GetAssetBundlePath();
        }
        else
        {
            string path = GetAssetBundlePath();
#if UNITY_ANDROID
            result="jar:file://"+path;
#elif UNITY_STANDALONE_WIN
            path = "file:///" + path;
#else
            result="file://"+path;
#endif
        }
        return result;
    }
}

