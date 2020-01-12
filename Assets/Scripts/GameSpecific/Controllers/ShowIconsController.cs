using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowIconsController : FGProgram {

    public UIScaleFader[] scalers;
    int deployedScalers = 0;

    public EaseType inEaseType;
    public EaseType outEaseType;

    public float InterIconDelay = 0.25f;

    public string ID = "Icons";

	// Use this for initialization
	void Start () {



        createSubprogram("Show");

        execute(this, "prepareScalersForInEffect");
        startWhile(this, "allScalersNotDeployed");
            execute(this, "deployScaler");
            delay(InterIconDelay);
        endWhile();

        programNotifyFinish();





        createSubprogram("Hide");

        execute(this, "prepareScalersForOutEffect");
        startWhile(this, "allScalersNotHidden");
            execute(this, "hideScaler");
            delay(InterIconDelay);
        endWhile();

        programNotifyFinish();



    }

    public void prepareScalersForInEffect()
    {
        foreach(UIScaleFader f in scalers)
        {
            f.setEaseType(inEaseType);
        }
    }

    public void prepareScalersForOutEffect()
    {
        foreach(UIScaleFader f in scalers)
        {
            f.setEaseType(outEaseType);
        }
    }

    public void deployScaler()
    {
        scalers[deployedScalers++].scaleIn();
    }

    public void hideScaler()
    {
        scalers[--deployedScalers].scaleOut();
    }

    public bool allScalersNotDeployed()
    {
        return deployedScalers != scalers.Length;
    }

    public bool allScalersNotHidden()
    {
        return deployedScalers != 0;
    }

}
