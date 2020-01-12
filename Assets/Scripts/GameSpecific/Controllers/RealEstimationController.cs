using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RealBottleGame
{
    public string code;
    public int estimation;
    public bool solved;
}

[System.Serializable]
public class RealVotesResult
{
    public bool solved;
    public int deleted;
    public int realamount;
    public List<int> votes;
}

[System.Serializable]
public class RealBottleGames
{
    public List<RealBottleGame> games = new List<RealBottleGame>();
}

public class RealEstimationController : FGProgram {

    public UITip estimationTip;

    public UIGeneralFader uiControlsFader;
    public InputField estimation;
    public UIController uiController;
    public UIGeneralFader HowManyGarbanzos;
    public UIGeneralFader EnterEstimation;
    public UIGeneralFader AmountInputField;
    public UIGeneralFader SubmitButton;
    public UIGeneralFader EnterCodeLabelFader;
    public UIGeneralFader DropdownFader;
    public UIGeneralFader NewCodeInputFader;
    public UIGeneralFader NextButtonFader;
    public UIGeneralFader YourEstimateLabelFader;
    public UIGeneralFader YourEstimationAmountFader;
    public UIGeneralFader EstimationRegisterOKFader;
    public HistogramComponent HistogramRenderer;
    public UIGeneralFader totalAverageLabel;
    public UITextFader codeNotFoundFader;
    public Transform winSpawnLocation;
    public GameObject winPrefab;
    public UIGeneralFader actualLabel;

    public string CurrentSchoolID = "";
    public int CurrentEstimation = -1;
    bool CurrentIsDeleted = false;
    bool CurrentIsSolved = false;
    int CurrentRealAmount = 0;
    bool useInputFieldCode = false;
    bool mustPersist = false;
    List<int> CurrentVotes = new List<int>();
    bool voteSubmittedOK = false;
    IEnumerator loadVotesCoRo = null;
    public bool VotesDataFromServerAvailable = false;

    // Use this for initialization
    void Start () {

        createSubprogram("Show");

        execute(this, "initialize");
        execute(uiController, "RealEstimatorState");
        execute(EnterCodeLabelFader, "fadeToOpaque");
        delay(0.15f);
        execute(DropdownFader, "fadeToOpaque");
        delay(0.015f);
        execute(NextButtonFader, "fadeToOpaque");



        createSubprogram("EnterOrShowEstimation");
        debug("...");
        execute(uiController, "RealEstimatorState");
        execute(EnterCodeLabelFader, "fadeToTransparent");
        delay(0.05f);
        execute(DropdownFader, "fadeToTransparent");
        delay(0.05f);
        execute(NewCodeInputFader, "fadeToTransparent");
        delay(0.05f);
        execute(NextButtonFader, "fadeToTransparent");
        delay(0.05f);
        waitForCondition(this, "VotesDataFromServerIsAvailable");
        execute(this, "AttemptAddLocalPersistence");
        execute(this, "ChoosePath");


        createSubprogram("PartII-a");
        execute(HowManyGarbanzos, "fadeToOpaque");
        delay(0.15f);
        execute(EnterEstimation, "fadeToOpaque");
        delay(0.15f);
        execute(AmountInputField, "fadeToOpaque");
        delay(0.15f);
        execute(SubmitButton, "fadeToOpaque");
        delay(0.15f);


        createSubprogram("PartII-b");
        debug("...");
        execute(YourEstimateLabelFader, "fadeToOpaque");
        delay(0.15f);
        execute(YourEstimationAmountFader, "fadeToOpaque");
        delay(2.0f);
        execute(this, "leave");


        createSubprogram("PartIII");
        debug("...");
        execute(this, "solve");


        createSubprogram("Hide");

        execute(uiControlsFader, "fadeToTransparent");
        delay(1.0f);
        programNotifyFinish();

        createSubprogram("VoteRegistration");
        execute(HowManyGarbanzos, "fadeToTransparent");
        execute(EnterEstimation, "fadeToTransparent");
        execute(AmountInputField, "fadeToTransparent");
        execute(SubmitButton, "fadeToTransparent");
        waitForCondition(this, "VoteHasBeenSumitted");
        execute(EstimationRegisterOKFader, "fadeToOpaque");
        delay(1.0f);
        execute(this, "doEstimationTip");
        delay(0.15f);
        waitForCondition(estimationTip, "hasBeenDispatched");
        delay(2.0f);
        execute(this, "leave");


        createSubprogram("CodeNotFound");
        execute(NewCodeInputFader, "fadeToTransparent");
        execute(NextButtonFader, "fadeToTransparent");
        execute(codeNotFoundFader, "fadeToOpaque");
        delay(2.0f);
        execute(codeNotFoundFader, "fadeToTransparent");
        execute(this, "leave");


        createSubprogram("Return");
       
        execute(HowManyGarbanzos, "fadeToTransparent");
        execute(EnterEstimation, "fadeToTransparent");
        execute(AmountInputField, "fadeToTransparent");
        execute(SubmitButton, "fadeToTransparent");
        execute(AmountInputField, "fadeToTransparent");
        execute(YourEstimateLabelFader, "fadeToTransparent");
        execute(YourEstimationAmountFader, "fadeToTransparent");
        execute(EstimationRegisterOKFader, "fadeToTransparent");
        execute(HistogramRenderer.GetComponent<UIGeneralFader>(), "fadeToTransparent");
        execute(totalAverageLabel, "fadeToTransparent");
        execute(actualLabel, "fadeToTransparent");
        execute(EnterCodeLabelFader, "fadeToTransparent");
        execute(DropdownFader, "fadeToTransparent");
        execute(NextButtonFader, "fadeToTransparent");
        execute(NewCodeInputFader, "fadeToTransparent");
        execute(codeNotFoundFader, "fadeToTransparent");
        delay(1.5f);
        execute(this, "deinitialize");
        delay(1.02f);
        programNotifyFinish();


    }

    public void deinitialize()
    {
        if(loadVotesCoRo!=null)
        {
            StopCoroutine(loadVotesCoRo);
        }

    }

    public void solve()
    {
        //UIController.GetSingleton().VirtualEstimatorPartIIIState();
        RealBottleGame g = ListUtils.FindOneInList<RealBottleGame>
        (LoadSaveController.persist_realBottleGames.games,
            (x => x.code == CurrentSchoolID));
        if (g != null)
        {
            if (g.solved == false)
            {
                bool win = AmIWinner();
                if (win)
                {
                    GameObject winObj = (GameObject)Instantiate(winPrefab);
                    winObj.transform.SetParent(winSpawnLocation);
                    winObj.transform.localScale = Vector3.one;
                    winObj.transform.localPosition = Vector3.zero;
                    IntuitionPointsController.AddScoreAndSave(200);
                }
                g.solved = true;
                LoadSaveController.SaveRealGames();
            }
        }

        HistogramData data = StatsUtils.Histogram(CurrentVotes, -1);

        totalAverageLabel.GetComponent<Text>().text = "Promedio: " + StatsUtils.Average(CurrentVotes) + " garbanzos";
        actualLabel.GetComponent<Text>().text = "Cantidad real: " + CurrentRealAmount + " garbanzos";
        if (data.binCount.Count >= 3)
        {
            HistogramRenderer.SetHistogram(data);
            HistogramRenderer.Grow();
            HistogramRenderer.GetComponent<UIGeneralFader>().fadeToOpaque();
        }

        totalAverageLabel.fadeToOpaque();
        actualLabel.fadeToOpaque();


    }

    public void leave()
    {
        if (loadVotesCoRo != null)
        {
            StopCoroutine(loadVotesCoRo);
        }
        NewCodeInputFader.GetComponent<InputField>().text = "";
        GoBackController.GetSingleton().GoBack();
    }

    public bool VotesDataFromServerIsAvailable()
    {
        return VotesDataFromServerAvailable;
    }

    public void initialize()
    {
        LoadSaveController.LoadRealGames();
        CurrentSchoolID = "";
        submitLock = false;
        clickOnNextLock = false;
        voteSubmittedOK = false;
        YourEstimateLabelFader.fadeToTransparentImmediately();
        NewCodeInputFader.GetComponent<InputField>().text = "";
        EstimationRegisterOKFader.fadeToTransparentImmediately();
        codeNotFoundFader.fadeToTransparentImmediately();
        initializeDropdown();
        loadVotesCoRo = null;
        CurrentIsSolved = false;
        CurrentIsDeleted = false;
        CurrentVotes = new List<int>();
        VotesDataFromServerAvailable = false;
        HistogramRenderer.GetComponent<UIGeneralFader>().fadeToTransparentImmediately();
        totalAverageLabel.fadeToTransparentImmediately();
        actualLabel.fadeToTransparentImmediately();
        HistogramRenderer.DestroyBars();
        codeNotFoundFader.fadeToTransparentImmediately();
        CurrentRealAmount = 0;
    }

    public bool VoteHasBeenSumitted()
    {
        return voteSubmittedOK;
    }

    public void doEstimationTip()
    {
        estimationTip.go();
    }

    public void initializeDropdown()
    {
        Dropdown d = DropdownFader.GetComponent<Dropdown>();
        d.options.Clear();
        for (int i = 0; i < LoadSaveController.persist_realBottleGames.games.Count; ++i)
        {
            string code = LoadSaveController.persist_realBottleGames.games[i].code;
            d.options.Add(new Dropdown.OptionData(code));
        }
        d.options.Add(new Dropdown.OptionData("Otro..."));
        d.value = 0;
        d.captionText.text = d.options[0].text;
        if (d.options.Count == 1)
        {
            NewCodeInputFader.fadeToOpaque();
            useInputFieldCode = true;
            NewCodeInputFader.GetComponent<InputField>().text = "";
            CurrentSchoolID = "";
        }
        else
        {
            CurrentSchoolID = d.options[0].text;
        }

    }

    public void deleteGroupFromList(string gr)
    {
        ListUtils.DeleteInList<RealBottleGame>
         (LoadSaveController.persist_realBottleGames.games,
             (x => x.code == gr));

        LoadSaveController.SaveRealGames();

    }

    // UI Events

    public void PressSubmit()
    {
        //  goTo("Hide");
        goTo("Return");
    }

    public void OnDropDownChange(Dropdown v)
    {
        Debug.Log("value: " + v.value);
        if (v.value == v.options.Count - 1)
        {
            NewCodeInputFader.fadeToOpaque();
            useInputFieldCode = true;
        }
        else
        {
            NewCodeInputFader.fadeToTransparent();
            CurrentSchoolID = v.options[v.value].text;
            useInputFieldCode = false;
        }
    }

    public void AttemptAddLocalPersistence()
    {
        if (mustPersist)
        {
            RealBottleGame g = ListUtils.FindOneInList<RealBottleGame>
                (LoadSaveController.persist_realBottleGames.games,
                (x) => x.code == CurrentSchoolID);
            if (g == null)
            {
                LoadSaveController.persist_realBottleGames.games.Add
                         (new RealBottleGame
                         {
                             code = CurrentSchoolID,
                             estimation = -1
                         });
                LoadSaveController.SaveRealGames();
            }
        }
    }

    IEnumerator GetNumberOfAnswersForSchoolID()
    {
        WWW www;
        bool done = false;
        int nRetries = 0;
        while (1 < 2)
        {
            yield return (www = new WWW(Config.ServerURL + "/real/votes?schoolid=" + CurrentSchoolID, null, Config.Headers));
            if (www.error == null)
            {

                if (www.text.ToLower().Contains("not"))
                {
                    done = true;
                    goTo("CodeNotFound");
                }
                else
                {

                    RealVotesResult res = JsonUtility.FromJson<RealVotesResult>(www.text);

                    if (CurrentIsDeleted == false && res.deleted > 0)
                    {
                        deleteGroupFromList(CurrentSchoolID);
                        NewCodeInputFader.GetComponent<InputField>().text = "";
                        CurrentIsDeleted = true;
                    }
                    if (CurrentIsSolved == false && res.solved == true)
                    {
                        CurrentIsSolved = true;
                    }

                    VotesDataFromServerAvailable = true;
                    CurrentVotes = res.votes;
                    CurrentRealAmount = res.realamount;
                    //personsEstimationsController.SetEstimations(NumberOfAnswersForGroupID);
                    done = true;
                }
            }
            else
            {
                if (nRetries == 1)
                {
                    NoConnectionController.NoConnectionWarning();
                }
                ++nRetries;
                yield return new WaitForSeconds(4.0f);
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

    bool clickOnNextLock = false;

    public void clickOnNext()
    {
        if (clickOnNextLock) return;
        clickOnNextLock = true;
        goTo("EnterOrShowEstimation");
        if (useInputFieldCode)
        {
            CurrentSchoolID = NewCodeInputFader.GetComponent<InputField>().text;
            mustPersist = true;

        }
        RealBottleGame g = ListUtils.FindOneInList<RealBottleGame>
        (LoadSaveController.persist_realBottleGames.games,
            (x => x.code == CurrentSchoolID));
        if (g != null)
        {
            CurrentEstimation = g.estimation;
        }
        loadVotesCoRo = GetNumberOfAnswersForSchoolID();
        StartCoroutine(loadVotesCoRo);
    }

    public void ChoosePath()
    {
        if(CurrentIsSolved)
        {
            goTo("PartIII");
        }
        else if (CurrentEstimation < 1)
        {
            goTo("PartII-a");
        }
        else
        {
            goTo("PartII-b");
            YourEstimationAmountFader.GetComponent<Text>().text =
                CurrentEstimation + " garbanzos";
        }
    }

    IEnumerator SendVoteToServerCoRo()
    {
        WWW www;
        bool done = false;
        int nRetries = 0;
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("schoolid", CurrentSchoolID);
        wwwForm.AddField("amount", AmountInputField.GetComponent<InputField>().text);

        while (!done)
        {
            yield return (www = new WWW(Config.ServerURL + "/real/votes", wwwForm.data, Config.Headers));
            if (www.error != null)
            {
                if (nRetries == 1)
                {

                    NoConnectionController.NoConnectionWarning();
                }
                ++nRetries;
                yield return new WaitForSeconds(4.0f);
            }
            else
            {
                done = true;
                voteSubmittedOK = true;
            }
        }
        int esteem;
        int.TryParse(AmountInputField.GetComponent<InputField>().text, out esteem);
        RealBottleGame g = ListUtils.FindOneInList<RealBottleGame>
        (LoadSaveController.persist_realBottleGames.games,
            (x => x.code == CurrentSchoolID));
        if (g != null)
        {
            g.estimation = esteem;
            LoadSaveController.SaveRealGames();
        }
    }

    public bool validateInput(string txt)
    {
        int amount;
        return (int.TryParse(txt, out amount) && amount > 0);
    }

    bool submitLock = false;

    public void clickOnSubmit()
    {
        if (submitLock == true) return;
        submitLock = true;
        if (validateInput(AmountInputField.GetComponent<InputField>().text))
        {
            StartCoroutine(SendVoteToServerCoRo());
            goTo("VoteRegistration");
        }
    }

    public bool AmIWinner()
    {
        int actual = CurrentRealAmount;
        int minDiff = 1000000;
        int closestEstimate = -1;
        for (int i = 0; i < CurrentVotes.Count; ++i)
        {
            if (Mathf.Abs(actual - CurrentVotes[i]) < minDiff)
            {
                minDiff = Mathf.Abs(actual - CurrentVotes[i]);
                closestEstimate = CurrentVotes[i];
            }
        }
        if (CurrentEstimation == closestEstimate)
        {
            return true;
        }
        else return false;
    }

}
