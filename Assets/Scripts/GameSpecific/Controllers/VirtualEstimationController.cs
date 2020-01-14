using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VotesResult
{
    public bool solved;
    public int type;
    public int deleted;
    public List<int> votes;
}

[System.Serializable]
public class IntegerResult
{
    public int result;
}

[System.Serializable]
public class VirtualBottleGame
{
    public string code;
    public int bottleType;
    public int estimation;
    public bool solved;
    public bool ownwer;
}

[System.Serializable]
public class VirtualBottleGames
{
    public List<VirtualBottleGame> games = new List<VirtualBottleGame>();
}


public class VirtualEstimationController : FGProgram {

    public Color[] colors;
    public Sprite[] sprites;

    public UITip estimationTip;

    public AudioClip clickOnBottleSound;

    public UITip shareTip;
    public UITextFader ChooseBottleFader;
    public UITextFader EnterCodeFader;
    public UIGeneralFader DropDownFader;
    public UIGeneralFader InputFieldFader;
    public UIGeneralFader ButtonFader;
    public GameObject[] bottleBackdrops;
    public UIFader[] BottleFader;
    public ShowIconsController showVirtualBottlesController;
    public IconPressEnabler iconPressEnabler;
    public UIController uiController;
    public PersonsEstimationsController personsEstimationsController;
    public UIGeneralFader HowManyGarbanzos;
    public UIGeneralFader AmountInputField;
    public UIGeneralFader SubmitButton;
    public UIGeneralFader AlreadyLabel;
    public UIGeneralFader SolveButton;
    public UIGeneralFader DeleteButton;
    public UIGeneralFader YourEstimation;
    public UIGeneralFader YourEstimationAmount;
    public UIGeneralFader EstimationRegisteredOK;
    public UIGeneralFader CodeFader;
    public HistogramComponent HistogramRenderer;
    public UIGeneralFader totalAverageLabel;
    public UIGeneralFader averageIndividualErrorLabel;
    public UIGeneralFader actualLabel;
    public UIGeneralFader overallErrorLabel;
    public UITextFader codeNotFoundFader;
    public Transform winSpawnLocation;
    public GameObject winPrefab;
    public Text groupCodeLabel;

    int[] ActualAmountOfGarbanzos = new int[]{ 0, 244, 699, 305, 273 };

    IEnumerator loadVotesCoRo = null;
    public string CurrentGroupID;
    public int chosenBottle = -1;
    bool CurrentIsOwner;
    bool CurrentIsDeleted;
    public int CurrentEstimation = -1;
    bool CurrentIsSolved = false;
    bool VotesDataFromServerAvailable = false;
    public int NumberOfAnswersForGroupID = -1;
    List<int> CurrentVotes = new List<int>();
    bool bottleSavedOnServer = false;
    bool GroupHasBeenDeleted = false;
    bool mustPersist = false;
    bool voteSubmittedOK = false;

    // Use this for initialization
    void Start () {

        execute(this, "initialize");
        execute(this, "EnableBackdrops");
        execute(showVirtualBottlesController, "goTo", "Show");
        waitForProgram(showVirtualBottlesController);
        execute(ChooseBottleFader, "fadeToOpaque");
        delay(0.5f);
        execute(DropDownFader, "fadeToOpaque");
        delay(0.25f);
        execute(this, "ShowEnterCode");
        execute(EnterCodeFader, "fadeToOpaque");
        delay(0.15f);
        execute(ButtonFader, "fadeToOpaque");

        createSubprogram("Dismiss");
        execute(ChooseBottleFader, "fadeToTransparent");
        execute(EnterCodeFader, "fadeToTransparent");
        execute(InputFieldFader, "fadeToTransparent");
        execute(DropDownFader, "fadeToTransparent");
        execute(ButtonFader, "fadeToTransparent");
        execute(showVirtualBottlesController, "goTo", "Hide");
        waitForProgram(showVirtualBottlesController);
        delay(0.5f);
        programNotifyFinish();

        createSubprogram("PartII");
        debug("...");
        //execute(this, "GetNewGroupIDFromServer");
        execute(showVirtualBottlesController, "goTo", "Hide");
        waitForProgram(showVirtualBottlesController);
        execute(ChooseBottleFader, "fadeToTransparent");
        execute(EnterCodeFader, "fadeToTransparent");
        execute(InputFieldFader, "fadeToTransparent");
        execute(DropDownFader, "fadeToTransparent");
        execute(ButtonFader, "fadeToTransparent");
        execute(this, "GetEstimationsFromServer");
        waitForCondition(this, "bottleTypeIsSet");
        execute(this, "AttemptAddLocalPersistence");
        delay(1.0f);
        execute(uiController, "VirtualEstimatorPartIIState");
        execute(this, "DisableBackdrops");
        execute(this, "ShowChosenBottle");
        execute(this, "ChoosePath");

        createSubprogram("PartII-a");
        debug("...");
        execute(HowManyGarbanzos, "fadeToOpaque");
        delay(0.5f);
        execute(AmountInputField, "fadeToOpaque");
        delay(0.5f);
        execute(this, "ShowSubmitButton");
        //execute(SubmitButton, "fadeToOpaque");
        delay(0.5f);
        programGoTo("PartII-c");

        createSubprogram("PartII-b");
        debug("...");
        execute(this, "SetUpEstimation");
        execute(YourEstimation, "fadeToOpaque");
        delay(0.25f);
        execute(YourEstimationAmount, "fadeToOpaque");
        delay(1.0f);
        execute(this, "doEstimationTip");
        delay(0.15f);
        waitForCondition(estimationTip, "hasBeenDispatched");
        delay(0.25f);
        execute(this, "doShareTip");
        programGoTo("PartII-c");

        createSubprogram("PartII-c");
        execute(AlreadyLabel, "fadeToOpaque");
        delay(0.25f);
        execute(this, "ShowSolveButton");
        //execute(SolveButton, "fadeToOpaque");
        delay(0.5f);
        createSubprogram("GetCodeFromServerAndUploadBottleTypeToServer");
        execute(showVirtualBottlesController, "goTo", "Hide");
        execute(showVirtualBottlesController, "run");
        execute(this, "GetNewGroupIDFromServer");
        waitForCondition(this, "NewGroupIDIsSet");
        debug("calling_set_bottle_type_on_server");
        execute(this, "SetBottleTypeOnServer");
        waitForCondition(this, "BottleTypeIsSavedOnServer");
        programGoTo("PartII");

        createSubprogram("PartIII");
        debug("...");
        waitForCondition(this, "VotesDataFromServerIsAvailable");
        execute(this, "showPartIIIComponents");
        execute(YourEstimation, "fadeToTransparent");
        execute(AlreadyLabel, "fadeToTransparent");
        execute(this, "solve");

        createSubprogram("VoteRegistration");
        execute(this, "HideChosenBottle");
        execute(ChooseBottleFader, "fadeToTransparent");
        execute(EnterCodeFader, "fadeToTransparent");
        execute(InputFieldFader, "fadeToTransparent");
        execute(DropDownFader, "fadeToTransparent");
        execute(ButtonFader, "fadeToTransparent");
        execute(HowManyGarbanzos, "fadeToTransparent");
        execute(AmountInputField, "fadeToTransparent");
        execute(SubmitButton, "fadeToTransparent");
        execute(personsEstimationsController, "fadeAllToTransparent");
        execute(AlreadyLabel, "fadeToTransparent");
        execute(SolveButton, "fadeToTransparent");
        execute(YourEstimation, "fadeToTransparent");
        execute(YourEstimationAmount, "fadeToTransparent");
        execute(CodeFader, "fadeToTransparent");
        delay(0.5f);
        waitForCondition(this, "VoteHasBeenSubmittedOK");
        execute(EstimationRegisteredOK, "fadeToOpaque");
        delay(2.0f);
        execute(EstimationRegisteredOK, "fadeToTransparent");
        //execute(this, "leave");
        execute(this, "preparePartII");
        programGoTo("PartII-b");

        createSubprogram("CodeNotFound");
        execute(codeNotFoundFader, "fadeToOpaque");
        delay(2.0f);
        execute(codeNotFoundFader, "fadeToTransparent");
        execute(this, "leave");

        createSubprogram("Return");
        execute(this, "HideEverything");
        execute(this, "stopLoadCoRo");
        delay(1.52f);
        execute(this, "EnableBackdrops");
        programNotifyFinish();

    }

    public void stopLoadCoRo()
    {
        if (loadVotesCoRo != null)
        {
            StopCoroutine(loadVotesCoRo);
        }
        loadVotesCoRo = null;
    }

    public void clickOnShareDirect()
    {
        MainScreenController.SetReturnToShare(true);
        ShareController.SetForceCode(CurrentGroupID);
        GoBackController.instance.GoBack();
    }

    public void preparePartII()
    {
        BottleFader[chosenBottle - 1].fadeToOpaque();
        CodeFader.fadeToOpaque();
        personsEstimationsController.fadeAllToOpaque();
    }

    public void HideEverything()
    {
        showVirtualBottlesController.goTo("Hide");
        HideChosenBottle();
        ChooseBottleFader.fadeToTransparent();
        EnterCodeFader.fadeToTransparent();
        InputFieldFader.fadeToTransparent();
        DropDownFader.fadeToTransparent();
        AlreadyLabel.fadeToTransparent();
        ButtonFader.fadeToTransparent();
        personsEstimationsController.fadeAllToTransparent();
        HowManyGarbanzos.fadeToTransparent();
        AmountInputField.fadeToTransparent();
        SubmitButton.fadeToTransparent();
        CodeFader.fadeToTransparent();
        SolveButton.fadeToTransparent();
        YourEstimation.fadeToTransparent();
        EstimationRegisteredOK.fadeToTransparent();
        YourEstimationAmount.fadeToTransparent();
        totalAverageLabel.fadeToTransparent();
        actualLabel.fadeToTransparent();
        overallErrorLabel.fadeToTransparent();
        averageIndividualErrorLabel.fadeToTransparent();
        DeleteButton.fadeToTransparent();

        HistogramRenderer.GetComponent<UIGeneralFader>().fadeToTransparent();

    }

    public void showPartIIIComponents()
    {
        showVirtualBottlesController.goTo("Hide");
        HideChosenBottle();
        ChooseBottleFader.fadeToTransparent();
        EnterCodeFader.fadeToTransparent();
        InputFieldFader.fadeToTransparent();
        DropDownFader.fadeToTransparent();
        ButtonFader.fadeToTransparent();
        personsEstimationsController.fadeAllToTransparent();
        HowManyGarbanzos.fadeToTransparent();
        AmountInputField.fadeToTransparent();
        SubmitButton.fadeToTransparent();
        SolveButton.fadeToTransparent();
        YourEstimation.fadeToTransparent();
        YourEstimationAmount.fadeToTransparent();
    }

    public void leave()
    {
        if (loadVotesCoRo != null)
        {
            StopCoroutine(loadVotesCoRo);
        }
        InputFieldFader.GetComponent<InputField>().text = "";
        GoBackController.GetSingleton().GoBack();
    }

    public void initialize()
    {
        CurrentGroupID = groupCodeLabel.text = "";
        NumberOfAnswersForGroupID = -1;
        voteSubmittedLock = false;
        clickOnNextLock = false;
        chosenBottle = -1;
        CurrentIsOwner = false;
        CurrentIsDeleted = false;
        initializeDropdown();
        VotesDataFromServerAvailable = false;
        InputFieldFader.fadeToTransparentImmediately();
        CurrentEstimation = -1;
        AmountInputField.GetComponent<InputField>().text = "";
        EstimationRegisteredOK.fadeToTransparentImmediately();
        bottleSavedOnServer = false;
        mustPersist = false;
        CurrentIsSolved = false;
        GroupHasBeenDeleted = false;
        CurrentVotes = new List<int>();
        HistogramRenderer.DestroyBars();
        codeNotFoundFader.fadeToTransparentImmediately();
        voteSubmittedOK = false;
    }

    public bool VoteHasBeenSubmittedOK()
    {
        return voteSubmittedOK;
    }

    public bool VotesDataFromServerIsAvailable()
    {
        return VotesDataFromServerAvailable;
    }

    public void ShowEnterCode()
    {
        Dropdown d = DropDownFader.GetComponent<Dropdown>();
        if (d.options.Count == 1)
        {
            InputFieldFader.fadeToOpaque();
            useInputFieldCode = true;
        }
        else useInputFieldCode = false;
    }

    public void ShowSubmitButton()
    {
        if (CurrentEstimation < 1)
        {
            SubmitButton.fadeToOpaque();
        }
    }

    public void ShowSolveButton()
    {
        if(CurrentIsOwner && CurrentEstimation > 0)
        {
            SolveButton.fadeToOpaque();
        }
    }

    public void SetUpEstimation()
    {
        YourEstimationAmount.GetComponent<Text>().text = CurrentEstimation + " garbanzos";
    }

    public void ChoosePath()
    {
        if(CurrentEstimation < 1)
        {
            goTo("PartII-a");
        }
        else
        {
            goTo("PartII-b");
        }
    }

    public void initializeDropdown()
    {
        Dropdown d = DropDownFader.GetComponent<Dropdown>();
        d.options.Clear();
        int firstColor = -1;
        for(int i = 0; i < LoadSaveController.persist_virtualBottleGames.games.Count; ++i)
        {
            string code = LoadSaveController.persist_virtualBottleGames.games[i].code;
            int color = LoadSaveController.persist_virtualBottleGames.games[i].bottleType - 1;
            if (i == 0) firstColor = color;
            d.options.Add(new Dropdown.OptionData(code, sprites[color]));
        }
        d.options.Add(new Dropdown.OptionData("Otro..."));
        d.value = 0;
        d.captionText.text = d.options[0].text;
        if (firstColor > -1)
        {
            d.captionImage.sprite = sprites[firstColor];
        }

    }

    public void doShareTip()
    {
        shareTip.go();
    }

    public void doEstimationTip()
    {
        estimationTip.go();
    }

public void SetBottleTypeOnServer()
    {
        StartCoroutine(SetBottleTypeOnServerCoRoutine());
    }

    public void GetNewGroupIDFromServer()
    {
        StartCoroutine(GetUniqueIndexFromServer());
    }

    public void GetEstimationsFromServer()
    {
        personsEstimationsController.SetEnabled(true);
        loadVotesCoRo = GetNumberOfAnswersForGroupID();
        StartCoroutine(loadVotesCoRo);
    }

    public void ShowChosenBottle()
    {
        BottleFader[chosenBottle - 1].Start();
        BottleFader[chosenBottle - 1].fadeToOpaque();
    }

    public void HideChosenBottle()
    {
        if (chosenBottle != -1)
        {
            BottleFader[chosenBottle - 1].Start();
            BottleFader[chosenBottle - 1].fadeToTransparent();
        }
    }

    public void EnableBackdrops()
    {
        foreach(GameObject go in bottleBackdrops)
        {
            go.SetActive(true);
        }
    }

    public void DisableBackdrops()
    {
        foreach (GameObject go in bottleBackdrops)
        {
            go.SetActive(false);
        }
    }

    public void clickOnBottle(int id)
    {
        CurrentGroupID = groupCodeLabel.text = "";
        SoundController.PlaySound(clickOnBottleSound);
        iconPressEnabler.DisabeAllButtons();
        chosenBottle = id;
        goTo("GetCodeFromServerAndUploadBottleTypeToServer");
        mustPersist = true;
        CurrentIsOwner = true;
    }

    public void clickOnBottle_1()
    {
        clickOnBottle(1);
    }

    public void clickOnBottle_2()
    {
        clickOnBottle(2);
    }

    public void clickOnBottle_3()
    {
        clickOnBottle(3);
    }

    public void clickOnBottle_4()
    {
        clickOnBottle(4);
    }

    public void deleteGroupFromList(string gr)
    {
        ListUtils.DeleteInList<VirtualBottleGame>
         (LoadSaveController.persist_virtualBottleGames.games,
             (x => x.code == gr));

        LoadSaveController.SaveVirtualGames();

    }

    IEnumerator GetNumberOfAnswersForGroupID()
    {
        WWW www;
        bool done = false;
        int nRetries = 0;
        while (1<2)
        {
            yield return (www = new WWW(Config.ServerURL + "/virtual/votes?groupid="+CurrentGroupID, null, Config.Headers));
            if (www.error == null)
            {

                if (www.text.ToLower().Contains("not"))
                {
                    goTo("CodeNotFound");
                    done = true;
                }
                else
                {

                    VotesResult res = JsonUtility.FromJson<VotesResult>(www.text);

                    if (CurrentIsDeleted == false && res.deleted > 0)
                    {
                        deleteGroupFromList(CurrentGroupID);
                        if (!CurrentIsOwner)
                        {
                            StartCoroutine(DeleteGroupFromServerCoRo());
                        }
                        InputFieldFader.GetComponent<InputField>().text = "";
                        CurrentIsDeleted = true;
                    }
                    if (CurrentIsSolved == false && res.solved == true)
                    {
                        goTo("PartIII");
                    }
                    CurrentIsSolved = res.solved;
                    chosenBottle = res.type;
                    VotesDataFromServerAvailable = true;
                    CurrentVotes = res.votes;
                    NumberOfAnswersForGroupID = res.votes.Count;

                    personsEstimationsController.SetEstimations(NumberOfAnswersForGroupID);
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
    /*
    IEnumerator DeleteGroupFromServerCoRo()
    {
        UnityWebRequest req = UnityWebRequest.Delete(Config.ServerURL + "/virtual/bottle?groupid=" + CurrentGroupID);
        req.SetRequestHeader("psk", Config.PSK);
        yield return req.SendWebRequest();
        GroupHasBeenDeleted = true;
    }*/
    /*
    IEnumerator GetBottleTypeFromServerCoRo()
    {
        WWW www;

        bool done = false;
        int nRetries = 0;
        while (!done)
        {
            yield return (www = new WWW(Config.ServerURL + "/virtual/bottle?groupid="+CurrentGroupID, null, Config.Headers));
            if (www.error == null)
            {
                IntegerResult iRes;
                iRes = JsonUtility.FromJson<IntegerResult>(www.text);
                chosenBottle = iRes.result;
                bottleSavedOnServer = true;
                done = true;
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
        }
    }*/

    IEnumerator SetBottleTypeOnServerCoRoutine()
    {
        WWW www;
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("groupid", CurrentGroupID);
        wwwForm.AddField("type", chosenBottle);
        bool done = false;
        int nRetries = 0;
        while(!done)
        {
            string FullURL = Config.ServerURL + "/virtual/bottle";
            yield return (www = new WWW(FullURL, wwwForm.data, Config.Headers));
            if(www.error == null)
            {
                bottleSavedOnServer = true;
                done = true;
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
        }
    }

    public void AttemptAddLocalPersistence()
    {
        if (mustPersist)
        {
            LoadSaveController.persist_virtualBottleGames.games.Add
                     (new VirtualBottleGame
                     {
                         bottleType = chosenBottle,
                         code = CurrentGroupID,
                         ownwer = CurrentIsOwner,                
                         estimation = -1
                     });
            LoadSaveController.SaveVirtualGames();
        }
    }

    public void SetGroupSolvedOnServer()
    {
        StartCoroutine(SetGroupSolvedOnServerCoRo());
    }

    IEnumerator SetGroupSolvedOnServerCoRo()
    {
        byte[] body = System.Text.Encoding.UTF8.GetBytes("groupid=" + CurrentGroupID);
        UnityWebRequest newReq = UnityWebRequest.Put(Config.ServerURL + "/virtual/votes", body);
        newReq.SetRequestHeader("psk", Config.PSK);
        newReq.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return newReq.SendWebRequest();

    }

    IEnumerator GetUniqueIndexFromServer()
    {
        WWW www;
        int index;
        bool done = false;
        int nRetries = 0;
        while (!done)
        {
            string FullURL = Config.ServerURL + "/uniqueindex";
            yield return (www = new WWW(FullURL, null, Config.Headers));

            if (www.error == null)
            {
                index = JsonUtility.FromJson<UniqueIndex>(www.text).uniqueindex;
                done = true;
                groupCodeLabel.text = CodeGeneration.CodeFromIndex(index);
                CodeFader.fadeToOpaque();
                CurrentGroupID = groupCodeLabel.text;
                /*LoadSaveController.persist_virtualBottleGames.games.Add
                 (new VirtualBottleGame {
                     bottleType = chosenBottle,
                     code = CurrentGroupID,
                     ownwer = true, 
                     estimation = -1 });
                CurrentIsOwner = true;
                LoadSaveController.SaveVirtualGames();*/
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
        }

    }

    public bool NewGroupIDIsSet()
    {

        return CurrentGroupID != "";
    }

  

    bool useInputFieldCode = false;

   
    public bool bottleTypeIsSet()
    {
        return chosenBottle != -1;
    }

    public bool BottleTypeIsSavedOnServer()
    {
        return bottleSavedOnServer;
    }

    public void AddNewGameToPersistance()
    {
        LoadSaveController.persist_virtualBottleGames.games.Add
                 (new VirtualBottleGame
                 {
                     bottleType = chosenBottle,
                     code = CurrentGroupID,
                     ownwer = false,
                     estimation = -1
                 });
        CurrentIsOwner = false;
        LoadSaveController.SaveVirtualGames();
    }

    public void clickOnDelete()
    {
        StartCoroutine(DeleteGroupFromServerCoRo());

        deleteGroupFromList(CurrentGroupID);

        //goTo("Return");
        GoBackController.GetSingleton().GoBack();
    }

    bool clickOnNextLock = false;

    public void clickOnNext()
    {
        if (clickOnNextLock == true) return;
        clickOnNextLock = false;
        if (chosenBottle != -1) return;
        bottleSavedOnServer = true;
        if (useInputFieldCode)
        {
            CurrentGroupID = InputFieldFader.GetComponent<InputField>().text;
            CurrentIsOwner = false;
            mustPersist = true;
        }
        else
        {
            CurrentGroupID = DropDownFader.GetComponent<Dropdown>().captionText.text;
            mustPersist = false;
        }
        groupCodeLabel.text = CurrentGroupID;
        CodeFader.fadeToOpaque();
        VirtualBottleGame g = ListUtils.FindOneInList<VirtualBottleGame>
        (LoadSaveController.persist_virtualBottleGames.games,
            (x => x.code == CurrentGroupID));
        if (g != null)
        {
            CurrentEstimation = g.estimation;
            chosenBottle = g.bottleType;
            CurrentIsOwner = g.ownwer;
            if (g.solved)
            {
                GetEstimationsFromServer();
                goTo("PartIII");
            }
            else goTo("PartII");
        }
        else
        {
            chosenBottle = -1;
            goTo("PartII");
            //StartCoroutine(GetBottleTypeFromServerCoRo());
        }
       

    }

    public void solve()
    {
        UIController.GetSingleton().VirtualEstimatorPartIIIState();

        HistogramData data = StatsUtils.Histogram(CurrentVotes, -1);
        VirtualBottleGame g = ListUtils.FindOneInList<VirtualBottleGame>
        (LoadSaveController.persist_virtualBottleGames.games,
            (x => x.code == CurrentGroupID));
        if (g != null)
        {

            LoadSaveController.SaveVirtualGames();
        }
        int ActualAmount = ActualAmountOfGarbanzos[chosenBottle];
        float Average = StatsUtils.Average(CurrentVotes);
        float AverageIndividualError = StatsUtils.GetAverageIndividualError(CurrentVotes, ActualAmount);
        float IndividualErrorPercentage = StatsUtils.ErrorToPercetage(ActualAmount, AverageIndividualError);
        float OverallError = Mathf.Abs((float)ActualAmount - Average);
        float OverallErrorPercentage = StatsUtils.ErrorToPercetage(ActualAmount, OverallError);
        int IntegerErrorPercentage = Mathf.FloorToInt(IndividualErrorPercentage);
        int IntegerOverallPercentage = Mathf.FloorToInt(OverallErrorPercentage);
        totalAverageLabel.GetComponent<Text>().text = "Promedio: " + StatsUtils.Average(CurrentVotes) + " garbanzos";
        averageIndividualErrorLabel.GetComponent<Text>().text = "Error individual promedio: " + IntegerErrorPercentage + "%";
        actualLabel.GetComponent<Text>().text = "Cantidad real: " + ActualAmountOfGarbanzos[chosenBottle] + " garbanzos";
        overallErrorLabel.GetComponent<Text>().text = "Error colectivo: " + IntegerOverallPercentage + "%";
        if (data.binCount.Count >= 3)
        {
            HistogramRenderer.SetHistogram(data);
            HistogramRenderer.Grow();
            HistogramRenderer.GetComponent<UIGeneralFader>().fadeToOpaque();
        }
        totalAverageLabel.fadeToOpaque();
        averageIndividualErrorLabel.fadeToOpaque();
        actualLabel.fadeToOpaque();
        overallErrorLabel.fadeToOpaque();
        if (CurrentIsOwner)
        {
            DeleteButton.fadeToOpaque();
        }

    }

    IEnumerator DeleteGroupFromServerCoRo()
    {

        UnityWebRequest req = UnityWebRequest.Delete(Config.ServerURL + "/virtual/bottle?groupid=" + CurrentGroupID);
        req.SetRequestHeader("psk", Config.PSK);

       
        bool done = false;
        while (!done)
        {

            yield return req.SendWebRequest();
            if (req.error == null)
            {
                done = true;
            }
            else
            {
                yield return new WaitForSeconds(20.0f);
            }
        }
    }

    public void clickOnSolve()
    {
        VirtualBottleGame g = ListUtils.FindOneInList<VirtualBottleGame>
        (LoadSaveController.persist_virtualBottleGames.games,
            (x => x.code == CurrentGroupID));
        if (g != null)
        {
            if(g.solved == false)
            {
                bool win = AmIWinner();
                if (win)
                {
                    GameObject winObj = (GameObject)Instantiate(winPrefab);
                    winObj.transform.SetParent(winSpawnLocation);
                    winObj.transform.localScale = Vector3.one;
                    winObj.transform.localPosition = Vector3.zero;
                    IntuitionPointsController.AddScoreAndSave(150);
                }
            }
            g.solved = true;
        }
        LoadSaveController.SaveVirtualGames();
        SetGroupSolvedOnServer();
        goTo("PartIII");
    }

    IEnumerator SendVoteToServerCoRo()
    {
        WWW www;
        bool done = false;
        int nRetries = 0;
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("groupid", CurrentGroupID);
        wwwForm.AddField("amount", AmountInputField.GetComponent<InputField>().text);
        int ce;
        int.TryParse(AmountInputField.GetComponent<InputField>().text, out ce);
        CurrentEstimation = ce;

        while (!done)
        {
            yield return (www = new WWW(Config.ServerURL + "/virtual/votes", wwwForm.data, Config.Headers));
            if(www.error != null)
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
        int estimation;
        int.TryParse(AmountInputField.GetComponent<InputField>().text, out estimation);
        VirtualBottleGame g = ListUtils.FindOneInList<VirtualBottleGame>
        (LoadSaveController.persist_virtualBottleGames.games,
            (x => x.code == CurrentGroupID));
        if (g != null)
        {
            g.estimation = estimation;
            LoadSaveController.SaveVirtualGames();
        }
    }

    public bool validateInput(string txt)
    {
        int amount;
        return (int.TryParse(txt, out amount) && amount > 0);
    }

    bool voteSubmittedLock = false;

    public void clickOnSubmit()
    {
        if (voteSubmittedLock) return;
        voteSubmittedLock = true;
        if (validateInput(AmountInputField.GetComponent<InputField>().text))
        {
            StartCoroutine(SendVoteToServerCoRo());
            goTo("VoteRegistration");
        }
    }

    public void OnDropDownChange(Dropdown v)
    {
        if(v.value == v.options.Count-1)
        {
            InputFieldFader.fadeToOpaque();
            useInputFieldCode = true;
        }
        else
        {
            InputFieldFader.fadeToTransparent();
            if (chosenBottle == -1)
            {
                CurrentGroupID = v.options[v.value].text;
                useInputFieldCode = false;
            }
        }
    }

    public bool AmIWinner()
    {
        int actual = ActualAmountOfGarbanzos[chosenBottle];
        int minDiff = 1000000;
        int closestEstimate = -1;
        for(int i = 0; i < CurrentVotes.Count; ++i)
        {
            if(Mathf.Abs(actual - CurrentVotes[i]) < minDiff)
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
