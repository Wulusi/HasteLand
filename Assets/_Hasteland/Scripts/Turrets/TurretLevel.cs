using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLevel : MonoBehaviour
{

    #region Turret Rotation Levels
    public int m_currentRotationIndex;
    public List<TurretRotationSpeed> m_turretRotationLevels;
    [System.Serializable]
    public struct TurretRotationSpeed
    {
        public float m_turretRotationSpeed;
    }

    public float GetCurrentRotationSpeed()
    {
        return m_turretRotationLevels[m_currentRotationIndex].m_turretRotationSpeed;
    }
    #endregion

    #region TurretFireRates
    [Space(10)]
    public int m_currentFireRateIndex;
    public List<TurretFireRate> m_turretFireRates;
    [System.Serializable]
    public struct TurretFireRate
    {
        public float m_turretFireRate;
    }
    public float GetCurrentFireRate()
    {
        return m_turretFireRates[m_currentFireRateIndex].m_turretFireRate;
    }
    #endregion

    #region TurretProjectiles
    [Space(10)]
    public int m_currentProjectileIndex;
    public List<TurretProjectiles> m_turretProjectiles;
    [System.Serializable]
    public struct TurretProjectiles
    {
        public GameObject m_turretProjectile;
    }

    public GameObject GetCurrentProjectileType()
    {
        return m_turretProjectiles[m_currentProjectileIndex].m_turretProjectile;
    }
    #endregion


}
