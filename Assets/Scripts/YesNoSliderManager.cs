using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesNoSliderManager : SliderManager
{
    public Text No;
    public Color NoColor = Color.red;
    public Text Yes;
    public Color YesColor = Color.green;

    public override void SetValue(int value)
    {
        switch(value)
        {
            case 0:
                Yes.color = YesColor * new Color(0.25f, 0.25f, 0.25f, 1.0f);
                No.color = NoColor;
                Yes.GetComponentInChildren<UITextFader>().opaqueColor = Yes.color;
                No.GetComponentInChildren<UITextFader>().opaqueColor = No.color;
                break;

            case 1:
                Yes.color = YesColor;
                No.color = NoColor * new Color(0.25f, 0.25f, 0.25f, 1.0f);
                Yes.GetComponentInChildren<UITextFader>().opaqueColor = Yes.color;
                No.GetComponentInChildren<UITextFader>().opaqueColor = No.color;
                break;
        }
    }
}
