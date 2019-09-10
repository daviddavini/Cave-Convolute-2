using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScript : MonoBehaviour
{
    public List<Side> sides = new List<Side>(){Side.Left, Side.Right};

    //public int mapSeed;

    public Rect localBoundingRect = Rect.zero;
    public Dictionary<Side, Vector2[]> sidePoints = new Dictionary<Side, Vector2[]>();

    public GameObject chunk;
    private MapArrayGenerator mapArrayGenerator;
    private WorldGeneratorScript worldGeneratorScript;
    //better name: tilemap creator?
    private SceneryGenerator[] sceneryGenerators;
    private TilemapGenerator tilemapGenerator;

    public GameObject connectorPrefab;
    public List<ConnectorScript> connectorScripts = new List<ConnectorScript>();

    //public bool isCameraInBoundingRect = false;

    void Awake()
    {
        //Debug.Log("Map Awake " + GetInstanceID());
        chunk = transform.root.gameObject;
        mapArrayGenerator = GetComponent<MapArrayGenerator>();
        worldGeneratorScript = FindObjectOfType<WorldGeneratorScript>();
        sceneryGenerators = GetComponents<SceneryGenerator>();
        tilemapGenerator = GetComponent<TilemapGenerator>();
    }

    public void Init()
    {
        //Debug.Log("Map Init " + GetInstanceID());
        //mapSeed = worldGeneratorScript.random.Next();
        mapArrayGenerator.Init(worldGeneratorScript.random.Next());
        tilemapGenerator.Init(worldGeneratorScript.random.Next());
        foreach(SceneryGenerator sceneryGenerator in sceneryGenerators)
        {
            sceneryGenerator.Init(worldGeneratorScript.random.Next());
        }
        foreach(Side side in sides)
        {
            InitConnector(side);
        }
    }

    void InitConnector(Side side)
    {
        GameObject connector = Instantiate(connectorPrefab, transform);
        ConnectorScript connectorScript = connector.GetComponent<ConnectorScript>();
        connectorScript.Init(side);
        connectorScripts.Add(connectorScript);
    }

    public void GenerateMapArray()
    {
        mapArrayGenerator.GenerateCenter();
        foreach(ConnectorScript connectorScript in connectorScripts)
        {
            connectorScript.GeneratePath();
        }
        mapArrayGenerator.GenerateWalls();
    }

    public void FitTogetherWithPaths()
    {
        foreach(ConnectorScript connectorScript in connectorScripts)
        {
            connectorScript.FitTogetherWithPath();
        }
    }

    public void Generate()
    {
        //centerSeed = worldGeneratorScript.random.Next();
        tilemapGenerator.CreateTilemap();
        foreach(SceneryGenerator sceneryGenerator in sceneryGenerators)
        {
            sceneryGenerator.GenerateEntities();
        }

        CreateLocalBoundingRect();
        CreateSidePoints();
        foreach(ConnectorScript connectorScript in connectorScripts)
        {
            connectorScript.CreateEdgeColliders(sidePoints[connectorScript.side]);
        }
    }

    public void ConnectConnectors()
    {
        foreach(ConnectorScript connectorScript in connectorScripts)
        {
            connectorScript.TryConnect();
        }
    }

    public bool TryConnectTo(ConnectorScript otherConnectorScript)
    {
        foreach(ConnectorScript connectorScript in connectorScripts)
        {
            if(connectorScript.TryConnectTo(otherConnectorScript)){return true;}
        }
        return false;
    }

    void CreateLocalBoundingRect()
    {
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
        Vector2 bottomLeftCorner = Vector2.zero;
        Vector2 topRightCorner = Vector2.zero;
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.CompressBounds();
            bottomLeftCorner = Vector2.Min(bottomLeftCorner, (Vector2)(Vector3)tilemap.cellBounds.min);
            topRightCorner = Vector2.Max(topRightCorner, (Vector2)(Vector3)tilemap.cellBounds.max);
            //boundingSize = Vector2.Max(boundingSize, (Vector3)tilemap.cellBounds);
        }
        //For some reason the decimals need to be rounded down...
        localBoundingRect.min = bottomLeftCorner;
        localBoundingRect.max = topRightCorner;
        // Vector2 position = (Vector2)transform.localPosition-boundingSize/2;
        // localBoundingRect.position = new Vector2((int)position.x, (int)position.y);
        // localBoundingRect.size = boundingSize;
    }

    void CreateSidePoints()
    {
        sidePoints[Side.None] = new Vector2[] {Vector2.zero, Vector2.zero};
        sidePoints[Side.Left] = new Vector2[] {
          new Vector2(localBoundingRect.xMin, localBoundingRect.yMin),// + (Vector2)transform.position,
          new Vector2(localBoundingRect.xMin, localBoundingRect.yMax)// + (Vector2)transform.position
        };
        sidePoints[Side.Right] = new Vector2[] {
          new Vector2(localBoundingRect.xMax, localBoundingRect.yMin),// + (Vector2)transform.position,
          new Vector2(localBoundingRect.xMax, localBoundingRect.yMax)// + (Vector2)transform.position
        };
        sidePoints[Side.Top] = new Vector2[] {
          new Vector2(localBoundingRect.xMin, localBoundingRect.yMax),// + (Vector2)transform.position,
          new Vector2(localBoundingRect.xMax, localBoundingRect.yMax)// + (Vector2)transform.position
        };
        sidePoints[Side.Bottom] = new Vector2[] {
          new Vector2(localBoundingRect.xMin, localBoundingRect.yMin),// + (Vector2)transform.position,
          new Vector2(localBoundingRect.xMax, localBoundingRect.yMin)// + (Vector2)transform.position
        };
    }

    void Update()
    {
        // if (IsInBoundingRect(Camera.main.transform.position))
        // {
        //     foreach(ConnectorScript connectorScript in connectorScripts)
        //     {
        //
        //     }
        //     if (amIInEnlargedScreen)
        //     {
        //         TryConnect();
        //         if (isConnected)
        //         {
        //             TryLoadConnectedChunk();
        //         }
        //     } else {
        //         if (isConnected)
        //         {
        //             TryUnloadConnectedChunk();
        //         }
        //     }
        // }
    }
    public bool IsInBoundingRect(Vector3 position)
    {
        return localBoundingRect.Contains(position-transform.position);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
