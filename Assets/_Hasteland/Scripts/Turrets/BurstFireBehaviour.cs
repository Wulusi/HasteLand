using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFireBehaviour : FireBulletBehaviour
{
    public int m_numOfBullets;
    public float m_burstDelay;
    private Coroutine m_burstFire;

    public override void FireBullet()
    {
        m_burstFire = StartCoroutine(SpawnBurst());
    }

    private IEnumerator SpawnBurst()
    {
        int m_burstNum = 1;
        bool firing = true;
        float timer = 0;
        base.FireBullet();
        while (firing)
        {
            if(timer > m_burstDelay)
            {
                base.FireBullet();
                m_burstNum++;
                if (m_burstNum == m_numOfBullets)
                {
                    firing = false;
                }
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
    }
}
