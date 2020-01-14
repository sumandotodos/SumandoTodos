using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    static UIController instance;

    public GameObject Title;
    public GameObject MainMenu;
    public GameObject RealEstimator;
    public GameObject VirtualEstimator;
    public GameObject VirtualEstimatorPartI;
    public GameObject VirtualEstimatorPartII;
    public GameObject VirtualEstimatorPartIII;
    public GameObject Tips;
    public GameObject Imagine;
    public GameObject Share;
    public GameObject NoToques;
    public GameObject TouchPlate;
    public GameObject Collaborate;
    public GameObject ThreeDeeStuff;
    public GameObject Questionnaire;
    public UIFader AppTitleFader;

    public static UIController GetSingleton()
    {
        return instance;
    }

    // Use this for initialization
    void Start () {
        InitialState();
        instance = this;
	}

    public void fadeAppTitleIn()
    {
        AppTitleFader.fadeToOpaque();
    }

    public void fadeAppTitleOut()
    {
        AppTitleFader.fadeToTransparent();
    }

    public void InitialState()
    {
        Tips.SetActive(true);
        ThreeDeeStuff.SetActive(false);
        Imagine.SetActive(false);
        NoToques.SetActive(true);
        Title.SetActive(true);
        MainMenu.SetActive(true);
        RealEstimator.SetActive(false);
        VirtualEstimator.SetActive(false);
        VirtualEstimatorPartIII.SetActive(false);
        TouchPlate.SetActive(false);
        Collaborate.SetActive(false);
        Share.SetActive(false);
        Questionnaire.SetActive(false);
    }

    public void RealEstimatorState()
    {
        ThreeDeeStuff.SetActive(false);
        RealEstimator.SetActive(true);
        Imagine.SetActive(false);
        Title.SetActive(false);
        RealEstimator.GetComponent<UIGeneralFader>().Start();
        VirtualEstimator.SetActive(false);
        TouchPlate.SetActive(false);
        Collaborate.SetActive(false);
        Share.SetActive(false);
    }

    public void VirtualEstimatorState()
    {
        ThreeDeeStuff.SetActive(true);
        RealEstimator.SetActive(false);
        Imagine.SetActive(false);
        RealEstimator.GetComponent<UIGeneralFader>().Start();
        VirtualEstimator.SetActive(true);
        VirtualEstimatorPartI.SetActive(true);
        VirtualEstimatorPartII.SetActive(false);
        VirtualEstimatorPartIII.SetActive(false);
        TouchPlate.SetActive(false);
        Title.SetActive(false);
        Collaborate.SetActive(false);
        Share.SetActive(false);
    }

    public void VirtualEstimatorPartIIState()
    {
        VirtualEstimatorPartI.SetActive(false);
        Imagine.SetActive(false);
        VirtualEstimatorPartII.SetActive(true);
        VirtualEstimatorPartIII.SetActive(false);
        TouchPlate.SetActive(false);
        Collaborate.SetActive(false);
        Title.SetActive(false);
        Share.SetActive(false);
    }

    public void VirtualEstimatorPartIIIState()
    {
        VirtualEstimatorPartIII.SetActive(true);

    }

    public void ImagineState()
    {
        ThreeDeeStuff.SetActive(false);
        Imagine.SetActive(true);
        NoToques.SetActive(true);
        Title.SetActive(false);
        //MainMenu.SetActive(false);
        RealEstimator.SetActive(false);
        VirtualEstimator.SetActive(false);
        TouchPlate.SetActive(true);
        Collaborate.SetActive(false);
        Share.SetActive(false);

    }

    public void TimeState()
    {
        ThreeDeeStuff.SetActive(false);
        Imagine.SetActive(false);
        NoToques.SetActive(false);
        Title.SetActive(false);
        //MainMenu.SetActive(false);
        RealEstimator.SetActive(false);
        VirtualEstimator.SetActive(false);
        TouchPlate.SetActive(true);
        Collaborate.SetActive(false);
        Share.SetActive(false);
        Questionnaire.SetActive(true);
    }

    public void CollaborateState()
    {
        ThreeDeeStuff.SetActive(false);
        Imagine.SetActive(false);
        NoToques.SetActive(true);
        Title.SetActive(false);
       // MainMenu.SetActive(false);
        RealEstimator.SetActive(false);
        VirtualEstimator.SetActive(false);
        TouchPlate.SetActive(false);
        Collaborate.SetActive(true);
        Share.SetActive(false);
    }

    public void ShareState()
    {
        ThreeDeeStuff.SetActive(false);
        Imagine.SetActive(false);
        NoToques.SetActive(true);
        Title.SetActive(false);
        //MainMenu.SetActive(false);
        RealEstimator.SetActive(false);
        VirtualEstimator.SetActive(false);
        TouchPlate.SetActive(false);
        Collaborate.SetActive(false);
        Share.SetActive(true);
    }
}
