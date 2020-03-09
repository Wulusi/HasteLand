using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFollower : MonoBehaviour
{
    private ObjectPooler m_pooler;
    public FollowTransform m_spawnedFollower;

    public void SpawnFollowerObject()
    {
        if(m_pooler == null)
        {
            m_pooler = ObjectPooler.instance;
        }
        m_pooler.NewObject(m_spawnedFollower.gameObject, transform.position, transform.rotation).GetComponent<FollowTransform>().SetFollower(transform);
    }
}
