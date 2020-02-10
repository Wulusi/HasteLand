using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [Header("Explosion Variables")]

    public float m_explodeRadius;
    public float m_explodeDamage;
    public LayerMask m_explosionHitLayer;

    [Header("Debugging")]
    public bool m_debug;
    public Color m_gizmoColor1;

    private void OnDrawGizmos()
    {
        if (!m_debug) return;
        Gizmos.color = m_gizmoColor1;
        Gizmos.DrawWireSphere(transform.position, m_explodeRadius);
    }

    public void ExplodeMe()
    {
        print("Exploide");
        Collider[] cols = Physics.OverlapSphere(transform.position, m_explodeRadius, m_explosionHitLayer);
        foreach (Collider col in cols)
        {
            col.GetComponent<Health>().TakeDamage(m_explodeDamage);
        }
    }
}
