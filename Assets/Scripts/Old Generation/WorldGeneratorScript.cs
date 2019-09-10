using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//split into chunk init script and world generation script?
public class WorldGeneratorScript : MonoBehaviour
{
    public GameObject chunkPrefab;

    public readonly int seed = (int)System.DateTime.Now.Ticks;
    public System.Random random;

    public GameObject homeMapPrefab;
    public List<GameObject> mapPrefabsForGeneratedChunks;

    public GameObject playerPrefab;

    public int lastChunkIndex = 1;
    public MapScript homeMapScript;
    public List<MapScript> mapScripts;
    public GameObject player;

    //private LoadManagerScript loadManagerScript;

    void Awake()
    {
        random = new System.Random(seed);
        CreateChunks();
        InitChunks();
        GenerateAndConnectChunks();
        DeactivateChunks();

        //loadManagerScript = GameObject.FindWithTag("Load Manager").GetComponent<LoadManagerScript>();
        //loadManagerScript.Init(homeMapScript, mapScripts);
    }

    void CreateChunks()
    {
        GameObject chunk = CreateChunk(homeMapPrefab);
        player = Instantiate(playerPrefab, chunk.transform);
        homeMapScript = chunk.GetComponentInChildren<MapScript>();

        foreach (GameObject mapPrefab in mapPrefabsForGeneratedChunks)
        {
            CreateChunk(mapPrefab);
        }
    }

    GameObject CreateChunk(GameObject mapPrefab)
    {
        GameObject chunk = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity);
        Instantiate(mapPrefab, chunk.transform);
        MapScript mapScript = chunk.GetComponentInChildren<MapScript>();
        mapScripts.Add(mapScript);
        chunk.name = "Chunk "+lastChunkIndex++;
        return chunk;
    }

    void GenerateAndConnectChunks()
    {
        //!!! NOTICE THE UGLINESS. some serious refactoring to do
        Tools.ShuffleList<MapScript>(random, mapScripts);
        foreach(MapScript mapScript in mapScripts)
        {
            mapScript.ConnectConnectors();
        }
        foreach(MapScript mapScript in mapScripts)
        {
            mapScript.GenerateMapArray();
        }
        foreach(MapScript mapScript in mapScripts)
        {
            mapScript.FitTogetherWithPaths();
        }
        foreach(MapScript mapScript in mapScripts)
        {
            mapScript.Generate();
        }
    }

    void InitChunks()
    {
        foreach(MapScript mapScript in mapScripts)
        {
            mapScript.Init();
        }
    }

    void DeactivateChunks()
    {
        foreach(MapScript mapScript in mapScripts)
        {
            if(mapScript != homeMapScript)
            {
                mapScript.chunk.SetActive(false);
            }
        }
    }

    public bool TryConnectConnector(ConnectorScript connectorScript)
    {
        MapScript mapScript = connectorScript.mapScript;

        int i = 0;
        while(i++ < 100)
        {
            MapScript otherMapScript = mapScripts[random.Next(0, mapScripts.Count)];
            if (mapScript == otherMapScript){continue;}

            if(otherMapScript.TryConnectTo(connectorScript)){return true;}
        }
        Debug.Log("Couldn't find pair for " + connectorScript.side + " connector");
        return false;
    }
}
