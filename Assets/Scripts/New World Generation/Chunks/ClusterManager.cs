using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterManager : MonoBehaviour
{
    public GameObject startingClusterPrefab;
    public List<GameObject> clusterPrefabs;

    private List<ClusterScript> clusterScripts;

    void Awake()
    {
        clusterScripts = new List<ClusterScript>();
        foreach(GameObject clusterPrefab in clusterPrefabs)
        {
            AddCluster(clusterPrefab);
        }
        JoinClusterScripts();
    }

    void JoinClusterScripts()
    {
        for (int i = 0; i < clusterScripts.Count; i++)
        {
            int entriesToFill = clusterScripts[i].UnjoinedEntries;
            for (int j = i+1; j < clusterScripts.Count; j++)
            {
                if (clusterScripts[j].UnjoinedEntries > 0)
                {
                    clusterScripts[i].Join(clusterScripts[j]);
                    entriesToFill--;
                }

            }

        }
    }

    void Start()
    {
        foreach (ClusterScript clusterScript in clusterScripts)
        {
            clusterScript.DeactivateChunks();
        }
        clusterScripts[0].chunkScripts[0].gameObject.SetActive(true);
		//PrintActiveChunks();
    }

    public void PrintActiveChunks()
	{
		foreach (ClusterScript clusterScript in clusterScripts)
		{
			clusterScript.PrintActiveChunks();
		}
	}

    void AddCluster(GameObject clusterPrefab)
    {
        GameObject cluster = Instantiate(clusterPrefab);
        clusterScripts.Add(cluster.GetComponent<ClusterScript>());
    }
}
