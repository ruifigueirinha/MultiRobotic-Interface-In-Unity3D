using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVLScript : MonoBehaviour
{
    [System.Serializable]
    public class DVL // Simplesmente armazena os materiais de cada beam e podera vir a fazer outras coisas
    {
        public Material[] beamMaterials;
        public Color32[] beamColors32; // materiais pertencentes aos beams

        public DVL() 
        {
            beamMaterials = new Material[4];
            beamColors32 = new Color32[4];
        }
    }


    public DVL dvl = new DVL();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
