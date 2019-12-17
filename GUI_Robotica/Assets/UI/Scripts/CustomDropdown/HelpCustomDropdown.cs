using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCustomDropdown : CustomDropdown
{
    public override void ValueEvaluate(int index)
    {
        switch (index)
        {
            case 0:
                OpenDocumentation();
                break;
            case 1:
                OpenAboutInfo();
                break;
            default:
                break;
        }
    }

    private void OpenDocumentation()
    {
        print("OpenDocumentation");
    }

    private void OpenAboutInfo()
    {
        print("OpenAboutInfo");
    }
}
