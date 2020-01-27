using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletBehaviour : MonoBehaviour
{
    public Transform m_fireSpot;
    public GameObject m_bulletPrefab;
    private ObjectPooler m_pooler;

    public float m_projectileSpeed;
    private void Start()
    {
        m_pooler = ObjectPooler.instance;
    }
    public void FireBullet()
    {
        BulletsBehaviour newBullet = m_pooler.NewObject(m_bulletPrefab, m_fireSpot.position, m_fireSpot.rotation).GetComponent<BulletsBehaviour>();
        newBullet.SetVelocity(m_fireSpot.forward * m_projectileSpeed);
    }
}
