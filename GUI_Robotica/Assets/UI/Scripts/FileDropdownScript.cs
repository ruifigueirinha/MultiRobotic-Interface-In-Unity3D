/* Contem as funcoes que os menus de dropdown correm*/


using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FileDropdownScript : MonoBehaviour
{
    private int value;
    private TMPro.TMP_Dropdown dropdown;
    private void Start() {
        dropdown = gameObject.GetComponent<TMPro.TMP_Dropdown>();
    }

    public void ValueEvaluate(){
        value = dropdown.value;
        
        switch (value)
        {
            case -1:
                break;
            case 0:
                GetOSM();
                break;
            case 1:
                DownloadMap();
                break;
            case 3:
                Exit();
                break;
            case 10:
                break;
            default:
                break;
        }
    }
   private void GetOSM(){
        string path = EditorUtility.OpenFilePanel("Open OSM file", "", "osm");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
        }
        ResetDropdown();
   }

    private void DownloadMap(){
        ProcessStartInfo proc = new ProcessStartInfo();
        proc.FileName = "/bin/bash";
        proc.WorkingDirectory = Application.dataPath + "/OSM2World";
        proc.Arguments = "getmap.sh";
        proc.WindowStyle = ProcessWindowStyle.Normal;
        proc.CreateNoWindow = true;
        Process.Start(proc);
        ResetDropdown();
    }

    private void Exit(){
        //var quitpanel = GameObject.Find("QuitPanel");
        //quitpanel.SetActive(true);
        UnityEngine.Debug.Log("Exit Clicked");
        ResetDropdown();
    }
   void ResetDropdown(){
       dropdown.value = 10;
   }
}
