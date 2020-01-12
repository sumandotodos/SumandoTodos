using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoadController : MonoBehaviour
{

    public string Directory;
    public int padding;
    public string prefix;
    public string suffix;

    public Sprite GetImage(string path)
    {
        Sprite newSpr = Resources.Load<Sprite>(path);
        return newSpr;
    }

    public Sprite GetImage(int index)
    {
        return GetImage(nameFromIndex(index));
    }

    public string nameFromIndex(int index)
    {
        string paddedIndex = "" + index;
        while (paddedIndex.Length < padding) paddedIndex = "0" + paddedIndex;
        return Directory + "/" + prefix + paddedIndex + suffix;
     }
}
