using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraWorldSpaceUI : MonoBehaviour
{
    // Update is called once per frame
    private Transform mainCameraTransform;

    void Awake() {
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        gameObject.transform.LookAt(mainCameraTransform);
        gameObject.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
