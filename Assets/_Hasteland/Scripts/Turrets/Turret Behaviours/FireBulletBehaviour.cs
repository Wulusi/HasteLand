﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletBehaviour : MonoBehaviour, IPausable
{
    public Transform m_fireSpot;
    public GameObject m_bulletPrefab;
    private ObjectPooler m_pooler;

    public float m_projectileSpeed;
    public float m_bulletDamage;
    public TurretController m_turretController;
    private void Start()
    {
        m_pooler = ObjectPooler.instance;
        AddMeToPauseManager(PauseManager.Instance);
    }
    public virtual void FireBullet()
    {
        BulletsBehaviour newBullet = m_pooler.NewObject(m_bulletPrefab, m_fireSpot.position, m_fireSpot.rotation).GetComponent<BulletsBehaviour>();
        if (m_turretController != null)
        {
            newBullet.SetVariables(m_fireSpot.forward * m_projectileSpeed, m_bulletDamage, m_turretController.m_currentTarget.transform);
        }
        else
        {
            newBullet.SetVariables(m_fireSpot.forward * m_projectileSpeed, m_bulletDamage);
        }
        
    }

    #region Pausing
    private bool m_isPaused;
    public void SetPauseState(bool p_paused)
    {
        m_isPaused = p_paused;
    }

    public bool AmIPaused()
    {
        return m_isPaused;
    }

    public void AddMeToPauseManager(PauseManager p_pauseManager)
    {
        p_pauseManager.AddNewObject(this);
    }
    #endregion
}
