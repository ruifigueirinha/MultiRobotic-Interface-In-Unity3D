using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class NavBallRotationConstraint : MonoBehaviour
{
    public Transform robotTransform;
    
    //void Start() 
    //{
    //    gameObject.transform.parent.rotation = Quaternion.identity;
    //    transform.localRotation = Quaternion.identity;
    //}
    
    void LateUpdate()
    {
        if (robotTransform != null) {
            //Quaternion tmpquat = Quaternion.Euler(robotTransform.localRotation.eulerAngles.x, robotTransform.localRotation.eulerAngles.y, (-1) * robotTransform.localRotation.eulerAngles.z);
            var rot = robotTransform.rotation;
            rot.z = -rot.z;

            transform.localRotation = rot; // robotTransform.localRotation;
        }
    }
}