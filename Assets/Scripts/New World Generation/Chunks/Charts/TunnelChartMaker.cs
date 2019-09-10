using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelChartMaker : ChartMaker
{
    protected List<Walker> walkers;
    public Vector2Int thicknessRange = new Vector2Int(0, 3);
    [Range(0,1)]
    public float edgeSmoothness = 0.5f;
    public float curviness = 0.6f;
    public float tendency = 0.007f;

    void MakeWalkerFromTo(float startTheta, Vector2 fromPos, Vector2 toPos)
    {
        Walker walker = new Walker(
          seed: this.random.Next(),
          startTheta: startTheta,
          startOmega: Tools.RandomFloat(this.random, -Mathf.PI, Mathf.PI),
          maxOmegaMagnitude: curviness,
          maxAlphaMagnitude: 1f,
          startPosition: fromPos,
          goalPosition: toPos,
          goalWeight: tendency,
          goalMetDistance: 0.5f
        );
        walker.Walk(70, 0.1f);
        walkers.Add(walker);
    }

    void PutWalkersOnGroundArray()
    {
        foreach(Walker walker in walkers)
        {
            for (int i = 0; i < walker.path.Count; i++) {
                //Debug.Log(walker.path[i]);
                int x = ((int)walker.path[i].x).Bounded(0, groundArray.GetLength(0)-1);
                int y = ((int)walker.path[i].y).Bounded(0, groundArray.GetLength(1)-1);
                groundArray[x, y] = true;
            }
        }
    }

    public override void Make(Frame frame)
    {
        walkers = new List<Walker>();
        foreach(Edge edge in frame.FrameWalls.Keys)
        {
            if(!frame.FrameWalls[edge].HasEntry) continue;
            MakeWalkerFromTo(-edge.ToAngle(), frame.EntryPosition(edge),
              new Vector2(frame.Width/2.0f, frame.Height/2.0f));
        }
        groundArray = new bool[frame.Width-2, frame.Height-2];
        PutWalkersOnGroundArray();
        groundArray.Add(groundArray.ThinOutline());
        int thickness = random.Next(thicknessRange.x, thicknessRange.y);
        for(int i = 0; i < thickness; i++)
            groundArray.Add(groundArray.Outline(), probability:edgeSmoothness, random:this.random);
        groundArray = groundArray.WithBorder();
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

    void DrawWalkerPath(Walker walker)
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < walker.path.Count-1; i++) {
            Vector3 pos1 = (Vector3)chart.GroundCoordsToLocalPos(walker.path[i]) + transform.position;
            Vector3 pos2 = (Vector3)chart.GroundCoordsToLocalPos(walker.path[i+1]) + transform.position;
            Gizmos.DrawLine(pos1, pos2);
        }
    }

    void OnDrawGizmos()
    {
        if(!drawGizmos || groundArray == null) return;

        foreach(Walker walker in walkers)
            DrawWalkerPath(walker);
    }
}
