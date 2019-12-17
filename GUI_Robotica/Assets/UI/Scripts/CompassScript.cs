using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompassScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextMesh;


    // Update is called once per frame
    void Update()
    {
        TextMesh.text = Mathf.RoundToInt(gameObject.transform.localRotation.eulerAngles.z).ToString() ;
    }
}
