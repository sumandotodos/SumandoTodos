using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackController : MonoBehaviour {

    public static GoBackController instance;

    public AudioClip goBackSound;

    public VirtualEstimationController virtualEstimationController;
    public RealEstimationController realEstimationController;
    public ImagineController imagineController;
    public CollaborationController collaborationController;
    public ShareController shareController;
    public QuestionnaireController questionnaireController;
    public UIFader ButtonFader;

    Dictionary<string, System.Func<bool>> StateDictionary;

    public string CurrentState = "";

    IEnumerator ShowButtonWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        ButtonFader.fadeToOpaque();
    }

    IEnumerator showButtonWithDelay;

    public void GoTo(string NewState)
    {
        StopCoroutine(showButtonWithDelay);
        CurrentState = NewState;
        showButtonWithDelay = ShowButtonWithDelay();
        StartCoroutine(showButtonWithDelay);
    }

    public static GoBackController GetSingleton()
    {
        return instance;
    }

    public void GoBackWithSound()
    {
        SoundController.PlaySound(goBackSound);
        GoBack();
    }

    public void GoBack()
    {

        if(StateDictionary.ContainsKey(CurrentState))
        {
            StateDictionary[CurrentState]();
            ButtonFader.fadeToTransparent();
            CurrentState = "";
        }
        else
        {
            Debug.Log("Nowhere to go back to");
        }

    }

    private void Start()
    {
        instance = this;
        StateDictionary = new Dictionary<string, System.Func<bool>>();
        StateDictionary["VirtualEstimation"] = ReturnFromVirtualEstimation;
        StateDictionary["RealEstimation"] = ReturnFromRealEstimation;
        StateDictionary["Imagine"] = ReturnFromImagine;
        StateDictionary["Collaboration"] = ReturnFromCollaboration;
        StateDictionary["Share"] = ReturnFromShare;
        StateDictionary["Time"] = ReturnFromTime;
        showButtonWithDelay = ShowButtonWithDelay();
    }

    bool ReturnFromImagine()
    {
        imagineController.goTo("Return");
        return true;
    }

    bool ReturnFromCollaboration()
    {
        collaborationController.goTo("Return");
        return true;
    }

    bool ReturnFromVirtualEstimation()
    {
        virtualEstimationController.goTo("Return");
        return true;
    }

    bool ReturnFromRealEstimation()
    {
        realEstimationController.goTo("Return");
        return true;
    }

    bool ReturnFromTime()
    {
        questionnaireController.goTo("Return");
        return true;
    }

    bool ReturnFromShare()
    {
        shareController.goTo("Return");
        return true;
    }

}
