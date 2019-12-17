
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
    
    public class SpawnPoints : MonoBehaviour
    {
       

        public GameObject point;
        public Vector3 spawnValues;
        public float spawnWait;
        public float spawnMostWait;
        public float spawnLeastWait;
        public int startWait;
        public bool stop;
        public GameObject sensorf;
        public SensorReceive sensorscript;
        public double x, y, z;
        
        //trigger 
        public bool arr1 = false;

        //Position Arrays 
        public Vector3[] spawnPositions;
        public Vector3[] sPos;
        public Vector3[] newPositions;


        public Geometry.Point pt;
        

       
        private void Start()
        {

           

            sensorf = GameObject.Find("SensorReceiver");
            sensorscript = sensorf.GetComponent<SensorReceive>();

            spawnPositions = new Vector3[50];
            sPos = new Vector3[50];
            newPositions = new Vector3[50];
            StartCoroutine(waitSpawner());
            
        }

        private void Update()
        {
            spawnWait = 0.0001f;
            

        }

        


        IEnumerator waitSpawner()
        {
            

            

            int i;
            sPos[0] = new Vector3(0,0,0);
            //spawnPositions[1] = new Vector3(0, 1, 0);

            for (i = 1; i < 50; i++)
            {



                //if (spawnPositions[i] != spawnPositions[i-1])
                //    for (int j = 0; j < 20; ++j)

                //Debug.Log(i);
                //Debug.Log(x + "" + y);
                spawnPositions[i] = new Vector3((float)sensorscript.x1, 1, (float)sensorscript.y1);
                //if (sPos[i] != sPos[i - 1])
                if (!(sPos.Contains(spawnPositions[i])))

                {
                    sPos[i] = spawnPositions[i];
                    Debug.Log(i + ". :" + sPos[i] + "8th element is:" + sPos[8]);
                    
                    Instantiate(point, sPos[i] + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);

                }
                else
                {
                    i -= 1;
                }
                
            }
            //Array 1 is created 
            Debug.Log("created array 1!");
            arr1 = true;
            i = 1;


            while (!stop)
            {

                //Debug.Log(sensorscript.x1);
                //Debug.Log("hii");
                //Debug.Log(sensorscript.x);

                Vector3 spawnPosition = new Vector3(sensorscript.x, 1, sensorscript.y);

                Instantiate(point, spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);

                yield return new WaitForSeconds(spawnWait);
            }


            if (arr1)
            {
               for(int j = 0; j < 3; j++)
                {
                    for (i = 1; i < 50; i++)
                    {
                        spawnPositions[i] = new Vector3((float)sensorscript.x1, 1, (float)sensorscript.y1);

                        if (!(newPositions.Contains(spawnPositions[i])))

                        {
                            newPositions[i] = spawnPositions[i];
                            //Debug.Log(i + ". :" + sPos[i] + "8th element is:" + sPos[8]);
                            //Instantiate(point, sPos[i] + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);

                        }
                        else
                        {
                            i -= 1;
                        }
                        
                    }
                    //created arr2
                    IEnumerable<Vector3> onlyFirstSelect = newPositions.Except(spawnPositions);
                    Debug.Log("created array 2 ! the exceptions are:" + onlyFirstSelect.ElementAtOrDefault(1));
                }
                
                
                
                
            }
            //if new points arrive, delete old ones --> specific algorithm which ones to delete?
            //if spawnpositions = 40 then reset i which means delete old ones and put new positions into array spawnpositions 
            //if (spawnPositions[49] != null)
            //{
            //    i = 1;
            //}





            //Debug.Log(spawnPositions[30]);
            //Debug.Log(i +". :"+spawnPositions[i]);
            // spawn soheres 
            //if (spawnPositions[i] != spawnPositions[i-1])
            //{
            //    Instantiate(point, spawnPositions[i] + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            //}

            yield return new WaitForSeconds(spawnWait);



            //Debug.Log("hii");
            //Debug.Log(sensorscript.x);

            //Vector3 spawnPosition = new Vector3(sensorscript.x, 1, sensorscript.y);

            //Instantiate(point, spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);

            //yield return new WaitForSeconds(spawnWait);
        }


        



    }
}

