using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class Node<T>
// {
//     public List<Node<T>> Neighbors {get; protected set;}
// }
//
// public class Cluster : IGraphNode
// {
//     public List<Node<T>> Neighbors {get; protected set;}
// }
//
// public class Graph<Node>
// {
//     public List<Node<T>> nodes;
//
//     public Graph()
//     {
//
//     }
// }

// public class Map
// {
//     public List<MapNode> MapNodes {get;}
//
//     public Map()
//     {
//         this.MapNodes = new List<MapNode>();
//     }
//
//     public Map(List<MapNode> MapNodes)
//     {
//         this.MapNodes = new List<MapNode>();
//         AddMapNodes(MapNodes);
//     }
//
//     public void AddMapNode(MapNode mapNode, List<MapNode> concurrentMapNodes = null)
//     {
//         foreach (KeyValuePair<Edge, MapNode> item in mapNode.Neighbors)
//         {
//             if(!MapNodes.Contains(item.Value) &&
//               (concurrentMapNodes == null || !concurrentMapNodes.Contains(item.Value)))
//             {throw new ArgumentException("Neighbor of added MapNode not contained in Map");}
//         }
//         MapNodes.Add(mapNode);
//     }
//
//     public void AddMapNodes(List<MapNode> MapNodes)
//     {
//         foreach (MapNode mapNode in MapNodes)
//         {
//             AddMapNode(mapNode, MapNodes);
//         }
//     }
//
//     //Completeness
//     public int EntryCount { get {
//         int entryCount = 0;
//         foreach(MapNode mapNode in MapNodes) {entryCount += mapNode.EntryCount;}
//         return entryCount;
//     }}
//
//     public int JoinedEntryCount { get {
//         int joinedEntryCount = 0;
//         foreach(MapNode mapNode in MapNodes) {joinedEntryCount += mapNode.JoinedEntryCount;}
//         return joinedEntryCount;
//     }}
//
//     public bool IsComplete {get {return this.JoinedEntryCount == this.EntryCount;}}
//
//     public List<MapNode> IncompleteNodes {get {return MapNodes.FindAll(n => !n.IsComplete);}}
//
//     public List<MapNode> NodesWithUnjoinedEntry(Edge edge) {
//         return MapNodes.FindAll(n => !n.IsEntryJoined(edge));
//     }
//
//     //Connectedness
//     public bool IsConnected { get {
//         return MapNodes.Count == 0 || MapNodes.Count == MapNodes[0].ConnectedComponent.Count;
//     }}
//
//     public List<List<MapNode>> ConnectedComponents { get {
//         List<List<MapNode>> connectedComponents = new List<List<MapNode>>();
//         List<MapNode> usedNodes = new List<MapNode>();
//         while (usedNodes.Count < MapNodes.Count)
//         {
//             List<MapNode> connectedComponent = MapNodes.Find(n => !usedNodes.Contains(n)).ConnectedComponent;
//             usedNodes.AddRange(connectedComponent);
//             connectedComponents.Add(connectedComponent);
//         }
//         return connectedComponents;
//     }}
//
//     public override string ToString()
//     {
//         return "Map(Size: " + MapNodes.Count + ", ConnectedComponents: "+ ConnectedComponents.Count +
//           ", UnjoinedEntries: "+ (EntryCount-JoinedEntryCount) + ")";
//     }
// }
