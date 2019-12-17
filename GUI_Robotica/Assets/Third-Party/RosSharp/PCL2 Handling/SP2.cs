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
using UnityEngine;
using System.Collections;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Geometry = RosSharp.RosBridgeClient.Messages.Geometry;

namespace RosSharp.RosBridgeClient
{

    public class SP2 : MonoBehaviour
    {

        public GameObject youbot;
        public GameObject point;

        public Vector3 spawnValues;
        public float spawnWait;

        //public float spawnMostWait;
        //public float spawnLeastWait;
        //public int startWait;

        public bool stop;
        private GameObject sensorf;
        private SensorReceive sensorscript;
        private GameObject markerScript1;
        
        private double x, y, z;


        private Vector3 transformedPosition;
        private Quaternion transformedRotation;

        private float test;

        public float ageCutoff = 4;
        public float nextDeletionTime = 1;
    
        public Dictionary<Vector3, DataPoint> dataPoints;
        public Geometry.Point pt;
        

        private void Start()
        {
            //get access to variables from sensorReceive script
            sensorf = GameObject.Find("SensorReceive");
            sensorscript = sensorf.GetComponent<SensorReceive>();

            //markerScript1 = GameObject.Find("ARUWPController");
            //markerScript = markerScript1.GetComponent<ARUWPMarker>();

            dataPoints = new Dictionary<Vector3, DataPoint>();

            StartCoroutine(waitSpawner());
            //Debug.Log("this is the entry"+markerScript.latestTransMatrix[0, 3]);
            
        }

        private void Update()
        {
            spawnWait = 0.0001f;
            //Debug.Log(sensorscript.x1);

        }

        IEnumerator waitSpawner()
        {
            while (!stop)
            {
                Vector3 dataPoint = new Vector3(-(float)sensorscript.y1, 0, (float)sensorscript.x1);

                //Debug.Log(markerScript.latestTransMatrix[0, 3]);
                //Debug.Log(dataPoints[]);
                //Debug.Log(dataPoint);

                //only update if marker is visible


                UpdateDataPoints(dataPoint);
                

                if (Time.time > nextDeletionTime)
                {
                    DeletePoints();
                }
                yield return new WaitForSeconds(spawnWait);
                
            }
            
        }



        bool UpdateDataPoints(Vector3 d)
        {
            
            DataPoint p;


            if (dataPoints.TryGetValue(d, out p) == true)
            {
                p.timeStamp = Time.time; // updates the time for this key so that its not deleted

                // inform caller data isn't new (but has been refreshed by time)
                return false;
            }

            // the key doesn't exist, so create it, and inform the caller
            // the key is new data

            Transform t = SpawnNewData(d);  // should return the transform of the new sphere

            //dataPoints.Add(d, new DataPoint(d.x, d.y, t));
            dataPoints[d] = new DataPoint(d.x, d.y, t); // stamps time in the constructor




            return true;
        }



        Transform SpawnNewData(Vector3 position)
        {
           

            //gameObject.transform.rotation = ARUWPUtils.QuaternionFromMatrix(markerScript.latestTransMatrix);


            transformedPosition = youbot.transform.position;
            transformedPosition += youbot.transform.TransformPoint(position);
            
            //Debug.Log(transformedPosition.x);

            GameObject o = Instantiate(point, transformedPosition, youbot.transform.rotation) as GameObject;

            return o.transform;
        }

        void DeletePoints()
        {
            // set ageCutoff as member of the class, this is how old data
            // can be before it should be pruned

            float cutoffAge = Time.time - ageCutoff;

            List<Vector3> toBeRemoved = new List<Vector3>();


            foreach (KeyValuePair<Vector3, DataPoint> d in dataPoints)
            {
                if (d.Value.timeStamp < cutoffAge)
                {
                    DestroyDataPoint(d.Value); // destroys gameobject specified in transform

                    toBeRemoved.Add(d.Key); 
                }
            }
            // remove points from dictionary, so that if later point is scanned again its treated as new
            foreach (Vector3 d in toBeRemoved)
            {
                dataPoints.Remove(d);
            }

        }

        void DestroyDataPoint(DataPoint objectInformation)
        {
            Destroy(objectInformation.trans.gameObject);
        }


    }
}

