﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave", menuName = "Wave", order = 0)]
public class Wave_Scriptable : ScriptableObject
{
    public enum SpawnType { Ground, Air, Both}

    public float m_levelDuration;
    public float m_playerStartingCurrency;

    [System.Serializable]
    public struct WaveType
    {
        public float m_endTime;
        public int m_maxEnemies;
        public SpawnType m_waveSpawns;
    }
    public List<WaveType> m_waves;


}
