using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareChartMaker : ChartMaker
{
    public override void Make(Frame frame)
    {
        //Debug.Log("Frame" + frame);
        groundArray = new bool[frame.Width, frame.Height];
        if(makeWalls)
            groundArray.FillRect(true,1,groundArray.GetLength(0)-1,1,groundArray.GetLength(1)-1);
        else
            groundArray.FillRect(true,0,groundArray.GetLength(0),0,groundArray.GetLength(1));
        foreach(KeyValuePair<Edge, FrameWall> item in frame.FrameWalls)
        {
            if (item.Value.HasEntry)
                MakeEntry(item.Key, item.Value);
        }
        base.Make(frame);
    }

    public void MakeEntry(Edge edge, FrameWall frameWall)
    {
        Vector2Int delta = groundArray.EdgeDelta(edge);
        Vector2Int start = groundArray.EdgeStart(edge);

        for (int i = frameWall.PreWall; i < frameWall.PreWall + frameWall.Entry; i++)
            groundArray[start.x + delta.x * i, start.y + delta.y * i] = true;
    }
}
