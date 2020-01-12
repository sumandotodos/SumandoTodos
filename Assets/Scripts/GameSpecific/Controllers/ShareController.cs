using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShareController : FGProgram
{
    public static ShareController instance;

    public UITextFader explainFader;
    public UIGeneralFader buttonFader;
    public UIGeneralFader dropdownFader;
    public Text debug_N;
    public Sprite[] sprites;

    public string ForceCode = "";

    public static void SetForceCode(string code)
    {
        instance.ForceCode = code;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        execute(this, "initialize");
        delay(0.5f);
        execute(explainFader, "fadeToOpaque");
        delay(0.5f);
        execute(dropdownFader, "fadeToOpaque");
        delay(1.0f);
        execute(buttonFader, "fadeToOpaque");

        createSubprogram("Return");
        execute(explainFader, "fadeToTransparent");
        execute(dropdownFader, "fadeToTransparent");
        execute(buttonFader, "fadeToTransparent");
        delay(2.0f);
        programNotifyFinish();

    }

    public void initialize()
    {
        explainFader.Start();
        explainFader.fadeToTransparentImmediately();
        buttonFader.Start();
        buttonFader.fadeToTransparentImmediately();
        dropdownFader.Start();
        dropdownFader.fadeToTransparentImmediately();
        initializeDropdown();
        MainScreenController.SetReturnToShare(false);

    }


    public void initializeDropdown()
    {
        Dropdown d = dropdownFader.GetComponent<Dropdown>();
        d.options.Clear();
        int firstColor = -1;
        for (int i = 0; i < LoadSaveController.persist_virtualBottleGames.games.Count; ++i)
        {
            string code = LoadSaveController.persist_virtualBottleGames.games[i].code;
            int color = LoadSaveController.persist_virtualBottleGames.games[i].bottleType - 1;
            if (i == 0) firstColor = color;
            d.options.Add(new Dropdown.OptionData(code, sprites[color]));
        }
        d.options.Add(new Dropdown.OptionData("Ninguno..."));
        d.value = 0;
        if (ForceCode != "")
        {
            int found = -1;
            for(int i = 0; i < d.options.Count; ++i)
            {
                if(d.options[i].text == ForceCode)
                {
                    found = i;
                    break;
                }
            }
            if (found != -1)
            {
                int color = LoadSaveController.persist_virtualBottleGames.games[found].bottleType - 1;
                d.captionText.text = d.options[found].text;
                d.captionImage.sprite = sprites[color];
            }
            ForceCode = "";
        }
        else
        {
            d.captionText.text = d.options[0].text;
            if (firstColor > -1)
            {
                d.captionImage.sprite = sprites[firstColor];
            }
        }

    }

    public void clickOnShare()
    {
        NativeShare ns = new NativeShare();
        //string imagePath = Application.dataPath + "/Assets/Resources/logo.png";
        string code = dropdownFader.GetComponent<Dropdown>().captionText.text;
        if(code == "Ninguno...")
        {

            ns.SetText("¡Juega con la app que te permitirá imaginar un mundo mejor! Para Android: "+Config.PlayStoreLink+"  Para iOS: "+Config.AppStoreLink);
        }
        else
        {
           
           
            //ns.SetTitle("Sumando todos");
            ns.SetText("¡Te han invitado a jugar al juego de la botella virtual! Usa el código: " + code + " Para Android: "+Config.PlayStoreLink+"  Para iOS: " + Config.AppStoreLink);
        }
        ns.Share();
        IntuitionPointsController.AddScore(15);
        IntuitionPointsController.SaveScore();
    }
}
