using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
//public struct ClusterInfo
//{
//    public List<GameObject> startingChunkPrefabs;
//    public List<GameObject> fillerChunkPrefabs;
//}

//public class ChunkManager : MonoBehaviour
//{
//    private MapMaker mapMaker;

//    private List<ChunkScript> chunkScripts;
//    private Dictionary<MapNode, ChunkScript> chunkScriptLookup;

//    public List<ClusterInfo> clusterInfos;
//    private int clusterCount = 0;

//    void Awake()
//    {
//        chunkScripts = new List<ChunkScript>();
//        chunkScriptLookup = new Dictionary<MapNode, ChunkScript>();

//        MapMaker mapMaker = new MapMaker();

//        foreach(ClusterInfo clusterInfo in clusterInfos)
//        {
//            List<FrameMaker> startingFrameMakers = clusterInfo.startingChunkPrefabs.Select(
//              p => p.GetComponentInChildren<FrameMaker>()).ToList();
//            List<FrameMaker> fillerFrameMakers = clusterInfo.fillerChunkPrefabs.Select(
//              p => p.GetComponentInChildren<FrameMaker>()).ToList();

//            mapMaker.MakeCluster(startingFrameMakers, fillerFrameMakers, 1);

//            clusterCount++;
//        }

//        foreach(KeyValuePair<MapNode, FrameMaker> item in mapMaker.NodeToFrameMaker)
//        {
//            AddChunk(item.Key, item.Value.transform.root.gameObject);
//        }

//        for(int i = 1; i < chunkScripts.Count; i++)
//            chunkScripts[i].gameObject.SetActive(false);

//    }

//    private int chunkIndex = 0;
//    void AddChunk(MapNode mapNode, GameObject chunkPrefab)
//    {
//        GameObject chunk = Instantiate(chunkPrefab);
//        chunk.name = "Chunk " + chunkIndex++;
//        ChunkScript chunkScript = chunk.GetComponent<ChunkScript>();
//        //chunkScript.chunkScriptLookup = chunkScriptLookup;
//        chunkScript.mapNode = mapNode;

//        //chunkScriptLookup[chunkScript.mapNode] = chunkScript;
//        chunkScripts.Add(chunkScript);
//    }


//}
