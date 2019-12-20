using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RosSharp.RosBridgeClient;

public class DestroyCamPreview : MonoBehaviour {
    public CamManager camManager;
    private RosConnector rosConn;
    private Button button;
	// Use this for initialization
	void Start () {

        rosConn = gameObject.transform.parent.GetComponent<RosConnector>();
        camManager = GameObject.FindGameObjectWithTag("CameraTopicSelector").GetComponent<CamManager>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
	}

    void OnButtonClick() {
        Debug.Log("Object Destroyed");
        Destroy(gameObject.transform.parent.gameObject);
        Debug.Log(rosConn.RosBridgeServerUrl);
        camManager.CamWebSockets[rosConn.RosBridgeServerUrl] = false;
    }

}
