using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMover : MonoBehaviour
{

    public int terrainSpd;
    public bool isLevelComplete;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveTiles(terrainSpd));
    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator moveTiles(int terrainSpd)
    {
        while (true)
        {
            transform.localPosition += new Vector3(-1, 0, 0) * terrainSpd * Time.deltaTime;

            yield return null;
        }
    }
}
