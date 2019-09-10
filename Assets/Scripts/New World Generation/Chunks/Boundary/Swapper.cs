using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swapper : MonoBehaviour
{
    private ChunkScript chunkScript;
    private BoxCollider2D boxCollider;
    public event Action<GameObject> OnSwapped = delegate {};

    //public List<Swapper> neighborSwappers;


    void Awake()
    {
        chunkScript = GetComponentInParent<ChunkScript>();
        chunkScript.OnMapNodeGiven += mapNode => Make(mapNode.frame);
    }

    void Make(Frame frame)
    {
        MakeBoxCollider2D(frame, 0, ref boxCollider);
    }

    void MakeBoxCollider2D(Frame frame, float buffer, ref BoxCollider2D boxCollider)
    {
        if (boxCollider != null)
            Destroy(boxCollider);
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(frame.Width-buffer, frame.Height-buffer);
        boxCollider.isTrigger = true;
    }

    void TrySwapInto(GameObject enteringGameObject)
    {
        

        if(!boxCollider.bounds.Contains(enteringGameObject.transform.position)) return;
        if(enteringGameObject.transform.FindParentWithTag("Chunk") == chunkScript.transform) return;

        if (enteringGameObject.CompareTag("Player"))
        {
            //Debug.Log("Swapped: " + enteringGameObject.name + chunkScript.gameObject);
            GameObject.FindWithTag("Cluster Manager").GetComponent<ClusterManager>().PrintActiveChunks();
        }

        enteringGameObject.transform.SetParent(chunkScript.transform);
        OnSwapped(enteringGameObject);
    } 

    void OnTriggerEnter2D(Collider2D collider)
    {
        TrySwapInto(collider.attachedRigidbody.gameObject);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        foreach(KeyValuePair<Edge, MapNode> item in chunkScript.mapNode.Neighbors)
        {
            Swapper neighborSwapper = item.Value.chunkScript.GetComponentInChildren<Swapper>();
            neighborSwapper.TrySwapInto(collider.attachedRigidbody.gameObject);
        }
    }
}
