using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPicker : MonoBehaviour
{
    public static LevelPicker Instance;
    private int m_currentLevel;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        
    }

    public void SetLevel(int p_newLevel)
    {
        m_currentLevel = p_newLevel;
    }

    public int GetLevel()
    {
        return m_currentLevel;
    }
}
