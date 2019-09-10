using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRange
{
    public Dictionary<Edge, FrameWallRange> FrameWallRanges {get;}
    public List<Edge> ForcedEdges {get{
        List<Edge> forcedEdges = new List<Edge>();
        foreach(KeyValuePair<Edge, FrameWallRange> item in FrameWallRanges)
            if(item.Value.HasForcedEdge) forcedEdges.Add(item.Key);
        return forcedEdges;
    }}

    public FrameRange(Dictionary<Edge, FrameWallRange> frameWallRanges)
    {
        foreach(Edge edge in EdgeExtensions.AllEdges())
        {
            if(!frameWallRanges.ContainsKey(edge))
                throw new ArgumentException("Missing framewall range for an edge");
        }
        FrameWallRanges = frameWallRanges;
    }

    public bool Contains(Frame frame)
    {
        foreach(KeyValuePair<Edge, FrameWall> item in frame.FrameWalls)
            if (!FrameWallRanges[item.Key].Contains(item.Value)) return false;
        return true;
    }

    public FrameRange Intersection(FrameRange otherFrameRange)
    {
        Dictionary<Edge, FrameWallRange> frameWallRanges = new Dictionary<Edge, FrameWallRange>();
        foreach(KeyValuePair<Edge, FrameWallRange> item in FrameWallRanges)
            frameWallRanges[item.Key] = item.Value.Intersection(otherFrameRange.FrameWallRanges[item.Key]);
        return new FrameRange(frameWallRanges);
    }

    public FrameRange WithOptionalEntriesRemoved()
    {
        Dictionary<Edge, FrameWallRange> frameWallRanges = new Dictionary<Edge, FrameWallRange>();
        foreach(KeyValuePair<Edge, FrameWallRange> item in FrameWallRanges)
            frameWallRanges[item.Key] = item.Value.HasForcedEdge ? item.Value : item.Value.WithEntryRemoved();
        return new FrameRange(frameWallRanges);
    }

    // public FrameRange WithOptionalEntriesForced()
    // {
    //     Dictionary<Edge, FrameWallRange> frameWallRanges = new Dictionary<Edge, FrameWallRange>();
    //     foreach(KeyValuePair<Edge, FrameWallRange> item in FrameWallRanges)
    //         frameWallRanges[item.Key] = item.Value.HasForcedEdge ? item.Value : item.Value.WithEntryForced();
    //     return new FrameRange(frameWallRanges);
    // }

    public Range LengthRange(Orientation ori)
    {
        Range lengthRange = Range.All;
        foreach(Edge edge in ori.GetEdges())
        {
            lengthRange = lengthRange.Intersection(FrameWallRanges[edge].LengthRange);
        }
        return lengthRange;
    }

    public Frame RandomFrame(System.Random random)
    {
        int horizontalLength = LengthRange(Orientation.Horizontal).RandomElement(random);
        int verticalLength = LengthRange(Orientation.Vertical).RandomElement(random);
        //Debug.Log(horizontalLength+ " "+ verticalLength+ "" +this);
        return new Frame(new Dictionary<Edge, FrameWall>(){
            {Edge.Left, FrameWallRanges[Edge.Left].RandomFrameWall(random, verticalLength)},
            {Edge.Right, FrameWallRanges[Edge.Right].RandomFrameWall(random, verticalLength)},
            {Edge.Top, FrameWallRanges[Edge.Top].RandomFrameWall(random, horizontalLength)},
            {Edge.Bottom, FrameWallRanges[Edge.Bottom].RandomFrameWall(random, horizontalLength)}
        });
    }

    public bool IsEmpty { get {
        foreach(KeyValuePair<Edge, FrameWallRange> item in FrameWallRanges)
            if (item.Value.IsEmpty) return true;
        return false;
    }}

    public override string ToString()
    {
        string str = "";
        foreach (KeyValuePair<Edge, FrameWallRange> item in FrameWallRanges)
        {
            str += item.Key + ": " + item.Value + "\n";
        }
        str = str.Substring(0, str.Length-1);
        return str;
    }

    public static FrameRange Empty = new FrameRange(new Dictionary<Edge, FrameWallRange>()
    {
        {Edge.Left, FrameWallRange.Empty},
        {Edge.Right, FrameWallRange.Empty},
        {Edge.Top, FrameWallRange.Empty},
        {Edge.Bottom, FrameWallRange.Empty}
    });

    public static FrameRange All = new FrameRange(new Dictionary<Edge, FrameWallRange>()
    {
        {Edge.Left, FrameWallRange.All},
        {Edge.Right, FrameWallRange.All},
        {Edge.Top, FrameWallRange.All},
        {Edge.Bottom, FrameWallRange.All}
    });
}
