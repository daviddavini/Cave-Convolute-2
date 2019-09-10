using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    public int id;
    static int idCount = 0;

    public ChunkScript chunkScript;

    public Frame frame {get;}
    //public Area area {get;}
    private Dictionary<Edge, MapNode> neighbors; //prevent from modifying after declaring
    public Dictionary<Edge, MapNode> Neighbors {get {return neighbors;}}

    public MapNode(Frame frame)
    {
        this.id = idCount++;
        this.frame = frame;
        this.neighbors = new Dictionary<Edge, MapNode>();
    }

    //ensure that joins are always mutual by hiding the one-way version
    private void AddNeighbor(Edge edge, MapNode neighbor)
    {
        if(this == neighbor)
            throw new ArgumentException("Cannot join MapNode with itself");
        //To keep or not to keep? requires new chunk loading algorithm
        if(this.IsJoinedTo(neighbor))
            throw new ArgumentException("Cannot join MapNodes twice in separate locations");

        if(neighbors.ContainsKey(edge))
            throw new ArgumentException("MapNode is already joined at edge");
        if(!frame.CompatibleWith(edge, neighbor.frame))
            throw new ArgumentException("MapNode's frame is incompatible with added neighbor frame at edge");

        neighbors[edge] = neighbor;
    }

    public void OneWayJoin(Edge edge, MapNode neighbor)
    {
        this.AddNeighbor(edge, neighbor);
    }

    public void Join(Edge edge, MapNode neighbor)
    {
        this.AddNeighbor(edge, neighbor);
        neighbor.AddNeighbor(edge.Opposite(), this);
    }

    //Completeness
    public int EntryCount {get {return frame.EntryCount;}}
    public int JoinedEntryCount {get {return neighbors.Count;}}
    public List<Edge> UnjoinedEntries {get {return frame.EntryEdges.FindAll(e => !neighbors.ContainsKey(e));}}
    public bool IsComplete {get {return this.EntryCount == this.JoinedEntryCount;}}

    public bool IsEntryJoined(Edge edge)
    {
        return !frame.EntryEdges.Contains(edge) || neighbors.ContainsKey(edge);
    }

    //Connectedness
    public bool IsJoinedTo(MapNode mapNode)
    {
        foreach(KeyValuePair<Edge, MapNode> item in neighbors)
        {
            if(mapNode == item.Value)
                return true;
        }
        return false;
    }
    public bool IsConnectedTo(MapNode mapNode) => this.ConnectedComponent.Contains(mapNode);

    public List<MapNode> ConnectedComponent { get{
        List<MapNode> connectedNodes = new List<MapNode>(){this};
        this.SearchForAndAddConnectedNodesTo(connectedNodes);
        return connectedNodes;
    }}
    public void SearchForAndAddConnectedNodesTo(List<MapNode> connectedNodes)
    {
        foreach(KeyValuePair<Edge, MapNode> item in neighbors)
        {
            if (!connectedNodes.Contains(item.Value)) {
                connectedNodes.Add(item.Value);
                item.Value.SearchForAndAddConnectedNodesTo(connectedNodes);
            }
        }
    }

    public override string ToString()
    {
        string str = "MapNode(Entries: {";
        foreach (Edge edge in frame.EntryEdges)
        {
            str += neighbors.ContainsKey(edge) ? edge.Abbreviation() : edge.Abbreviation().ToLower();
            str += ", ";
        }
        str = str.Substring(0, str.Length-2) + "})";
        return str; //+ "} " + frame.ToString() + "}";
    }

    public static MapNode Test1 = new MapNode(Frame.Test1);
    public static MapNode Test2 = new MapNode(Frame.Test2);
    public static MapNode Test3 = new MapNode(Frame.Test3);
    public static MapNode Test4 = new MapNode(Frame.Test4);
    public static MapNode Test5 = new MapNode(Frame.Test5);
}
