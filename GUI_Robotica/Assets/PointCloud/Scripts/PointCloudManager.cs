// Script que gere as PointClouds instanciadas com leitura do ficheiro em threading para nao parar o jogo enquanto as carrega

using System.Collections;
//using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;

// Classes PointCloud
[System.Serializable]
public class PointCloud : MonoBehaviour // Classe que armazena point clouds to tipo XYZ com possibilidade de RGB
{
    [HideInInspector]
    public long npoints;
    private bool _loaded = false;
    public List<Submesh> SubmeshList; // Guarda os parametros das diferentes meshes
    private List<Mesh> SubmeshMeshes;
    public Vector3[] points;
    public Color32[] colors32;
    GameObject ParentPCL;

    private Material material;
    public Shader shader;

    private MeshRenderer meshRenderer;
    [HideInInspector]
    public bool loaded
    {
        get { return _loaded; } // retorna se a pcl esta carregada
        set 
        {
            _loaded = value;
        }
    }

    public class Submesh // estrutura que define uma submesh da point cloud 
    {
        public List<Vector3> subMeshpoints;
        public List<Color32> subMeshcolors32;
        public int submesh_nPoints;
        public Submesh() 
        {
            subMeshpoints = new List<Vector3>();
            subMeshcolors32 = new List<Color32>();
            subMeshpoints.Capacity = System.UInt16.MaxValue; // Reserva espaço na lista
            subMeshcolors32.Capacity = System.UInt16.MaxValue; // Reserva espaço na lista
        }
    }

    public PointCloud() 
    {
        this.npoints = 0;
        SubmeshList = new List<Submesh>();
        SubmeshMeshes = new List<Mesh>();

    }
    void Start() 
    {
        ParentPCL = gameObject;
        Debug.Log("Finding Shader...");
        shader = Shader.Find("Point Cloud/Point");
        if (shader != null) 
        {
            Debug.Log("shader found");
        }
    }
    void Update() 
    {
        //Debug.Log("Loaded false");
        //if (loaded == true) 
        //{
        //    Debug.Log("Loaded true");
        //    InstantiateSubmeshes();
        //    loaded = false;
        //}
    }
    //private void InstantiateSubmeshes() // Instancia as submeshes
    //{
    //    foreach (var submesh in SubmeshList)
    //    {
    //        Debug.Log("Instantiating Submesh");
    //        int[] submeshindices;
    //        Mesh PCL_mesh = new Mesh();
    //        GameObject meshObj = new GameObject("Submesh");
    //        //meshObj.transform.position = gameObject.transform.GetChild(transform.childCount - 1).position;
    //        meshObj.transform.SetParent(ParentPCL.transform); // fica child do ultimo child 
    //        meshObj.AddComponent<MeshFilter>();
    //        meshObj.AddComponent<MeshRenderer>();

    //        material = new Material(Shader.Find("Point Cloud/Point"));

    //        meshObj.GetComponent<MeshFilter>().mesh = PCL_mesh;
    //        meshRenderer = meshObj.GetComponent<MeshRenderer>();
    //        meshRenderer.material = material;
    //        //material.SetTexture("_MainTex", sprite);

    //        PCL_mesh.vertices = submesh.subMeshpoints.ToArray();
    //        material.SetFloat("_Size", .1f); //pointSize

    //        submeshindices = new int[submesh.subMeshpoints.Count];
    //        for (int i = 0; i < submesh.subMeshpoints.Count; i++)
    //            submeshindices[i] = i;

    //        PCL_mesh.SetIndices(submeshindices, MeshTopology.Points, 0, false);

    //        // Se cada ponto possuir cor atribui
    //        if (submesh.subMeshcolors32 != null)
    //        {
    //            PCL_mesh.colors32 = submesh.subMeshcolors32.ToArray();
    //        }

    //        Debug.Log("Submesh Added");
    //        SubmeshMeshes.Add(PCL_mesh);
    //    }
    //}
    public void ChangePCLColor() // mudar a cor dos pontos para original ou por topografia
    {
    
    }
    public void SaveMesh() 
    { 
    
    }
    public void ConvertToObj() 
    { 
    
    }
}

//[System.Serializable] 
//public class PointCloudXYZ : PointCloud
//{
//    public string fileExtension { get; } = "xyz";

//    public override void SaveMesh()
//    {

//    }

//    public override void ConvertToObj()
//    {

//    }

//}

//[System.Serializable]
//public class PointCloudXYZRGB : PointCloud
//{
//    public string fileExtension { get; } = "xyzrgb";

//    public override void SaveMesh()
//    {

//    }

//    public override void ConvertToObj()
//    {

//    }

//}


public class PointCloudManager : MonoBehaviour
{
    /********************************/

    PointCloud pointCloud;
    [SerializeField]
    private List<PointCloud> PCLList; // Lista de Point Clouds carregadas
    //private List<PointCloud> PCLToBeInstantiated; // Lista de Point Clouds carregadas
    private Queue<PointCloud> PCLToBeInstantiated;

    [SerializeField]
    public Dictionary<string, PointCloud> pointCloudsDict = new Dictionary<string, PointCloud>(); // Dicionario que armazena todas as point clouds criadas e associa com o nome

    private Thread t = null;
    private List<Thread> tList; // thread que efectua a leitura do ficheiro em paralelo

    private Material material;
    public Shader shader;
    private Texture sprite;
    private MeshRenderer meshRenderer;

    private Vector3[] points;
    private Color32[] colors32;
    private string[] stringBuffer;
    //private bool loaded = false; // thread carregou os pontos da PCL (Nota: Bloquear recurso enquanto se instancia a mesh)

    [HideInInspector]
    public Mesh PCL_mesh;

    int[] indices;

    //[Range(1.0f, 10.0f)]
    private float scale = 1.0f;

    //[Range(0.0f, 10.0f)]
    private float pointSize;


    //GUI
    private bool drawGUI = true;
    private float progress = 0;
    private string guiText;

    //[SerializeField]
    //public enum MeshTopologyEnum 
    //{
    //    Points, Lines, LineStrip, Quads
    //};
    //public MeshTopologyEnum meshTopologyEnum = new MeshTopologyEnum();
    //public MeshTopology meshTopology;


    public void OpenFileExplorer() // Abertura do ficheiro da pcl
    {
        string[] extensionFilters = { "xyz files", "xyz, xyzrgb, xyzrgba", "txt files", "txt" , "All Files", "*" };
        string path = EditorUtility.OpenFilePanelWithFilters("Select Point Cloud file", "", extensionFilters);
        Debug.Log(path);
        string fileExtension = Path.GetExtension(path);
        Debug.Log(fileExtension);
        
        if (path != " ")
        {

            switch (fileExtension)
            {
                case ".xyz": // carrega uma pointcloud xyz sem cor. Embora nao tenha cor, esta pode ser atribuida considerando o Z do ponto
                    //pointCloudXYZ = new PointCloud();

                    //task = new Task(ReadFileXYZ(path)); // Equivalente a StartCoroutine
                    Debug.Log("xyz file will be loaded");
                    break;
                case ".xyzrgb": // carrega uma pointcloud xyzrgb
                    //pointCloudXYZRGB = new PointCloud();

                    //task = new Task(ReadFileXYZRGB(path)); // Equivalente a StartCoroutine
                    Debug.Log("xyzrgb file will be loaded");
                    break;
                case ".txt": // caso generalista em que a point cloud tanto pode ser xyz ou xyzrgb sendo que e feita essa avaliacao
                    //string tmp_string;
                    //try
                    //{
                    //    if (t.IsAlive)
                    //    {
                    //        Debug.Log("Still loading a PCL please wait");
                    //        break;
                    //    }
                    //}
                    //catch {  }

                    GameObject ParentMeshObj = new GameObject(Path.GetFileNameWithoutExtension(path)); // Objecto de jogo parente das submeshes
                    ParentMeshObj.transform.SetParent(this.transform); // torna o objecto de jogo parente das submeshes filho do objecto de jogo PCL manager
                    pointCloud = ParentMeshObj.AddComponent<PointCloud>(); // Adiciona o MonoBehaviour PointCloud ao objecto de jogo parente das submeshes
                    PCLList.Add(pointCloud);

                    string[] tmp_strings_buffer;
                    using (StreamReader sr = new StreamReader(path))
                    {
                        char[] delimiterChars = { ' ', ',' };
                        tmp_strings_buffer = sr.ReadLine().Split(delimiterChars);
                        Debug.Log(tmp_strings_buffer.Length);
                        if(tmp_strings_buffer.Length == 3) // xyz
                        {
                            //pointCloud = new PointCloud();


                            //task = new Task(ReadFileXYZ(path)); // Equivalente a StartCoroutine
                        
                            Debug.Log("xyz PCL format detected");
                        }
                        else if(tmp_strings_buffer.Length == 6) // xyz rgb
                        {

                            //task = new Task(ReadFileXYZRGB(path)); // Equivalente a StartCoroutine
                            //pointCloud = new PointCloud();
                            Debug.Log("Starting Thread");
                            t = new Thread(() => ReadFile_Thread(path, PCLList[PCLList.Count - 1]));
                            t.Start();
                            tList.Add(t);

                            // Inicia uma coroutina que verifica se a point cloud ja foi carregada
                            StartCoroutine(WaitForThreadtoCompleteAndInstantiate(ParentMeshObj));

                            //Debug.Log("xyzrgb PCL format detected");
                        }
                    }
                    break;
                default:
                    Debug.Log("Unsupported file format");
                    break;
            }
        }
        else
        {
            Debug.Log("Error opening file");
        }
    }

    private void ReadFile_Thread(string path, PointCloud pointCloud) // Thread de leitura do ficheiro e preenchimento da class PointCloud e Submeshes
    {
        lock (pointCloud) 
        {
            string filename;
            filename = Path.GetFileNameWithoutExtension(path);
            //PointCloud pointCloud = new PointCloud(); // Instancia um objecto de point cloud
            PointCloud.Submesh submesh = new PointCloud.Submesh(); //submesh inicial da PCL
            pointCloud.SubmeshList.Add(submesh);

            Debug.Log(path);
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            int lineCount = File.ReadAllLines(path).Length;

            //List<Vector3> subMeshpoints = new List<Vector3>();
            //subMeshpoints.Capacity = System.UInt16.MaxValue; // Reserva espaço na lista
            //List<Color32> subMeshcolors = new List<Color32>();
            //subMeshcolors.Capacity = System.UInt16.MaxValue; // Reserva espaço na lista
            //int SubmeshVertexcounter = 0; // Conta o numero de vertices da submesh para separar a pointcloud em meshes de 65536 vertices max
            //int Submeshcounter = 0; // conta o numero de submeshes da pointcloud

            string buffer;
            //points = new Vector3[lineCount];
            stringBuffer = new string[lineCount];
            //colors32 = new Color32[lineCount];

            char[] delimiterChars = { ' ', ',' };

            float tmp_float;
            List<float> tmpfloats = new List<float>(); // variavel temporaria que armazena os floats de cada linha

            using (StreamReader sr = new StreamReader(path))
            {

                foreach (string line in File.ReadAllLines(path))
                {
                    buffer = sr.ReadLine();
                    if (buffer.StartsWith("#"))
                    {
                        continue;
                    }
                    else
                    {
                        stringBuffer = buffer.Split(delimiterChars); //sr.ReadLine().Split();
                        foreach (string item in stringBuffer)
                        {
                            try
                            {
                                tmp_float = float.Parse(item, NumberStyles.Any, ci) * scale;
                                tmpfloats.Add(tmp_float);
                            }
                            catch { continue; }
                            //Debug.Log(tmp_float);
                        }
                        //points[i] = PclRos2PclUnity(tmpfloats[0], tmpfloats[1], tmpfloats[2]);//new Vector3(tmpfloats[0], tmpfloats[1], tmpfloats[2]);
                        //colors32[i] = new Color32((byte)tmpfloats[3], (byte)tmpfloats[4], (byte)tmpfloats[5], 255);

                        if (tmpfloats.Count == 3) // Caso seja uma pointcloud XYZ
                        {
                            submesh.subMeshpoints.Add(PclRos2PclUnity(tmpfloats[0], tmpfloats[1], tmpfloats[2]));
                        }
                        if (tmpfloats.Count == 6) // Caso seja uma pointcloud XYZ RGB
                        {
                            submesh.subMeshpoints.Add(PclRos2PclUnity(tmpfloats[0], tmpfloats[1], tmpfloats[2]));
                            submesh.subMeshcolors32.Add(new Color32((byte)tmpfloats[3], (byte)tmpfloats[4], (byte)tmpfloats[5], 255));
                        }
                        tmpfloats.Clear();

                        pointCloud.npoints++; // conta o numero de pontos da point cloud inteira
                        submesh.submesh_nPoints++;

                        if (submesh.submesh_nPoints == System.UInt16.MaxValue) // quando o numero limite de vertices e atingido, reset da contagem e armazenamento
                        {
                            //InstantiateSubMesh(subMeshpoints, subMeshcolors); // instanciar a submesh // nao possivel fazer no thread
                            pointCloud.SubmeshList.Add(submesh); // adiciona a submesh a pointcloud
                            submesh = new PointCloud.Submesh(); // cria uma nova submesh
                        }
                    }
                }
            }
            PCLToBeInstantiated.Enqueue(pointCloud);
            pointCloudsDict.Add(filename, pointCloud);
            //Debug.Log("Thread Closing");
       }
    }

    IEnumerator WaitForThreadtoCompleteAndInstantiate(GameObject ParentPCL) 
    {
        //while (t.IsAlive) 
        //{
        //    //Debug.Log("Thread is alive");
        //    yield return null;
        //}
        while (true) 
        {
            if (PCLToBeInstantiated.Count > 0) 
            {
                foreach (var submesh in PCLToBeInstantiated.Dequeue().SubmeshList)
                {
                    Debug.Log("Instantiating PCL");
                    InstantiateSubMesh(ParentPCL, submesh.subMeshpoints, submesh.subMeshcolors32);
                }
                //PCLToBeInstantiated.RemoveAt(PCLToBeInstantiated.Count - 1);
                break;
            }
            yield return null;
        }
        Debug.Log("Coroutine Ended");
    }

    //IEnumerator ReadFileXYZ(string path) // Leitura de PointClouds sem cor
    //{
    //    string filename;
    //    filename = Path.GetFileNameWithoutExtension(path);
    //    pointCloudXYZ.npoints = 0;
    //    Debug.Log(path);
    //    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
    //    ci.NumberFormat.CurrencyDecimalSeparator = ".";

    //    int lineCount = File.ReadAllLines(path).Length;
    //    string buffer;
    //    points = new Vector3[lineCount];
    //    stringBuffer = new string[lineCount];
    //    colors32 = new Color32[lineCount];


    //    List<Vector3> subMeshpoints = new List<Vector3>();
    //    subMeshpoints.Capacity = System.UInt16.MaxValue; // Reserva espaço na lista
    //    //List<Color32> subMeshcolors = new List<Color32>();
    //    //subMeshcolors.Capacity = System.UInt16.MaxValue; // Reserva espaço na lista
    //    int SubmeshVertexcounter = 0; // Conta o numero de vertices da submesh para separar a pointcloud em meshes de 65536 vertices max
    //    int Submeshcounter = 0; // conta o numero de submeshes da pointcloud

    //    char[] delimiterChars = { ' ', ',' };

    //    float tmp_float;
    //    List<float> tmpfloats = new List<float>();

    //    using (StreamReader sr = new StreamReader(path))
    //    {
    //        for (int i = 0; i < lineCount; ++i)
    //        {
    //            buffer = sr.ReadLine();
    //            if (buffer.StartsWith("#"))
    //            {
    //                continue;
    //            }
    //            else
    //            {
    //                stringBuffer = buffer.Split(delimiterChars); //sr.ReadLine().Split();
    //                foreach (string item in stringBuffer)
    //                {
    //                    try
    //                    {
    //                        tmp_float = float.Parse(item, NumberStyles.Any, ci) * scale;
    //                        tmpfloats.Add(tmp_float);
    //                    }
    //                    catch { continue; }
    //                    //Debug.Log(tmp_float);
    //                }
    //                //points[i] = PclRos2PclUnity(tmpfloats[0], tmpfloats[1], tmpfloats[2]);// new Vector3(tmpfloats[0], tmpfloats[1], tmpfloats[2]);
    //                subMeshpoints.Add(PclRos2PclUnity(tmpfloats[0], tmpfloats[1], tmpfloats[2]));
    //                //subMeshcolors.Add(new Color32((byte)tmpfloats[3], (byte)tmpfloats[4], (byte)tmpfloats[5], 255)); 

    //                tmpfloats.Clear(); 

    //                pointCloudXYZ.npoints++; // conta o numero de pontos da point cloud inteira
    //                SubmeshVertexcounter++;

    //                if(SubmeshVertexcounter == System.UInt16.MaxValue) // quando o numero limite de vertices e atingido, reset da contagem e armazenamento
    //                {
    //                    SubmeshVertexcounter = 0; // reset do contador de vertices da submesh
    //                    InstantiateSubMesh(subMeshpoints); // instanciar a submesh
    //                    subMeshpoints.Clear();
    //                    Submeshcounter++;
    //                } 
    //            }
    //        }
    //        Debug.Log(pointCloudXYZ.npoints);
    //        yield return null;
    //        //Debug.Log(points.Length);
    //    }
    //    loaded = true;
    //    //InstantiateSubMesh(points);
    //    pointCloudsDict.Add(filename, pointCloudXYZ);
    //    yield return null;
    //}

    //IEnumerator ReadFileXYZRGB(string path) // Leitura de Pointclouds com cor
    //{
    //    string filename;
    //    filename = Path.GetFileNameWithoutExtension(path);
    //    pointCloudXYZRGB.npoints = 0;
    //    Debug.Log(path);
    //    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
    //    ci.NumberFormat.CurrencyDecimalSeparator = ".";

    //    int lineCount = File.ReadAllLines(path).Length;


    //    List<Vector3> subMeshpoints = new List<Vector3>();
    //    subMeshpoints.Capacity = System.UInt16.MaxValue; // Reserva espaço na lista
    //    List<Color32> subMeshcolors = new List<Color32>();
    //    subMeshcolors.Capacity = System.UInt16.MaxValue; // Reserva espaço na lista
    //    int SubmeshVertexcounter = 0; // Conta o numero de vertices da submesh para separar a pointcloud em meshes de 65536 vertices max
    //    int Submeshcounter = 0; // conta o numero de submeshes da pointcloud

    //    string buffer;
    //    points = new Vector3[lineCount];
    //    stringBuffer = new string[lineCount];
    //    colors32 = new Color32[lineCount];

    //    char[] delimiterChars = { ' ', ',' };

    //    float tmp_float;
    //    List<float> tmpfloats = new List<float>(); // variavel temporaria que armazena os floats de cada linha

    //    using (StreamReader sr = new StreamReader(path))
    //    {
    //        for (int i = 0; i < lineCount; ++i)
    //        {
    //            buffer = sr.ReadLine();
    //            if (buffer.StartsWith("#"))
    //            {
    //                continue;
    //            }
    //            else
    //            {
    //                stringBuffer = buffer.Split(delimiterChars); //sr.ReadLine().Split();
    //                foreach (string item in stringBuffer)
    //                {
    //                    //try
    //                    //{
    //                        tmp_float = float.Parse(item, NumberStyles.Any, ci) * scale;
    //                        tmpfloats.Add(tmp_float);
    //                    //}
    //                    //catch { continue; }
    //                    //Debug.Log(tmp_float);
    //                }
    //                //points[i] = PclRos2PclUnity(tmpfloats[0], tmpfloats[1], tmpfloats[2]);//new Vector3(tmpfloats[0], tmpfloats[1], tmpfloats[2]);
    //                //colors32[i] = new Color32((byte)tmpfloats[3], (byte)tmpfloats[4], (byte)tmpfloats[5], 255);
    //                subMeshpoints.Add(PclRos2PclUnity(tmpfloats[0], tmpfloats[1], tmpfloats[2]));
    //                subMeshcolors.Add(new Color32((byte)tmpfloats[3], (byte)tmpfloats[4], (byte)tmpfloats[5], 255)); 

    //                tmpfloats.Clear(); 

    //                pointCloudXYZRGB.npoints++; // conta o numero de pontos da point cloud inteira
    //                SubmeshVertexcounter++;

    //                if(SubmeshVertexcounter == System.UInt16.MaxValue) // quando o numero limite de vertices e atingido, reset da contagem e armazenamento
    //                {
    //                    SubmeshVertexcounter = 0; // reset do contador de vertices da submesh
    //                    InstantiateSubMesh(subMeshpoints, subMeshcolors); // instanciar a submesh
    //                    subMeshpoints.Clear();
    //                    subMeshcolors.Clear();
    //                    Submeshcounter++;
    //                }
    //                //yield return null;
    //            }
    //        }
    //        Debug.Log("Numero de submeshes" + Submeshcounter);
    //        Debug.Log(pointCloudXYZRGB.npoints);

    //        //Debug.Log(points.Length);
    //    }
    //    loaded = true;
    //    //InstantiateSubMesh(points, colors32);
    //    pointCloudsDict.Add(filename, pointCloudXYZRGB);
    //    yield return null;
    //}




    public void InstantiateSubMesh(GameObject Parent, List<Vector3> vertices, List<Color32> colors32 = null) // Colors é um argumento opcional
    {
        Debug.Log("Instantiating Submesh");
        PCL_mesh = new Mesh();
        GameObject meshObj = new GameObject("Submesh");
        meshObj.transform.position = Parent.transform.position;
        meshObj.transform.SetParent(Parent.transform); // fica child do ultimo child 
        meshObj.AddComponent<MeshFilter>();
        meshObj.AddComponent<MeshRenderer>();
        
        material = new Material(shader);
        
        meshObj.GetComponent<MeshFilter>().mesh = PCL_mesh;
        meshRenderer = meshObj.GetComponent<MeshRenderer>();
        meshRenderer.material = material;
        //material.SetTexture("_MainTex", sprite);


        PCL_mesh.vertices = vertices.ToArray();
        material.SetFloat("_Size", pointSize);

        indices = new int[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
            indices[i] = i;

        PCL_mesh.SetIndices(indices, MeshTopology.Points, 0, false);

        // Se cada ponto possuir cor atribui
        if(colors32 != null)
        {
            PCL_mesh.colors32 = colors32.ToArray();
        }
        
        Debug.Log("Submesh Added");
        //loaded = false;
        //meshesList.Add(PCL_mesh);
    }

    public void ChangeMeshColors(Color32[] colors32)
    {

    }

    //public void ChangeMeshTopology(Mesh mesh) 
    //{
    //    switch (meshTopologyEnum)
    //    {
    //        case MeshTopologyEnum.Points:
    //            PCL_mesh.SetIndices(indices, MeshTopology.Points, 0, false);
    //            break;
    //        case MeshTopologyEnum.Lines:
    //            PCL_mesh.SetIndices(indices, MeshTopology.Lines, 0, false);
    //            break;
    //        case MeshTopologyEnum.LineStrip:
    //            PCL_mesh.SetIndices(indices, MeshTopology.LineStrip, 0, false);
    //            break;
    //        case MeshTopologyEnum.Quads:
    //            PCL_mesh.SetIndices(indices, MeshTopology.Quads, 0, false);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    // Converte os eixos de coordenadas de ROS para Unity
    Vector3 PclRos2PclUnity(Vector3 vector3)
    {
        return new Vector3(-vector3.y, vector3.z, vector3.x);
    }

    Vector3 PclRos2PclUnity(float x, float y, float z) 
    {
        return new Vector3(-y, z, x);
    }

    //void OnGUI()
    //{
    //    if (drawGUI == true)
    //    {
    //        if (!loaded)
    //        {     
    //            GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2, 400.0f, 20));
    //            GUI.Box(new Rect(0, 0, 200.0f, 20.0f), guiText);
    //            GUI.Box(new Rect(0, 0, progress * 200.0f, 20), "");
    //            GUI.EndGroup();
    //        }
    //    }
    //}

    void OnEnable() // Corre quando o gameObject e instanciado e activado
    {
        PCLList = new List<PointCloud>();
        //PCLToBeInstantiated = new List<PointCloud>();
        PCLToBeInstantiated = new Queue<PointCloud>();
        tList = new List<Thread>();
    }
}



////////////// BAK


    // public void InstantiateSubMesh(Vector3[] vertices, Color32[] colors32 = null) // Colors é um argumento opcional
    // {
    //     PCL_mesh = new Mesh();
    //     GameObject meshObj = new GameObject("PointCloudSubmesh");
    //     meshObj.transform.position = gameObject.transform.GetChild(transform.childCount - 1).position;
    //     meshObj.transform.SetParent(gameObject.transform.GetChild(transform.childCount - 1).transform); // fica child do ultimo child 
    //     meshObj.AddComponent<MeshFilter>();
    //     meshObj.AddComponent<MeshRenderer>();
        
    //     material = new Material(shader);
        
    //     meshObj.GetComponent<MeshFilter>().mesh = PCL_mesh;
    //     meshRenderer = meshObj.GetComponent<MeshRenderer>();
    //     meshRenderer.material = material;
    //     //material.SetTexture("_MainTex", sprite);


    //     PCL_mesh.vertices = points;
    //     material.SetFloat("_Size", pointSize);

    //     indices = new int[points.Length];
    //     for (int i = 0; i < points.Length; i++)
    //         indices[i] = i;

    //     PCL_mesh.SetIndices(indices, MeshTopology.Points, 0, false);

    //     // Se cada ponto possuir cor atribui
    //     if(colors32 != null)
    //     {
    //         PCL_mesh.colors32 = colors32;
    //     }
        
    //     Debug.Log("PCL Instantiated");
    //     //meshesList.Add(PCL_mesh);
    // }