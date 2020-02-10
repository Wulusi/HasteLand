using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{
    public GameObject m_spawnedParticle;
    private ObjectPooler m_pooler;
    private void Start()
    {
        m_pooler = ObjectPooler.instance;
    }

    /// <summary>
    /// Spawns an unparented particle object at the p_position
    /// </summary>
    /// <param name="p_positon"></param>
    public void SpawnParticlePrefab(Vector3 p_positon)
    {
        m_pooler.NewObject(m_spawnedParticle, p_positon, Quaternion.identity);
    }
    /// <summary>
    /// Spawns an unparented particle object at this objects postion.
    /// </summary>
    public void SpawnParticlePrefab()
    {
        m_pooler.NewObject(m_spawnedParticle, transform.position, Quaternion.identity);
    }
    /// <summary>
    /// Spawns a line renderer, and requires a starting position, and ending position.
    /// </summary>
    /// <param name="p_startingPos"></param>
    /// <param name="p_endingPos"></param>
    public void SpawnParticleLineRenderer(Vector3 p_startingPos, Vector3 p_endingPos)
    {
        LineRenderer newLine = m_pooler.NewObject(m_spawnedParticle, p_startingPos, Quaternion.identity).GetComponent<LineRenderer>();

        newLine.SetPosition(0, p_startingPos);
        newLine.SetPosition(1, p_endingPos);
    }
}