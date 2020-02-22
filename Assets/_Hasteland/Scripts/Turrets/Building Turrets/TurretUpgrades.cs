using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgrades : MonoBehaviour
{
    [Tooltip("Used by the building manager, to determine what type of tower this is.")]
    public int m_towerTypeIndex;
    public int m_currentUpgradeLevel;

    private bool m_canUpgrade = true;
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

    public void EnableUpgrading()
    {
        m_canUpgrade = true;
    }
    public void DisableUpgrading()
    {
        m_canUpgrade = false;
    }

    public bool GetUpgradeState()
    {
        return m_canUpgrade;
    }

    public float GetCurrentUpgradeCost()
    {
        if (m_currentUpgradeLevel +1 >= m_turretUpgradeLevels.Count)
        {
            return Mathf.Infinity;
        }
        return m_turretUpgradeLevels[m_currentUpgradeLevel + 1].m_newTurretCost;
    }

    public int GetCurrentUpgradeLevel()
    {
        return m_currentUpgradeLevel;
    }

    public void DestroyTurret()
    {

    }
}
