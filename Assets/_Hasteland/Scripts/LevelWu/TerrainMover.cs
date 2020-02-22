using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMover : MonoBehaviour, PooledObjInterface
{
    public bool isStartingTiles;
    public int terrainSpd;
    public bool isLevelComplete;

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
        transform.localPosition += new Vector3(-1, 0, 0) * terrainSpd * 1 / 60f; //Time.deltatime messes this up majorly
    }

    public void OnPooledObjSpawn()
    {
        StartCoroutine(moveTiles(terrainSpd));
        GameHub.LevelController.currentTerrainList.Add(this.gameObject);
    }

    public void OnDisable()
    {
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
}
