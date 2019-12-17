using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {

    public Text Text;
    public Text Text2;
    public GameObject youbot;
    private Vector3 position;
    private float x, y, z;
    private float o, p, q;


    // Use this for initialization
    void Start () {
        x = youbot.transform.position.x;
        y = youbot.transform.position.y;
        z = youbot.transform.position.z;
        
    }
	
	// Update is called once per frame
	void Update () {
        x = youbot.transform.position.x;
        y = youbot.transform.position.y;
        z = youbot.transform.position.z;

        o = youbot.transform.rotation.x;
        p = youbot.transform.rotation.y;
        q = youbot.transform.rotation.z;

        Text.text = (" x:" + x + " y:"+y + " z:" + z);
        Text2.text = (" rotx:" + o + " roty:" + p + " rotz:" + q);
    }
}
