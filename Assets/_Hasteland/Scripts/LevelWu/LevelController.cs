using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelController : MonoBehaviour, IPausable
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
        //Vsync is set to 1 to have timeframe performance and refresh rate of digital monitors to be the same
        //Before implementing this change certain features of the game on mobile and pc build would run at abnormal speeds
        //Either too fast or not fast enough
        QualitySettings.vSyncCount = 1;
        poolManager = PoolManager.Instance;
        terrainCount = 0;
        AddMeToPauseManager(PauseManager.Instance);
    }

    // Update is called once per frame
    void Update()
    {
        //If level is paused or failed do no progress update loops further
        if (m_levelFailed || m_paused) return;
        m_frameCount += 1;
        SpawnTerrain();
        UpdateSpeed();
    }

    //Public function to intiliaze a failed state and pause the game upon level failiure
    private int m_frameCount;
    public void PlayerDied()
    {
        m_levelFailed = true;
    }

    private void SpawnTerrain()
    {
        float totalTime = m_frameCount;
        float duration = 1200f / TerrainSpd;

        //Similar to modulus, spawn sections of the map every duration float
        //Duration is a function of 1200/Terrain speed
        if (Mathf.Repeat(totalTime, duration) == 0)
        {

            spawnMapSections();
        }
    }

    //Updates the current speed of all terrain tiles
    private void UpdateSpeed()
    {
        for (int i = 0; i < currentTerrainList.Count - 1; i++)
        {
            currentTerrainList[i].GetComponent<TerrainMover>().terrainSpd = TerrainSpd;
        }
    }

    //Spawn map sections from a predetermined pool of terrain game objects
    void spawnMapSections()
    {
        //Increase the float for terrain count, is could be used to gauge the level progress or extend level length
        terrainCount++;

        //Old random number generator
        //randomInt = Random.Range(0, spawnReference.Length);
        //GameObject currentTerrain =
        //    poolManager.SpawnFromPool(spawnReference[randomInt].name, spawnPos.transform.position, Quaternion.identity);

        //Generates a random number using Linq and sorting algorithms
        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, spawnReference.Length).OrderBy(x => rnd.Next()).Take(6).ToList();

        //Instantiate(spawnReference[0], spawnPos.transform.position, Quaternion.identity);
        
        //Instantiates the spawned section of the terrain into a container to accesss their speed parameters
        //Set the speed of the spawned terrain to the current speed of all terrain tiles
        //GameObject currentTerrain =
        poolManager.SpawnFromPool(spawnReference[randomNumbers[0]].name, spawnPos.transform.position, Quaternion.identity);
        //currentTerrain.GetComponent<TerrainMover>().terrainSpd = TerrainSpd;
    }

    //public function to enable pause from game controllers
    private bool m_paused;
    public void SetPauseState(bool p_paused)
    {
        m_paused = p_paused;
    }

    //Adds the current controller to the pause manager, to have it able to become paused during a game
    public void AddMeToPauseManager(PauseManager p_pauseManager)
    {
        p_pauseManager.AddNewObject(this);
    }
}
