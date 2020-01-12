using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagineTipsController : FGProgram
{

    public UITip[] ImagineTips;

    // Use this for initialization
    void Start()
    {

        execute(this, "doImagineTip");
        delay(0.15f);
        waitForCondition(ImagineTips[0], "hasBeenDispatched");

        programNotifyFinish();

    }

    public void doImagineTip()
    {
        ImagineTips[0].go();
    }

}
