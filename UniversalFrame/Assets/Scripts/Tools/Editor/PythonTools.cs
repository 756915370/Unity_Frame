using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class PythonTools
{
    [MenuItem("Tools/TestPython")]
    public static void TestPython()
    {
        
        string frontPath = Application.dataPath;
        int pos = frontPath.IndexOf("Assets");
        string rootPath = frontPath.Substring(0, pos);
        UnityEngine.Debug.Log(rootPath + "Tools/Python/test.bat");
        Process proc = Process.Start(rootPath + "Tools/Python/test.bat");
        
        proc.WaitForExit();
    }
}

