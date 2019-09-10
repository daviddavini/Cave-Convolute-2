using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ClusterScript : MonoBehaviour
{
    public List<GameObject> startingChunkPrefabs;
    public List<GameObject> fillerChunkPrefabs;
    public List<GameObject> transitionChunkPrefabs;
    public List<GameObject> endingChunkPrefabs;

    private List<FrameMaker> startingFrameMakers;
    private List<FrameMaker> fillerFrameMakers;
    private List<FrameMaker> endingFrameMakers;
    private List<FrameMaker> transitionFrameMakers;

    private int entries = 1;
    private int joinedEntries = 0;
    public int UnjoinedEntries { get {
            return entries - joinedEntries;
    } }

    public List<ChunkScript> chunkScripts;
    //public List<MapNode> openMapNodes;

    private MapMaker mapMaker;
    
    void Awake()
    {
        MakeCluster();
    }

    public void DeactivateChunks()
    {
        foreach(ChunkScript chunkScript in chunkScripts)
        {
            chunkScript.gameObject.SetActive(false);
        }
    }

    void MakeCluster()
    {
        chunkScripts = new List<ChunkScript>();

        mapMaker = new MapMaker();

        startingFrameMakers = startingChunkPrefabs.Select(
          p => p.GetComponentInChildren<FrameMaker>()).ToList();
        fillerFrameMakers = fillerChunkPrefabs.Select(
          p => p.GetComponentInChildren<FrameMaker>()).ToList();
        transitionFrameMakers = transitionChunkPrefabs.Select(
          p => p.GetComponentInChildren<FrameMaker>()).ToList();
        endingFrameMakers = endingChunkPrefabs.Select(
          p => p.GetComponentInChildren<FrameMaker>()).ToList();

        mapMaker.MakeCluster(startingFrameMakers, fillerFrameMakers, endingFrameMakers, entries);
        //openMapNodes = mapMaker.map.incompleteNodes;

        foreach(KeyValuePair<MapNode, FrameMaker> item in mapMaker.NodeToFrameMaker)
        {
            AddChunk(item.Key);
        }
    }

    private int chunkIndex = 0;
    void AddChunk(MapNode mapNode)
    {
        GameObject chunkPrefab = mapMaker.NodeToFrameMaker[mapNode].transform.root.gameObject;
        GameObject chunk = Instantiate(chunkPrefab, transform);
        chunk.name = "Chunk " + chunkIndex++;
        ChunkScript chunkScript = chunk.GetComponent<ChunkScript>();

        chunkScript.mapNode = mapNode;

        chunkScripts.Add(chunkScript);
    }

    public void Join(ClusterScript other)
    {
        MapNode newNode;
        bool success = this.mapMaker.TryJoin(other.mapMaker, transitionFrameMakers, out newNode);
        if (success == true)
        {
            AddChunk(newNode);
            joinedEntries++;
        }
    }

    public void PrintActiveChunks()
	{
        foreach(ChunkScript chunkScript in chunkScripts)
		{
			Debug.Log(chunkScript.gameObject.name + ": " + chunkScript.gameObject.activeSelf);
		}
	}
}
