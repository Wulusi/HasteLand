using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    public List<GameObject> m_turretPrefabs;
    public List<GameObject> m_turretDestroyedPrefabs;

    private ObjectPooler m_pooler;
    public Transform m_turretLocation;
    private GameObject m_currentTurret;

    private void Start()
    {
        m_pooler = ObjectPooler.instance;
    }
    public void SpawnTurret(int p_turretType)
    {
        m_currentTurret = m_pooler.NewObject(m_turretPrefabs[p_turretType], m_turretLocation.position, Quaternion.identity);

        m_currentTurret.transform.parent = transform.parent;
        this.gameObject.SetActive(false);
    }

    public void DespawnTurret()
    {
        m_pooler.NewObject(m_turretDestroyedPrefabs[m_turretPrefabs.IndexOf(m_currentTurret)], m_turretLocation.position, m_currentTurret.transform.rotation);
        this.gameObject.SetActive(true);
    }


}
