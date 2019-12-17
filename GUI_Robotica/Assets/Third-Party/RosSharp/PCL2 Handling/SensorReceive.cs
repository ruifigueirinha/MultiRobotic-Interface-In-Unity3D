/*
Author: Linh Kaestner (linh4138@yahoo.de)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using RosSharp.RosBridgeClient.Messages.Sensor;
using UnityEngine;
using Geometry = RosSharp.RosBridgeClient.Messages.Geometry;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SensorReceive : MessageReceiver
    {
        public override Type MessageType { get { return (typeof(Geometry.Point)); } }
        
        private byte[] sensorData;
        private byte sensorData1;
        private int height;
        public float x = 0;
        public double x1 = 0;
        public float y = 0;
        public double y1 = 0;
        public bool isMessageReceived;
        private int lengthOfSensor;
        public static Geometry.Point xyz; //este geometry point é um geometry_msgs/Point


        public GameObject point;
        public Vector3 spawnValues;
        public float spawnWait;
        public float spawnMostWait;
        public float spawnLeastWait;
        public int startWait;
        public bool stop;



        private MeshRenderer meshRenderer;
        private Texture2D texture2D;

        private void Awake()
        {
            MessageReception += ReceiveMessage;
            
        }
        private void Start()
        {
            texture2D = new Texture2D(1, 1);
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Standard"));

            
        }

        private void ReceiveMessage(object sender, MessageEventArgs e)
        {

            xyz = ((Geometry.Point)e.Message);

            x = ((Geometry.Point)e.Message).x;
            y = ((Geometry.Point)e.Message).y;

            x1 = Math.Round(x, 4);
            y1 = Math.Round(y, 4);

            isMessageReceived = true;
            //Debug.Log(x);
            Debug.Log("x=" + xyz.x + " y=" + xyz.y);

        }

        protected override void ReceiveMessage(PointCloud2 message)
        {
            throw new NotImplementedException();
        }
    }
}


