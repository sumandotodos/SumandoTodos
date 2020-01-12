using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostDragMessage : MonoBehaviour
{

    public UITextFader textFader;
    public UIMover mover;
    public float AbsSpeed;

    public float delay = 4.0f;

    public void SetText(string t)
    {
        textFader.GetComponent<Text>().text = t;
    }

    public void SetDirection(bool dir)
    {
        float factor = 1;
        if (dir == false) factor = -1;
        mover.ySpeed = AbsSpeed * factor;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        textFader.setSpeed(6.0f);
        textFader.fadeToOpaque();
        yield return new WaitForSeconds(0.35f);
        textFader.setSpeed(0.4f);
        textFader.fadeToTransparent();
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
