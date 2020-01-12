using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShitController : MonoBehaviour
{

    public Text daShit;
    public Text daErrorz;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Config.init();
        string url = Config.ServerURL + "/healthcheck";
        daShit.text = url;
        yield return new WaitForSeconds(2.0f);
        WWW www;
        yield return (www = new WWW(url, null, Config.Headers));
        daShit.text = www.text;
        daErrorz.text = www.error;
        yield return new WaitForSeconds(2.0f);

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("groupid", "AAAA");
        wwwForm.AddField("amount", "888");
        url = Config.ServerURL + "/virtual/votes";
        daShit.text = url;
        yield return new WaitForSeconds(2.0f);
        yield return (www = new WWW(url, wwwForm.data, Config.Headers));
        daShit.text = www.text;
        daErrorz.text = www.error;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
