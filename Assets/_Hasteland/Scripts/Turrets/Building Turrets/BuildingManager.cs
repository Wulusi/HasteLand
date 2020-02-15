using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[System.Serializable]
public class BuildingManagerEvents : UnityEngine.Events.UnityEvent{ }
public class BuildingManager : MonoBehaviour
{
    public bool m_enablePlayerBuilding;
    public LayerMask m_buildingLayer;
    public Camera m_mainCamera;

    public BuildingEvents m_events;
    [System.Serializable]
    public struct BuildingEvents
    {
        public BuildingManagerEvents m_openBuildMenuEvent;
        public BuildingManagerEvents m_closeBuildMenuEvent;

    }
    private void Start()
    {
        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }
    }

    private void Update()
    {

        CheckScreenTap();

    }


    private BuildSpot m_currentBuildSpot;
    
    private void CheckScreenTap()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!m_enablePlayerBuilding)
            {
                RaycastHit hit;
                if (Physics.Raycast(m_mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100f, m_buildingLayer))
                {
                    m_currentBuildSpot = hit.transform.GetComponent<BuildSpot>();
                    m_enablePlayerBuilding = true;
                    m_events.m_openBuildMenuEvent.Invoke();
                }
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    m_enablePlayerBuilding = false;
                    m_events.m_closeBuildMenuEvent.Invoke();
                }
            }
        }

    }

    public void ChooseNewTurretType(int p_turretType)
    {
        m_events.m_closeBuildMenuEvent.Invoke();
        m_currentBuildSpot.SpawnTurret(p_turretType);
        m_enablePlayerBuilding = false;
    }


}
