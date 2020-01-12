using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListOfResults
{

    public List<int> result;

}

public class MainScreenController : FGProgram {

    public static MainScreenController instance;

    public UIFader globalFader;
    public ShowTitleController showTitleController;
    public ShowIconsController showIconsController;
    public ShowIconsController showVirtualBottlesController;
    public UIController uiController;
    public UIFader Overlay;
    public UIGeneralFader PointsFader;
    public RealEstimationController realEstimationController;
    public VirtualEstimationController virtualEstimationController;
    public IconPressEnabler iconPressEnabler;
    public TipsController tipsController;
    public WelcomeTipsController welcomeTipsController;
    public MenuTipsController menuTipsController;
    public BottleTipsController bottleTipsController;
    public GoBackController goBackController;
    public ImagineController imagineController;
    public CollaborationController collaborationController;
    public ShareController shareController;
    public QuestionnaireController questionnaireController;

    public string ProfePDFAddress = "https://apps.flygames.org/pdf/GuiaDocentes.pdf";

    FGProgram programToWaitFor;

    private void Awake()
    {
        instance = this;
    }

    bool returnToShare = false;
    public static void SetReturnToShare(bool en)
    {
        instance.returnToShare = en;
    }

    // Use this for initialization
    void Start () {

        Config.init();
        LoadSaveController.LoadVirtualGames();

        delay(0.5f);
        waitForTask(globalFader, "fadeToTransparentTask", this);
        waitForProgram(showTitleController);
        delay(0.5f);
        programGoTo("ShowMainMenu");

        createSubprogram("ShowMainMenu");
        execute(Overlay, "fadeToOpaque");
        execute(uiController, "InitialState");
        execute(showIconsController, "goTo", "Show");
        waitForProgram(showIconsController);
        waitForProgram(menuTipsController);
        execute(iconPressEnabler, "EnableAllButtons");
        execute(PointsFader, "fadeToOpaque");



        createSubprogram("RealBottle");

        execute(goBackController, "GoTo", "RealEstimation");
        execute(iconPressEnabler, "DisabeAllButtons");
        execute(PointsFader, "fadeToTransparent");
        execute(showIconsController, "goTo", "Hide");
        waitForProgram(showIconsController);
        waitForProgram(bottleTipsController);
        execute(realEstimationController, "goTo", "Show");
        waitForProgram(realEstimationController);
        programGoTo("ShowMainMenu");



        createSubprogram("VirtualBottle");

        execute(goBackController, "GoTo", "VirtualEstimation");
        execute(iconPressEnabler, "DisabeAllButtons");
        execute(PointsFader, "fadeToTransparent");
        execute(showIconsController, "goTo", "Hide");
        waitForProgram(showIconsController);
        waitForProgram(bottleTipsController);
        execute(iconPressEnabler, "EnableAllButtons");
        waitForProgram(virtualEstimationController);
        //programGoTo("ShowMainMenu");
        execute(this, "choosePath");


        createSubprogram("Time");

        execute(goBackController, "GoTo", "Time");
        execute(Overlay, "fadeToTransparent");
        execute(iconPressEnabler, "DisabeAllButtons");
        execute(PointsFader, "fadeToTransparent");
        execute(showIconsController, "goTo", "Hide");
        waitForProgram(showIconsController);
        execute(uiController, "TimeState");
        waitForProgram(questionnaireController);
        programGoTo("ShowMainMenu");


        createSubprogram("Imagine");

        execute(goBackController, "GoTo", "Imagine");
        execute(Overlay, "fadeToTransparent");
        execute(iconPressEnabler, "DisabeAllButtons");
        execute(PointsFader, "fadeToTransparent");
        execute(showIconsController, "goTo", "Hide");
        waitForProgram(showIconsController);
        execute(uiController, "ImagineState");
        waitForProgram(imagineController);
        programGoTo("ShowMainMenu");



        createSubprogram("Collaborate");
        execute(goBackController, "GoTo", "Collaboration");
        execute(Overlay, "fadeToTransparent");
        execute(iconPressEnabler, "DisabeAllButtons");
        execute(PointsFader, "fadeToTransparent");
        execute(showIconsController, "goTo", "Hide");
        waitForProgram(showIconsController);
        debug("hidden");
        delay(0.75f);
        execute(uiController, "CollaborateState");
        waitForProgram(collaborationController);
        programGoTo("ShowMainMenu");


        createSubprogram("Share");
        debug("...");
        execute(goBackController, "GoTo", "Share");
        execute(Overlay, "fadeToTransparent");
        execute(iconPressEnabler, "DisabeAllButtons");
        execute(PointsFader, "fadeToTransparent");
        execute(showIconsController, "goTo", "Hide");
        waitForProgram(showIconsController);
        debug("hidden");
        execute(uiController, "ShareState");
        waitForProgram(shareController);
        programGoTo("ShowMainMenu");

        run();


    }

    //programGoTo("ShowMainMenu");
    public void choosePath()
    {
        if (returnToShare) goTo("Share");
        else goTo("ShowMainMenu");
    }

    public void runtimeWaitForProgram()
    {
        programToWaitFor.goTo("Show");
        waitForProgram(programToWaitFor);
    }


    // UI Events
    public void Virtual()
    {
        uiController.VirtualEstimatorState();
        goTo("VirtualBottle");
    }

    public void Real()
    {
        uiController.RealEstimatorState();
        goTo("RealBottle");
    }

    public void Join()
    {
        goTo("Collaborate");
        //Application.OpenURL("mailto:suma@sumandotodos.es?subject=Desde%20App%20Imagine&body=");
    }

    public void Imagine()
    {
        goTo("Imagine");
    }

    public void WhatsappShare()
    {
        goTo("Share");
    }

    public void Time()
    {
        goTo("Time");
    }

    public void Profe()
    {
        Application.OpenURL(ProfePDFAddress);
    }

}
