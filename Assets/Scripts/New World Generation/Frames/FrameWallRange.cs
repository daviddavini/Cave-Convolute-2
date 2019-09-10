using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct FrameWallRange
{
    public FrameWall FrameWallMin {get;}
    public FrameWall FrameWallMax {get;}
    public bool IsEmpty {get;}
    public bool HasForcedEdge {get{return FrameWallMin.HasEntry;}}

    public Range LengthRange {get{
        return new Range(FrameWallMin.Length, FrameWallMax.Length);
    }}

    public FrameWallRange(FrameWall frameWallMin, FrameWall frameWallMax, bool isEmpty = false)
    {
        FrameWallMin = frameWallMin;
        FrameWallMax = frameWallMax;
        IsEmpty = isEmpty;
        if(!IsValid()){
            //Debug.Log("Framewall min is not less than or equal to framewall max, setting empty");
            IsEmpty = true;
        }
        //throw new ArgumentException("Framewall min is not less than or equal to framewall max");
    }

    public bool IsValid()
    {
        return FrameWallMin.LessThanOrEqualTo(FrameWallMax);
    }

    public bool Contains(FrameWall frameWall)
    {
        if(IsEmpty) return false;
        return FrameWallMin.LessThanOrEqualTo(frameWall)
          && frameWall.LessThanOrEqualTo(FrameWallMax);
    }

    public FrameWallRange Intersection(FrameWallRange otherWallRange)
    {
        FrameWall newFrameWallMin = FrameWall.Max(FrameWallMin, otherWallRange.FrameWallMin);
        FrameWall newFrameWallMax = FrameWall.Min(FrameWallMax, otherWallRange.FrameWallMax);
        return new FrameWallRange(newFrameWallMin, newFrameWallMax);
    }

    public FrameWall RandomFrameWall(System.Random random, int length)
    {
        //Debug.Log("Length: " + length + this);
        //kinda inefficient, but only used for init so I guess okay...
        List<FrameWall> validFrameWalls = new List<FrameWall>();
        for(int preWall = FrameWallMin.PreWall; preWall <= FrameWallMax.PreWall; preWall++){
            for(int entry = FrameWallMin.Entry; entry <= FrameWallMax.Entry; entry++){
                for(int postWall = FrameWallMin.PostWall; postWall <= FrameWallMax.PostWall; postWall++){
                    if(preWall+entry+postWall == length)
                        validFrameWalls.Add(new FrameWall(preWall, entry, postWall));
                }
            }
        }
        //Debug.Log(FrameWallMin + "to " + FrameWallMax);
        //Debug.Log("Number of valid walls:" + validFrameWalls.Count);
        return validFrameWalls.RandomElement(random);
    }

    public FrameWallRange WithEntryRemoved()
    {
        return new FrameWallRange(FrameWallMin.WithEntryRemoved(), FrameWallMax.WithEntryRemoved());
    }

    // public FrameWallRange WithEntryForced()
    // {
    //     FrameWall newFrameWallMin = new FrameWall()
    //     return new FrameWallRange(FrameWallMin.WithEntry(), FrameWallMax.WithEntryRemoved());
    // }

    public override string ToString()
    {
        if(IsEmpty) return "FrameWallRange.Empty";
        return FrameWallMin.ToString() + " to " + FrameWallMax.ToString();
    }

    public static FrameWallRange All = new FrameWallRange(FrameWall.MinValue, FrameWall.MaxValue);
    public static FrameWallRange Empty = new FrameWallRange(FrameWall.MinValue, FrameWall.MinValue, true);
}
