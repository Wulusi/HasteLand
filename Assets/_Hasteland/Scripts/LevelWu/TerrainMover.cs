using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMover : MonoBehaviour, PooledObjInterface
{
    public bool isStartingTiles;
    public int terrainSpd;
    public bool isLevelComplete;

    [Range(0,5)]
    public int m_maxItems;
    public List<Transform> m_objectSpawnPositions;
    public List<GameObject> m_spawnablePrefabs;
    private ObjectPooler m_pooler;

    private List<GameObject> m_environmentObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (isStartingTiles)
        {
            StartCoroutine(moveTiles(terrainSpd));
        }
    }

    IEnumerator moveTiles(int terrainSpd)
    {
        while (true)
        {
            yield return null;
        }
    }

    public void Update()
    {
        if (GameHub.LevelController.m_levelFailed) return;
        transform.localPosition += new Vector3(-1, 0, 0) * terrainSpd * 1 / 60f; //Time.deltatime messes this up majorly
    }

    public void OnPooledObjSpawn()
    {
        StartCoroutine(moveTiles(terrainSpd));
        GameHub.LevelController.currentTerrainList.Add(this.gameObject);
    }

    private void OnEnable()
    {
        CreateRandomizedTerrain();
    }

    private void CreateRandomizedTerrain()
    {
        if(m_pooler == null)
        {
            m_pooler = ObjectPooler.instance;
        }

        int randomAmount = Random.Range(0, m_maxItems);
        List<Transform> possibleSpawns = new List<Transform>(m_objectSpawnPositions);

        for (int i = 0; i < randomAmount; i++)
        {
            int randomSpawn = Random.Range(0, possibleSpawns.Count);
            
            GameObject newPiece = m_pooler.NewObject(m_spawnablePrefabs[Random.Range(0, m_spawnablePrefabs.Count)], possibleSpawns[randomSpawn].position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            m_environmentObjects.Add(newPiece);
            newPiece.transform.parent = transform;
            m_objectSpawnPositions.RemoveAt(randomSpawn);
        }
    }

    public void OnDisable()
    {

        ClearEnvironmentPieces();
        StopCoroutine(moveTiles(terrainSpd));

        if (isStartingTiles)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            if (GameHub.LevelController != null)
            {
                GameHub.LevelController.currentTerrainList.Remove(this.gameObject);
            }
        }
    }

    private void ClearEnvironmentPieces()
    {
        foreach(GameObject piece in m_environmentObjects)
        {
            m_pooler.ReturnToPool(piece);
        }
        m_environmentObjects.Clear();
    }
}
