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

        despawner = transform.GetChild(1).gameObject;

        StartCoroutine(SpawnIntervals());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnIntervals()
    {
        while (!isLevelFinished)
        {

            float duration = 4.0f;
            float totalTime = 0f;

            while (totalTime % duration == 0)
            {
                totalTime = Time.time;
                //do nothing;
                yield return null;
            }
            Debug.Log("total time is " + totalTime);
            //Spawn tile here
            spawnMapSections();
            //Reset Spawntimer;
            totalTime = 0;
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
