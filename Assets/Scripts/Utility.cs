using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static List<int> GetRandomInts(int length, int minValue, int maxValue)
    {
        List<int> result = new List<int>(length);
        while (result.Count < length)
        {
            int value = Random.Range(minValue, maxValue);
            if (result.Contains(value) == false)
                result.Add(value);
        }
        return result;
    }
}
