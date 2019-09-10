using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T RandomElement<T>(this List<T> list, System.Random random)
    {
        if (list.Count == 0)
            throw new ArgumentException("Cannot get random element from empty list");
        return list[random.Next(0, list.Count)];
    }

    public static List<T> RandomElements<T>(this List<T> list, int n, System.Random random)
    {
        if (n > list.Count)
            throw new ArgumentException("Number of elements n is greater than list count");

        List<T> selected = new List<T>();
        int leftToSelect = n;
        for(int i = 0; i < list.Count; i++)
        {
            if (random.NextDouble() < (double)leftToSelect / (list.Count - i))
            {
                selected.Add(list[i]);
                leftToSelect--;
            }
        }
        return selected;
    }

    public static void Shuffle<T>(this List<T> list, System.Random random)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = random.Next(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
