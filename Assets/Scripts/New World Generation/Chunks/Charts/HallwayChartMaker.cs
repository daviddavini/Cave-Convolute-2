using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayChartMaker : ChartMaker
{
    public override void Make(Frame frame)
    {
        groundArray = new bool[frame.Width, frame.Height];
        foreach (KeyValuePair<Edge, FrameWall> item in frame.FrameWalls)
        {
            if (item.Value.HasEntry)
                MakeHallway(item.Key, frame);
        }
        base.Make(frame);
    }

    public void MakeHallway(Edge edge, Frame frame)
    {
        FrameWall frameWall = frame.FrameWalls[edge];
        Vector2Int delta = groundArray.EdgeDelta(edge);
		Vector2Int perpDelta = groundArray.PerpEdgeDelta(edge);
        Vector2Int start = groundArray.EdgeStart(edge);
        FrameWall counterClockwiseFrameWall = frame.FrameWalls[edge.RotatedRightTo(Edge.Top)];
        FrameWall clockwiseFrameWall = frame.FrameWalls[edge.RotatedRightTo(Edge.Bottom)];
        int end;
        bool hasAdjacentEdge = true;
        if (counterClockwiseFrameWall.HasEntry)
            end = counterClockwiseFrameWall.GetWallBySign(edge.GetSign()) + counterClockwiseFrameWall.Entry;
        else if (clockwiseFrameWall.HasEntry)
            end = clockwiseFrameWall.GetWallBySign(edge.GetSign()) + clockwiseFrameWall.Entry;
        else
        {
            hasAdjacentEdge = false;
            end = (frame.GetDimension(edge.Opposite().GetOrientation())) / 2;
        }

        //Debug.Log("End:" + end);
        
        for (int i = frameWall.PreWall; i < frameWall.PreWall + frameWall.Entry; i++)
        {
            for(int j = 0; j < end; j++)
            {
                groundArray[start.x + delta.x*i + perpDelta.x*j, start.y + delta.y*i + perpDelta.y*j] = true;
            }
        }

        if(hasAdjacentEdge == false)
        {
            //Vector2Int centerHallStart = start + delta * (frameWall.PreWall+frameWall.Entry) + perpDelta * end;
            //Vector2Int centerHallDelta = delta;
            //Vector2Int centerHallEnd = frameWall.PreWall+frameWall.Entry -

            int centerRoomRadius = frameWall.PreWall + frameWall.Entry/2 - frame.GetDimension(edge.Opposite().GetOrientation()) / 2;
           
            groundArray.FillRect(true, frame.Width/2 - centerRoomRadius, frame.Width/2 + centerRoomRadius,
                frame.Height/2 - centerRoomRadius, frame.Height/2 + centerRoomRadius);
            //for (int i = frameWall.PreWall; i < frameWall.PreWall + frameWall.Entry; i++)
            //{
            //    for (int j = 0; j < end; j++)
            //    {
            //        groundArray[start.x + delta.x * i + perpDelta.x * j, start.y + delta.y * i + perpDelta.y * j] = true;
            //    }
            //}
        }
            
    }
}
