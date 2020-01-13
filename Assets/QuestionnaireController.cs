using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class QuestionnaireController : FGProgram
{
    public UIGeneralFader Slider1;
    public UIGeneralFader Slider2;
    public UITMPFader Question1;
    public UITMPFader Question2;
    public UIGeneralFader Slider3;
    public UITMPFader Question3;

    public UIGeneralFader NextButtonFader;
    public UIGeneralFader NextButton2Fader;
    public UIGeneralFader DoneButtonFader;

    public SliderManager SliderManager1;
    public SliderManager SliderManager2;
    public SliderManager sliderManager3;

    public InputField NameInput;
    public InputField SchoolInput;
    public InputField AddressInput;

    public UITMPFader ThanksForFader;

    public PersonalInfoController personalInfoController;

    int Question1Value;
    int Question2Value;
    int Question3Value;

    Questionaire QuestionnaireValue = new Questionaire();

    // Start is called before the first frame update
    void Start()
    {
        execute(this, "Initialize");
        delay(0.5f);
        execute(Question1, "fadeToOpaque");
        delay(0.5f);
        execute(Slider1, "fadeToOpaque");
        delay(1.5f);
        execute(Question2, "fadeToOpaque");
        delay(0.5f);
        execute(Slider2, "fadeToOpaque");
        delay(0.5f);
        execute(NextButtonFader, "fadeToOpaque");

        createSubprogram("SecondPage");

        execute(NextButtonFader, "fadeToTransparent");
        execute(Slider1, "fadeToTransparent");
        execute(Slider2, "fadeToTransparent");
        execute(Question1, "fadeToTransparent");
        execute(Question2, "fadeToTransparent");
        delay(1.5f);
        execute(Question3, "fadeToOpaque");
        delay(0.5f);
        execute(Slider3, "fadeToOpaque");
        delay(1.0f);
        execute(NextButton2Fader, "fadeToOpaque");

        createSubprogram("ThirdPage");

        execute(NextButton2Fader, "fadeToTransparent");
        execute(this, "RidOfQuestion3");
        delay(1.0f);
        execute(DoneButtonFader, "fadeToOpaque");
        waitForProgram(personalInfoController);

        createSubprogram("Finish");

        execute(personalInfoController, "DismissAll");
        execute(DoneButtonFader, "fadeToTransparent");
        delay(1.25f);
        execute(ThanksForFader, "fadeToOpaque");
        delay(2.5f);
        execute(ThanksForFader, "fadeToTransparent");
        delay(1.0f);
        execute(this, "Publish");
        programNotifyFinish();



        run();
         
    }

    public void RidOfQuestion3()
    {
        Question3.fadeToTransparent();
        Slider3.fadeToTransparent();
    }

    public void Initialize()
    {
        ThanksForFader.Start();
        ThanksForFader.fadeToTransparentImmediately();
        Slider1.GetComponentInChildren<Slider>().value = 0;
        SliderManager1.SetValue(0);
        Slider2.GetComponentInChildren<Slider>().value = 0;
        SliderManager2.SetValue(0);
        Slider3.GetComponentInChildren<Slider>().value = 0;
        sliderManager3.SetValue(0);
        Slider3.Start();
        Question3.Start();
        Slider1.Start();
        Slider1.fadeToTransparentImmediately();
        Slider2.fadeToTransparentImmediately();
        Slider3.fadeToTransparentImmediately();
        Question1.Start();
        Question2.Start();
        Question1.fadeToTransparentImmediately();
        Question2.fadeToTransparentImmediately();
        Question3.fadeToTransparentImmediately();
        Slider2.Start();
        Slider2.fadeToTransparentImmediately();
        NextButtonFader.Start();
        DoneButtonFader.Start();
        NextButton2Fader.Start();
        NextButton2Fader.fadeToTransparentImmediately();
        NextButtonFader.fadeToTransparentImmediately();
        DoneButtonFader.fadeToTransparentImmediately();
    }

    public void NextButton()
    {
        goTo("SecondPage");
    }

    public void NextButton2()
    {
        goTo("ThirdPage");
    }

    public void DoneButton()
    {
        goTo("Finish");
    }

    public void OnQuestion1SliderChange()
    {
        Question1Value = Mathf.FloorToInt(Slider1.GetComponentInChildren<Slider>().value);
        SliderManager1.SetValue(Question1Value);
        QuestionnaireValue.Question1 = Question1Value;
    }

    public void OnQuestion2SliderChange()
    {
        Question2Value = Mathf.FloorToInt(Slider2.GetComponentInChildren<Slider>().value);
        SliderManager2.SetValue(Question2Value);
        QuestionnaireValue.Question2 = Question2Value;
    }

    public void OnDurationSliderChange()
    {
        Question3Value = Mathf.FloorToInt(Slider3.GetComponentInChildren<Slider>().value);
        sliderManager3.SetValue(Question3Value);
        QuestionnaireValue.Question3 = Question3Value;
    }

    public void Publish()
    {
        StartCoroutine(RESTAPIPublishCoroutine());

    }

    IEnumerator RESTAPIPublishCoroutine()
    {
        QuestionnaireValue.Name = NameInput.text;
        QuestionnaireValue.School = SchoolInput.text;
        QuestionnaireValue.Address = AddressInput.text;
        UnityWebRequest newReq;
        WWWForm form = new WWWForm();
        form.AddField("Question1", QuestionnaireValue.Question1);
        form.AddField("Question2", QuestionnaireValue.Question2);
        form.AddField("Question3", QuestionnaireValue.Question3);
        form.AddField("YouAre", QuestionnaireValue.YouAre);
        form.AddField("Name", QuestionnaireValue.Name);
        form.AddField("School", QuestionnaireValue.School);
        form.AddField("Address", QuestionnaireValue.Address);
        newReq = UnityWebRequest.Post(Config.ServerURL + "/questionnaire", form);
        newReq.SetRequestHeader("psk", Config.PSK);
        newReq.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return newReq.SendWebRequest();

    }


}
