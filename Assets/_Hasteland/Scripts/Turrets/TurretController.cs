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
        Idle, RotateToEnemy, RotateToDefault
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

    [Header("Rotation Variables")]
    public Transform m_rotateX;
    public Transform m_rotateY;

    [Tooltip("Used to determine how close the turret needs to be in order to start slowing down. A higher number means it has to be closer.")]
    public float m_rotationSensitivity;

    private List<GameObject> m_targetsInRange;
    private GameObject m_currentTarget;
    private TurretLevel m_turretLevel;
    void Start()
    {
        m_turretLevel = GetComponent<TurretLevel>();
        StartCoroutine(FireTurretTest());
    }

    public Transform m_debugTarget;
    void Update()
    {
        RotateTurret(m_debugTarget.position);
        Debug.DrawLine(m_rotateY.position, m_rotateY.forward * 15 + m_rotateY.position, Color.red);
        Debug.DrawLine(m_rotateX.position, m_rotateX.forward * 15 + m_rotateX.position, Color.blue);


        
    }

    private void CheckState()
    {
        switch (m_currentState)
        {
            case TurretBehaviourState.Idle:
                break;
            case TurretBehaviourState.RotateToEnemy:
                break;
            case TurretBehaviourState.RotateToDefault:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AddEnemyToList(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        RemoveEnemyFromList(other.gameObject);
    }

    private void AddEnemyToList(GameObject p_addedTarget)
    {
        if (!m_targetsInRange.Contains(p_addedTarget))
        {
            m_targetsInRange.Add(p_addedTarget);
        }
    }
    private void RemoveEnemyFromList(GameObject p_removeTarget)
    {
        if (m_targetsInRange.Contains(p_removeTarget))
        {
            m_targetsInRange.Remove(p_removeTarget);
        }
    }



    private void FireTurret()
    {
        m_turretEvents.m_fireTurretEvent.Invoke();
    }


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
    private bool CorrectAngle(Vector3 p_targetPos, float p_stopAngle)
    {
        float angle = Vector3.Angle(transform.forward, new Vector3(p_targetPos.x, p_targetPos.y, p_targetPos.z) - transform.position);
        if (angle < p_stopAngle)
        {
            return true;
        }
        return false;
    }

    private IEnumerator FireTurretTest()
    {
        while (true)
        {


            yield return new WaitForSeconds(m_turretLevel.m_turretFireRates[m_turretLevel.m_currentFireRateIndex].m_turretFireRate);
            print("Testing Fire Behaviour");
            FireTurret();
        }
    }
}
