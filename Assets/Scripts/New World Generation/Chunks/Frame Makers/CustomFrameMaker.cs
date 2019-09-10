using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Layout
{
    public List<Edge> entries;
    public List<Edge> addedRotationsRightTo;
    public Layout(List<Edge> entries, List<Edge> addedRotationsRightTo)
    {
        this.entries = entries;
        this.addedRotationsRightTo = addedRotationsRightTo;
    }

    public static Layout Hallway = new Layout(
      new List<Edge>(){Edge.Left, Edge.Right},
      new List<Edge>(){Edge.Top}
    );
    public static Layout Corner = new Layout(
      new List<Edge>(){Edge.Left, Edge.Top},
      new List<Edge>(){Edge.Top, Edge.Left, Edge.Bottom}
    );
    public static Layout T = new Layout(
      new List<Edge>(){Edge.Left, Edge.Top, Edge.Right},
      new List<Edge>(){Edge.Top, Edge.Left, Edge.Bottom}
    );
    public static Layout AllEntry = new Layout(
      new List<Edge>(){Edge.Left, Edge.Top, Edge.Right, Edge.Bottom},
      new List<Edge>(){}
    );
    public static Layout OneEntry = new Layout(
      new List<Edge>(){Edge.Bottom},
      new List<Edge>(){Edge.Top, Edge.Left, Edge.Bottom}
    );
}

public class CustomFrameMaker : FrameMaker
{
    public Vector2Int entryLengthRange;
    public int minEntryToEdgeDistance = 3;
    public Vector2Int widthRange;
    public bool isSquare;
    public Vector2Int heightRange;
    public List<Layout> customLayouts;
    public bool addHallwayLayout;
    public bool addCornerLayout;
    public bool addTLayout;
    public bool addAllEntryLayout;
    public bool addOneEntryLayout;

    public List<Layout> Layouts {get{
        List<Layout> layouts = new List<Layout>();
        foreach(Layout customLayout in customLayouts)
        {
            layouts.Add(customLayout);
        }
        if (addHallwayLayout)
            layouts.Add(Layout.Hallway);
        if (addTLayout)
            layouts.Add(Layout.T);
        if (addCornerLayout)
            layouts.Add(Layout.Corner);
        if (addAllEntryLayout)
            layouts.Add(Layout.AllEntry);
        if (addOneEntryLayout)
            layouts.Add(Layout.OneEntry);
        return layouts;
    }}

    FrameWall RandomEntryFrameWall(int entry, int length, System.Random random)
    {
        int preWall = random.Next(minEntryToEdgeDistance, length-entry-minEntryToEdgeDistance + 1);
        int postWall = length - preWall - entry;
        return new FrameWall(preWall, entry, postWall);
    }

    List<Frame> MakeFrames(int entry, int width, int height, List<Edge> entries,
      List<Edge> addedRotationsRightTo, System.Random random)
    {
        List<Frame> frames = new List<Frame>();

        FrameWall leftWall = entries.Contains(Edge.Left) ? RandomEntryFrameWall(entry, height, random)
          : new FrameWall(height);
        FrameWall rightWall = entries.Contains(Edge.Right) ? RandomEntryFrameWall(entry, height, random)
          : new FrameWall(height);
        FrameWall topWall = entries.Contains(Edge.Top) ? RandomEntryFrameWall(entry, width, random)
          : new FrameWall(width);
        FrameWall bottomWall = entries.Contains(Edge.Bottom) ? RandomEntryFrameWall(entry, width, random)
          : new FrameWall(width);

        Frame frame = new Frame(new Dictionary<Edge, FrameWall>(){
            {Edge.Left, leftWall}, {Edge.Right, rightWall}, {Edge.Top, topWall}, {Edge.Bottom, bottomWall}
        });
        frames.Add(frame);

        foreach(Edge rotEdge in addedRotationsRightTo)
        {
            frames.Add(frame.RotatedRightTo(rotEdge));
        }

        return frames;
    }

    List<Frame> MakeRandomFrame(Vector2Int entryLengthRange, Vector2Int widthRange, Vector2Int heightRange,
      bool isSquare, List<Edge> entries, List<Edge> addedRotationsRightTo, System.Random random)
    {
        int entry = random.Next(entryLengthRange.x, entryLengthRange.y);
        int width = random.Next(widthRange.x, widthRange.y);
        int height = isSquare ? width : random.Next(heightRange.x, heightRange.y);

        return MakeFrames(entry, width, height, entries, addedRotationsRightTo, random);
    }

    public override List<Frame> PreferredFrames(System.Random random)
    {
        List<Frame> preferredFrames = new List<Frame>();

        foreach(Layout layout in Layouts)
        {
            preferredFrames.AddRange(MakeRandomFrame(entryLengthRange, widthRange, heightRange, isSquare,
              layout.entries, layout.addedRotationsRightTo, random));
        }

        return preferredFrames;
    }
}
