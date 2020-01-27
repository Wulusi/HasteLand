using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretEvent : UnityEngine.Events.UnityEvent { }
public class TurretController : MonoBehaviour
{
    #region Behaviour States
    public enum TurretBehaviourState
    {
        Idle, RotateToEnemy
    }
    public TurretBehaviourState m_currentState;
    #endregion


    #region Unity Events
    public TurretEvents m_turretEvents;
    [System.Serializable]
    public struct TurretEvents
    {
        public TurretEvent m_fireTurretEvent;
    }

    #endregion

    [Header("DetectionRadius")]
    public float m_detectionRadius;
    public LayerMask m_detectionMask;

    [Header("Rotation Variables")]
    public Transform m_rotateY;
    public Transform m_rotateX;

    [Tooltip("Used to determine how close the turret needs to be in order to start slowing down. A higher number means it has to be closer.")]
    public float m_rotationSensitivity;

    private List<GameObject> m_targetsInRange = new List<GameObject>();
    private GameObject m_currentTarget;
    private TurretLevel m_turretLevel;

    [Header("Fire Properties")]
    public float m_shootingAngle;
    private bool m_canFire = true;
    private WaitForSeconds m_fireDelay;

    [Header("DebuggingTools")]
    public bool m_debugging;
    public Color m_gizmosColor1;
    void Start()
    {
        m_turretLevel = GetComponent<TurretLevel>();
        m_fireDelay = new WaitForSeconds(m_turretLevel.m_turretFireRates[m_turretLevel.m_currentFireRateIndex].m_turretFireRate);
    }

    void Update()
    {
        CheckState();
        Debug.DrawLine(m_rotateY.position, m_rotateY.forward * 15 + m_rotateY.position, Color.red);
        Debug.DrawLine(m_rotateX.position, m_rotateX.forward * 15 + m_rotateX.position, Color.blue);
    }

    #region StateMachine
    private void CheckState()
    {
        switch (m_currentState)
        {
            #region Idle State
            case TurretBehaviourState.Idle:
                RotateTurret(transform.forward + transform.position);
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
                    RotateTurret(m_currentTarget.transform.position);
                    if (CorrectAngle(m_currentTarget.transform.position, m_shootingAngle))
                    {
                        FireTurret();
                    }
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



    private void RotateTurret(Vector3 p_targetPos)
    {

        #region yRotation

        //Create a direction towards the target
        Vector3 currentTarget = (new Vector3(p_targetPos.x, 0f, p_targetPos.z) - new Vector3(m_rotateY.position.x, 0, m_rotateY.position.z)).normalized * m_rotationSensitivity;

        //Find out how much to rotate
        float currentDistance = Vector3.Cross(m_rotateY.forward, (currentTarget - (m_rotateY.forward))).magnitude;

        //Find out which way to rotate the Y
        float dirToRotate = Vector3.Dot(m_rotateY.right, currentTarget - m_rotateY.forward);

        //Edge case
        if (Vector3.Angle(m_rotateY.forward, currentTarget) > 150)
        {
            dirToRotate = 1;
            currentDistance = 1.5f;
        }


        m_rotateY.Rotate(m_rotateY.up, (currentDistance > 1 ? m_turretLevel.GetCurrentRotationSpeed() : currentDistance * m_turretLevel.GetCurrentRotationSpeed()) * Mathf.Sign(dirToRotate));

        #endregion

        #region xRotation
        //Isolate the target, so only one axis has to be worked with
        //Find the horizontal distance from the target to the turret.
        float disXFromTransform = Vector3.Distance(new Vector3(m_rotateX.position.x, 0f, m_rotateX.position.z), new Vector3(p_targetPos.x, 0, p_targetPos.z));

        //Find the difference in height
        float disYFromTarget = Mathf.Abs(p_targetPos.y - m_rotateX.position.y);

        //Use the distance and height to create a virtual target, that rotates with the y axis, thus negating the z axis
        currentTarget = m_rotateY.localRotation * new Vector3(0, disYFromTarget * (p_targetPos.y > m_rotateX.transform.position.y ? 1 : -1), disXFromTransform).normalized * m_rotationSensitivity;
        Debug.DrawLine(m_rotateX.position, currentTarget + m_rotateX.position, Color.green);

        //Find which way to turn, and how much to rotate
        currentDistance = Vector3.Cross(m_rotateX.forward, currentTarget - m_rotateX.forward).magnitude;
        dirToRotate = Vector3.Dot(-m_rotateX.up, currentTarget - m_rotateX.forward);
        //Edge case
        if (Vector3.Angle(m_rotateX.forward, currentTarget) > 150)
        {
            dirToRotate = 1;
            currentDistance = 1.5f;
        }

        //Rotate
        m_rotateX.Rotate(Vector3.right, (currentDistance > 3 ? m_turretLevel.GetCurrentRotationSpeed() : currentDistance / 3 * m_turretLevel.GetCurrentRotationSpeed()) * Mathf.Sign(dirToRotate));






        #endregion

    }


    #region Fire
    private bool CorrectAngle(Vector3 p_targetPos, float p_stopAngle)
    {
        float angle = Vector3.Angle(m_rotateX.forward, new Vector3(p_targetPos.x, p_targetPos.y, p_targetPos.z) - m_rotateX.position);
        if (angle < p_stopAngle)
        {
            return true;
        }
        return false;
    }
    private void FireTurret()
    {
        if (m_canFire)
        {
            m_turretEvents.m_fireTurretEvent.Invoke();
            StartCoroutine(TurretShotRecharge());
        }
    }

    private IEnumerator TurretShotRecharge()
    {
        m_canFire = false;
        yield return m_fireDelay;
        m_canFire = true;
    }
    #endregion


    private void OnDrawGizmos()
    {
        if (!m_debugging) return;
        Gizmos.color = m_gizmosColor1;
        Gizmos.DrawWireSphere(transform.position, m_detectionRadius);

    }
}
