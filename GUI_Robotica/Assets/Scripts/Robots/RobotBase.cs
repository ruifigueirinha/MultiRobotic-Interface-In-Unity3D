using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script que armazena todas as variaveis do robot
public class RobotBase : MonoBehaviour
{

    private GameObject robot;
    private bool _selected;
    private Material[] materials;
    private Shader highlightShader;
    private Shader defaultShader;

    [HideInInspector]
    public bool selected
    { get { return _selected; } set { _selected = value; if (_selected) RobotSelected(); else RobotDeselected(); } }
     
    
    
    // Start is called before the first frame update
    void Awake()
    {
        robot = gameObject;
        materials = gameObject.GetComponent<Renderer>().materials;
        defaultShader = Shader.Find("Standard");
        highlightShader = Shader.Find("Outlined/UltimateOutline");
    }

    private void RobotSelected() // metodo que corre quando o robot e selecionado
    { 
        AssignOutlineMaterials();
        //Debug.Log("Robot Selected");
    }

    private void RobotDeselected() // metodo que corre quando o robot e selecionado
    {
        AssignDefaultMaterials();
        //Debug.Log("Robot Deselected");
    }


    public void AssignDefaultMaterials()
    {
        foreach (var item in materials)
        {
            item.shader = defaultShader;
        }
    }

    public void AssignOutlineMaterials()
    {
        foreach (var item in materials)
        {
            item.shader = highlightShader;
        }
    }
}
