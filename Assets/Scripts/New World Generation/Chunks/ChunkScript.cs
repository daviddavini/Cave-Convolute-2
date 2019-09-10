using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkScript : MonoBehaviour
{
    //public HallwayFrameMaker HallwayFrameMaker;

    private MapNode _mapNode;
    public MapNode mapNode
    {
        get { return _mapNode; }
        set {
            _mapNode = value;
            _mapNode.chunkScript = this;
            OnMapNodeGiven(value);

            //For debug purposes only
            foreach(MapNode m in _mapNode.Neighbors.Values)
            {
                neighboringIds.Add(m.id);
            }
            
        }
    }

    public List<int> neighboringIds = new List<int>();

    public System.Random _ChunkRandom;
    public System.Random ChunkRandom
    {
        get { return _ChunkRandom; }
        set
        {
            _ChunkRandom = value;
            OnChunkRandomSet(_ChunkRandom);
        }
    }

    private Chart _chart;
    public Chart chart
    {
      get {return _chart;}
      set {_chart = value; OnChartSet(value);}
    }

    public event Action<MapNode> OnMapNodeGiven = delegate { };
    public event Action<Chart> OnChartSet = delegate { };
    public event Action<System.Random> OnChunkRandomSet = delegate { };

    void Awake()
    {
        ChunkRandom = new System.Random();
    }

    public Frame TryMakeFrame(FrameRange frameRange)
    {
        return GetComponentInChildren<FrameMaker>().TryMakeFrame(frameRange);
    }
}
