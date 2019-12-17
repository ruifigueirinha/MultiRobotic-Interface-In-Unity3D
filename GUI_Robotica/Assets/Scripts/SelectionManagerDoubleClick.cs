using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManagerDoubleClick : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Robot";
    [SerializeField] private Material[] highlightMaterials;
    [SerializeField] private Material[] defaultMaterial;
    private Material[] tmpMaterial;
    private Shader highlightShader;
    private Shader defaultShader;

    private Transform selection;

    private float clicked = 0;
    private float clicktime = 0;
    private float clickdelay = 0.5f;
    private void Start() {
        defaultShader = Shader.Find("Standard");
        highlightShader = Shader.Find("Outlined/UltimateOutline");
    }
    private void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0)){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Debug.Log("Didn't click Robot");
            //AssignDefautMaterials(); 
            // clicked = 0;
            // clicktime = 0;   

            AssignDefautMaterials();
            if (Physics.Raycast(ray, out hit)){
                selection = hit.transform;


                // Se o click foi no robot
                if (selection.transform.gameObject.tag == "Robot" && !(EventSystem.current.IsPointerOverGameObject())) // !EventSystem.current.IsPointerOverGameObject() para nao selecionar por cima da UI
                {
                    clicked++;
                    if(clicked == 1){
                        clicktime = Time.time;
                        AssignOutlineMaterials();
                        Debug.Log("Single Clicked Robot");
                    }
                        
                    if(clicked > 1 && Time.time - clicktime < clickdelay){
                        clicked = 0;
                        clicktime = 0;
                        //Camera.main.transform
                        AssignOutlineMaterials();
                        Debug.Log("Double Clicked Robot");
                    }
                    else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
                }
                else if(selection.transform.gameObject.tag != "Robot"){
                    Debug.Log("Didn't click Robot");
                    AssignDefautMaterials();
                    clicked = 0;
                    clicktime = 0;
                }               
            }
        }
    }

    private void AssignDefautMaterials(){
        if (tmpMaterial != null) {
            foreach (var item in tmpMaterial)
            {
                item.shader = defaultShader;
            }
        }
    }

    private void AssignOutlineMaterials(){
        tmpMaterial = selection.GetComponent<Renderer>().materials;
        foreach (var item in selection.GetComponent<Renderer>().materials)
        {
            item.shader = highlightShader;
        }
    }
}


