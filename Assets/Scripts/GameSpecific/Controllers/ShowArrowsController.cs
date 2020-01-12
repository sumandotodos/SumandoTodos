using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowArrowsController : FGProgram
{

    public UIFader upFader;
    public UIFader downFader;
    public UIMover upMover;
    public UIMover downMover;
    // Start is called before the first frame update
    void Start()
    {
        delay(0.75f);
        execute(this, "reset");
        delay(0.45f);
        execute(this, "fadeout");
        delay(0.3f);
        execute(this, "reset");
        delay(0.45f);
        execute(this, "fadeout");
        delay(0.3f);
        execute(this, "reset");
        delay(0.45f);
        execute(this, "fadeout");
        delay(0.3f);
        execute(this, "reset");
        delay(0.45f);
        execute(this, "fadeout");
        delay(0.3f);
        execute(this, "reset");
        delay(0.45f);
        execute(this, "fadeout");
        delay(0.3f);
        execute(this, "teardown");
        programNotifyFinish();

        createSubprogram("Cancel");
        execute(this, "teardown");
        programNotifyFinish();

    }

    public void fadeout()
    {
        upFader.fadeToTransparent();
        downFader.fadeToTransparent();
    }

    public void reset()
    {
        upFader.Start();
        upFader.fadeToTransparentImmediately();
        upFader.fadeToOpaque();
        upMover.Start();
        upMover.Reset();
        downFader.Start();
        downFader.fadeToTransparentImmediately();
        downFader.fadeToOpaque();
        downMover.Start();
        downMover.Reset();
    }

    public void teardown()
    {
        upFader.fadeToTransparentImmediately();
        downFader.fadeToTransparentImmediately();
    }


}
