using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Chart
{
    public bool[,] Ground {get;}
    public bool[,] Wall {get;}
    public bool[,] Interior {get;}
    public Vector2 CenterPos {get;}
    public Rect BoundingRect {get;}
    public int Area {get;}

    public Chart(bool[,] ground, bool makeWalls)
    {
        Ground = ground;
        Wall = makeWalls ? ground.Outline() : new bool[0,0];
        Interior = ground.Interior();
        CenterPos = new Vector2(ground.GetLength(0)/2.0f, ground.GetLength(1)/2.0f);
        BoundingRect = new Rect(
          new Vector2(-ground.GetLength(0)/2.0f, -ground.GetLength(1)/2.0f),
          new Vector2(ground.GetLength(0), ground.GetLength(1))
        );
        Area = ground.ConvertToInt().Sum();
    }

    public Vector2Int LocalPosToGroundCoords(Vector2 position)
    {
        //Debug.Log(position);
        Vector2 localPos = position - BoundingRect.position;
        int i = (int) localPos.x;//(Ground.GetLength(0) * localPos.x / BoundingRect.size.x);
        int j = (int) localPos.y;//(Ground.GetLength(1) * localPos.y / BoundingRect.size.y);
        return new Vector2Int(i,j);
    }

    public Vector2 GroundCoordsToLocalPos(Vector2 coords)
    {
        return coords + BoundingRect.position;
    }

    public bool HasGroundAt(Vector2 position)
    {
        Vector2Int groundCoords = LocalPosToGroundCoords(position);
        //Debug.Log(groundCoords);
        return Ground[groundCoords.x, groundCoords.y];
    }

    public Orientation GetOrientation()
    {
        return Ground.GetLength(0) > Ground.GetLength(1) ? Orientation.Horizontal : Orientation.Vertical;
    }
}

public abstract class ChartMaker : MonoBehaviour
{
    protected ChunkScript chunkScript;

    protected bool[,] groundArray;
    protected Chart chart;

    public bool makeWalls = true;

    public bool drawGizmos = false;
    private Color gizmosColor;
    private static int chunkIndex = 0;
    private static List<Color> gizmosColors = new List<Color>(){
      Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow,
      new Color(1,0.5f,0,1), new Color(0.5f,1,0,1), new Color(0,0.5f,1,1),
      new Color(0,1,0.5f,1), new Color(1,0,0.5f,1), new Color(0.5f,0,1,1)
    };

    protected System.Random random;

    void Awake()
    {
        random = new System.Random();

        chunkScript = GetComponentInParent<ChunkScript>();
        chunkScript.OnMapNodeGiven += mapNode => Make(mapNode.frame);

        gizmosColor = gizmosColors[chunkIndex++ % gizmosColors.Count];
    }

    public virtual void Make(Frame frame)
    {
        chart = new Chart(groundArray, makeWalls: this.makeWalls);
        chunkScript.chart = chart;
    }

    void DrawArray(bool[,] array)
    {
        for (int x = 0; x < array.GetLength(0); x++) {
            for (int y = 0; y < array.GetLength(1); y++) {
                Gizmos.color = array[x,y] ? gizmosColor : Color.black;
                Vector3 localPosition = new Vector3(x + 0.5f, y + 0.5f, 0) - (Vector3)chart.CenterPos;
                Gizmos.DrawWireCube(transform.position + localPosition,Vector3.one*0.8f);
            }
        }
    }

    void OnDrawGizmos()
    {
        if(!drawGizmos || groundArray == null) return;

        DrawArray(groundArray);

        //Handles.color = Color.white;
        //Handles.Label(transform.position, chunkScript.mapNode.id.ToString());
    }
}
