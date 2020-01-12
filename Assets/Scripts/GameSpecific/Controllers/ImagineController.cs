using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VoteResult
{
    public int same_percentage;
}

public class ImagineController : FGProgram
{

    public AudioClip pageSound;
    public AudioClip voteSound;

    public GameObject postVoteMessagePrefab;
    public Transform topAnchor;
    public Transform bottomAnchor;
    public Transform parent; 

    bool ShowingBocadillo = true;

    public FGTable VIPTable;
    public FGTable DeclarationsTable;

    public Text VIPNameLabel;
    public Text VIPPositionLabel;
    public UITextFader VIPNameFader;
    public UITextFader VIPPositionFader;
    public Image VIPCaretoImage;
    public UIFader VIPCaretoFader;

    public Text DeclarationText;
    public UIScaleFader BocadilloScaler;
    public UIScaleFader PensadilloScaler;
    public Image ThoughtImage;

    public UIFader backgroundFader;

    public ImageLoadController peopleImageController;
    public ImageLoadController thoughtsImageController;

    public ShowArrowsController showArrowsController;
    public ImagineTipsController tipsController;

    public DragController dragController;

    int msgId = -1;
    int vote = 0;

    // Start is called before the first frame update
    void Start()
    {
        execute(this, "prepare");
        delay(0.5f);
        execute(backgroundFader, "fadeToOpaque");
        execute(showArrowsController, "run");
        waitForProgram(tipsController);
        execute(showArrowsController, "goTo", "Cancel");
        programGoTo("ShowImagine");

        createSubprogram("ShowImagine");
        execute(VIPCaretoFader, "fadeToOpaque");
        delay(0.25f);
        execute(VIPNameFader, "fadeToOpaque");
        delay(0.25f);
        execute(VIPPositionFader, "fadeToOpaque");
        delay(0.5f);
        execute(BocadilloScaler, "scaleIn");
        delay(0.5f);
        execute(this, "SetIneractEnabled", true);

        createSubprogram("NextImagine");
        execute(VIPCaretoFader, "fadeToTransparent");
        execute(VIPNameFader, "fadeToTransparent");
        waitForTask(VIPPositionFader, "fadeToTransparentTask", this);
        execute(this, "prepare");
        delay(1.5f);
        programGoTo("ShowImagine");

        createSubprogram("ToPensadillo");
        execute(this, "BocadilloCubicIn");
        waitForTask(BocadilloScaler, "scaleOutTask", this);
        execute(this, "PensadilloCubicOut");
        execute(PensadilloScaler, "scaleIn");

        createSubprogram("ToBocadillo");
        execute(this, "PensadilloCubicIn");
        waitForTask(PensadilloScaler, "scaleOutTask", this);
        execute(this, "BocadilloCubicOut");
        execute(BocadilloScaler, "scaleIn");

        createSubprogram("Return");
        execute(backgroundFader, "fadeToTransparent");
        execute(VIPCaretoFader, "fadeToTransparent");
        execute(VIPNameFader, "fadeToTransparent");
        execute(VIPPositionFader, "fadeToTransparent");
        execute(this, "dismissBocadillos");
        delay(1.5f);
        programNotifyFinish();
    }

    public void dismissBocadillos()
    {
        BocadilloScaler.setEaseType(EaseType.cubicIn);
        PensadilloScaler.setEaseType(EaseType.cubicIn);
        BocadilloScaler.scaleOut();
        PensadilloScaler.scaleOut();

    }

    public void SpawnPostVoteMessage(int percentage, bool direction)
    {
        GameObject newMessage = (GameObject)Instantiate(postVoteMessagePrefab);
        newMessage.GetComponent<PostDragMessage>().SetText(percentage + "% ha imaginado lo mismo que tú");
        newMessage.GetComponent<PostDragMessage>().SetDirection(direction);
        newMessage.transform.SetParent(parent);
        newMessage.transform.localScale = Vector3.one * 0.2f;
        if (direction)
        {
            newMessage.transform.localPosition = bottomAnchor.transform.localPosition;
        }
        else
        {
            newMessage.transform.localPosition = topAnchor.transform.localPosition;
        }
    }

    IEnumerator SetAndGetVote()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("msgid", msgId);
        wwwForm.AddField("vote", vote);

        WWW www;
        yield return www = new WWW(Config.ServerURL + "/imagine/votes", wwwForm.data, Config.Headers);

        if (www.error == null)
        {
            VoteResult vr = JsonUtility.FromJson<VoteResult>(www.text);
            SoundController.PlaySound(voteSound);
            SpawnPostVoteMessage(vr.same_percentage, vote == -1);
        }

    }

    public void SubmitVote(int v)
    {
        vote = v;
        StartCoroutine(SetAndGetVote());
    }

    public void BocadilloCubicIn()
    {
        BocadilloScaler.SetSpeed(3.7f);
        BocadilloScaler.setEaseType(EaseType.cubicIn);
    }
    public void BocadilloCubicOut()
    {
        BocadilloScaler.SetSpeed(3.7f);
        BocadilloScaler.setEaseType(EaseType.cubicOut);
    }
    public void PensadilloCubicOut()
    {
        PensadilloScaler.SetSpeed(3.7f);
        PensadilloScaler.setEaseType(EaseType.cubicOut);
    }
    public void PensadilloCubicIn()
    {
        PensadilloScaler.SetSpeed(3.7f);
        PensadilloScaler.setEaseType(EaseType.cubicIn);
    }

    public void prepare()
    {
        SetIneractEnabled(false);
        vote = 0;
        GetNextVIP();
        GetNextDeclaration();
        dragController.Reset();
        BocadilloScaler.Start();
        BocadilloScaler.scaleOutImmediately();
        BocadilloScaler.setEaseType(EaseType.boingOutMore);
        BocadilloScaler.SetSpeed(0.7f);

    }

    public void GetNextVIP() 
    {

        int row = VIPTable.getNextRowIndex();
        string vipname = ((string)VIPTable.getElement("nombre", row)).ToUpper();
        string vipposition = ((string)VIPTable.getElement("cargo", row)).ToUpper();
        int nImage = (int)VIPTable.getElement("nimagen", row);

        Sprite vipface = peopleImageController.GetImage(nImage);

        if (VIPCaretoImage.sprite != null)
        {
            if (VIPCaretoImage.sprite.texture != null)
                DrainResource(VIPCaretoImage.sprite.texture);
        }
        VIPCaretoImage.sprite = vipface;
        VIPCaretoFader.Start();
        VIPCaretoFader.fadeToTransparentImmediately();

        VIPNameLabel.text = vipname;
        VIPPositionLabel.text = vipposition;

        VIPNameFader.Start();
        VIPNameFader.fadeToTransparentImmediately();

        VIPPositionFader.Start();
        VIPPositionFader.fadeToTransparentImmediately();

    }

    private void DrainResource(Texture tex)
    {
        if(tex!=null)
        {
            Resources.UnloadAsset(tex);
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }

    public void GetNextDeclaration()
    {

        int row = DeclarationsTable.getNextRowIndex();

        msgId = row;

        string text = (string)DeclarationsTable.getElement("texto", row);
        int nImage = (int)DeclarationsTable.getElement("nimagen", row);

        Sprite thought = thoughtsImageController.GetImage(nImage);

        float width = thought.rect.width;
        float height = thought.rect.height;
        float aspect = width / height;

        if(aspect > 1.0f)
        {
            ThoughtImage.transform.localScale = new Vector3(1.0f, 1.0f / aspect, 1.0f);
        }
        else
        {
            ThoughtImage.transform.localScale = new Vector3(aspect, 1.0f, 1.0f);
        }

        DeclarationText.text = text;
        if (ThoughtImage.sprite != null)
        {
            if (ThoughtImage.sprite.texture != null)
                DrainResource(ThoughtImage.sprite.texture);
        }
        ThoughtImage.sprite = thought;

    }

    public void SwitchBocadilloPensadillo()
    {
        if (!interactable) return;

        if(mustPreventSwitch)
        {
            mustPreventSwitch = false;
            return;
        }

        if (ShowingBocadillo)
        {
            goTo("ToPensadillo");
        }
        else
        {
            goTo("ToBocadillo");
        }
        ShowingBocadillo = !ShowingBocadillo;
    }

    bool mustPreventSwitch = false;

    public void CancelSwitch()
    {
        mustPreventSwitch = true;
    }

    public void NextImagine()
    {
        SoundController.PlaySound(pageSound);
        SetIneractEnabled(false);
        if (!ShowingBocadillo)
        {
            PensadilloScaler.setEaseType(EaseType.cubicIn);
            PensadilloScaler.scaleOut();
            BocadilloScaler.setEaseType(EaseType.cubicOut);
            BocadilloScaler.scaleIn();
        }
        goTo("NextImagine");
    }

    public bool interactable = false;
    public void SetIneractEnabled(bool en)
    {
        interactable = en;
        dragController.SetInteractEnabled(en);
    }
}
