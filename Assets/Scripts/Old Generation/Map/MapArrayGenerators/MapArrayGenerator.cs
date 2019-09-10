using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapArrayTile {None, Ground, Wall};

public abstract class MapArrayGenerator : MonoBehaviour
{
    protected System.Random random;
    private Walker walker;
    private bool showGizmos = false;

    public MapArrayTile[,] mapArray;
    public MapArrayTile[,] wallArray;
    public Rect roomBoundingRect;
    public Vector2 originalMapCenterCoords;
    public Vector2 originalToFinalCoordConversion = new Vector2(0,0);

    // public Vector3 centerLocalPosition;
    // public MapArrayTile[,] roomArray;
    // public MapArrayTile[,] pathArray;
    // public MapArrayTile[,] helpedPathArray;
    // public MapArrayTile[,] helperPathArrays;
    //public Rect mapBoundingRect;
    //public List<ConnectorSpot> connectorSpots = new List<ConnectorSpot>();

    public void CreateRandom(int seed)
    {
        this.random = new System.Random(seed);
    }

    public void CreateMapArray(int mapWidth, int mapHeight, Rect roomBoundingRect)
    {
        mapArray = new MapArrayTile[mapWidth, mapHeight];
        FillWith(mapArray, MapArrayTile.None);

        ///!!! WARNING: ASSUMES THAT ROOM STARTS IN CENTER
        this.roomBoundingRect = roomBoundingRect;
        originalMapCenterCoords = new Vector2((int)this.roomBoundingRect.center.x,
          (int)this.roomBoundingRect.center.y);

        walker = new Walker(
          seed: this.random.Next(),
          startTheta: 0f,
          startOmega: Tools.RandomFloat(this.random, -3.14f, 3.14f),
          maxOmegaMagnitude: 0.7f,
          maxAlphaMagnitude: 1f,
          startPosition: new Vector2(mapArray.GetLength(0), mapArray.GetLength(1)/2.0f),
          goalPosition: new Vector2(mapArray.GetLength(0)/2.0f, mapArray.GetLength(1)/2.0f),
          goalWeight: 0.007f
        );
        walker.Walk(70, 0.1f);
    }

    //where the map array creates random and map array
    public abstract void Init(int seed);

    // public void Generate()
    // {
    //     GenerateCenter();
    //     foreach (ConnectorScript connectorScript in connectorScripts)
    // }

    public abstract void GenerateCenter();

    protected abstract MapArrayTile[,] GeneratePathInto(int seed, Side side,
      MapArrayGenerator otherMapArrayGenerator, out Vector2 connectorCoords, out Vector2 plugCoords);

    // public MapArrayGenerator GeneratePathMapArrayGenerator(Side side, out Vector3 connectorLocalPosition)
    // {
    //     connector -- pathMapArray -- connector
    // }

    public void GeneratePathTo(Side side, MapArrayGenerator otherMapArrayGenerator,
      out Vector3 connectorLocalPosition, out Vector3 otherConnectorLocalPosition)
    {
        //choose edge point for each mapArray
        Vector2 connectorCoords;
        Vector2 otherConnectorCoords;
        Vector2 plugCoords;
        Vector2 otherPlugCoords;

        //generate into each copy mapArray from point
        int pathSeed = random.Next();
        MapArrayTile[,] pathMapArray = GeneratePathInto(pathSeed, side, this, out connectorCoords, out plugCoords);
        MapArrayTile[,] otherPathMapArray = GeneratePathInto(pathSeed, ConnectorScript.oppositeSide[side],
          otherMapArrayGenerator, out otherConnectorCoords, out otherPlugCoords);

        Vector2 normalToOtherCoordConversion = otherConnectorCoords - connectorCoords;

        connectorLocalPosition = this.MapCoordsToLocalPosition(otherPlugCoords - normalToOtherCoordConversion);
        otherConnectorLocalPosition = otherMapArrayGenerator.MapCoordsToLocalPosition(otherPlugCoords);
        // Debug.Log(connectorLocalPosition);
        // Debug.Log(otherConnectorLocalPosition);

        //clip other maparray room away from copy maparray
        // otherPathMapArray = MakeWalls(otherPathMapArray);
        // ClipWith(otherPathMapArray, otherMapArrayGenerator.mapArray);

        //extend this maparray by other maparray path
        // Debug.Log(connectorCoords);
        // Debug.Log(otherConnectorCoords);
        mapArray = Combine(mapArray, pathMapArray);
        ExtendWithAt(otherPathMapArray, normalToOtherCoordConversion);
    }

    public void GenerateWalls()
    {
        mapArray = MakeWalls(mapArray);
    }

    public void FitTogetherWithAt(MapArrayGenerator otherMapArrayGenerator,
      Vector2 connectorPosition, Vector2 otherConnectorPosition)
    {
        Vector2 connectorCoords = this.LocalPositionToMapCoords(connectorPosition);
        Vector2 otherConnectorCoords = otherMapArrayGenerator.LocalPositionToMapCoords(otherConnectorPosition);
        ClipWith(otherMapArrayGenerator.mapArray, mapArray, connectorCoords - otherConnectorCoords);
    }

    //check that it uses the right map array, arg not field
    protected void ExtendWithAt(MapArrayTile[,] extenderMapArray,
      Vector2 regularToExtenderCoordConversion)
    {
        //dont just change map, also change rects and coord to position conversion somehow
        Vector2 mapSize = new Vector2(mapArray.GetLength(0), mapArray.GetLength(1));
        Vector2 extenderMapSize = new Vector2(extenderMapArray.GetLength(0), extenderMapArray.GetLength(1));

        Vector2 topRightCorner = Vector2.Max(mapSize, extenderMapSize-regularToExtenderCoordConversion);
        Vector2 bottomLeftCorner = Vector2.Min(Vector2.zero, Vector2.zero-regularToExtenderCoordConversion);

        Vector2 newMapSize = topRightCorner - bottomLeftCorner;
        Vector2 regularToNewCoordConversion = - bottomLeftCorner;
        Vector2 extenderToNewCoordConversion = regularToNewCoordConversion - regularToExtenderCoordConversion;

        // Debug.Log(mapSize);
        // Debug.Log(extenderMapSize);
        // Debug.Log(topRightCorner);
        // Debug.Log(bottomLeftCorner);

        MapArrayTile[,] newMapArray = new MapArrayTile[(int)newMapSize.x, (int)newMapSize.y];
        FillWith(newMapArray, MapArrayTile.None);

        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                newMapArray[i+(int)regularToNewCoordConversion.x, j+(int)regularToNewCoordConversion.y] = mapArray[i,j];
            }
        }
        for (int i = 0; i < extenderMapArray.GetLength(0); i++)
        {
            for (int j = 0; j < extenderMapArray.GetLength(1); j++)
            {
                newMapArray[i+(int)extenderToNewCoordConversion.x, j+(int)extenderToNewCoordConversion.y] = extenderMapArray[i,j];
            }
        }

        mapArray = newMapArray;
        roomBoundingRect.position += regularToNewCoordConversion;
        originalToFinalCoordConversion += regularToNewCoordConversion;
    }

    //if needed, move to after, and add offset for unequal array dimensions
    protected static void ClipWith(MapArrayTile[,] clippeeMapArray, MapArrayTile[,] clipperMapArray,
      Vector2 clippeeToClipperCoordConversion)
    {
        //Debug.Log("clipping away from array");
        Vector2 clippeeMapSize = new Vector2(clippeeMapArray.GetLength(0), clippeeMapArray.GetLength(1));
        Vector2 clipperMapSize = new Vector2(clipperMapArray.GetLength(0), clipperMapArray.GetLength(1));

        // Debug.Log(clippeeMapSize);
        // Debug.Log(clipperMapSize);
        // Debug.Log(clippeeMapArray.GetLength(0));
        // Debug.Log(clippeeMapArray.GetLength(1));
        // Debug.Log(clippeeToClipperCoordConversion);

        Vector2 commonTopRight = Vector2.Min(clippeeMapSize, clipperMapSize-clippeeToClipperCoordConversion);
        Vector2 commonBottomLeft = Vector2.Max(Vector2.zero, Vector2.zero-clippeeToClipperCoordConversion);

        // Debug.Log(commonTopRight);
        // Debug.Log(commonBottomLeft);
        MapArrayTile clippeeTile;
        MapArrayTile clipperTile;

        for (int i = (int)commonBottomLeft.x; i < (int)commonTopRight.x; i++)
        {
            for (int j = (int)commonBottomLeft.y; j < (int)commonTopRight.y; j++)
            {
                clippeeTile = clippeeMapArray[i,j];
                clipperTile = clipperMapArray[i+(int)clippeeToClipperCoordConversion.x,j+(int)clippeeToClipperCoordConversion.y];
                if(clippeeTile == MapArrayTile.Ground && clipperTile == MapArrayTile.Wall)
                {
                    clippeeMapArray[i,j] = MapArrayTile.None;
                    clipperMapArray[i+(int)clippeeToClipperCoordConversion.x,j+(int)clippeeToClipperCoordConversion.y]
                      = MapArrayTile.Ground;
                } else if(clippeeTile != MapArrayTile.None && clipperTile != MapArrayTile.None)
                {
                    clippeeMapArray[i,j] = MapArrayTile.None;
                }
            }
        }
    }

    protected static int NumberOfInAround(MapArrayTile mapArrayTile, MapArrayTile[,] mapArr, int i, int j)
    {
        int sum = 0;
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (mapArr[i+x,j+y] == mapArrayTile) {sum++;}
            }
        }
        return sum;
    }

    public static void FillWith(MapArrayTile[,] mapArr, MapArrayTile mapArrayTile)
    {
        for (int i = 0; i < mapArr.GetLength(0); i++)
        {
            for (int j = 0; j < mapArr.GetLength(1); j++)
            {
                mapArr[i,j] = mapArrayTile;
            }
        }
    }

    protected static void FillBoxWith(MapArrayTile[,] mapArr,
      int startX, int endX, int startY, int endY, MapArrayTile mapArrayTile)
    {
        for (int i = startX; i < endX; i++)
        {
            for (int j = startY; j < endY; j++)
            {
                mapArr[i,j] = mapArrayTile;
            }
        }
    }

    protected static MapArrayTile[,] AddBorderTo(MapArrayTile[,] mapArr)
    {
        MapArrayTile[,] mapArrWithBorder = new MapArrayTile[mapArr.GetLength(0)+2, mapArr.GetLength(1)+2];
        FillWith(mapArrWithBorder, MapArrayTile.None);
        for (int i = 0; i < mapArr.GetLength(0); i++)
        {
            for (int j = 0; j < mapArr.GetLength(1); j++)
            {
                mapArrWithBorder[i+1,j+1] = mapArr[i,j];
            }
        }
        return mapArrWithBorder;
    }

    protected static MapArrayTile[,] Combine(MapArrayTile[,] mapArr1, MapArrayTile[,] mapArr2)
    {
        MapArrayTile[,] newMapArr = new MapArrayTile[mapArr1.GetLength(0), mapArr1.GetLength(1)];
        for (int i = 0; i < mapArr1.GetLength(0); i++)
        {
            for (int j = 0; j < mapArr1.GetLength(1); j++)
            {
                if(mapArr1[i,j] != MapArrayTile.None)
                {
                    newMapArr[i,j] = mapArr1[i,j];
                } else if(mapArr2[i,j] != MapArrayTile.None)
                {
                    newMapArr[i,j] = mapArr2[i,j];
                }
            }
        }
        return newMapArr;
    }

    protected static MapArrayTile[,] MakeWalls(MapArrayTile[,] mapArr)
    {
        //Debug.Log("Making walls");
        MapArrayTile[,] mapArrWithBorder = AddBorderTo(mapArr);
        MapArrayTile[,] newMapArr = new MapArrayTile[mapArr.GetLength(0), mapArr.GetLength(1)];
        for (int i = 0; i < mapArr.GetLength(0); i++)
        {
            for (int j = 0; j < mapArr.GetLength(1); j++)
            {
                if(mapArr[i,j] == MapArrayTile.None &&
                  NumberOfInAround(MapArrayTile.Ground, mapArrWithBorder, i+1, j+1) > 0)
                {
                    newMapArr[i,j] = MapArrayTile.Wall;
                } else
                {
                    newMapArr[i,j] = mapArr[i,j];
                }
            }
        }
        return newMapArr;
    }

    public Vector3 MapCoordsToLocalPosition(float i, float j)
    {
        return MapCoordsToLocalPosition(new Vector2(i, j));
    }

    public Vector3 MapCoordsToLocalPosition(Vector2 coords)
    {
        return -(Vector3)originalToFinalCoordConversion - (Vector3)originalMapCenterCoords
          + new Vector3(coords.x + .5f, coords.y +.5f, 0);
    }

    public Vector2 LocalPositionToMapCoords(Vector3 pos)
    {
        return originalToFinalCoordConversion + originalMapCenterCoords
          + new Vector2(pos.x - .5f, pos.y - .5f);
    }

    void OnDrawGizmos()
    {
        if(showGizmos)
        {
            Dictionary<MapArrayTile, Color> toColor = new Dictionary<MapArrayTile, Color>()
            {
              {MapArrayTile.None, Color.white},
              {MapArrayTile.Ground, Color.green},
              {MapArrayTile.Wall, Color.red}
            };
            if (mapArray != null) {
                for (int x = 0; x < mapArray.GetLength(0); x++) {
                    for (int y = 0; y < mapArray.GetLength(1); y++) {
                        Gizmos.color = toColor[mapArray[x,y]];
                        Vector3 pos = MapCoordsToLocalPosition(x, y) + transform.position;
                        Gizmos.DrawWireCube(pos,Vector3.one*0.8f);
                    }
                }
            }
        }

        Gizmos.color = Color.blue;
        if (walker != null) {
            for (int i = 0; i < walker.path.Count-1; i++) {
                Vector3 pos1 = MapCoordsToLocalPosition(walker.path[i].x, walker.path[i].y) + transform.position;
                Vector3 pos2 = MapCoordsToLocalPosition(walker.path[i+1].x, walker.path[i+1].y) + transform.position;
                Gizmos.DrawLine(pos1, pos2);
            }
        }
    }
}
