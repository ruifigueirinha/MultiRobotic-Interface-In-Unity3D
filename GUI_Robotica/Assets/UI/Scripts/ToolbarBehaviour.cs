/* Contem as funcoes que os menus de dropdown correm*/


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ToolbarBehaviour : MonoBehaviour
{
    [SerializeField]
    //private List<TMPro.TMP_Dropdown> ToolbarList = new List<TMPro.TMP_Dropdown>(); // lista que contem todos os menus de dropdown
    private TMPro.TMP_Dropdown[] ToolbarList;
    // Start is called before the first frame update
    void Start()
    {
        
        ToolbarList = gameObject.transform.parent.GetComponentsInChildren<TMPro.TMP_Dropdown>();
        //ToolbarList.AddRange = gameObject.transform.parent.GetComponentInChildren<TMPro.TMP_Dropdown>();
    }


   public void GetOSM(){
        string path = EditorUtility.OpenFilePanel("Open OSM file", "", "osm");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
        }
   }
}
