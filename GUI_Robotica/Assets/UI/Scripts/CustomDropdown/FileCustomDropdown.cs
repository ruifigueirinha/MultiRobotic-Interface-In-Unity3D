using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FileCustomDropdown : CustomDropdown
{
    public GameObject QuitPanel;
    public override void ValueEvaluate(int index) 
    {
        switch (index)
        {
            case 0:
                GetOSM();
                break;
            case 1:
                DownloadMap();
                break;
            case 2:
                Exit();
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    private void GetOSM()
    {
        string path = EditorUtility.OpenFilePanel("Open OSM file", "", "osm");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
        }
    }

    private void DownloadMap()
    {
        ProcessStartInfo proc = new ProcessStartInfo();
        proc.FileName = "/bin/bash";
        proc.WorkingDirectory = Application.dataPath + "/OSM2World";
        proc.Arguments = "getmap.sh";
        proc.WindowStyle = ProcessWindowStyle.Normal;
        proc.CreateNoWindow = true;
        Process.Start(proc);  
    }

    private void Exit()
    {
        UnityEngine.Debug.Log("Exit Clicked");
        if (QuitPanel != null)
            QuitPanel.SetActive(true);
    }
}
