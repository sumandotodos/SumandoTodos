using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleTipsController : FGProgram
{

    public UITip[] BottleTips;

    // Use this for initialization
    void Start()
    {

        execute(this, "doBottleTip");
        delay(0.15f);
        waitForCondition(BottleTips[0], "hasBeenDispatched");

        programNotifyFinish();

    }

    public void doBottleTip()
    {
        BottleTips[0].go();
    }




}