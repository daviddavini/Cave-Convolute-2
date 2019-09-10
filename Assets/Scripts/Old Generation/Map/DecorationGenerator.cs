using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationGenerator : SceneryGenerator
{
    public GameObject decorationPrefab;
    public Vector2 populationSizeRange;
    private int populationSize;

    public override void GenerateEntities()
    {
        int populationSize = random.Next((int)populationSizeRange.x, (int)populationSizeRange.y);
        for (int i = 0; i < populationSize; i++)
        {
            Spawn(decorationPrefab);
        }
    }
}
