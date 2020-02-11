﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelfDestruct : MonoBehaviour
{
    private ParticleSystem m_attachedParticalSystem;

    [HideInInspector]
    public float m_destructTime;
    private float m_destructTimer;

    private void Start()
    {
        m_attachedParticalSystem = GetComponent<ParticleSystem>();
        m_destructTime = m_attachedParticalSystem.main.duration + m_attachedParticalSystem.main.startLifetimeMultiplier;
    }

    private void Update()
    {
        if (isActiveAndEnabled)
        {
            m_destructTimer += Time.deltaTime;
        }

        if (m_destructTimer >= m_destructTime)
        {
            m_destructTimer = 0;
            ObjectPooler.instance.ReturnToPool(this.gameObject);
        }
    }
}