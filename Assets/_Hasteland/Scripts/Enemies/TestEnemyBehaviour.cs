using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test enemy movement. Should be removed when the real enemy movement is implemented
/// </summary>
public class TestEnemyBehaviour : MonoBehaviour, IPausable
{
    public float m_speed;
    private Transform m_targetPoint;
    public Health m_health;

    public float m_applyDamage;
    
    private void Start()
    {
        AddMeToPauseManager(PauseManager.Instance);
    }
    public void ResetMe( Transform p_playerPoint)
    {
        m_targetPoint = p_playerPoint;
        m_health.Respawn();
    }

    private void Update()
    {
        if (GameHub.LevelController.m_levelFailed) return;
        if (!m_isPaused)
        {
            transform.Translate((m_targetPoint.position - transform.position).normalized * m_speed * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == m_targetPoint.gameObject)
        {
            m_targetPoint.gameObject.GetComponent<Health>().TakeDamage(m_applyDamage);
            m_health.TakeDamage(1000);
        }
    }

    private bool m_isPaused;
    public void SetPauseState(bool p_paused)
    {
        m_isPaused = p_paused;
    }

    public void AddMeToPauseManager(PauseManager p_pauseManager)
    {
        p_pauseManager.AddNewObject(this);
    }
}
