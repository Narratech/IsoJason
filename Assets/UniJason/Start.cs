using System;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

[InitializeOnLoad]
public class Start {
    static Start()
    {
        /*
        UnityEngine.Debug.Log("Up and running");
        Process myProcess = new Process();
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.StartInfo.RedirectStandardOutput = true;
        string so = Environment.OSVersion.ToString();
        UnityEngine.Debug.Log(so);
        if (so.Contains("Windows")) {
            UnityEngine.Debug.Log("Se est� ejecutando sobre Windows"); 
            myProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
            string r = "/c" + "\"" + Application.dataPath + "/UniJason/script.bat" + "\"";
            myProcess.StartInfo.Arguments = r;
        }
        else {
            string r = Application.dataPath + "/UniJason/script.sh";
            myProcess.StartInfo.FileName = "/bin/sh";
		myProcess.StartInfo.Arguments = r;
        }
        myProcess.Start();
        Connection c = Connection.getInstance();
        string jsonMap = "{\"name\":\"unity\",\"parameters\":{}}";
        Connection.getInstance().sendEvent(true, jsonMap);
        while (c.ReceivedEvent()==null){
            System.Threading.Thread.Sleep(1);
        }         
        */
    }
}