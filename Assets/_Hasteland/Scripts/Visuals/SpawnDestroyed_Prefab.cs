using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used on the destroyed version gameobject. Does not spawn the object.
/// </summary>
public class SpawnDestroyed_Prefab : MonoBehaviour
{
    public List<Transform> m_objects;
    private List<Vector3> m_objectsStarts = new List<Vector3>();
    private List<Quaternion> m_objectStartingRotation = new List<Quaternion>();

    private List<Rigidbody> m_rbs = new List<Rigidbody>();
    public float m_explosionForce;
    public float m_explosionRadius;
    public float m_upwardsModifier;

    private bool m_initialized;

    public float m_maxLifespan;
    private ObjectPooler m_pooler;
    private WaitForSeconds m_delay;
    [Header("Debugging")]
    public bool m_debugging;
    public Color m_gizmosColor1;

    private void Initiate()
    {
        m_pooler = ObjectPooler.instance;
        m_delay = new WaitForSeconds(m_maxLifespan);
        foreach (Transform newObj in m_objects)
        {
            m_objectsStarts.Add(transform.InverseTransformPoint(newObj.transform.position));
            m_objectStartingRotation.Add(newObj.transform.rotation);
            Rigidbody rb = newObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                m_rbs.Add(rb);
            }
        }
    }

    public void SpawnDestroyed(Vector3 p_addedVelocity)
    {
        if (!m_initialized)
        {
            m_initialized = true;
            Initiate();
        }

        foreach (Rigidbody rb in m_rbs)
        {
            rb.velocity = p_addedVelocity;
            rb.AddExplosionForce(m_explosionForce, transform.position, m_explosionRadius, m_upwardsModifier, ForceMode.Impulse);
        }
        StartCoroutine(Lifespan());
    }

    private void OnDrawGizmos()
    {
        if (!m_debugging) return;
        Gizmos.color = m_gizmosColor1;
        Gizmos.DrawWireSphere(transform.position, m_explosionRadius);
    }

    private IEnumerator Lifespan()
    {
        yield return m_delay;
        ResetMe();
        m_pooler.ReturnToPool(this.gameObject);
    }

    private void ResetMe()
    {
        foreach (Rigidbody rb in m_rbs)
        {
            rb.velocity = Vector3.zero;
        }

        foreach (Transform currentObj in m_objects)
        {
            currentObj.transform.rotation = m_objectStartingRotation[m_objects.IndexOf(currentObj)];
            currentObj.transform.localPosition = m_objectsStarts[m_objects.IndexOf(currentObj)];
        }
    }
}
