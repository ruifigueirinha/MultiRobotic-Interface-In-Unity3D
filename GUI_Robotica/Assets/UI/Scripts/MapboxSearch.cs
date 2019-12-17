using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;

public class MapboxSearch : MonoBehaviour
{
private GameObject mapbox;
private AbstractMap abstractMap;
private string latlon;

private bool isMapinitialized = false;
    
    private void Awake() {
        mapbox = GameObject.FindGameObjectWithTag("Mapbox");
    }
    public void SetLatLon(string ll){
        latlon = ll;
    }
    public void GetMap(){
        abstractMap = mapbox.GetComponent<AbstractMap>();
        try{
            if(abstractMap.enabled)
                abstractMap.UpdateMap(Conversions.StringToLatLon(latlon)); 
        }
        catch{
            
        }
    }
}
