using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker
{
    private Map map;
    public Dictionary<MapNode, FrameMaker> NodeToFrameMaker {get; private set;}
    private System.Random random;

    //private List<FrameMaker> startingFrameMakers;
    //private List<FrameMaker> fillerFrameMakers;

    public MapMaker(Map map = null, System.Random random = null)
    {
        this.random = random ?? new System.Random();
        this.map = map ?? new Map();
        NodeToFrameMaker = new Dictionary<MapNode, FrameMaker>();
    }

    private void StartMap(List<FrameMaker> startingFrameMakers)
    {
        foreach(FrameMaker frameMaker in startingFrameMakers)
        {
            TryAddNodeToMap(frameMaker, FrameRange.All, EntryCountPreference.Max);
        }
    }

    public void MakeCluster(List<FrameMaker> startingFrameMakers, List<FrameMaker> fillerFrameMakers, List<FrameMaker> endingFrameMakers,
      int leftoverEntries)
    {
        StartMap(startingFrameMakers);
        CompleteMap(fillerFrameMakers, leftoverEntries);
        CapMap(endingFrameMakers);
    }

    public void CapMap(List<FrameMaker> endingFrameMakers)
    {
        if(endingFrameMakers.Count == 0)
        {
            Debug.Log("No ending frame makers, so avoiding Capping process");
            return;
        }
        List<MapNode> incompleteNodes = new List<MapNode>(map.IncompleteNodes);
        foreach(MapNode incompleteNode in incompleteNodes)
        {
            while(!incompleteNode.IsComplete)
            {
                TryCapNode(incompleteNode, endingFrameMakers);
            }
        }
    }

    public bool TryJoin(MapMaker other, List<FrameMaker> transitionFrameMakers, out MapNode newNode)
    {
        List<MapNode> chosenNodes = new List<MapNode>(){
            this.map.IncompleteNodes.RandomElement(random),
            other.map.IncompleteNodes.RandomElement(random)
        };
        bool successfulConnection = TryConnectTwoNodes(chosenNodes, transitionFrameMakers, out newNode);
        if(!successfulConnection)
        {
            Debug.Log("Transition Frame Makers uncompatible. Join failed");
        }
        return successfulConnection;
    }

    private void CompleteMap(List<FrameMaker> fillerFrameMakers, int leftoverEntries = 0, bool onlyCap = false)
    {
        Debug.Log("Entering complete map");
        if (onlyCap == true)
            Debug.Log("Type of Complete: cap map");
        int i = 0;
        while(true)
        {
            Debug.Log(map);
            //map.PrintMapNodes();
            if (i++ >= 30)
            {
                //Debug.Log("Map Maker was unable to complete map");
                return;
            }

            List<MapNode> incompleteNodes = map.IncompleteNodes;
            Debug.Log("# incomplete nodes: " + incompleteNodes.Count);
            if(map.UnjoinedEntryCount < leftoverEntries)
            {
                Debug.Log("Whoops! Went under the entry requirement with only " + map.UnjoinedEntryCount + " entries");
                return;
            }
            if (map.UnjoinedEntryCount == leftoverEntries)
            {
                Debug.Log("Satisfied the leftover entry requirement with " + map.UnjoinedEntryCount + " entries");
                map.PrintIncompleteMapNodes();
                return;
            }
            if (!onlyCap && incompleteNodes.Count > 1 && map.UnjoinedEntryCount > leftoverEntries + 1)
            {
                //Debug.Log("Action: Trying to connect two nodes");
                TryConnectTwoNodesFrom(incompleteNodes, fillerFrameMakers);
            } else
            {
                //Debug.Log("Action: Trying to cap a node");
                TryCapNodeFrom(incompleteNodes, fillerFrameMakers);
            }
        }
    
    }

    private bool TryCapNodeFrom(List<MapNode> incompleteNodes, List<FrameMaker> fillerFrameMakers)
    {
        MapNode chosenNode = incompleteNodes.RandomElement(random);
        return TryCapNode(chosenNode, fillerFrameMakers);

        //if (incompleteNodes.Any(o => o != incompleteNodes[0]))
    }

    private bool TryCapNode(MapNode chosenNode, List<FrameMaker> fillerFrameMakers)
    {
        Edge chosenEntry = chosenNode.UnjoinedEntries.RandomElement(random);

        FrameRange frameRange = chosenNode.frame.FrameRangeOff(chosenEntry);
        //frameRange = frameRange.WithOptionalEntriesRemoved();

        FrameMaker frameMaker = fillerFrameMakers.RandomElement(random);

        MapNode newNode = TryAddNodeToMap(frameMaker, frameRange, EntryCountPreference.Min);
        if (newNode == null) return false;

        chosenNode.Join(chosenEntry, newNode);
        //map.AddMapNode(chosenNode);
        return true;
    }

    private bool TryConnectTwoNodes(List<MapNode> chosenNodes, List<FrameMaker> fillerFrameMakers, out MapNode newNode)
    {
        newNode = null;
        List<Edge> chosenEntries = new List<Edge>() {
          chosenNodes[0].UnjoinedEntries.RandomElement(random),
          chosenNodes[1].UnjoinedEntries.RandomElement(random)
        };
        if (chosenEntries[0] == chosenEntries[1]) return false;

        if(chosenEntries[0] == chosenEntries[1])
        {
            Debug.Log("Entries are the same edge, so not going to connect");
        }
        //else
        //{
        //    frameRange = chosenNodes[0].frame.FrameRangeOff(chosenEntries[0])
        //        .Intersection(chosenNodes[1].frame.FrameRangeOff(chosenEntries[1]));
        //    frameRange = frameRange.WithOptionalEntriesRemoved();
        //}
        FrameRange frameRange = chosenNodes[0].frame.FrameRangeOff(chosenEntries[0])
          .Intersection(chosenNodes[1].frame.FrameRangeOff(chosenEntries[1]));
        //frameRange = frameRange.WithOptionalEntriesRemoved();

        FrameMaker frameMaker = fillerFrameMakers.RandomElement(random);
        newNode = TryAddNodeToMap(frameMaker, frameRange, EntryCountPreference.Min);
        if (newNode == null) return false;

        Debug.Log("Connected: " + map.MapNodes.IndexOf(chosenNodes[0]) + " + "
          + map.MapNodes.IndexOf(chosenNodes[1]));
        chosenNodes[0].Join(chosenEntries[0], newNode);
        chosenNodes[1].Join(chosenEntries[1], newNode);
        //map.AddMapNodes(chosenNodes);
        return true;
    }

    private bool TryConnectTwoNodesFrom(List<MapNode> incompleteNodes, List<FrameMaker> fillerFrameMakers)
    {
        //List<MapNode> chosenNodes = incompleteNodes.RandomElements(2, random);
        List<MapNode> chosenNodes = new List<MapNode>(){incompleteNodes.RandomElement(random)};
        //why is this line so important again?
        List<MapNode> unjoinedIncompleteNodes = incompleteNodes
          .Where(n => !(n == chosenNodes[0]) && !n.IsJoinedTo(chosenNodes[0])).ToList();
        List<MapNode> unconnectedIncompleteNodes = unjoinedIncompleteNodes
          .Where(n => !n.IsConnectedTo(chosenNodes[0])).ToList();
        if(unconnectedIncompleteNodes.Count > 0)
            chosenNodes.Add(unconnectedIncompleteNodes.RandomElement(random));
        else
            chosenNodes.Add(unjoinedIncompleteNodes.RandomElement(random));
        MapNode newNode;
        return TryConnectTwoNodes(chosenNodes, fillerFrameMakers, out newNode);
    }

    private MapNode TryAddNodeToMap(FrameMaker frameMaker, FrameRange frameRange, EntryCountPreference ecp)
    {
        //Debug.Log("trying to add node");
        Frame frame = frameMaker.TryGetFrame(frameRange, ecp);
        if (frame == null) return null;

        Debug.Log("Adding frame with " + frame.EntryCount + " entries");
        MapNode newNode = new MapNode(frame);
        NodeToFrameMaker[newNode] = frameMaker;
        map.AddMapNode(newNode);
        return newNode;
    }
}
