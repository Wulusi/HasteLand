using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletEvent: UnityEngine.Events.UnityEvent { }

public class BulletsBehaviour : MonoBehaviour
{
    public Vector3 m_velocity;
    public float m_gravity;
    public LayerMask m_collisionDetectionMask;
    public float m_collisionRadius;
    public BulletEvent m_bulletHitEvent;

    private ObjectPooler m_pooler;
    [Header("Debugging")]
    public bool m_debugGizmos;
    public Color m_gizmosColor1, m_gizmosColor2;


    private void Start()
    {
        m_pooler = ObjectPooler.instance;
    }
    private void FixedUpdate()
    {
        m_velocity = new Vector3(m_velocity.x, m_velocity.y - (Mathf.Abs(m_gravity) /50), m_velocity.z);
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, m_collisionRadius,m_velocity.normalized, out hit, m_velocity.magnitude*Time.fixedDeltaTime, m_collisionDetectionMask))
        {
            transform.position += m_velocity.normalized * hit.distance;
            HitObject(hit.transform.gameObject);
            
        }
        else
        {
            transform.position += m_velocity * Time.fixedDeltaTime;
        }
    }

    private void HitObject(GameObject p_hitObject)
    {
        m_bulletHitEvent.Invoke();
        print(name + " collided with " + p_hitObject.name + ".");
        m_pooler.ReturnToPool(this.gameObject);
    }

    public void SetVelocity(Vector3 p_newVelocity)
    {
        m_velocity = p_newVelocity;
    }

    private void OnDrawGizmos()
    {
        if (!m_debugGizmos) return;
        Gizmos.color = m_gizmosColor1;
        Gizmos.DrawWireSphere(transform.position, m_collisionRadius);
        Gizmos.color = m_gizmosColor2;
        Gizmos.DrawWireSphere(transform.position + m_velocity * Time.fixedDeltaTime, m_collisionRadius);
    }


}
