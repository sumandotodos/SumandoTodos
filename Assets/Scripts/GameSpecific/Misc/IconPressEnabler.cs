using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconPressEnabler : MonoBehaviour {

    public UIButtonPress[] Buttons;

    public void EnableAllButtons()
    {
        foreach (UIButtonPress b in Buttons) {
            b.setEnabled(true);
        }
    }

    public void DisabeAllButtons()
    {
        foreach (UIButtonPress b in Buttons)
        {
            b.setEnabled(false);
        }
    }
}
