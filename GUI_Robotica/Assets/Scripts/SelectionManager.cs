using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private string selectableTag = "Robot";
    [SerializeField] private Material[] highlightMaterials;
    [SerializeField] private Material[] defaultMaterial;
    private Material[] tmpMaterial;
    private Shader highlightShader;
    private Shader defaultShader;

    [SerializeField]
    private LayerMask selectableLayer;

    private Transform selection;
    private Transform currSelection;
    private RobotBase robotScript;

    private float clicked = 0;
    private float clicktime = 0;
    private float clickdelay = 0.5f;
    private void Start() {
        mainCamera = Camera.main;
        defaultShader = Shader.Find("Standard");
        highlightShader = Shader.Find("Outlined/UltimateOutline");
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer) && !(EventSystem.current.IsPointerOverGameObject())){
                selection = hit.transform;
                if (selection.tag == "Robot" && selection != currSelection)
                {
                    SelectCurrentRobot();
                }
            }
            else
            {
                ClearCurrSelectionRobot();
            }
        }
    }

    /********* Managing Robot selections methods ************/
    private void ClearCurrSelectionRobot() {
        if (currSelection != null)
        {
            Debug.Log(currSelection.name + " deselected");
            currSelection.GetComponent<RobotBase>().selected = false;
            currSelection = null;
        }
    }

    private void SelectCurrentRobot() {
        ClearCurrSelectionRobot();
        robotScript = selection.GetComponent<RobotBase>();
        robotScript.selected = true;
        Debug.Log(selection.name + " selected");
        currSelection = selection;
    }


}

    //private void AssignDefautMaterials(){
    //    if (tmpMaterial != null) {
    //        robotScript.selected = false;
    //        foreach (var item in tmpMaterial)
    //        {
    //            item.shader = defaultShader;
    //        }
    //    }
    //}

    //private void AssignOutlineMaterials(){
    //    tmpMaterial = selection.GetComponent<Renderer>().materials;
    //    foreach (var item in tmpMaterial)
    //    {
    //        item.shader = highlightShader;
    //    }
    //}
//}





//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class SelectionManager : MonoBehaviour
//{
//    private Camera mainCamera;

//    [SerializeField] private string selectableTag = "Robot";
//    [SerializeField] private Material[] highlightMaterials;
//    [SerializeField] private Material[] defaultMaterial;
//    private Material[] tmpMaterial;
//    private Shader highlightShader;
//    private Shader defaultShader;

//    private Transform selection;
//    private RobotScript robotScript;

//    private float clicked = 0;
//    private float clicktime = 0;
//    private float clickdelay = 0.5f;
//    private void Start()
//    {
//        mainCamera = Camera.main;
//        defaultShader = Shader.Find("Standard");
//        highlightShader = Shader.Find("Outlined/UltimateOutline");
//    }
//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;

//            //Debug.Log("Didn't Hit Robot");


//            if (Physics.Raycast(ray, out hit))
//            {
//                selection = hit.transform;
//                if (selection.transform.gameObject.tag == "Robot" && !(EventSystem.current.IsPointerOverGameObject())) // !EventSystem.current.IsPointerOverGameObject() para nao selecionar por cima da UI
//                {
//                    robotScript = selection.GetComponent<RobotScript>();
//                    robotScript.selected = true;

//                    Debug.Log(selection.name + " selected");
//                }
//                else if (selection.transform.gameObject.tag != "Robot")
//                { // se acertou num collider que nao era um robot
//                    if (selection != null && selection.tag == "Robot")
//                        selection.GetComponent<RobotScript>().selected = false;
//                }
//            }
//            else
//            { // se nao acertou em nenhum collider
//                if (selection != null && selection.tag == "Robot")
//                { selection.GetComponent<RobotScript>().selected = false; }
//            }
//        }
//    }

//    //private void AssignDefautMaterials(){
//    //    if (tmpMaterial != null) {
//    //        robotScript.selected = false;
//    //        foreach (var item in tmpMaterial)
//    //        {
//    //            item.shader = defaultShader;
//    //        }
//    //    }
//    //}

//    //private void AssignOutlineMaterials(){
//    //    tmpMaterial = selection.GetComponent<Renderer>().materials;
//    //    foreach (var item in tmpMaterial)
//    //    {
//    //        item.shader = highlightShader;
//    //    }
//    //}
//}

