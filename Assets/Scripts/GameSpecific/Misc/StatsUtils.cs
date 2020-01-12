using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HistogramData
{
    public List<int> binCount = new List<int>();
    public List<int> binLowest = new List<int>();
    public int binSize = -1;
    public int min = 0;
    public int max = 0;
    public int maxCount;
}

public class StatsUtils : MonoBehaviour
{

    public static bool GetMinMax(List<int> r, out int min, out int max)
    {
        if(r.Count < 1)
        {
            min = max = 0;
            return false;
        }

        int _min = r[0], _max = r[0];
        for (int i = 1; i < r.Count; ++i)
        {
            if (r[i] < _min) _min = r[i];
            if (r[i] > _max) _max = r[i];
        }
        min = _min;
        max = _max;
        return true;
    }

    public static int NumerOfItemsInRange(List<int> r, int minRange, int maxRange)
    {
        int count = 0;
        for(int i = 0; i < r.Count; ++i)
        {
            if (r[i] >= minRange && r[i] <= maxRange) ++count;
        }
        return count;
    }

    public static bool HasEmptyBins(List<int> r, int binSize)
    {
        int min, max;
        GetMinMax(r, out min, out max);
        int range = max - min;
        int nBins = range / binSize;
        for(int i = 0; i < nBins; ++i)
        {
            if(NumerOfItemsInRange(r, min + i*binSize, min + (i+1)*binSize) == 0)
            {
                return true;
            }
        }
        return false;
    }

    public static int LongestZeroRun(List<int> r)
    {
        int count = 0;
        int thisRun = 0;
        bool zeroRun = false;
        int min, max;
        GetMinMax(r, out min, out max);
        for(int i = min; i <= max; ++i)
        {
            int NinRange = NumerOfItemsInRange(r, i, i);
            if (NinRange == 0 && zeroRun == false)
            {
                zeroRun = true;
                thisRun = 1;
            }
            else if (NinRange == 0 && zeroRun == true)
            {
                thisRun++;
            }
            else if (NinRange != 0 && zeroRun == true)
            {
                zeroRun = false;
                if (thisRun > count) count = thisRun;
            }
            else if (NinRange == 0 && i == max && zeroRun == true)
            {
                thisRun++;
                if (thisRun > count) count = thisRun;
            }
        }

        return count;
    }

    public static int AutoBinSize(List<int> r)
    {
        return LongestZeroRun(r) + 1;
    }

    public static int Average(List<int> r)
    {
        float res = 0.0f;
        for(int i = 0; i < r.Count; ++i)
        {
            res += ((float)r[i]);
        }
        res = res / ((float)r.Count);
        return (int)(res);
    }

    public static HistogramData Histogram(List<int> r, int binSize)
    {
        HistogramData result = new HistogramData();
        if(r.Count == 1)
        {
            result.binCount.Add(1);
            result.binLowest.Add(r[0]);
            result.binSize = 1;
            result.min = result.max = r[0];
            result.maxCount = 1;
            return result;
        }
        int maxCount = 0;
        if (binSize == -1)
        {
            if (r.Count == 2)
            {
                binSize = Mathf.Abs(r[1] - r[0]) / 2;
            }
            else
            {
                binSize = AutoBinSize(r);
            }
        }
        int min, max;
        GetMinMax(r, out min, out max);
        for (int i = min; i < max; i += binSize)
        {
            int NinRange = NumerOfItemsInRange(r, i, i + binSize);
            if (NinRange > maxCount) maxCount = NinRange;
            result.binCount.Add(NinRange);
            result.binLowest.Add(i);
        }
        result.min = min;
        result.max = max;
        result.binSize = binSize;
        result.maxCount = maxCount;
        return result;
    }

    public static HistogramData Histogram(ListOfResults r, int binSize)
    {
        return Histogram(r.result, -1);
    }

    // Start is called before the first frame update
    void Start()
    {



    }


}
