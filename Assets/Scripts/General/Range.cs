using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//don't use default constructor!!

//WARNING: THIS IS AN INCLUSIVE RANGE

public readonly struct Range
{
    public int Min {get;}
    public int Max {get;}

    public Range(int min = 0, int max = int.MaxValue)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(int n)
    {
        return Min <= n && n <= Max;
    }

    public int RandomElement(System.Random random)
    {
        return random.Next(Min, Max+1);
    }

    public Range Intersection(Range other)
    {
        return new Range(Mathf.Max(Min, other.Min), Mathf.Min(Max, other.Max));
    }

    public override string ToString()
    {
        return string.Format("[{0} - {1}]", Min,
          (Max == int.MaxValue) ? "Inf" : Max.ToString());
    }

    public static Range All = new Range(0);
}
