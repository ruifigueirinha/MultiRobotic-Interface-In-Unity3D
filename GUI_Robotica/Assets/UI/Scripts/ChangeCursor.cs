using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    //void Start()
    //{
    //    cursorTexture.Resize(32, 32);
    //}
   
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        //Debug.Log("Cursor Entering " + name + " GameObject");
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        //Debug.Log("Cursor Exiting " + name + " GameObject");
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

}