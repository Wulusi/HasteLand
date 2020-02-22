using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHead : MonoBehaviour, IPausable
{
    [Header("Rotation Variables")]
    public float m_rotationTime;
    public Transform m_parentTurret;
    public Transform m_rotateY;
    public Transform m_rotateX;
    public float m_defaultRestingAngle, m_parabolicRestingAngle;
    [Tooltip("Used to determine how close the turret needs to be in order to start slowing down. A higher number means it has to be closer.")]
    public float m_rotationSensitivity;

    #region Parabolic Variables
    [Header("Bullet Values")]
    public float m_bulletGravity;
    public float m_projectileSpeed;
    #endregion



    [Header("Fire Properties")]
    public float m_fireDelayTime;
    public float m_shootingAngle;
    private bool m_canFire = true;
    public TurretEvent m_fireTurretEvent;
    public virtual void Start()
    {
        AddMeToPauseManager(PauseManager.Instance);
    }
    public void RotateToResting(TurretController.TurretRotationType p_turretRotationType)
    {
        switch (p_turretRotationType)
        {
            case TurretController.TurretRotationType.Default:
                RotateTurret(Quaternion.AngleAxis(m_defaultRestingAngle, Vector3.right) * transform.forward + transform.position, TurretController.TurretRotationType.Default);
                break;
            case TurretController.TurretRotationType.Parabolic:
                RotateTurret(Quaternion.AngleAxis(m_parabolicRestingAngle, Vector3.right) * transform.forward + transform.position, TurretController.TurretRotationType.Default);
                break;
        }
    }

    public void RotateTurret(Vector3 p_targetPos, TurretController.TurretRotationType p_rotationType)
    {

        transform.eulerAngles = new Vector3(0, 0, 0);
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


        m_rotateY.Rotate(m_rotateY.up, (currentDistance > 1 ? m_rotationTime : currentDistance * m_rotationTime) * Mathf.Sign(dirToRotate));

        #endregion

        #region xRotation
        //Isolate the target, so only one axis has to be worked with
        //Find the horizontal distance from the target to the turret.
        float disXFromTransform = Vector3.Distance(new Vector3(m_rotateX.position.x, 0f, m_rotateX.position.z), new Vector3(p_targetPos.x, 0, p_targetPos.z));

        //Find the difference in height
        float disYFromTarget = Mathf.Abs(p_targetPos.y - m_rotateX.position.y);

        //Use the distance and height to create a virtual target, that rotates with the y axis, thus negating the z axis




        //Change the current target so that a parabolic calculation is used
        if (p_rotationType == TurretController.TurretRotationType.Parabolic)
        {
            float sqrt = (m_projectileSpeed * m_projectileSpeed * m_projectileSpeed * m_projectileSpeed) - (m_bulletGravity * (m_bulletGravity * (disXFromTransform * disXFromTransform) + 2 * disYFromTarget * (m_projectileSpeed * m_projectileSpeed)));
            if (sqrt < 0)
            {
                print("No solution");
            }
            sqrt = Mathf.Sqrt(sqrt);
            float calculateAnglePos = Mathf.Atan(((m_projectileSpeed * m_projectileSpeed) + sqrt) / (m_bulletGravity * disXFromTransform)) * Mathf.Rad2Deg;
            //float calculateAngleNeg = Mathf.Atan(((m_projectileSpeed * m_projectileSpeed) - sqrt) / (m_bulletGravity * disXFromTransform)) * Mathf.Rad2Deg;

            currentTarget = (Quaternion.AngleAxis(-calculateAnglePos, Vector3.right) * Vector3.forward).normalized * m_rotationSensitivity;
            currentTarget = Quaternion.AngleAxis(m_rotateY.eulerAngles.y, Vector3.up) * currentTarget;
            Debug.DrawLine(transform.position, transform.position + currentTarget, Color.green);


            FireTurret();

        }
        //Use the virtual current target
        else
        {
            currentTarget = m_rotateY.localRotation * new Vector3(0, disYFromTarget * (p_targetPos.y > m_rotateX.transform.position.y ? 1 : -1), disXFromTransform).normalized * m_rotationSensitivity;
            Debug.DrawLine(m_rotateX.position, currentTarget + m_rotateX.position, Color.green);
        }


        currentDistance = Vector3.Cross(m_rotateX.forward, currentTarget - m_rotateX.forward).magnitude;
        dirToRotate = Vector3.Dot(-m_rotateX.up, currentTarget - m_rotateX.forward);
        //Edge case
        if (Vector3.Angle(m_rotateX.forward, currentTarget) > 150)
        {
            dirToRotate = 1;
            currentDistance = 1.5f;
        }

        //Rotate
        m_rotateX.Rotate(Vector3.right, (currentDistance > 3 ? m_rotationTime : currentDistance / 3 * m_rotationTime) * Mathf.Sign(dirToRotate));







        #endregion

    }

    public bool CorrectAngle(Vector3 p_targetPos)
    {
        float angle = Vector3.Angle(m_rotateX.forward, new Vector3(p_targetPos.x, p_targetPos.y, p_targetPos.z) - m_rotateX.position);
        if (angle < m_shootingAngle)
        {
            return true;
        }
        return false;
    }

    public virtual void FireTurret()
    {
        if (m_canFire)
        {
            m_fireTurretEvent.Invoke();
            StartCoroutine(TurretShotRecharge());
        }
    }

    private IEnumerator TurretShotRecharge()
    {
        m_canFire = false;
        float timer = 0;
        while (timer < m_fireDelayTime)
        {
            yield return null;
            if (!m_isPaused)
            {
                
                timer += Time.deltaTime;
            }
        }
        m_canFire = true;
    }

    #region Pausing
    private bool m_isPaused;
    public void SetPauseState(bool p_paused)
    {
        m_isPaused = p_paused;
    }

    public bool AmIPaused()
    {
        return m_isPaused;
    }

    public void AddMeToPauseManager(PauseManager p_pauseManager)
    {
        p_pauseManager.AddNewObject(this);
    }
    #endregion
}
