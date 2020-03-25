using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class TurretUpgrades : MonoBehaviour
{
    [Tooltip("Used by the building manager, to determine what type of tower this is.")]
    public int m_towerTypeIndex;
    public int m_currentUpgradeLevel;

    private bool m_canUpgrade = true;

    //The spot that this turret was built upon.
    private BuildSpot m_occupiedSpot;


    [System.Serializable]
    public struct UpgradeLevels
    {
        public float m_newTurretCost;
        public GameObject m_newTurretObject;
        public float m_returnValue;
    }

    public List<UpgradeLevels> m_turretUpgradeLevels;

    public TurretUpgradeEvents m_turretUpgradeEvents;
    [System.Serializable]
    public struct TurretUpgradeEvents
    {
        public TurretEvent m_turretDestroyedEvent;
    }
    private void OnEnable()
    {
        foreach (UpgradeLevels lvl in m_turretUpgradeLevels)
        {
            lvl.m_newTurretObject.SetActive(false);
        }
        m_turretUpgradeLevels[m_currentUpgradeLevel].m_newTurretObject.SetActive(true);
    }
    public void UpgradeTurret()
    {
        m_turretUpgradeLevels[m_currentUpgradeLevel].m_newTurretObject.SetActive(false);
        m_currentUpgradeLevel++;
        m_turretUpgradeLevels[m_currentUpgradeLevel].m_newTurretObject.SetActive(true);

        //Analytics for reporting which turret is being upgraded to which level
        Analytics.CustomEvent("turret_upgraded", new Dictionary<string, object>
        {
            {"turret_type", this.gameObject.name},
            {"turret_upgrade_level",  m_currentUpgradeLevel },
        });
        GameHub.TrackEventsContinuous.numOfTotalUpgrades++;
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
        if (m_currentUpgradeLevel + 1 >= m_turretUpgradeLevels.Count)
        {
            return Mathf.Infinity;
        }
        return m_turretUpgradeLevels[m_currentUpgradeLevel + 1].m_newTurretCost;
    }

    public float GetCurrentReturnCost()
    {
        return m_turretUpgradeLevels[m_currentUpgradeLevel].m_returnValue;
    }
    public int GetCurrentUpgradeLevel()
    {
        return m_currentUpgradeLevel;
    }

    /// <summary>
    /// Called from the build spot that creates this turret. Used to re-enable the build spot if this turret is destroyed.
    /// </summary>
    /// <param name="p_currentBuildSpot"></param>
    public void AssignBuildSpot(BuildSpot p_currentBuildSpot)
    {
        m_occupiedSpot = p_currentBuildSpot;
    }

    public void DestroyTurret()
    {
        m_currentUpgradeLevel = 0;
        m_turretUpgradeEvents.m_turretDestroyedEvent.Invoke();
        m_occupiedSpot.RebuildBuildSpot();
        ObjectPooler.instance.ReturnToPool(this.gameObject);
    }
}
