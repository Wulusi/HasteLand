using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : BulletsBehaviour
{

    [Header("Missile Properties")]
    public float m_rotateSpeed;
    public bool m_heatSeeking;
    private float m_missileSpeed;
    public float m_lifespan;

    private Transform m_targetUnit;
    private WaitForSeconds m_lifeDelay;
    private Coroutine m_lifeCoroutine;

    public override void SetVariables(Vector3 p_newVelocity, float p_damage, Transform p_target)
    {
        if(m_lifeDelay == null)
        {
            m_lifeDelay = new WaitForSeconds(m_lifespan);
        }
        m_missileSpeed = p_newVelocity.magnitude;
        m_bulletDamage = p_damage;
        m_targetUnit = p_target;
        m_lifeCoroutine = StartCoroutine(LifeTime());

    }

    public void RotateMissile(Vector3 p_targetPos)
    {
        Quaternion lookAt = Quaternion.LookRotation(p_targetPos - transform.position, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAt, m_rotateSpeed * Time.deltaTime);
    }

    public override void FixedUpdate()
    {
        if (AmIPaused()) return;
        if (m_heatSeeking)
        {
            RotateMissile(m_targetUnit.position);
        }
        Vector3 velocity = transform.forward * m_missileSpeed * Time.fixedDeltaTime;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, m_collisionRadius, velocity, out hit, velocity.magnitude, m_collisionDetectionMask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red, 2f);
            HitObject(hit.transform.gameObject, true);
            StopAllCoroutines();
        }
        else
        {
            transform.position += velocity;
        }

    }

    private IEnumerator LifeTime()
    {
        yield return m_lifeDelay;
        DestroyBullet();
        
    }
}
