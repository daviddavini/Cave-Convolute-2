using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// job - to create the tilemaps from mapArray
// Better name: map generator
// solution: responsible for other map's tilemaps
public class TilemapGenerator : MonoBehaviour
{
    public List<Tile> groundTileOptions;
    private Tile groundTile;
    public List<Tile> wallTileOptions;
    private Tile wallTile;
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    private System.Random random;

    private MapArrayGenerator mapArrayGenerator;

    void Awake()
    {
        mapArrayGenerator = GetComponent<MapArrayGenerator>();
    }

    public void Init(int seed)
    {
        random = new System.Random(seed);
        groundTile = groundTileOptions[random.Next(0,groundTileOptions.Count)];
        wallTile = wallTileOptions[random.Next(0,wallTileOptions.Count)];
    }

    public void CreateTilemap()
    {
        // IMPORTANT! Gameobject must be enabled for LocalToCell to work!!
        //replace with cave biomes
        // Debug.Log("making tilemap");
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        for (int i = 0; i < mapArrayGenerator.mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArrayGenerator.mapArray.GetLength(1); j++)
            {
                if(mapArrayGenerator.mapArray[i,j] == MapArrayTile.Ground)
                {
                    Vector3 localTilePosition = mapArrayGenerator.MapCoordsToLocalPosition(i, j);
                    Vector3Int tileCell = floorTilemap.LocalToCell(localTilePosition + floorTilemap.transform.localPosition);
                    floorTilemap.SetTile(tileCell, groundTile);
                } else if (mapArrayGenerator.mapArray[i,j] == MapArrayTile.Wall)
                {
                    Vector3 localTilePosition = mapArrayGenerator.MapCoordsToLocalPosition(i, j);
                    Vector3Int tileCell = wallTilemap.LocalToCell(localTilePosition + floorTilemap.transform.localPosition);
                    wallTilemap.SetTile(tileCell, wallTile);
                }
            }
        }
    }
}
