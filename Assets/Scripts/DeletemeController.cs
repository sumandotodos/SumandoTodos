using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DeletemeController : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        byte[] body = System.Text.Encoding.UTF8.GetBytes("groupid=");
        UnityWebRequest newReq = UnityWebRequest.Put("localhost:9911/puttest", body);
        newReq.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        newReq.SendWebRequest();
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
