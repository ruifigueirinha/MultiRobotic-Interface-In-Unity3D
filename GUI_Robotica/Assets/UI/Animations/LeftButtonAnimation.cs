using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeftButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public RectTransform rectTransform;
    public float minSize = 80.0f;
    public float maxSize = 200.0f;

    void Awake()
    {
       rectTransform = gameObject.GetComponent<RectTransform>();
       minSize = rectTransform.rect.width;
       //Debug.Log(minSize);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse is over GameObject.");
        StopAllCoroutines();
        StartCoroutine(IncreaseSize());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse is NOT over GameObject.");
        StopAllCoroutines();
        StartCoroutine(DecreaseSize());
    }

    IEnumerator IncreaseSize()
    {

        while(rectTransform.sizeDelta.x < maxSize)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x + 15.0f, rectTransform.sizeDelta.y);
            yield return new WaitForSeconds(0.000000001f);
        }

    }

    IEnumerator DecreaseSize()
    {

            while (rectTransform.sizeDelta.x > minSize)
            {
                if (rectTransform.sizeDelta.x - 15.0f < minSize)
                    rectTransform.sizeDelta = new Vector2(minSize, rectTransform.sizeDelta.y);
                else
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x - 15.0f, rectTransform.sizeDelta.y);
                    
                }
            yield return new WaitForSeconds(0.000000001f);
            }
    }
}
