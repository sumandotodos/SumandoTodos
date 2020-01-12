using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTitleController : FGProgram {

    public UIScaleFader GlassesScaler;
    public UIScaleFader TitleScaler;
    public WelcomeTipsController welcomeTipsController;
    public Text VersionLabel;
    public UITextFader VersionFader;
    public UIFader logoFader;
    public UIScaleFader logoScaler;

    bool skip = false;

	// Use this for initialization
	void Start () {

        execute(this, "initialize");
        //execute(TitleScaler, "scaleIn");
        //delay(0.25f);
        //execute(GlassesScaler, "scaleIn");
        execute(logoFader, "fadeToOpaque");
        execute(logoScaler, "scaleIn");
        execute(VersionFader, "fadeToOpaque");
        delay(1.0f, this, "checkSkip");
        waitForProgram(welcomeTipsController);
        delay(3.0f, this, "checkSkip");
        execute(logoFader, "fadeToTransparent");
        execute(VersionFader, "fadeToTransparent");
        execute(this, "dismiss");

        programNotifyFinish();
   
    }

    public void initialize()
    {
        VersionLabel.text = Config.Version;
    }

    public void checkSkip()
    {
        if(Input.GetMouseButtonDown(0))
        {
            skip = true;
            cancelDelay();
        }
        if(skip)
        {
            cancelDelay();
        }
    }

    public void dismiss()
    {
        GlassesScaler.setEaseType(EaseType.cubicIn);
        TitleScaler.setEaseType(EaseType.cubicIn);
        GlassesScaler.scaleOut();
        TitleScaler.scaleOut();
    }


}
