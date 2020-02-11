using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GaussEvent : UnityEngine.Events.UnityEvent { }
public class TurretHead_GaussCannon : TurretHead
{
    public LayerMask m_gaussHitMask;
    public LayerMask m_gaussBoundsMask;
    public float m_gaussReloadTime;
    public float m_gaussCannonDamage;
    private WaitForSeconds m_gaussReloadDelay;
    private bool m_canFireGauss = true;
    public Transform m_gaussFirePos;
    public GaussEvent m_gaussFired;
    public override void Start()
    {
        base.Start();
        m_gaussReloadDelay = new WaitForSeconds(m_gaussReloadTime);
    }
    public override void FireTurret()
    {
        base.FireTurret();
        if (m_canFireGauss)
        {

            RaycastHit hit;
            if (Physics.Raycast(m_gaussFirePos.position, m_gaussFirePos.forward, out hit, 200, m_gaussBoundsMask))
            {
                //Draw line here
                Debug.DrawLine(m_gaussFirePos.position, hit.point, Color.blue, 1f);
                RaycastHit[] hits = Physics.RaycastAll(m_gaussFirePos.position, m_gaussFirePos.forward, Vector3.Distance(transform.position, hit.point), m_gaussHitMask);
                foreach(RaycastHit newHit in hits)
                {
                    Health health = newHit.transform.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeDamage(m_gaussCannonDamage);
                    }
                }
            }
            StartCoroutine(GaussReload());
        }
    }

    private IEnumerator GaussReload()
    {
        m_gaussFired.Invoke();
        m_canFireGauss = false;
        yield return m_gaussReloadDelay;
        m_canFireGauss = true;
    }
}
