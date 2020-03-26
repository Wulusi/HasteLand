using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelController : MonoBehaviour
{
    public Transform spawnPos, despawnPos;

    public GameObject[] spawnReference;

    public List<GameObject> currentTerrainList = new List<GameObject>();

    private GameObject despawner;

    private PoolManager poolManager;

    private bool isLevelFinished;

    private int randomInt, lastInt, terrainCount;

    [Range(0, 40)]
    public int TerrainSpd;

    public bool m_levelFailed;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        poolManager = PoolManager.Instance;
        terrainCount = 0;
        //Assign 0 by default
        lastInt = 0;
        //StartCoroutine(SpawnIntervals());
    }

    // Update is called once per frame
    void Update()
    {
        if (m_levelFailed) return;
        SpawnTerrain();
        UpdateSpeed();
    }


    public void PlayerDied()
    {
        m_levelFailed = true;
    }

    private void SpawnTerrain()
    {
        float totalTime = Time.frameCount;
        float duration = 1200f / TerrainSpd;

        if (Mathf.Repeat(totalTime, duration) == 0)
        {

            spawnMapSections();
        }
    }

    private void UpdateSpeed()
    {
        for (int i = 0; i < currentTerrainList.Count - 1; i++)
        {
            currentTerrainList[i].GetComponent<TerrainMover>().terrainSpd = TerrainSpd;
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
        currentTerrain.GetComponent<TerrainMover>().terrainSpd = TerrainSpd;

    }
}
