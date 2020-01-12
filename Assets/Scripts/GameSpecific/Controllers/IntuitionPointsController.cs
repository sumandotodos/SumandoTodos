using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class IntuitionPointsController : MonoBehaviour
        
{
    public static IntuitionPointsController instance;

    public static int score = 0;

    public Text scoreLabel;

    void Start()
    {
        instance = this;
        scoreLabel.text = "";
        StartCoroutine(LoadScoreFromServer());
    }

    IEnumerator LoadScoreFromServer()
    {
        WWW www;
        bool done = false;
        int nRetries = 0;
        while (!done)
        {
            yield return www = new WWW(Config.ServerURL + "/points/" + SystemInfo.deviceUniqueIdentifier, null, Config.Headers);
            if (www.error == null)
            {
                IntuitionPoints p = JsonUtility.FromJson<IntuitionPoints>(www.text);
                scoreLabel.text = "" + p.points;
                score += p.points;
                done = true;
            }
            else
            {

                if (nRetries == 1)
                {

                    NoConnectionController.NoConnectionWarning();
                }
                ++nRetries;
                yield return new WaitForSeconds(8.0f);
            }
        }
    }

    IEnumerator SaveScoreToServer()
    {
        bool done = false;
        int nRetries = 0;
        byte[] body = System.Text.Encoding.UTF8.GetBytes("newscore=" + score);
        UnityWebRequest newReq = UnityWebRequest.Put(Config.ServerURL + "/points/" + SystemInfo.deviceUniqueIdentifier, body);
        newReq.SetRequestHeader("psk", Config.PSK);
        newReq.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        while (!done)
        {
            yield return newReq.SendWebRequest();
            if(newReq.error == null)
            {
                done = true;
            }
            else
            {
                if (nRetries == 1)
                {

                    NoConnectionController.NoConnectionWarning();
                }
                ++nRetries;
                yield return new WaitForSeconds(8.0f);
            }
        }
    }

    public static void SaveScore()
    {
        instance.StartCoroutine(instance.SaveScoreToServer());
    }

    public static void AddScore(int sc)
    {
        score += sc;
        instance.scoreLabel.text = "" + score;
    }

    public static void AddScoreAndSave(int sc)
    {
        AddScore(sc);
        SaveScore();
    }

}
