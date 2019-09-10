using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryMaker : MonoBehaviour
{
    ChunkScript chunkScript;
    List<EdgeCollider2D> edgeColliders;
    private Vector2 center;

    void Awake()
    {
        edgeColliders = new List<EdgeCollider2D>();
        chunkScript = GetComponentInParent<ChunkScript>();
        chunkScript.OnMapNodeGiven += mapNode => Make(mapNode.frame);
    }

    void Make(Frame frame)
    {
        if (edgeColliders != null)
            foreach (EdgeCollider2D edgeCollider in edgeColliders) Destroy(edgeCollider);
        edgeColliders = new List<EdgeCollider2D>();
        center = new Vector2(frame.Width/2.0f, frame.Height/2.0f);

        foreach(KeyValuePair<Edge, FrameWall> item in frame.FrameWalls)
        {
            Vector2 edgeStart = frame.EdgeStart(item.Key);
            Vector2 edgeDelta = frame.EdgeDelta(item.Key);
            if(item.Value.HasEntry)
            {
                AddEdgeCollider(new Vector2[]{
                    edgeStart,
                    edgeStart + edgeDelta * item.Value.PreWall
                });
                AddEdgeCollider(new Vector2[]{
                    edgeStart + edgeDelta * (item.Value.PreWall+item.Value.Entry),
                    edgeStart + edgeDelta * item.Value.Length
                });
            } else
            {
                AddEdgeCollider(new Vector2[]{
                    edgeStart,
                    edgeStart + edgeDelta * item.Value.Length
                });
            }

        }
    }

    void AddEdgeCollider(Vector2[] points)
    {
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        Vector2[] localPoints = new Vector2[points.Length];
        for (int i = 0 ;  i < points.Length; i++) {
            localPoints[i] = points[i] - center;
        }
        edgeCollider.points = localPoints;
        edgeColliders.Add(edgeCollider);
    }
}
