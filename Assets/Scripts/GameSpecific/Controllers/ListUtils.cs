using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListUtils : MonoBehaviour
{
    public static T FindOneInList<T>(List<T> theList, System.Func<T, bool> f)
    {
        for (int i = 0; i < theList.Count; ++i)
        {
            if (f(theList[i])) return theList[i];
        }
        return default(T);
    }

    public static void DeleteInList<T>(List<T> theList, System.Func<T, bool> f)
    {
        for (int i = 0; i < theList.Count; ++i)
        {
            if (f(theList[i])) { theList.RemoveAt(i); --i; }
        }
    }


}