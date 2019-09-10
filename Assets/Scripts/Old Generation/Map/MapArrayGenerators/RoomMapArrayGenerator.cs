using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum MapArrayTile {None, Ground, Wall, Connector};

public class RoomMapArrayGenerator : MapArrayGenerator
{
    //public List<ConnectorSpot> connectorSpots = new List<ConnectorSpot>();

    public Vector2 roomWidthRange;
    public Vector2 roomHeightRange;
    public bool sameDimensions = false;
    public Vector2 hallLengthRange;
    public Vector2 hallThicknessRange;

    //private int roomWidth;
    //private int roomHeight;
    //private int hallLength;

    public override void Init(int seed)
    {
        base.CreateRandom(seed);

        int roomWidth = this.random.Next((int)roomWidthRange.x, (int)roomWidthRange.y);
        int roomHeight = sameDimensions ? roomWidth : this.random.Next((int)roomHeightRange.x, (int)roomHeightRange.y);
        int hallLength = this.random.Next((int)hallLengthRange.x, (int)hallLengthRange.y);

        Rect roomBoundingRect = new Rect(new Vector2(hallLength, hallLength),
          new Vector2(roomWidth, roomHeight));
        base.CreateMapArray(roomWidth + hallLength*2, roomHeight + hallLength*2, roomBoundingRect);
    }

    public override void GenerateCenter()
    {
        //Add Room Rectangle
        for (int i = (int)roomBoundingRect.min.x; i < (int)roomBoundingRect.max.x; i++)
        {
            for (int j = (int)roomBoundingRect.min.y; j < (int)roomBoundingRect.max.y; j++)
            {
                mapArray[i,j] = MapArrayTile.Ground;
            }
        }
    }

    protected override MapArrayTile[,] GeneratePathInto(int seed, Side side,
      MapArrayGenerator otherMapArrayGenerator, out Vector2 connectorCoords, out Vector2 plugCoords)
    {
        System.Random random = new System.Random(seed);
        int hallThickness = random.Next((int)hallThicknessRange.x, (int)hallThicknessRange.y);
        int hallYCoord = random.Next((int)otherMapArrayGenerator.roomBoundingRect.min.y +hallThickness/2,
          (int)otherMapArrayGenerator.roomBoundingRect.max.y -(hallThickness+1)/2);
        int hallXCoord = random.Next((int)otherMapArrayGenerator.roomBoundingRect.min.x +hallThickness/2,
          (int)otherMapArrayGenerator.roomBoundingRect.max.x -(hallThickness+1)/2);

        MapArrayTile[,] otherMapArray = otherMapArrayGenerator.mapArray;
        Rect otherRoomBoundingRect = otherMapArrayGenerator.roomBoundingRect;

        plugCoords = Vector2.zero;
        connectorCoords = Vector2.zero;

        switch(side) {
        case Side.Left:
            connectorCoords = new Vector2(-0.5f, hallYCoord);
            plugCoords = new Vector2((int)otherRoomBoundingRect.min.x-0.5f, hallYCoord);
            break;
        case Side.Right:
            connectorCoords = new Vector2(otherMapArray.GetLength(0)-0.5f, hallYCoord);
            plugCoords = new Vector2((int)otherRoomBoundingRect.max.x-0.5f, hallYCoord);
            break;
        case Side.Top:
            connectorCoords = new Vector2(hallXCoord, otherMapArray.GetLength(1)-0.5f);
            plugCoords = new Vector2(hallXCoord, (int)otherRoomBoundingRect.max.y-0.5f);
            break;
        case Side.Bottom:
            connectorCoords = new Vector2(hallXCoord, -0.5f);
            plugCoords = new Vector2(hallXCoord, (int)otherRoomBoundingRect.min.y-0.5f);
            break;
        default:
            Debug.Log("Side does not exist");
            break;
        }

        MapArrayTile[,] pathArray = new MapArrayTile[otherMapArray.GetLength(0), otherMapArray.GetLength(1)];
        FillWith(pathArray, MapArrayTile.None);

        //Returns the position of the connector in map coords
        switch(side) {
        case Side.Left:
            FillBoxWith(pathArray, 0, (int)otherRoomBoundingRect.min.x,
              hallYCoord-hallThickness/2, hallYCoord+(hallThickness+1)/2, MapArrayTile.Ground);
            break;
        case Side.Right:
            FillBoxWith(pathArray, (int)otherRoomBoundingRect.max.x, pathArray.GetLength(0),
              hallYCoord-hallThickness/2, hallYCoord+(hallThickness+1)/2, MapArrayTile.Ground);
            break;
        case Side.Top:
            FillBoxWith(pathArray, hallXCoord-hallThickness/2, hallXCoord+(hallThickness+1)/2,
              (int)otherRoomBoundingRect.max.y, pathArray.GetLength(1), MapArrayTile.Ground);
            break;
        case Side.Bottom:
            FillBoxWith(pathArray, hallXCoord-hallThickness/2, hallXCoord+(hallThickness+1)/2,
              0, (int)otherRoomBoundingRect.min.y, MapArrayTile.Ground);
            break;
        default:
            Debug.Log("Side does not exist");
            break;
        }
        return pathArray;
    }
}
