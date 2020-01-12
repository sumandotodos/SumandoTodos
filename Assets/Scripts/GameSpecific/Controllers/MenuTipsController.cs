using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTipsController : FGProgram
{

    public UITip[] MenuTips;

    // Use this for initialization
    void Start()
    {

        execute(this, "doMenuTip1");
        delay(0.15f);
        waitForCondition(MenuTips[0], "hasBeenDispatched");

        execute(this, "doMenuTip2");
        delay(0.15f);
        waitForCondition(MenuTips[1], "hasBeenDispatched");
       
        programNotifyFinish();

    }

    public void doMenuTip1()
    {
        MenuTips[0].go();
    }

    public void doMenuTip2()
    {
        MenuTips[1].go();
    }




}