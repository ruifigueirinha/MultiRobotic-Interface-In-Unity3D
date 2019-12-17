using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "MainCamera" || other.transform.tag == "Camera") {
            Debug.Log("Entered");
            RenderSettings.fog = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.tag == "MainCamera" || other.transform.tag == "Camera")
        {
            Debug.Log("Exited");
            RenderSettings.fog = false;
        }
    }
}
