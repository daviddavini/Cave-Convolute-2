using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryGenerator : MonoBehaviour
{
    protected MapArrayGenerator mapArrayGenerator;
    protected System.Random random;

    public virtual void Awake()
    {
        mapArrayGenerator = GetComponent<MapArrayGenerator>();
    }

    public virtual void Init(int seed)
    {
        random = new System.Random(seed);
    }

    public virtual void GenerateEntities()
    {
    }

    public void Spawn(GameObject entityPrefab)
    {
        Vector2 coords = Vector2.zero;
        while (mapArrayGenerator.mapArray[(int)Mathf.Round(coords.x), (int)Mathf.Round(coords.y)] != MapArrayTile.Ground)
        {
            coords = Tools.RandomVector(this.random, new Vector2(0,0),
              new Vector2(mapArrayGenerator.mapArray.GetLength(0)-1, mapArrayGenerator.mapArray.GetLength(1)-1));
        }
        Instantiate(entityPrefab, transform.position + mapArrayGenerator.MapCoordsToLocalPosition(coords),
          Quaternion.identity, transform.root);
    }
}
