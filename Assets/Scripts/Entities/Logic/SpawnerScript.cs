using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    public GameObject spawneePrefab;
    public Vector2 spawnTimeDelayRange = new Vector2(3,8);
    private EventManager eventManager;
    //exclusive upper bound
    //public Vector2Int numberOfSpawneesRange = new Vector2Int(0, 2);
    public float activatedChance = 1f;
    private bool isActivated = false;
    public int numberOfSpawnees = 1;

    //so that someday we can stop this coroutine
    private List<GameObject> spawnees = new List<GameObject>();

    void Awake()
    {
        eventManager = GameObject.FindWithTag("Event Manager").GetComponent<EventManager>();
        if(Random.Range(0.0f, 1.0f) < activatedChance)
            isActivated = true;
        //numberOfSpawnees = Random.Range(numberOfSpawneesRange.x, numberOfSpawneesRange.y);
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        if(isActivated)
            StartCoroutine(SpawnTick());
    }

    IEnumerator SpawnTick()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(spawnTimeDelayRange.x, spawnTimeDelayRange.y));
            TrySpawn();
        }
    }

    void TrySpawn()
    {
        //!!! CHECK IF THIS IS WORKING LATER
        spawnees.RemoveAll((s) => s == null);
        if (spawnees.Count < numberOfSpawnees)
        {
            spawnees.Add(Instantiate(spawneePrefab, transform.position, Quaternion.identity, transform.root));
            eventManager.TriggerEvent(EventType.OnSpawn, gameObject);
        }
    }
}
