using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletEvent : UnityEngine.Events.UnityEvent { }

public class BulletsBehaviour : MonoBehaviour
{
    [Header("Bullet Physics")]
    public Vector3 m_velocity;
    public float m_gravity;
    public bool m_armorPiercing;
    public float m_collisionRadius;
    public LayerMask m_collisionDetectionMask;
    public LayerMask m_boundsLayer;
    public BulletEvent m_bulletHitEvent;

    [HideInInspector]
    public ObjectPooler m_pooler;
    [HideInInspector]
    public float m_bulletDamage;

    
    [Header("Visuals")]
    public bool m_rotateWithVelocity;
    public Transform m_visualsTransform;

    [Header("Debugging")]
    public bool m_debugGizmos;
    public Color m_gizmosColor1, m_gizmosColor2;


    private void Start()
    {
        m_pooler = ObjectPooler.instance;
    }
    public virtual void FixedUpdate()
    {
        m_velocity = new Vector3(m_velocity.x, m_velocity.y - (Mathf.Abs(m_gravity) / 50), m_velocity.z);
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, m_collisionRadius, m_velocity.normalized, out hit, m_velocity.magnitude * Time.fixedDeltaTime, m_collisionDetectionMask))
        {
            Vector3 hitPos = transform.position + m_velocity.normalized * hit.distance;

            bool destroyObject = true;


            if (m_armorPiercing)
            {
                if (Physics.SphereCast(new Ray(transform.position, m_velocity.normalized), m_collisionRadius, m_velocity.magnitude * Time.fixedDeltaTime, m_boundsLayer))
                {
                    transform.position = hitPos;
                }
                else
                {
                    transform.position += m_velocity * Time.fixedDeltaTime;
                    destroyObject = false;
                }
            }
            else
            {
                transform.position = hitPos;
            }


            HitObject(hit.transform.gameObject, destroyObject);

        }
        else
        {
            transform.position += m_velocity * Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        if (m_rotateWithVelocity)
        {
            m_visualsTransform.LookAt(transform.position + m_velocity);
        }
    }
    public void HitObject(GameObject p_hitObject, bool p_objectPool)
    {
        Health hitHealth = p_hitObject.GetComponent<Health>();
        if (hitHealth != null)
        {
            hitHealth.TakeDamage(m_bulletDamage);
        }
        if (p_objectPool)
        {
            DestroyBullet();
        }
    }
    public void DestroyBullet()
    {
        m_bulletHitEvent.Invoke();
        m_pooler.ReturnToPool(this.gameObject);

    }
    public virtual void SetVariables(Vector3 p_newVelocity, float p_damage, Transform p_target = null)
    {
        m_velocity = p_newVelocity;
        m_bulletDamage = p_damage;

    }

    public virtual void OnDrawGizmos()
    {
        if (!m_debugGizmos) return;
        Gizmos.color = m_gizmosColor1;
        Gizmos.DrawWireSphere(transform.position, m_collisionRadius);
        Gizmos.color = m_gizmosColor2;
        Gizmos.DrawWireSphere(transform.position + m_velocity * Time.fixedDeltaTime, m_collisionRadius);
    }


}
