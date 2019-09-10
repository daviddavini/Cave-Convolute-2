// ﻿using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class ChunkLoader : EventEffect
// {
//     private ChunkScript root;
//     private List<ChunkScript> loadedChunkScripts;
//     //public GameObject loaderGameObject;
//     private ChunkManager chunkManager;
//
//
//     Awake()
//     {
//         base.Awake();
//         chunkManager = GameObject.FindWithTag("Chunk Manager").GetComponent<ChunkManager>();
//     }
//
//     DoEffect()
//     {
//         ChunkScript newRoot = eventGameObject.GetComponentInParent<ChunkScript>();
//         LoadChunksAround(chunkScript.mapNode);
//     }
//
//     LoadChunksAround(MapNode mapNode)
//     {
//         Vector3 rootPosition = chunkScript.position;
//         for(MapNode neighbor in mapNode.Neighbors) {
//             chunkManager.chunkScripts[neighbor].transform.position =
//         }
//         for(MapNode neighbor in mapNode.Neighbors) {
//             for(MapNode secondNeighbor in neighbor.Neighbors) {
//                 if(!)
//             }
//         }
//     }
// }
