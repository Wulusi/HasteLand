using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A test enemy spawner. Should be removed when the real spawner is added
/// </summary>
public class TestEnemySpawner : MonoBehaviour, IPausable
{
    public List<Transform> m_spawnPoints;
    public Transform m_playerTransform;

    public float m_spawnRate;

    public List<GameObject> m_enemyTypes;


    private void Start()
    {
        AddMeToPauseManager(PauseManager.Instance);
        m_pooler = ObjectPooler.instance;
        StartCoroutine(SpawnCoroutine());
    }


    private IEnumerator SpawnCoroutine()
    {
        float timer = 0;
        while (true)
        {
            if (!m_isPaused)
            {
                timer += Time.deltaTime;
                if (timer > m_spawnRate)
                {
                    SpawnNewEnemy();
                    timer = 0;
                }
            }
            yield return null;
        }
    }

    private ObjectPooler m_pooler;
    public void SpawnNewEnemy()
    {
        int newEnem = Random.Range(0, m_enemyTypes.Count);
        int newSpawn = Random.Range(0, m_spawnPoints.Count);
        m_pooler.NewObject(m_enemyTypes[newEnem], m_spawnPoints[newSpawn].position, Quaternion.identity).GetComponent<TestEnemyBehaviour>().ResetMe(m_playerTransform);
    }

    #region Pause Functions
    private bool m_isPaused = false;
    public void AddMeToPauseManager(PauseManager p_pauseManager)
    {
        p_pauseManager.AddNewObject(this);
    }

    public void SetPauseState(bool p_paused)
    {
        m_isPaused = p_paused;
    }

    #endregion

}
