using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UniqueIndex
{
    public int uniqueindex;
}

public class CodeGeneration : MonoBehaviour
{

    static char[] Runes = new char[] { 'Q', '6', 'A', 'J', 'B', 'W', '9', '8', 'H', 'G', '3', 'T',
    'P', 'K', 'R', 'Y', '7', 'U', 'D', 'M' };

    static long MaxMod = 64000000;
    static long stride = 22064069;

    public static string CodeFromIndex(int index)
    {
        return CodeFromSeed(SeedFromIndex(index));
    }

    public static long SeedFromIndex(int index)
    {
        long offset = 0;
        for(int i = 0; i < index; ++i) 
        {
            offset += stride;
            offset = offset % MaxMod;
        }
        return offset;
    }

    public static string CodeFromSeed(long seed)
    {
        string result = "";
        result += Runes[seed % 20];
        seed = seed / 20;
        result += Runes[(seed+1) % 20];
        seed = seed / 20;
        result += Runes[(seed+2) % 20];
        seed = seed / 20;
        result += Runes[(seed+3) % 20];
        seed = seed / 20;
        result += Runes[(seed+4) % 20];
        seed = seed / 20;
        result += Runes[(seed+5) % 20];
        seed = seed / 20;
        return result;
    }

}
