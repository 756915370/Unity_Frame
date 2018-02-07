using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleEditor
{
    [MenuItem("Tools/BuildAssetBundle")]
    public static void BuildAssetBundle()
    {
        //streamingassetpath
        string outPath = IPathTools.GetAssetBundlePath(); //Application.streamingAssetsPath + "/AssetBundle";
        BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }
    [MenuItem("Tools/MarkAssetBundle")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        string path = Application.dataPath + "/Prefabs/Scenes/";
        DirectoryInfo dir = new DirectoryInfo(path);//得到文件夹信息
        FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();//文件夹下的所有文件包括文件夹
        for (int i = 0; i < fileInfo.Length; i++)
        {
            FileSystemInfo info = fileInfo[i];
            if (info is DirectoryInfo)//说明当前info是文件夹
            {
                string tempPath = Path.Combine(path, info.Name);
                SceneOverView(tempPath);
            }
        }
        string outPath = IPathTools.GetAssetBundlePath();
        CopyRecord(path, outPath);
        AssetDatabase.Refresh();
    }
    private static void CopyRecord(string sourcePath,string newPath)
    {
        DirectoryInfo dir = new DirectoryInfo(sourcePath);
        if (!dir.Exists)
        {
            Debug.LogWarning("不存在文件夹:" + sourcePath);
            return;
        }
        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;
            if (file != null && file.Extension == ".txt")
            {
                string sourFile = sourcePath + file.Name;
                string newFile = newPath + "/" + file.Name;
                File.Copy(sourFile, newFile, true);
            }
        }
    }
    /// <summary>
    /// 对整个场景遍历 
    /// </summary>
    /// <param name="scenePath"></param>
    private static void SceneOverView(string scenePath)
    {
        string textFileName = "Record.txt";
        string tempPath = scenePath + textFileName;
        FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate);
        StreamWriter bw = new StreamWriter(fs);
        //存储对应关系
        Dictionary<string, string> readDict = new Dictionary<string, string>();
        ChangeHead(scenePath, readDict);
        bw.Write(readDict.Count);
        bw.Write("\n");
        foreach(string key in readDict.Keys)
        {
            bw.Write(key);
            bw.Write(" ");
            bw.Write(readDict[key]);
            bw.Write("\n");
        }
        bw.Close();
        fs.Close();
    }
    /// <summary>
    /// 截取相对路径
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="writer"></param>
    private static void ChangeHead(string fullPath, Dictionary<string, string> writer)
    {
        //得到的是.../Assets前面的路径
        int assetIndex = fullPath.IndexOf("Assets");
        int fullPathLength = fullPath.Length;
        //得到Assets/后面的路径
        string scenesPath = fullPath.Substring(assetIndex, fullPathLength - assetIndex);
        DirectoryInfo dir = new DirectoryInfo(fullPath);
        if (dir != null)
        {
            ListFiles(dir, scenesPath, writer);
        }
        else
        {
            Debug.LogError("路径不存在");
        }
    }
    /// <summary>
    /// 遍历场景中每一个功能文件夹
    /// </summary>
    /// <param name="ufllPath"></param>
    /// <param name="writer"></param>
    private static void ListFiles(FileSystemInfo info, string scenesPath, Dictionary<string, string> writer)
    {
        if (!info.Exists)
        {
            Debug.LogWarning("文件不存在");
            return;
        }
        //FileSystemInfo包括文件夹和文件
        FileSystemInfo[] files = (info as DirectoryInfo).GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i] as FileInfo;
            if (file != null)//说明是文件
            {
                ChangeMark(file, scenesPath, writer);
            }
            else
            {
                ListFiles(files[i], scenesPath, writer);
            }
        }
    }
    /// <summary>
    /// 将\替换为/
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string FixedWindowPath(string path)
    {
        path = path.Replace("\\", "/");
        return path;
    }
    /// <summary>
    /// 得到一个文件所属的Bundle tag
    /// </summary>
    /// <param name="file"></param>
    /// <param name="scenesPath"></param>
    /// <returns></returns>
    public static string GetBundleTag(FileInfo file, string scenesPath)
    {
        //例 scenesPath:Assets/Prefabs/Scenes/Scene1
        //file路径 ...Assets\Prefabs\Scenes\Scene1\GameObject\Cube.prefab
        //file.Name Cube.prefab
        //得到的结果应为 Scenes/Scene1/GameObject
        string fileFullPath = file.FullName;
        fileFullPath = FixedWindowPath(fileFullPath);
        int assetCount = fileFullPath.IndexOf(scenesPath);
        assetCount += scenesPath.Length + 1;//此时assetCount指向Scene1的索引
        //lastIndexOf得到最后一个索引的位置
        int nameCount = fileFullPath.LastIndexOf(file.Name);
        int tempCount = scenesPath.LastIndexOf("/");
        //此时sceneHead为Scene1
        string sceneHead = scenesPath.Substring(tempCount + 1, scenesPath.Length - tempCount - 1);
        int temlength = nameCount - assetCount;
        if (temlength > 0)
        {
            string subStr = fileFullPath.Substring(assetCount, fileFullPath.Length - assetCount);
            string[] result = subStr.Split('/');
            //得到Scenes/Scene1
            return sceneHead + "/" + result[0];
        }
        else
        {
            //直接放在Scene1文件夹里
            return sceneHead;
        }
    }
    //改变物体的tag
    private static void ChangeMark(FileInfo tempFile, string scenesPath, Dictionary<string, string> writer)
    {
        if (tempFile.Extension == ".meta")
        {
            return;
        }
        string markstr = GetBundleTag(tempFile, scenesPath);
        SetBundleTag(tempFile, markstr, writer);
    }
    private static void SetBundleTag(FileInfo file,string markStr,Dictionary<string,string> writer)
    {
        string fullPath = file.FullName;
        fullPath = FixedWindowPath(fullPath);
        int assetCount = fullPath.IndexOf("Assets");
        //得到在assets下的文件路径
        string assetPath = fullPath.Substring(assetCount, fullPath.Length - assetCount);
        //得到文件打包的信息
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        importer.assetBundleName = markStr;
        if (file.Extension == ".unity")//场景文件
        {
            importer.assetBundleVariant = "u3d";
        }
        else
        {
            importer.assetBundleVariant = "ld";
        }
        string[] subMark = markStr.Split('/');
        string modleName = "";
        if (subMark.Length > 1)
        {
            modleName = subMark[subMark.Length - 1];
        }
        else
        {
            //Scene1 .../Scene1
            modleName = markStr;
        }
        string modlePath = markStr.ToLower() + "." + importer.assetBundleVariant;
        if (!writer.ContainsKey(modleName))
        {
            writer.Add(modleName, modlePath);
        }
    }
   
}
