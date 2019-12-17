//Obtem os topicos de imagem e disponibiliza os no dropdown das camaras

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using RosSharp.RosBridgeClient.Messages.dds_unity;

public class GetRosCameraTopics : MonoBehaviour {

    private StringArraySubscriber StringArrayScript;
    private bool thereisPoseScript = false;
    GameObject ROS_connector;
    private string[] topics;
    private string[] types;

    public TMP_Dropdown dropdown;

    public string[] imgTopics;

    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    private Dictionary<string, string> topicList = new Dictionary<string, string>();
    public List<string> CimageList = new List<string>(); //lista de topicos de compressed image

    // Use this for initialization
    void Start () {
        ROS_connector = GameObject.FindGameObjectWithTag("RosSensorInfo1");
        dropdown = gameObject.GetComponent<TMP_Dropdown>();
        try {
            StringArrayScript = ROS_connector.GetComponent<StringArraySubscriber>();
        } catch { }

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > nextActionTime)
        {
            dropdown.ClearOptions();
            CimageList.Add("");

            nextActionTime += period;
            topics = StringArrayScript.topics;
            types = StringArrayScript.types;

            //for (int j = 0; j < topics.Length; j++)
            //{
            //    Debug.Log("Topic: " + topics[j] + " Type: " + types[j]);
            //}

            //Preenche a lista de topicos
            for (int i = 0; i < topics.Length; i++)
            {
                try
                {
                    topicList.Add(topics[i], types[i]);
                    //Debug.Log("List size: " + topicList.Count);
                }
                catch (System.Exception)
                {


                }
            }
            //foreach (KeyValuePair<string, string> pair in topicList)
            //{
            //    Debug.Log("Dictionary: " + string.Format("{0}, {1}", pair.Key, pair.Value));
            //}

            //Seleciona os topicos que sao sensor_msgs/CompressedImage e coloca-os numa lista a parte so de Compressed Image
            foreach (KeyValuePair<string, string> pair in topicList)
            {
                if (pair.Value == "sensor_msgs/CompressedImage")
                {
                    try
                    {
                        CimageList.Add(pair.Key);
                    }
                    catch (System.Exception)
                    {

                    }
                    //Debug.Log(string.Format("{0}", pair.Key));
                }
            }
            dropdown.AddOptions(CimageList);
            
            CimageList.Clear();
            topicList.Clear();
        }
    }
}
