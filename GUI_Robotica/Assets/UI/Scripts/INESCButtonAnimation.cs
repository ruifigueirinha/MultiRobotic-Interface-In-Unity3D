using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class INESCButtonAnimation : MonoBehaviour, IPointerClickHandler
{

    public RectTransform rectTransform; // rect transform de MainButtons
    public float minSize = 0.0f;
    public float maxSize = 136.0f;
    private bool open = false;

    void Awake()
    {
       //rectTransform = gameObject.transform.parent.GetChild(1).GetComponent<RectTransform>();
       minSize = rectTransform.rect.height;
       //Debug.Log(minSize);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Button Clicked");
        StopAllCoroutines();
        if(open)
            StartCoroutine(DecreaseSize());
        else
            StartCoroutine(IncreaseSize());
    }

    IEnumerator IncreaseSize()
    {
        open = true;
        while(rectTransform.sizeDelta.y < maxSize)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x , rectTransform.sizeDelta.y + 15.0f);
            yield return new WaitForSeconds(0.000000001f);
        }
    }

    IEnumerator DecreaseSize()
    {
        open = false;
        while (rectTransform.sizeDelta.y > minSize)
        {
            if (rectTransform.sizeDelta.y - 15.0f < minSize)
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, minSize);
            else
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - 15.0f);
                    
            }
        yield return new WaitForSeconds(0.000000001f);
        }  
    }
}
