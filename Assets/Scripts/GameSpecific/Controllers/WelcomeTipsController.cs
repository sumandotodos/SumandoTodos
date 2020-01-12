using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeTipsController : FGProgram {

    public UITip[] WelcomeTips;

	// Use this for initialization
	void Start () {

        execute(this, "doWelcomeTip1");
        delay(0.15f);
        waitForCondition(WelcomeTips[0], "hasBeenDispatched");
        execute(this, "doWelcomeTip2");
        delay(0.15f);
        waitForCondition(WelcomeTips[1], "hasBeenDispatched");
        execute(this, "doWelcomeTip3");
        delay(0.15f);
        waitForCondition(WelcomeTips[2], "hasBeenDispatched");
        execute(this, "doWelcomeTip4");
        delay(0.15f);
        waitForCondition(WelcomeTips[3], "hasBeenDispatched");

        programNotifyFinish();

    }
		
    public void doWelcomeTip1()
    {
        WelcomeTips[0].go();
    }

    public void doWelcomeTip2()
    {
        WelcomeTips[1].go();
    }

    public void doWelcomeTip3()
    {
        WelcomeTips[2].go();
    }

    public void doWelcomeTip4()
    {
        WelcomeTips[3].go();
    }

}
