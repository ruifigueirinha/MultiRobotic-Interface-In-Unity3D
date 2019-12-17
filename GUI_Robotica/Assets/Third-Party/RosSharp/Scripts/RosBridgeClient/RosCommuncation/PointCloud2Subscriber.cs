//Rui Figueirinha

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RosSharp;


namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    [RequireComponent(typeof(MeshFilter))]
    public class PointCloud2Subscriber : Subscriber<Messages.Sensor.PointCloud2>
    {
        public MeshRenderer meshRenderer;

        //private Texture2D texture2D;
        private Messages.Sensor.PointField[] pcl_fields;
        private byte[] pcl_data;
        private int pcl_point_step;
        private int pcl_width;
        private int pcl_height;

        private bool isMessageReceived;

        private bool runOnce = true;
        public bool invertZ;

        [SerializeField]
        private List<GameObject> sphereList;
        [SerializeField]
        private List<GameObject> pclPointList;

        private List<Vector3> meshVertices;
        private List<Mesh> meshesList;  //Lista de meshes

       // private Vector3[] meshVertices;

        GameObject pclPoint;

        private string tempTopic;
        private uint vector3_counter;
        private Vector3 xyz_coords;


        public GameObject spherePrefab;

        MeshRenderer meshRender;
        Mesh mesh;
        public Material material;
        public Shader shader;
        public Texture sprite;
        public float size;


        //protected override void Start()
        //{



        // void OnEnable(){
        // 	base.Start();
        //     texture2D = new Texture2D(1, 1);
        //     //Application.targetFrameRate = 300;
        //     //meshRenderer.material = new Material(Shader.Find("Standard"));
        // }


        private void Awake()
        {
            sphereList = new List<GameObject>();
            pclPointList = new List<GameObject>();
            meshVertices = new List<Vector3>();

            meshesList = new List<Mesh>();

            material = new Material(shader);
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            meshRender = GetComponent<MeshRenderer>();
            meshRender.material = material;
            material.SetTexture("_MainTex", sprite);


        }

        private void Update()
        {
            if (Topic != null && tempTopic != Topic)
            {
                tempTopic = Topic;
                base.Start();
                //texture2D = new Texture2D(1, 1);
            }

            if (isMessageReceived)
                ProcessMessage();


        }

        protected override void ReceiveMessage(Messages.Sensor.PointCloud2 message)
        {
            pcl_fields = message.fields;
            pcl_data = message.data;
            pcl_point_step = message.point_step;
            pcl_height = message.height;
            pcl_width = message.width;


            isMessageReceived = true;
        }

        void ProcessMessage()
        {


            for (int i = 0; i < (pcl_width*pcl_height); i++)
            {

                float x = System.BitConverter.ToSingle(pcl_data, i * (int)pcl_point_step + pcl_fields[0].offset);
                float y = System.BitConverter.ToSingle(pcl_data, i * (int)pcl_point_step + pcl_fields[1].offset);
                float z = System.BitConverter.ToSingle(pcl_data, i * (int)pcl_point_step + pcl_fields[2].offset);

                if (!Single.IsNaN(x) && !Single.IsNaN(y) && !Single.IsNaN(z)) { //Verifica se os pontos da PCL nao sao NaN

                    //float xvalue = x - xyz_coords.x;

                    //if(Math.Abs(xvalue) )
                    
                    xyz_coords.x = x;
                    xyz_coords.y = y;
                    xyz_coords.z = z;


                    //Debug.Log(xyz_coords);

                }




                //if (runOnce)
                //{
                //    for (int j = 0; j < pcl_width * pcl_height; j++)
                //    {
                //        sphereList.Add(Instantiate(spherePrefab, new Vector3(0, 0, 0), gameObject.transform.rotation, gameObject.transform));
                //    }
                //}
                //runOnce = false;


                if (invertZ)
                    xyz_coords = PclRos2PclUnity(xyz_coords);


                //new Quaternion(0, 0, 0, 0)));//Instantiate(spherePrefab, xyz_coords, gameObject.transform.rotation));//new Quaternion(0, 0, 0, 0));
                //pclPointList.Add(Instantiate(InstantiateMesh(), xyz_coords, gameObject.transform.rotation, gameObject.transform));
                //sphereList.Add(InstantiateMesh())//Instantiate(spherePrefab, xyz_coords, gameObject.transform.rotation));//new Quaternion(0, 0, 0, 0));



                //sphereList[i].transform.localPosition = xyz_coords;
                if(xyz_coords != Vector3.zero && i % 10 == 0)
                    meshVertices.Add(xyz_coords);

          

            }
            isMessageReceived = false;

            AddMesh(meshVertices); //Adiciona os novos vertices a pointcloud aquando da rececao de nova informacao
            //UpdateMesh(meshVertices);
            meshVertices.Clear();


        }

        
        Vector3 PclRos2PclUnity(Vector3 vector3)
        {
            return new Vector3(-vector3.y, -vector3.z, vector3.x);
        }


        void AddMesh(List<Vector3> points)
        {
            GameObject SubmeshObj = new GameObject("Submesh");
            SubmeshObj.transform.position = gameObject.transform.position;
            SubmeshObj.AddComponent<MeshFilter>();
            SubmeshObj.AddComponent<MeshRenderer>();
            Mesh PCL_mesh = new Mesh();
            material = new Material(shader);
            SubmeshObj.GetComponent<MeshFilter>().mesh = PCL_mesh;
            meshRender = SubmeshObj.GetComponent<MeshRenderer>();
            meshRender.material = material;
            material.SetTexture("_MainTex", sprite);


            PCL_mesh.vertices = points.ToArray();
            //material.SetFloat("_Size", size);

            int[] indices = new int[points.Count];
            for (int i = 0; i < points.Count; i++)
                indices[i] = i;

            PCL_mesh.SetIndices(indices, MeshTopology.Points, 0, false);

            meshesList.Add(PCL_mesh);


        }

        void UpdateMesh(List<Vector3> points)
        {


            // mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100f);
            //mesh.vertices = Vertex;

            mesh.Clear();

            // make changes to the Mesh by creating arrays which contain the new values

            mesh.vertices = points.ToArray();
            //material.SetFloat("_Size", size);

            int[] indices = new int[points.Count];
            for (int i = 0; i < points.Count; i++)
                indices[i] = i;
            
            mesh.SetIndices(indices, MeshTopology.Points, 0, false);


        }
    }
}

