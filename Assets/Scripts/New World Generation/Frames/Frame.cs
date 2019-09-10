using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame
{
    //private readonly Dictionary<Edge, FrameWall> frameWalls;
    public Dictionary<Edge, FrameWall> FrameWalls {get;} //{return frameWalls;}}

    //this? or with get private set properties?
    public int Width {get;}
    public int Height {get;}

    public int GetDimension(Orientation ori)
    {
        return ori == Orientation.Horizontal ? Width : Height;
    }

    //private readonly List<Edge> entryEdges;  //my way of making a private set list
    public List<Edge> EntryEdges {get;} //{return entryEdges;}}

    public int EntryCount {get;}// {return entryEdges.Count;}}

    public Frame(Dictionary<Edge, FrameWall> frameWalls)
    {
        foreach(Edge edge in EdgeExtensions.AllEdges())
        {
            if(!frameWalls.ContainsKey(edge))
                throw new ArgumentException("Missing framewall for an edge");
        }
        if(frameWalls[Edge.Left].Length != frameWalls[Edge.Right].Length)
            throw new ArgumentException("Parallel vertical framewalls have different Lengths");
        if(frameWalls[Edge.Bottom].Length != frameWalls[Edge.Top].Length)
            throw new ArgumentException("Parallel horizontal framewalls have different Lengths");
        FrameWalls = frameWalls;

        Width = frameWalls[Edge.Bottom].Length;
        Height = frameWalls[Edge.Left].Length;

        EntryEdges = new List<Edge>();
        foreach(KeyValuePair<Edge, FrameWall> item in frameWalls)
        {
            if (item.Value.HasEntry)
                EntryEdges.Add(item.Key);
        }
        EntryCount = EntryEdges.Count;
    }

    //my edge
    public FrameRange FrameRangeOff(Edge edge)
    {
        FrameWall wall = FrameWalls[edge];
        if(!wall.HasEntry) return FrameRange.Empty;

        Dictionary<Edge, FrameWallRange> frameWallRanges = new Dictionary<Edge, FrameWallRange>();
        frameWallRanges[edge.Opposite()] = new FrameWallRange(
          new FrameWall(0, wall.Entry, 0),
          new FrameWall(int.MaxValue, wall.Entry, int.MaxValue)
        );
        foreach(Edge missingEdge in EdgeExtensions.AllEdges())
        {
            if(!frameWallRanges.ContainsKey(missingEdge))
                frameWallRanges[missingEdge] = FrameWallRange.All;
        }

        return new FrameRange(frameWallRanges);
    }

    //my edge
    public bool CompatibleWith(Edge edge, Frame frame)
    {
        return FrameRangeOff(edge).Contains(frame);
    }

    public override string ToString()
    {
        string str = "";
        foreach (Edge edge in EdgeExtensions.AllEdges())
        {
            str += edge + ": " + FrameWalls[edge] + "\n";
        }
        str = str.Substring(0, str.Length-1);
        return str;
    }

    public Vector2 EdgeDelta(Edge edge)
    {
        switch (edge)
        {
        case Edge.Left: return Vector2.up;
        case Edge.Right: return Vector2.up;
        case Edge.Top: return Vector2.right;
        case Edge.Bottom: return Vector2.right;
        default: return Vector2.zero;
        }
    }

    public Vector2 EdgeStart(Edge edge)
    {
        switch (edge)
        {
        case Edge.Left: return Vector2.zero;
        case Edge.Right: return new Vector2(Width, 0);
        case Edge.Top: return new Vector2(0, Height);
        case Edge.Bottom: return Vector2.zero;
        default: return Vector2.zero;
        }
    }

    public Vector2 EntryPosition(Edge edge)
    {
        return EdgeStart(edge) + EdgeDelta(edge) * (FrameWalls[edge].PreWall + FrameWalls[edge].Entry/2.0f);
    }

    public Vector2 CenterToEntry(Edge edge)
    {
        //TODO: make sure frame wall is nice and encapsulated, for easy change
        FrameWall frameWall = FrameWalls[edge];
        Vector2 entryOffset = EdgeDelta(edge) * (frameWall.PreWall+frameWall.Entry/2.0f - frameWall.Length/2.0f);
        return entryOffset + CenterToEdge(edge);
    }

    public Vector2 CenterToEdge(Edge edge)
    {
        switch (edge)
        {
        case Edge.Left: return new Vector2(-Width/2.0f, 0);
        case Edge.Right: return new Vector2(Width/2.0f, 0);
        case Edge.Top: return new Vector2(0, Height/2.0f);
        case Edge.Bottom: return new Vector2(0, -Height/2.0f);
        default: return Vector2.zero;
        }
    }

    public Frame RotatedRightTo(Edge rotEdge)
    {
        Dictionary<Edge, FrameWall> frameWalls = new Dictionary<Edge, FrameWall>();
        foreach(KeyValuePair<Edge, FrameWall> item in FrameWalls)
        {
            //Debug.Log(item.Key + " " + item.Key.RotatedRightTo(rotEdge));
            frameWalls[item.Key.RotatedRightTo(rotEdge)] = item.Value;
        }
        return new Frame(frameWalls);
    }

    
    public static Frame Test1 = new Frame(new Dictionary<Edge, FrameWall>(){
      {Edge.Left, new FrameWall(1,3,1)},
      {Edge.Right, new FrameWall(3,2,0)},
      {Edge.Bottom, new FrameWall(6,0,6)},
      {Edge.Top, new FrameWall(6,5,1)}
    });
    public static Frame Test2 = new Frame(new Dictionary<Edge, FrameWall>(){
      {Edge.Left, new FrameWall(3,2,3)},
      {Edge.Right, new FrameWall(4,1,3)},
      {Edge.Top, new FrameWall(6)},
      {Edge.Bottom, new FrameWall(0,6,0)}
    });
    public static Frame Test3 = new Frame(new Dictionary<Edge, FrameWall>(){
      {Edge.Left, new FrameWall(2,8,2)},
      {Edge.Right, new FrameWall(4,4,4)},
      {Edge.Top, new FrameWall(10)},
      {Edge.Bottom, new FrameWall(5,0,5)}
    });
    public static Frame Test4 = new Frame(new Dictionary<Edge, FrameWall>(){
        {Edge.Left, new FrameWall(10)},
        {Edge.Right, new FrameWall(4,2,4)},
        {Edge.Top, new FrameWall(8,4,8)},
        {Edge.Bottom, new FrameWall(20)}
    });
    public static Frame Test5 = new Frame(new Dictionary<Edge, FrameWall>(){
        {Edge.Left, new FrameWall(4,2,4)},
        {Edge.Right, new FrameWall(4,2,4)},
        {Edge.Top, new FrameWall(4,2,4)},
        {Edge.Bottom, new FrameWall(4,2,4)}
    });
}
