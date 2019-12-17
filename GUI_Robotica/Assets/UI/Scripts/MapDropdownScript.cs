/* Contem as funcoes que os menus de dropdown correm*/


using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Mapbox.Unity.Map;

public class MapDropdownScript : MonoBehaviour
{
    private int value;
    private TMPro.TMP_Dropdown dropdown;

    private GameObject mapbox;
    private AbstractMap abstractMap;
    private void Awake() {
        dropdown = gameObject.GetComponent<TMPro.TMP_Dropdown>();
        mapbox = GameObject.FindGameObjectWithTag("Mapbox");
    }

    public void ValueEvaluate(){
        value = dropdown.value;
        switch (value)
        {
            case 0:
                GetOSM();
                break;
            case 1:
                DownloadMap();
                break;
            case 2:
                GetMapbox();
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
    private void GetMapbox(){
         GameObject popupwindow = gameObject.transform.GetChild(2).gameObject;
         popupwindow.SetActive(true);
         ResetDropdown();
         //abstractMap.CenterLatitudeLongitude = new Vector2()
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
   void ResetDropdown(){
       dropdown.value = 100;
   }
}
