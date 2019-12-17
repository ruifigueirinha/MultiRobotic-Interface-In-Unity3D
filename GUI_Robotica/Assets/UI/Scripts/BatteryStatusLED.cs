using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryIndicator : MonoBehaviour {


    public GameObject Battery;
    public int criticalValue;
    private Slider slider;
    private Image glow;
    private Image batt_indicator;
    private TextMeshPro batt_text;

	// Use this for initialization
	void Start () {
        slider = Battery.GetComponent<Slider>();
        glow = gameObject.transform.GetChild(1).GetComponent<Image>();
        batt_indicator = gameObject.GetComponent<Image>();
        //batt_text = gameObject.transform.GetChild(0).GetComponent<TextMeshPro>();
    }

    //// Update is called once per frame
    void Update()
    {

        if (slider.value < criticalValue)
            StartCoroutine(Blink());
        else
        {
           // Color c_text = batt_text.color;
            Color c = glow.color;
            Color batt_c = batt_indicator.color;
            c.a = 0.0f;
            glow.color = c;
            batt_c.a = 0.3f;
            batt_indicator.color = batt_c;
        }


    }

    IEnumerator Blink()
    {
        //slider.value < criticalValue
        while (true)
        {
            if (!(slider.value < criticalValue))
                break;

            Color c = glow.color;
            Color batt_c = batt_indicator.color;
            for (float f = 0.0f; f <= 1.0f; f += 0.02f)
            {
                c.a = f;
                batt_c.a += 0.02f;
                glow.color = c;
                batt_indicator.color = batt_c;
                yield return new WaitForSeconds(0.01f);
            }
            for (float f = 1.0f; f >= 0.0f; f -= 0.02f)
            {
                c.a = f;
                batt_c.a -= 0.02f;
                glow.color = c;
                batt_indicator.color = batt_c;
                yield return new WaitForSeconds(0.01f);
            }
        }  
    }
}
