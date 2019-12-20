using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FileCustomDropdown : CustomDropdown
{

    [SerializeField]
    private GameObject pointCloudManagerPrefab;
    private GameObject pointCloudManager;
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
                if (pointCloudManager == null)
                {
                    //UnityEngine.Debug.Log("Instantiating PCL Manager");
                }
                pointCloudManager = Instantiate(pointCloudManagerPrefab);
                pointCloudManager.GetComponent<PointCloudManager>().OpenFileExplorer();
                break;
            case 3:
                break;
            case 4:
                Exit();
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
