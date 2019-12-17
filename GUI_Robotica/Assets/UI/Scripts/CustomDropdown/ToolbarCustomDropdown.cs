using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// Colocar este script num objecto que sirva de toolbar parente de CustomDropdowns

public class ToolbarCustomDropdown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isHovered;
    private bool _isDropdownOpen;

    public bool isDropdownOpen {get {return _isDropdownOpen;} set{_isDropdownOpen = value;}}
    public bool isHovered {get {return _isHovered;} set{_isHovered = value;}}
    public void OnPointerEnter(PointerEventData eventData)
    {
        //print("Hovered");
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //print("N hovered");
        _isHovered = false;
        _isDropdownOpen = false;
    }

    public void AnyDropdownOpen()
    {
        _isDropdownOpen = true;
    }
}
