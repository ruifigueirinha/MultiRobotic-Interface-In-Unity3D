using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Minimap_script : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse enter");
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse exit");
        isOver = false;
    }
}