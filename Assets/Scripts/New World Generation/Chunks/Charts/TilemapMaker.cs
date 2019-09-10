using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapMaker : MonoBehaviour
{
    private Tile groundTile;
    public Tile[] groundTileOptions;
    public static int groundTileIndex = 0;
    public Tile wallTile;

    private Grid grid;
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;

    public static Tile[] groundTiles;

    private ChunkScript chunkScript;

    void Awake()
    {
        grid = GetComponentInChildren<Grid>();

        groundTile = groundTileOptions[groundTileIndex++ % groundTileOptions.Length];

        chunkScript = GetComponentInParent<ChunkScript>();
        chunkScript.OnChartSet += Make;
    }

    void Make(Chart chart)
    {
        // Debug.Log(grid.transform.localPosition);
        // Debug.Log(chart.CenterPos);
        grid.transform.localPosition -=
          new Vector3((chart.Ground.GetLength(0)%2)/2.0f, (chart.Ground.GetLength(1)%2)/2.0f, 0);
        MakeTilemap(groundTilemap, chart.Ground, groundTile, chart.CenterPos);
        MakeTilemap(wallTilemap, chart.Wall, wallTile, chart.CenterPos);
    }

    void MakeTilemap(Tilemap tilemap, bool[,] array, Tile tile, Vector3 center)
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < array.GetLength(0); x++) {
            for (int y = 0; y < array.GetLength(1); y++) {
                if(array[x,y])
                {
                    Vector3 localPos = new Vector3(x + 0.5f, y + 0.5f, 0) - center;
                    Vector3Int tileCell = tilemap.LocalToCell(localPos + transform.position);
                    tilemap.SetTile(tileCell, tile);
                }
            }
        }
    }
}
