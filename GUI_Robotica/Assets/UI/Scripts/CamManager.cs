using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CamManager : MonoBehaviour
{
    [SerializeField]
    public Dictionary<string, bool> CamWebSockets;
    public bool localhost = true;
    public string IPV4;
    // Use this for initialization
    void Start()
    {
        CamWebSockets = new Dictionary<string, bool>();
        for (int i = 9093; i <= 9100; i++)
        {
            if(localhost)
                CamWebSockets.Add("ws://127.0.0.1:" + i, false);
            else
                CamWebSockets.Add("ws://" + IPV4  + ":" + i, false);
        }      
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
