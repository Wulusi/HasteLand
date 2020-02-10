using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombsBehaviour : BulletsBehaviour
{
    
    [Header("Cluster Bombs")]
    public GameObject m_miniBombs;
    public int m_numOfMiniBombs;
    public float m_miniBombsSpawnRadius;
    public float m_miniBombsSpawnVelocity;
    public float m_miniBombsDamage;
    public float m_explodeVelocity;

    public BulletEvent m_explodeEvent;
    private ObjectPooler m_pooler;
    private Coroutine m_explodeInArc;

    [Header("Debugging Cluster")]
    public bool m_debugCluster;
    public Color m_clusterGizmoColor1;

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!m_debugCluster) return;
        Gizmos.color = m_clusterGizmoColor1;
        Gizmos.DrawWireSphere(transform.position, m_miniBombsSpawnRadius);
    }

    public override void SetVariables(Vector3 p_newVelocity, float p_damage, Transform p_target = null)
    {
        base.SetVariables(p_newVelocity, p_damage, p_target);

        if(m_pooler == null)
        {
            m_pooler = ObjectPooler.instance;
        }
        m_explodeInArc = StartCoroutine(CheckExplosion());
    }

    private IEnumerator CheckExplosion()
    {
        while (m_velocity.y > m_explodeVelocity)
        {
            yield return null;
        }
        Explode();
        DestroyBullet();
    }

    public void Explode()
    {
        StopAllCoroutines();

        m_explodeEvent.Invoke();

        for (int i = 0; i < m_numOfMiniBombs; i++)
        {
            BulletsBehaviour newBomb = m_pooler.NewObject(m_miniBombs, transform.position + Random.insideUnitSphere * m_miniBombsSpawnRadius, Quaternion.identity).GetComponent<BulletsBehaviour>();
            newBomb.SetVariables(((newBomb.transform.position - transform.position) * m_miniBombsSpawnVelocity) + m_velocity, m_miniBombsDamage);
        }
    }
}
