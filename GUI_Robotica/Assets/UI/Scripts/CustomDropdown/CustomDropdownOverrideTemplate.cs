using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CustomDropdownOverrideTemplate : CustomDropdown
{
    public override void ValueEvaluate(int index)
    {
        Debug.Log("Overriden ValueEvaluate");
    }
}
