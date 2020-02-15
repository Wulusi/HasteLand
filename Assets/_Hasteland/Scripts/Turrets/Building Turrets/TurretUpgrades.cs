using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgrades : MonoBehaviour
{
    public int m_currentUpgradeLevel;
    [System.Serializable]
    public struct UpgradeLevels
    {
        public float m_newTurretCost;
        public GameObject m_newTurretObject;
    }

    public List<UpgradeLevels> m_turretUpgradeLevels;


    public void UpgradeTurret()
    {
        m_turretUpgradeLevels[m_currentUpgradeLevel].m_newTurretObject.SetActive(false);
        m_currentUpgradeLevel++;
        m_turretUpgradeLevels[m_currentUpgradeLevel].m_newTurretObject.SetActive(true);
    }
}
