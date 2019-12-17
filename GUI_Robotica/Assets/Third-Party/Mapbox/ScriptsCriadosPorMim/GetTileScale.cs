using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Map;


public class GetTileScale : MonoBehaviour
{
    // Start is called before the first frame update
    private AbstractMap map;

    void Start()
    {
        map = gameObject.GetComponent<AbstractMap>();
        Debug.Log(Conversions.GetTileScaleInMeters((float)map.CenterLatitudeLongitude.x, (int)map.Zoom));
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
