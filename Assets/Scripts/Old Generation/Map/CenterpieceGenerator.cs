using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//expand later or merge with other entity gens?
public class CenterpieceGenerator : SceneryGenerator
{
    public List<GameObject> centerpiecePrefabs;
    private GameObject centerpiecePrefab;

    public override void GenerateEntities()
    {
        centerpiecePrefab = Tools.RandomItemFromList<GameObject>(random, centerpiecePrefabs);
        Instantiate(centerpiecePrefab,
          transform.position + mapArrayGenerator.MapCoordsToLocalPosition(mapArrayGenerator.roomBoundingRect.center),
          Quaternion.identity, transform.root);
    }
}
