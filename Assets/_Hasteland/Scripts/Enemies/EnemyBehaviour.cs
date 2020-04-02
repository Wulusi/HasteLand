using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test enemy movement. Should be removed when the real enemy movement is implemented
/// </summary>
public class EnemyBehaviour : MonoBehaviour, IPausable
{
    public float m_speed;
    private Transform m_targetPoint;
    public Health m_health;

    public float m_applyDamage;
    private EnemySpawner m_enemySpawner;

    //Enemy State Behaviours and variables
    private Vector3 scaleChange, positionChange;
    private int prepCounter;

    public enum EnemyState { targeting, prepping, attacking };
    public EnemyState enemyState;


    private void OnEnable()
    {
        //Reset 
        this.transform.localScale = new Vector3(1, 1, 1);
        scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
        positionChange = new Vector3(0, -0.5f, 0);
    }

    private void Start()
    {
        m_enemySpawner = EnemySpawner.Instance;
        AddMeToPauseManager(PauseManager.Instance);
    }

    private void OnDisable()
    {
        enemyState = EnemyState.targeting;
    }

    public void ResetMe(Transform p_playerPoint)
    {
        m_targetPoint = p_playerPoint;
        m_health.Respawn();
    }

    private void Update()
    {
        if (GameHub.LevelController.m_levelFailed)
        return;

        ActivateEnemyStates();
    }

    private void ActivateEnemyStates()
    {
        if (!m_isPaused)
        {
            switch (enemyState)
            {
                case EnemyState.targeting:
                    {
                        transform.Translate((m_targetPoint.position - transform.position).normalized * 2.5f * m_speed * Time.deltaTime, Space.World);

                        float distance = Vector3.Distance(m_targetPoint.position, this.transform.position);

                        //Debug.Log("Current distance" + distance);

                        if (distance < 10f)
                        {
                            enemyState = EnemyState.prepping;
                        }

                        //this.transform.localPosition += 25f * Time.deltaTime * positionChange;

                        //if (this.transform.localPosition.x > 0.5f || this.transform.localPosition.x < 0.1f)
                        //{
                        //    positionChange = -positionChange;
                        //}
                        break;
                    }

                case EnemyState.prepping:
                    {
                        //Debug.Log(this.gameObject.name + " Ready to Attack");

                        this.transform.localScale += 1.5f * scaleChange;

                        if (transform.localScale.x < 0.95f || transform.localScale.x > 1.1f)
                        {
                            scaleChange = -scaleChange;
                        }

                        if (transform.localScale.x == 1)
                        {
                            prepCounter++;
                            //Debug.Log(this.gameObject.name + " Counter is " + prepCounter);
                        }

                        if (prepCounter >= 20f)
                        {
                            enemyState = EnemyState.attacking;
                        }

                        break;
                    }

                case EnemyState.attacking:
                    {
                        //Debug.Log("attacking");

                        transform.Translate((m_targetPoint.position - transform.position).normalized * m_speed * Time.deltaTime, Space.World);

                        break;
                    }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == m_targetPoint.gameObject)
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

    public void IDied()
    {
        m_enemySpawner.EnemyDied();
    }
}
