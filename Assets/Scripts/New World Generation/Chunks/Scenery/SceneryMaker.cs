using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneryPrefab
{
    public GameObject prefab;
    public float weight = 1;
}

public class SceneryMaker : MonoBehaviour
{
    public SceneryPrefab[] sceneryPrefabs;
    public float absentProbability;
    public float maxOffset;
    protected System.Random random;
    protected ChunkScript chunkScript;
    public float edgeBuffer;
    protected Rect sceneryRect;

    protected virtual void Awake()
    {
        chunkScript = GetComponentInParent<ChunkScript>();
        chunkScript.OnChartSet += Make;
        InitRandom(chunkScript.ChunkRandom);
    }

    private void InitRandom(System.Random chunkRandom)
    {
        random = new System.Random(chunkRandom.Next());
    }

    protected virtual void Make(Chart chart)
    {
        MakeSceneryRect(chart);
    }

    protected GameObject GetRandomSceneryPrefab()
    {
        float totalWeight = sceneryPrefabs.Sum(x => x.weight);
        float prob = (float)random.NextDouble();
        float currentWeight = 0;
        foreach(SceneryPrefab sceneryPrefab in sceneryPrefabs)
        {
            currentWeight += sceneryPrefab.weight;
            if(prob < currentWeight/totalWeight)
                return sceneryPrefab.prefab;
        }
        Debug.Log("No Scenery Prefab was made");
        return null;
    }

    private void MakeSceneryRect(Chart chart)
    {
        Vector2 edgeBuffer = new Vector2(this.edgeBuffer, this.edgeBuffer);
        sceneryRect = new Rect(chart.BoundingRect.position + edgeBuffer,
          chart.BoundingRect.size - 2*edgeBuffer);
    }

    protected Vector2 PercentagePosToLocalPos(Vector2 percentagePos)
    {
        return sceneryRect.position + Tools.MultiplyComponents(sceneryRect.size, percentagePos);
    }

    protected Rect PercentageRectToLocalRect(Rect percentageRect)
    {
        return new Rect(sceneryRect.position + Tools.MultiplyComponents(sceneryRect.size, percentageRect.position),
          Tools.MultiplyComponents(sceneryRect.size, percentageRect.size));
    }

    protected void MakeScenery(Vector3 localPos)
    {
        GameObject prefab = GetRandomSceneryPrefab();
        if(random.NextDouble() < absentProbability) return;
        if(prefab == null) return;

        Vector3 offset = Tools.RandomVector(random, new Vector3(-maxOffset, -maxOffset, 0),
          new Vector3(maxOffset, maxOffset, 0));

        Instantiate(prefab, transform.position + localPos + offset, Quaternion.identity,
          transform);
    }
}
