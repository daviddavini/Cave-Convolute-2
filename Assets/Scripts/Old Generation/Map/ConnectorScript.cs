using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side {None, Left, Right, Top, Bottom};

public class ConnectorScript : MonoBehaviour
{
    public static Dictionary<Side, Side> oppositeSide = new Dictionary<Side, Side>(){
        {Side.Left, Side.Right}, {Side.Right, Side.Left}, {Side.Top, Side.Bottom}, {Side.Bottom, Side.Top}
    };

    public Side side = Side.None;

    //public int pathSeed;

    //Communicating with parent chunk and World Generator
    private WorldGeneratorScript worldGeneratorScript;
    public GameObject chunk;
    public MapScript mapScript;
    private MapArrayGenerator mapArrayGenerator;

    private EdgeCollider2D edgeCollider;

    //Keeping track of connections
    [HideInInspector]
    public bool isConnected = false;
    public bool isDominant = false;
    private ConnectorScript connectedConnectorScript;

    private Vector2 bufferMultipliers = new Vector2(1.5f, 1.5f);
    private Rect enlargedScreenRect;

    void Awake()
    {
        //Debug.Log("Connector Awake " + (GetInstanceID()));
        worldGeneratorScript = FindObjectOfType<WorldGeneratorScript>();
        //Change to tag later
        chunk = transform.root.gameObject;
        mapArrayGenerator = GetComponentInParent<MapArrayGenerator>();
        mapScript = GetComponentInParent<MapScript>();

        edgeCollider = GetComponent<EdgeCollider2D>();

        Vector2 screenSize = new Vector2 (Screen.width, Screen.height);
        Vector2 enlargedScreenSize = new Vector2 (bufferMultipliers.x*screenSize.x, bufferMultipliers.y*screenSize.y);
        enlargedScreenRect = new Rect(-(enlargedScreenSize - screenSize)/2, enlargedScreenSize);
    }

    public void Init(Side side)
    {
        //Debug.Log("Connector Init " + (GetInstanceID()));
        this.side = side;
    }

    public void GeneratePath()
    {
        if (isDominant && isConnected)
        {
            Vector3 connectorLocalPosition;
            Vector3 otherConnectorLocalPosition;
            if(!isConnected) Debug.Log("Unconnected Path Generation Attempt");
            //pathSeed = worldGeneratorScript.random.Next();
            mapArrayGenerator.GeneratePathTo(side,
              connectedConnectorScript.mapArrayGenerator,
              out connectorLocalPosition, out otherConnectorLocalPosition);

            // Debug.Log(connectorLocalPosition);
            // Debug.Log(otherConnectorLocalPosition);

            transform.localPosition = connectorLocalPosition;
            connectedConnectorScript.transform.localPosition = otherConnectorLocalPosition;
        }
    }

    public void FitTogetherWithPath()
    {
        if (!isDominant && isConnected)
        {
            //Debug.Log("Clipping");
            mapArrayGenerator.FitTogetherWithAt(connectedConnectorScript.mapArrayGenerator,
              transform.localPosition, connectedConnectorScript.transform.localPosition);
        }
    }

    public void TryConnect()
    {
        //add consideration for posibility for failure?
        if(!isConnected)
        {
            if(worldGeneratorScript.TryConnectConnector(this))
            {
                isDominant = true;
            }
        }
    }

    public bool TryConnectTo(ConnectorScript otherConnectorScript)
    {
        if (!isConnected && side == oppositeSide[otherConnectorScript.side])
        {
            otherConnectorScript.ConnectTo(this);
            this.ConnectTo(otherConnectorScript);
            return true;
        }
        return false;
    }

    void ConnectTo(ConnectorScript otherConnectorScript)
    {
        isConnected = true;
        connectedConnectorScript = otherConnectorScript;
    }

    public void CreateEdgeColliders(Vector2[] sidePoints)
    {
        Vector2[] localSidePoints = new Vector2[sidePoints.Length];
        for (int i = 0; i < sidePoints.Length; i++) {
            localSidePoints[i] = sidePoints[i] - (Vector2)transform.localPosition;
        }
        edgeCollider.points = localSidePoints;
    }

    void Update()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        bool amIInEnlargedScreen = IsOnScreen(screenPosition);
        //bool isCameraInMyChunk = IsInMyChunk(Camera.main.transform.position);
        if (mapScript.IsInBoundingRect(Camera.main.transform.position))
        {
            if (amIInEnlargedScreen)
            {
                TryConnect();
                if (isConnected)
                {
                    TryLoadConnectedChunk();
                }
            } else {
                if (isConnected)
                {
                    TryUnloadConnectedChunk();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //put colliding object in whatever map it is in, if ambiguous
        if (mapScript.IsInBoundingRect(other.attachedRigidbody.transform.position))
        {
            //Might be bugs in here
            if(other.attachedRigidbody.transform.root != chunk.transform)
            {
                Debug.Log("Swapped To My Chunk "+other.gameObject);
                other.attachedRigidbody.transform.SetParent(chunk.transform);
            }
        }
    }

    void TryLoadConnectedChunk()
    {
        //Debug.Log(connectedChunk);
        if(!connectedConnectorScript.chunk.activeSelf){
            Debug.Log("I'M LOADING " + side + gameObject);
            connectedConnectorScript.chunk.SetActive(true);
            connectedConnectorScript.chunk.transform.position +=
              transform.position - connectedConnectorScript.transform.position;
        }
    }
    void TryUnloadConnectedChunk()
    {
        if(connectedConnectorScript.chunk.activeSelf
            && connectedConnectorScript.transform.position == transform.position){
          Debug.Log("I'M UNLOADING " + side + gameObject);
          connectedConnectorScript.chunk.SetActive(false);
        }
    }

    public bool IsOnScreen(Vector2 screenPosition)
    {
        return enlargedScreenRect.Contains(screenPosition);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 enlargedScreenWorldSize = Camera.main.ScreenToWorldPoint(enlargedScreenRect.max)
          - Camera.main.ScreenToWorldPoint(enlargedScreenRect.min);
        Vector3 toForceLoad = Camera.main.transform.position;
        Gizmos.DrawWireCube(Camera.main.transform.position, enlargedScreenWorldSize);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(edgeCollider.points[0]+(Vector2)transform.position,
          edgeCollider.points[1]+(Vector2)transform.position);
    }
}
