using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretEvent : UnityEngine.Events.UnityEvent { }
public class TurretController : MonoBehaviour, IPausable
{
    public enum TurretRotationType
    {
        Default, Parabolic
    }
    public TurretRotationType m_turretRotationType;
    #region Behaviour States
    public enum TurretBehaviourState
    {
        Idle, RotateToEnemy, BeingBuilt
    }
    public TurretBehaviourState m_currentState;
    #endregion


    #region Unity Events
    public TurretEvents m_turretEvents;
    [System.Serializable]
    public struct TurretEvents
    {
        public TurretEvent m_startTurretBuilding;
    }

    #endregion



    [Header("Turret Heads")]
    public List<TurretHead> m_attachedTurrets;

    [Header("DetectionRadius")]
    public float m_detectionRadius;
    public LayerMask m_detectionMask;


    
    private List<GameObject> m_targetsInRange = new List<GameObject>();
    [HideInInspector]
    public GameObject m_currentTarget;



    [Header("DebuggingTools")]
    public bool m_debugging;
    public Color m_gizmosColor1;

    private void Start()
    {
        AddMeToPauseManager(PauseManager.Instance);
    }



    void Update()
    {
        if (m_isPaused) return;
        CheckState();

    }

    #region StateMachine
    private void CheckState()
    {
        switch (m_currentState)
        {
            #region Idle State
            case TurretBehaviourState.Idle:
                RotateAllTurretsToRest();


                CheckDetectionRadius();
                if (TargetExists())
                {
                    ChangeState(TurretBehaviourState.RotateToEnemy);
                }
                break;
            #endregion

            #region RotateToEnemy State
            case TurretBehaviourState.RotateToEnemy:
                CheckDetectionRadius();
                if (!TargetExists())
                {
                    ChangeState(TurretBehaviourState.Idle);
                }
                else
                {
                    RotateAllTurrets(m_currentTarget.transform.position);
                    CheckFireOnAllTurrets();
                }
                break;
                #endregion
        }
    }

    private void ChangeState(TurretBehaviourState p_newState)
    {
        m_currentState = p_newState;
        switch (p_newState)
        {
            case TurretBehaviourState.Idle:

                break;
            case TurretBehaviourState.RotateToEnemy:

                break;

        }
    }
    #endregion

    #region Active States
    public void FinishedBuild()
    {
        ChangeState(TurretBehaviourState.Idle);
    }

    public void StartBuilding()
    {
        ChangeState(TurretBehaviourState.BeingBuilt);
        m_turretEvents.m_startTurretBuilding.Invoke();
    }
    #endregion


    #region Detection
    private void CheckDetectionRadius()
    {
        List<GameObject> existingTargets = new List<GameObject>();
        foreach (GameObject currentTarget in m_targetsInRange)
        {
            existingTargets.Add(currentTarget);
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, m_detectionRadius, m_detectionMask);
        foreach (Collider col in cols)
        {
            if (existingTargets.Contains(col.gameObject))
            {
                existingTargets.Remove(col.gameObject);
            }
            else
            {
                m_targetsInRange.Add(col.gameObject);
            }
        }

        foreach (GameObject removeTarget in existingTargets)
        {
            if (m_targetsInRange.Contains(removeTarget))
            {
                m_targetsInRange.Remove(removeTarget);
            }
        }
    }

    private bool TargetExists()
    {
        if (m_currentTarget == null)
        {
            if (m_targetsInRange.Count > 0)
            {
                m_currentTarget = m_targetsInRange[0];
                return true;
            }
            return false;
        }
        else
        {
            if (!m_currentTarget.activeSelf)
            {
                if (m_targetsInRange.Contains(m_currentTarget))
                {
                    m_targetsInRange.Remove(m_currentTarget);
                }
                if (m_targetsInRange.Count > 0)
                {
                    m_currentTarget = m_targetsInRange[0];
                    return true;
                }
                else
                {
                    m_currentTarget = null;
                    return false;
                }
            }
            else
            {
                if (!m_targetsInRange.Contains(m_currentTarget))
                {
                    if (m_targetsInRange.Count > 0)
                    {
                        m_currentTarget = m_targetsInRange[0];
                    }
                    else
                    {
                        m_currentTarget = null;
                        return false;
                    }
                }
                return true;
            }
        }
    }
    #endregion

    #region Rotate All Turrets
    public virtual void RotateAllTurrets(Vector3 p_targetPosition)
    {
        foreach(TurretHead turret in m_attachedTurrets)
        {
            turret.RotateTurret(p_targetPosition, m_turretRotationType);
        }
    }

    private void RotateAllTurretsToRest()
    {
        foreach(TurretHead turret in m_attachedTurrets)
        {
            turret.RotateToResting(m_turretRotationType);
        }
    }
    #endregion


    #region Turret Fire
    public void CheckFireOnAllTurrets()
    {
        foreach(TurretHead turret in m_attachedTurrets)
        {
            if (turret.CorrectAngle(m_currentTarget.transform.position))
            {
                turret.FireTurret();
            }
            
        }
    }
    #endregion




    private void OnDrawGizmos()
    {
        if (!m_debugging) return;
        Gizmos.color = m_gizmosColor1;
        Gizmos.DrawWireSphere(transform.position, m_detectionRadius);

    }

    #region Pausing
    private bool m_isPaused;
    public void SetPauseState(bool p_paused)
    {
        m_isPaused = p_paused;
    }

    public void AddMeToPauseManager(PauseManager p_pauseManager)
    {
        p_pauseManager.AddNewObject(this);
    }
    #endregion
}
