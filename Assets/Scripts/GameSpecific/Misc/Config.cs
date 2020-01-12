using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static string ServerURL = "https://apps.flygames.org/imagine";
    public static string PSK = "31416";
    public static Dictionary<string, string> Headers = new Dictionary<string, string>();
    public static string Version = "V 1.0";
    public static void init()
    {
        Headers.Add("psk", PSK);
    }
    public static string PlayStoreLink = "https://play.google.com/store/apps/details?id=org.flygames.imagine&hl=es";
    public static string AppStoreLink = "https://itunes.apple.com/es/app/imagine/id1461743723?mt=8";


}
