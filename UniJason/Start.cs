using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

[InitializeOnLoad]
public class Start {
    static Start()
    {
        UnityEngine.Debug.Log("Up and running");
        Process myProcess = new Process();
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.StartInfo.RedirectStandardOutput = true;
        myProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
        string r = "/c" + "\"" + Application.dataPath + "/UniJason/script.bat" + "\"";
        myProcess.StartInfo.Arguments = r;
        myProcess.Start();
        System.Threading.Thread.Sleep(5000);
    }
}