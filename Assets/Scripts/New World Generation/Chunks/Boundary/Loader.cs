using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private ChunkScript chunkScript;
    private Swapper swapper;

    private Loader lastLoadOrigin;
    //private int depth = int.MaxValue;
    //private int maxDepth = 1;

    //private static float loadTimeDelay = 1;

    void Awake()
    {
        swapper = GetComponent<Swapper>();
        swapper.OnSwapped += CheckForLoad;
        chunkScript = GetComponentInParent<ChunkScript>();
    }

    void CheckForLoad(GameObject swappedGameObject)
    {
        if (swappedGameObject.CompareTag("Player"))
        {
            Debug.Log("Load Origin: " + chunkScript.gameObject.name);
            SpreadLoadAndUnload(0, 1, transform.position, this);
            // SpreadUnload(0, this);
            // SpreadLoad(transform.position, 0, this);
        }
    }

    void SpreadLoadAndUnload(int depth, int loadToDepth, Vector3 position, Loader loadOrigin)
    {
        if (depth <= loadToDepth){
            TryLoad(position, loadOrigin);
        } else {
            TryUnload(loadOrigin);
            return;
        }

        foreach(KeyValuePair<Edge, MapNode> item in chunkScript.mapNode.Neighbors)
        {
            ChunkScript neighborChunkScript = item.Value.chunkScript;
            Loader neighborLoader = neighborChunkScript.GetComponentInChildren<Loader>();
            Vector3 deltaPos = chunkScript.mapNode.frame.CenterToEntry(item.Key)
              - neighborChunkScript.mapNode.frame.CenterToEntry(item.Key.Opposite());
            neighborLoader.SpreadLoadAndUnload(depth+1, loadToDepth, position + deltaPos, loadOrigin);
        }
    }

    void TryLoad(Vector3 position, Loader loadOrigin)
    {
        this.lastLoadOrigin = loadOrigin;
        if(chunkScript.gameObject.activeSelf == true && chunkScript.transform.position == position) return;

        //Debug.Log("Load: " + chunkScript.gameObject.name);
        chunkScript.gameObject.SetActive(true);
        chunkScript.transform.position = position;
    }

    void TryUnload(Loader loadOrigin)
    {
        if(this.lastLoadOrigin == loadOrigin) return;

        //Debug.Log("Unload: " + chunkScript.gameObject.name);
        this.lastLoadOrigin = loadOrigin;
        chunkScript.gameObject.SetActive(false);
    }
}
