using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDestroyed : MonoBehaviour
{
    public Rigidbody m_rb;

    public GameObject m_spawnOnDeathPrefab;

    private ObjectPooler m_pooler;
    private void Start()
    {
        m_pooler = ObjectPooler.instance;
    }
    public void SpawnObject()
    {
        GameObject spawned = m_pooler.NewObject(m_spawnOnDeathPrefab, transform.position, Quaternion.identity);
        SpawnDestroyed_Prefab spawnDestroy = spawned.GetComponent<SpawnDestroyed_Prefab>();
        if (spawnDestroy != null)
        {
            if (m_rb != null)
            {
                spawnDestroy.SpawnDestroyed(m_rb.velocity);
            }
            else
            {
                spawnDestroy.SpawnDestroyed(Vector3.zero);
            }
        }
    }
}
