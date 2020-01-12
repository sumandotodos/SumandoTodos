using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HistogramComponent : MonoBehaviour
{
    public float width;
    public float height;
    List<Image> barsImages = new List<Image>();
    List<GameObject> barsObjects = new List<GameObject>();
    List<GameObject> barsLabels = new List<GameObject>();
    List<TweenableSoftFloat> barsHeights = new List<TweenableSoftFloat>();
    HistogramData histData;

    public void DestroyBars()
    {
        foreach (GameObject gmob in barsObjects)
        {
            Destroy(gmob);
        }
        foreach(GameObject lab in barsLabels)
        {
            Destroy(lab);
        }
        barsObjects = new List<GameObject>();
        barsLabels = new List<GameObject>();
        barsHeights = new List<TweenableSoftFloat>();
    }

    float barsMaxHeight;
    float barsWidth;

    public void SetNBars(int n)
    {
        DestroyBars();
        barsImages = new List<Image>();
        barsObjects = new List<GameObject>();
        barsHeights = new List<TweenableSoftFloat>();
        for (int i = 0; i < n; ++i)
        {

            float thisWidth = this.GetComponent<RectTransform>().sizeDelta.x;
            float thisHeight = this.GetComponent<RectTransform>().sizeDelta.y;
            float barWidthLocal = (float)100 / (float)n;
            float barWidthParent = ((float)thisWidth / (float)n);

            GameObject newLabel = new GameObject();
            newLabel.AddComponent<UITextFader>().opaqueColor = Color.white;
            newLabel.AddComponent<Text>();
            newLabel.GetComponent<Text>().text = ("" + histData.binLowest[i]);
            newLabel.GetComponent<Text>().font = Resources.Load<Font>("Fonts/expressway rg") as Font;
            newLabel.GetComponent<Text>().fontSize = 82;
            newLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
            newLabel.transform.SetParent(this.transform);
            newLabel.transform.localScale = Vector3.one;
            newLabel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            newLabel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            newLabel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            newLabel.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(-50.0f + barWidthParent * i, -210.0f);
            barsLabels.Add(newLabel);

            if (i == n - 1)
            {
                newLabel = new GameObject();
                newLabel.AddComponent<UITextFader>().opaqueColor = Color.white;
                newLabel.AddComponent<Text>();
                newLabel.GetComponent<Text>().text = ("" + histData.max);
                newLabel.GetComponent<Text>().font = Resources.Load<Font>("Fonts/expressway rg") as Font;
                newLabel.GetComponent<Text>().fontSize = 82;
                newLabel.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                newLabel.transform.SetParent(this.transform);
                newLabel.transform.localScale = Vector3.one;
                newLabel.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                newLabel.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                newLabel.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                newLabel.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(-50.0f + barWidthParent * n, -210.0f);
                barsLabels.Add(newLabel);
            }


            GameObject newBarGO = new GameObject();
            newBarGO.AddComponent<UIFader>();
            barsObjects.Add(newBarGO);
            barsImages.Add(newBarGO.AddComponent<Image>());
            newBarGO.GetComponent<UIFader>().Start();
            newBarGO.transform.SetParent(this.transform);
            newBarGO.transform.localScale = Vector3.one;



            newBarGO.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            newBarGO.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            newBarGO.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            newBarGO.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(barWidthParent * i, 0);
            barsMaxHeight = thisHeight;
            barsWidth = barWidthParent;
            newBarGO.GetComponent<RectTransform>().sizeDelta = new Vector2(barWidthParent, 0);
            TweenableSoftFloat newSF = new TweenableSoftFloat();
            newSF.setEaseType(EaseType.cubicOut);
            newSF.setSpeed(375.0f);
            newSF.setValue(0.0f);
            newSF.setValueImmediate(0.0f);
            barsHeights.Add(newSF);

        }

        this.GetComponent<UIGeneralFader>().refreshChildren();
    }

    public void SetBarsColor(Color c)
    {
        for(int i = 0; i < barsImages.Count; ++i)
        {
            barsImages[i].color = c;
            barsImages[i].GetComponent<UIFader>().opaqueColor = c;
        }
    }

    public void SetHistogram(HistogramData h)
    {
        histData = h;
        SetNBars(h.binCount.Count);
        SetBarsColor(Color.green);

    }

    public void Grow()
    {
        for(int i = 0; i < barsHeights.Count; ++i)
        {
            float f = ((float)histData.binCount[i] / (float)histData.maxCount) * barsMaxHeight;
            Debug.Log(f);
            barsHeights[i].setValue(f);
        }
    }

    void Update()
    {
        for(int i = 0; i < barsHeights.Count; ++i)
        {
            barsHeights[i].update();
            barsObjects[i].GetComponent<RectTransform>().sizeDelta = 
                new Vector2(barsWidth, barsHeights[i].getValue());
        }
    }
}
