using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave_Scriptable> m_allWaveTypes;
    public static WaveManager Instance;
    private Wave_Scriptable m_myWaveType;
    private void Awake()
    {
        Instance = this;
        m_myWaveType = m_allWaveTypes[LevelPicker.Instance.GetLevel()];
    }

    public Wave_Scriptable GetCurrentWaveType()
    {
        return m_myWaveType;
    }
    public float GetLevelTime()
    {
        return m_myWaveType.m_levelDuration;
    }
    public float GetCurrentCurrency()
    {
        return m_myWaveType.m_playerStartingCurrency;
    }
}
