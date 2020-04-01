using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A test enemy spawner. Should be removed when the real spawner is added
/// </summary>
public class EnemySpawner : MonoBehaviour, IPausable
{
    public List<Transform> m_spawnPoints;
    public LayerMask m_enemyMask;

    public Transform m_playerTransform;

    public float m_spawnRate;
    public float m_spawnRad;

    private ObjectPooler m_pooler;
    public GameObject m_groundEnemy, m_airEnemy;

    private Wave_Scriptable m_myWaveType;
    private int m_currentEnemyCount;
    private int m_currentWave;
    

    public static EnemySpawner Instance;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddMeToPauseManager(PauseManager.Instance);
        m_pooler = ObjectPooler.instance;
        m_myWaveType = WaveManager.Instance.GetCurrentWaveType();
        StartCoroutine(SpawnCoroutine());
    }


    private IEnumerator SpawnCoroutine()
    {
        float timer = 0;
        float totalTimer = 0;
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

                //Check for next wave
                totalTimer += Time.deltaTime;
                
                if(m_currentWave+1 < m_myWaveType.m_waves.Count)
                if (totalTimer > m_myWaveType.m_waves[m_currentWave].m_endTime)
                {
                    m_currentWave++;
                }
            }
            yield return null;
        }
    }

    public void SpawnNewEnemy()
    {
        if(m_currentEnemyCount +1 <= m_myWaveType.m_waves[m_currentWave].m_maxEnemies)
        {
            m_currentEnemyCount++;
            Transform newSpawn = NewSpawnPos();
            if (newSpawn == null) return;
            switch (m_myWaveType.m_waves[m_currentWave].m_waveSpawns)
            {
                case Wave_Scriptable.SpawnType.Air:
                    m_pooler.NewObject(m_airEnemy, newSpawn.position, newSpawn.rotation).GetComponent<EnemyBehaviour>().ResetMe(m_playerTransform);
                    break;
                case Wave_Scriptable.SpawnType.Ground:
                    m_pooler.NewObject(m_groundEnemy, newSpawn.position, newSpawn.rotation).GetComponent<EnemyBehaviour>().ResetMe(m_playerTransform);
                    break;
                case Wave_Scriptable.SpawnType.Both:
                    m_pooler.NewObject(Random.Range(0f, 1f) > .5 ? m_groundEnemy : m_airEnemy, newSpawn.position, newSpawn.rotation).GetComponent<EnemyBehaviour>().ResetMe(m_playerTransform);
                    break;
            }
            
        }
    }
 
    public Transform NewSpawnPos()
    {
        List<Transform> currentSpawns = new List<Transform>();
        foreach(Transform checkSpawn in m_spawnPoints)
        {
            if(Physics.OverlapSphere(checkSpawn.position, m_spawnRad, m_enemyMask).Length == 0)
            {
                currentSpawns.Add(checkSpawn);
            }
        }

        if(currentSpawns.Count > 0)
        {
            return currentSpawns[Random.Range(0, currentSpawns.Count)];
        }
        else
        {
            return null;
        }
    }

    public void EnemyDied()
    {
        m_currentEnemyCount--;
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
