using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Transform spawnPos, despawnPos;

    public GameObject[] spawnReference;

    private PoolManager poolManager;

    private bool isLevelFinished;

    // Start is called before the first frame update
    void Start()
    {
        poolManager = PoolManager.Instance;

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

            float duration = 3f;
            float totalTime = 0f;

            while (totalTime <= duration)
            {
                totalTime += Time.deltaTime;
                //do nothing;
                yield return null;
            }
            //Spawn tile here
            spawnMapSections();
            //Reset Spawntimer;
            totalTime = 0;
        }
    }

    void spawnMapSections()
    {
        var terrainName = spawnReference[Random.Range(0, spawnReference.Length)].ToString();

        poolManager.SpawnFromPool(terrainName, spawnPos.transform.position, Quaternion.identity);
    }
}
