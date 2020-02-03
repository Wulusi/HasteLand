using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretVisualController : MonoBehaviour
{
    private Animator m_animCont;
    [System.Serializable]
    public struct TurretEventTypes
    {
        public TurretEvent m_startBuildingEvent;
        public TurretEvent m_builtEvent;

        public TurretEvent m_turretDied;
        
    }
    public TurretEventTypes m_turretEvents;

    [System.Serializable]
    public struct AnimationVariableNames
    {
        public string m_startBuilding;
    }
    public AnimationVariableNames m_animNames;

    private void Awake()
    {
        m_animCont = GetComponent<Animator>();
    }
    public void StartBuilding()
    {
        m_animCont.SetBool(m_animNames.m_startBuilding, true);
        m_turretEvents.m_startBuildingEvent.Invoke();
    }

    public void FinishedBuilding()
    {
        m_animCont.SetBool(m_animNames.m_startBuilding, false);
        m_turretEvents.m_builtEvent.Invoke();
    }

    public void TurretDied()
    {
        m_turretEvents.m_turretDied.Invoke();
    }
}

