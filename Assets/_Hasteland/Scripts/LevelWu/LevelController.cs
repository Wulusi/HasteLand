using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelController : MonoBehaviour
{
    public Transform spawnPos, despawnPos;

    public GameObject[] spawnReference;

    private GameObject despawner;

    private PoolManager poolManager;

    public float terrainCount;

    private bool isLevelFinished;

    private int randomInt, lastInt;

    // Start is called before the first frame update
    void Start()
    {
        poolManager = PoolManager.Instance;
        terrainCount = 0;
        //Assign 0 by default
        lastInt = 0;

        //StartCoroutine(SpawnIntervals());
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTerrain();
    }

    private void FixedUpdate()
    {
    }

    private void SpawnTerrain()
    {
        float totalTime = Time.frameCount;
        float duration = 240f;

        //Spawn a new section of the map every 4 seconds
        if (Mathf.Repeat(totalTime, duration) == 0)
        {
            Debug.Log("Before Spawn Count" + terrainCount);
            spawnMapSections();
        }
    }

    //IEnumerator SpawnIntervals()
    //{
    //    float totalTime = Time.time;

    //    while (!isLevelFinished)
    //    {
    //        while (Mathf.Repeat(totalTime, 4) == 0)
    //        {
    //            Debug.Log("Spawning Tile " + totalTime);
         
    //            //Not sure why this is giving null ref
    //            spawnMapSections();
    //            yield return null;
    //        }
    //    }
    //}

    void spawnMapSections()
    {
        terrainCount++;

        //randomInt = Random.Range(0, spawnReference.Length);
        //GameObject currentTerrain =
        //    poolManager.SpawnFromPool(spawnReference[randomInt].name, spawnPos.transform.position, Quaternion.identity);

        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, spawnReference.Length).OrderBy(x => rnd.Next()).Take(6).ToList();

        GameObject currentTerrain =
            poolManager.SpawnFromPool(spawnReference[randomNumbers[0]].name, spawnPos.transform.position, Quaternion.identity);

        Debug.Log("Spawned Terrain " + terrainCount + " " + currentTerrain);
    }
}
