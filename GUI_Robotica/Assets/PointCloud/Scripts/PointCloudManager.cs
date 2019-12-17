using System.Collections;
//using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEditor;

// Classes PointCloud
[System.Serializable]
public abstract class PointCloud
{
    public long npoints;
    protected Vector3[] m_points;

    private Vector3 PclRos2PclUnity(Vector3 vector3)
    {
        return new Vector3(-vector3.y, -vector3.z, vector3.x);
    }
    public abstract void SaveMesh();
    public abstract void ConvertToObj();
}

[System.Serializable] 
public class PointCloudXYZ : PointCloud
{
    public string fileExtension { get; } = "xyz";

    public override void SaveMesh()
    {

    }

    public override void ConvertToObj()
    {

    }

}

[System.Serializable]
public class PointCloudXYZRGB : PointCloud
{
    public string fileExtension { get; } = "xyzrgb";

    public override void SaveMesh()
    {

    }

    public override void ConvertToObj()
    {

    }

}


public class PointCloudManager : MonoBehaviour
{
    /********************************/

    PointCloudXYZ pointCloudXYZ;
    PointCloudXYZRGB pointCloudXYZRGB;

    [SerializeField]
    public List<PointCloud> pointCloudsList = new List<PointCloud>(); // Lista que armazena todas as point clouds criadas
    Task task;
    

    private Material material;
    public Shader shader;
    public Texture sprite;
    private MeshRenderer meshRenderer;

    private Vector3[] points;
    private Color[] colors;
    private string[] stringBuffer;
    private bool loaded = false;


    [HideInInspector]
    public Mesh PCL_mesh;

    int[] indices;

    [Range(1.0f, 10.0f)]
    public float scale = 1.0f;

    [Range(0.0f, 10.0f)]
    public float pointSize;


    //GUI
    private bool drawGUI = true;
    private float progress = 0;
    private string guiText;

    [SerializeField]
    public enum MeshTopologyEnum 
    {
        Points, Lines, LineStrip, Quads
    };
    public MeshTopologyEnum meshTopologyEnum = new MeshTopologyEnum();
    public MeshTopology meshTopology;

    public void OpenFileExplorer() // Abertura do ficheiro da pcl
    {
        string[] extensionFilters = { "xyz files", "xyz, xyzrgb, xyzrgba", "txt files", "txt" , "All Files", "*" };
        string path = EditorUtility.OpenFilePanelWithFilters("Select Point Cloud file", "", extensionFilters);
        Debug.Log(path);
        string fileExtension = Path.GetExtension(path);
        Debug.Log(fileExtension);
        if (path.Length != 0)
        {
            switch (fileExtension)
            {
                case ".xyz":
                    pointCloudXYZ = new PointCloudXYZ();
                    task = new Task(ReadFileXYZ(path)); // Equivalente a StartCoroutine
                    Debug.Log("xyz file will be loaded");
                    break;
                case ".xyzrgb":
                    pointCloudXYZRGB = new PointCloudXYZRGB();
                    task = new Task(ReadFileXYZRGB(path)); // Equivalente a StartCoroutine
                    Debug.Log("xyzrgb file will be loaded");
                    break;
                case ".txt":
                    string tmp_string;
                    using (StreamReader sr = new StreamReader(path))
                    {
                        tmp_string = sr.ReadLine();
                        if(tmp_string.Length == 3)
                        {
                            pointCloudXYZ = new PointCloudXYZ();
                            task = new Task(ReadFileXYZ(path)); // Equivalente a StartCoroutine
                            Debug.Log("xyz PCL format detected");
                        }
                        if(tmp_string.Length == 6)
                        {
                            pointCloudXYZRGB = new PointCloudXYZRGB();
                            task = new Task(ReadFileXYZRGB(path)); // Equivalente a StartCoroutine
                            Debug.Log("xyzrgb PCL format detected");
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

    IEnumerator ReadFileXYZ(string path) // Leitura de PointClouds sem cor
    {
        pointCloudXYZ.npoints = 0;
        Debug.Log(path);
        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        int lineCount = File.ReadAllLines(path).Length;
        string buffer;
        points = new Vector3[lineCount];
        stringBuffer = new string[lineCount];
        colors = new Color[lineCount];

        char[] delimiterChars = { ' ', ',' };

        float tmp_float;
        List<float> tmpfloats = new List<float>();

        using (StreamReader sr = new StreamReader(path))
        {
            for (int i = 0; i < lineCount; ++i)
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
                    points[i] = new Vector3(tmpfloats[0], tmpfloats[1], tmpfloats[2]);
                    tmpfloats.Clear();
                    pointCloudXYZ.npoints++;
                }
            }
            Debug.Log(pointCloudXYZ.npoints);
            yield return null;
            //Debug.Log(points.Length);
        }
        loaded = true;
        InstantiateMesh(points);
        pointCloudsList.Add(pointCloudXYZ);
        yield return null;
    }

    IEnumerator ReadFileXYZRGB(string path) // Leitura de Poinclouds com cor
    {
        pointCloudXYZRGB.npoints = 0;
        Debug.Log(path);
        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        int lineCount = File.ReadAllLines(path).Length;
        string buffer;
        points = new Vector3[lineCount];
        stringBuffer = new string[lineCount];
        colors = new Color[lineCount];

        char[] delimiterChars = { ' ', ',' };

        float tmp_float;
        List<float> tmpfloats = new List<float>();

        using (StreamReader sr = new StreamReader(path))
        {
            for (int i = 0; i < lineCount; ++i)
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
                    points[i] = new Vector3(tmpfloats[0], tmpfloats[1], tmpfloats[2]);
                    colors[i] = new Color(tmpfloats[3], tmpfloats[4], tmpfloats[5]);
                    tmpfloats.Clear();
                    pointCloudXYZ.npoints++;
                }
            }
            Debug.Log(pointCloudXYZRGB.npoints);
            yield return null;
            //Debug.Log(points.Length);
        }
        loaded = true;
        InstantiateMesh(points, colors);
        pointCloudsList.Add(pointCloudXYZRGB);
        yield return null;
    }



    public void InstantiateMesh(Vector3[] vertices, Color[] colors = null) // Colors é um argumento opcional
    {
        PCL_mesh = new Mesh();
        GameObject meshObj = new GameObject("PointCloud");
        meshObj.transform.position = gameObject.transform.position;
        meshObj.transform.SetParent(this.transform);
        meshObj.AddComponent<MeshFilter>();
        meshObj.AddComponent<MeshRenderer>();
        
        material = new Material(shader);
        
        meshObj.GetComponent<MeshFilter>().mesh = PCL_mesh;
        meshRenderer = meshObj.GetComponent<MeshRenderer>();
        meshRenderer.material = material;
        material.SetTexture("_MainTex", sprite);


        PCL_mesh.vertices = points;
        material.SetFloat("_Size", pointSize);

        indices = new int[points.Length];
        for (int i = 0; i < points.Length; i++)
            indices[i] = i;

        PCL_mesh.SetIndices(indices, MeshTopology.Points, 0, false);

        // Se cada ponto possuir cor atribui
        if(colors != null)
        {
            PCL_mesh.colors = colors;
        }
       
        
        Debug.Log("PCL Instantiated");
        //meshesList.Add(PCL_mesh);
    }

    public void ChangeMeshColors(Color[] colors)
    {

    }

    public void ChangeMeshTopology(Mesh mesh) 
    {
        switch (meshTopologyEnum)
        {
            case MeshTopologyEnum.Points:
                PCL_mesh.SetIndices(indices, MeshTopology.Points, 0, false);
                break;
            case MeshTopologyEnum.Lines:
                PCL_mesh.SetIndices(indices, MeshTopology.Lines, 0, false);
                break;
            case MeshTopologyEnum.LineStrip:
                PCL_mesh.SetIndices(indices, MeshTopology.LineStrip, 0, false);
                break;
            case MeshTopologyEnum.Quads:
                PCL_mesh.SetIndices(indices, MeshTopology.Quads, 0, false);
                break;
            default:
                break;
        }
    }
    Vector3 PclRos2PclUnity(Vector3 vector3)
    {
        return new Vector3(-vector3.y, -vector3.z, vector3.x);
    }

    //void OnGUI()
    //{
    //    if (drawGUI == true)
    //    {
    //        if (!loaded)
    //        {
    //            //SceneManager.LoadScene("water_environment");
    //            GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2, 400.0f, 20));
    //            GUI.Box(new Rect(0, 0, 200.0f, 20.0f), guiText);
    //            GUI.Box(new Rect(0, 0, progress * 200.0f, 20), "");
    //            GUI.EndGroup();
    //        }
    //    }
    //}

    void Start()
    {
        
        //pointCloudsList.Add(pointCloudXYZ);
    }
    void Update() 
    {
        //if(material != null)
        //    material.SetFloat("_Size", pointSize);
        
    }

}
