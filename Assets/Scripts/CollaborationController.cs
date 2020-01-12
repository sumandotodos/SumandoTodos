using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollaborationController : FGProgram
{
    public UITextFader explainFader;
    public UITextFader explainFader2;
    public UITextFader explainFader3;
    public UIGeneralFader buttonFader;
    public UITextFader Link1Fader;
    public UITextFader Link2Fader;
    public UITextFader Link3Fader;
    public UIFader Icon1Fader;
    public UIFader Icon2Fader;
    public UIFader Icon3Fader;
    public string mailtoAddress = "suma@sumandotodos.es";
    public string subject = "Desde%20App%20Imagine";

    public string[] OpenURL;

    // Start is called before the first frame update
    void Start()
    {

        execute(this, "initialize");
        execute(explainFader, "fadeToOpaque");
        execute(explainFader2, "fadeToOpaque");
        execute(explainFader3, "fadeToOpaque");
        delay(0.5f);
        execute(Icon1Fader, "fadeToOpaque");
        delay(0.15f);
        execute(Link1Fader, "fadeToOpaque");
        delay(0.3f);
        execute(Icon2Fader, "fadeToOpaque");
        delay(0.15f);
        execute(Link2Fader, "fadeToOpaque");
        delay(0.3f);
        execute(Icon3Fader, "fadeToOpaque");
        delay(0.15f);
        execute(Link3Fader, "fadeToOpaque");
        delay(0.3f);
        delay(0.2f);
        execute(buttonFader, "fadeToOpaque");

        createSubprogram("Return");
        execute(buttonFader, "fadeToTransparent");
        execute(explainFader, "fadeToTransparent");
        execute(explainFader2, "fadeToTransparent");
        execute(explainFader3, "fadeToTransparent");
        execute(Icon1Fader, "fadeToTransparent");
        execute(Link1Fader, "fadeToTransparent");
        execute(Icon2Fader, "fadeToTransparent");
        execute(Link2Fader, "fadeToTransparent");
        execute(Icon3Fader, "fadeToTransparent");
        execute(Link3Fader, "fadeToTransparent");
        delay(1.5f);

        programNotifyFinish();

    }

    public void initialize()
    {
        explainFader.Start();
        explainFader.fadeToTransparentImmediately();
        explainFader2.Start();
        explainFader2.fadeToTransparentImmediately();
        buttonFader.Start();
        buttonFader.fadeToTransparentImmediately();
    }

    public void ClickOnSendMail()
    {
        Application.OpenURL("mailto:"+mailtoAddress+"?subject="+subject+"&body=");
    }

    public void ClickOnLink(int id)
    {
        Application.OpenURL(OpenURL[id]);
    }
}
