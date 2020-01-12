using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum NoConnectionState { connected, disconnected };

public class NoConnectionController : MonoBehaviour
{

    NoConnectionState state = NoConnectionState.connected;
    public UITip NoConnectionTip;

    public static NoConnectionController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static void NoConnectionWarning()
    {
        instance.StartNoConnectionRoutine();
    }

    public void StartNoConnectionRoutine()
    {
        NoConnectionTip.go();
        StartCoroutine(AttemptHealthcheck());
    }

    IEnumerator AttemptHealthcheck()
    {
        bool finish = false;
        while(!finish)
        {
            yield return new WaitForSeconds(4.0f);
            WWW www;
            yield return www = new WWW(Config.ServerURL + "/healthcheck", null, Config.Headers);
            if (www.error == null)
            {
                finish = true;
                NoConnectionTip.dismiss();
            }
        }
    }
}
