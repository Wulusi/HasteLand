using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Transform spawnPos, despawnPos;

    public GameObject[] spawnReference;

    private GameObject despawner;

    private PoolManager poolManager;

    private bool isLevelFinished;

    // Start is called before the first frame update
    void Start()
    {
        poolManager = PoolManager.Instance;

        //StartCoroutine(SpawnIntervals());
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void FixedUpdate()
    {
        SpawnTerrain();
    }

    private void SpawnTerrain()
    {
        float totalTime = Time.time;
        float duration = 4f;

        //Spawn a new section of the map every 4 seconds
        if(totalTime % duration == 0)
        {
            spawnMapSections();
        }
    }

    IEnumerator SpawnIntervals()
    {
        float totalTime = Time.time;

        while (!isLevelFinished)
        {
            while (Mathf.Repeat(totalTime, 4) == 0)
            {
                Debug.Log("Spawning Tile " + totalTime);
                //Not sure why this is giving null ref
                spawnMapSections();
                yield return null;
            }
        }
    }

    void spawnMapSections()
    {
        var terrain = spawnReference[Random.Range(0, spawnReference.Length)];

        bool isFirstSpawn = false;

        if (!isFirstSpawn)
        {
            Debug.Log("terrain name is " + terrain);

            poolManager.SpawnFromPool(terrain.name, spawnPos.transform.position, Quaternion.identity);
        } else
        {
            GameObject lastTerrain = terrain;
            if(terrain != lastTerrain)
            {
                poolManager.SpawnFromPool(terrain.name, spawnPos.transform.position, Quaternion.identity);
            } else
            {
                poolManager.SpawnFromPool(terrain.name, spawnPos.transform.position, Quaternion.identity);
            }

        }
    }
}
