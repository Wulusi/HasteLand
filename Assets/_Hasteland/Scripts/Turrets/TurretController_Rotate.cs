using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController_Rotate : TurretController
{
    public float m_rotationSensitivity;
    public float m_rotationTime;
    public override void RotateAllTurrets(Vector3 p_targetPosition)
    {

        #region yRotation

        //Create a direction towards the target
        Vector3 currentTarget = (new Vector3(p_targetPosition.x, 0f, p_targetPosition.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized * m_rotationSensitivity;

        //Find out how much to rotate
        float currentDistance = Vector3.Cross(transform.forward, (currentTarget - (transform.forward))).magnitude;

        //Find out which way to rotate the Y
        float dirToRotate = Vector3.Dot(transform.right, currentTarget - transform.forward);

        //Edge case
        if (Vector3.Angle(transform.forward, currentTarget) > 150)
        {
            dirToRotate = 1;
            currentDistance = 1.5f;
        }


        transform.Rotate(transform.up, (currentDistance > 1 ? m_rotationTime : currentDistance * m_rotationTime) * Mathf.Sign(dirToRotate));

        #endregion

        base.RotateAllTurrets(p_targetPosition);
    }
}
