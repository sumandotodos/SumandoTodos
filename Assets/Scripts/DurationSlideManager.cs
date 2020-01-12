using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationSlideManager : SliderManager
{
    public Text SliderText;

    public string[] SliderTextContents;

    public override void SetValue(int value)
    {
        SliderText.text = SliderTextContents[value];
    }
}
