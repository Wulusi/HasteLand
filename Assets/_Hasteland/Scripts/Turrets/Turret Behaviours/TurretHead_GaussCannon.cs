using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GaussEvent : UnityEngine.Events.UnityEvent { }
public class TurretHead_GaussCannon : TurretHead
{
    [Header("Gauss Canon")]
    public GameObject m_gaussBullet;
    public float m_gaussBulletSpeed;
    public float m_gaussBulletDamage;
    public float m_gaussReloadTime;
    private WaitForSeconds m_gaussReloadDelay;
    private bool m_canFireGauss = true;
    public Transform m_gaussFirePos;
    public GaussEvent m_gaussFired;
    private ObjectPooler m_gaussPooler;
    
    public override void Start()
    {
        base.Start();
        m_gaussReloadDelay = new WaitForSeconds(m_gaussReloadTime);
        m_gaussPooler = ObjectPooler.instance;
    }
    public override void FireTurret()
    {
        base.FireTurret();
        if (m_canFireGauss)
        {
            m_canFireGauss = false;
            m_gaussPooler.NewObject(m_gaussBullet, m_gaussFirePos.position, m_gaussFirePos.rotation).GetComponent<BulletsBehaviour>().SetVariables(m_gaussFirePos.forward * m_gaussBulletSpeed, m_gaussBulletDamage);


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
