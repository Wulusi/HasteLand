using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[System.Serializable]
public class BuildingManagerEvents : UnityEngine.Events.UnityEvent { }

/// <summary>
/// Manages all the building, and upgrading, that the player performs.
/// </summary>
public class BuildingManager : MonoBehaviour, IPausable
{
    public Camera m_mainCamera;
    public EventSystem m_buildingEventsSystem;

    private bool m_enablePlayerBuilding;
    public LayerMask m_buildingLayer, m_turretsLayer;

    private BuildSpot m_currentBuildSpot;
    private int m_menuTypeOpened;

    public BuildingEvents m_events;




    [System.Serializable]
    public struct BuildingEvents
    {
        public BuildingManagerEvents m_openBuildMenuEvent;
        public BuildingManagerEvents m_closeBuildMenuEvent;

        public BuildingManagerEvents m_openUpgradeMenuEvent;
        public BuildingManagerEvents m_closeUpgradeMenuEvent;
    }

    #region Building Turrets
    [Header("Building Turrets")]
    public List<BuildingTurretsVars> m_buildingTurretVars;
    [System.Serializable]
    public struct BuildingTurretsVars
    {
        public TurretUpgrades m_turretCost;
        public UnityEngine.UI.Button m_upgradeButton;
    }

    private Coroutine m_checkBuildingCosts;
    #endregion

    #region Turret Upgrades

    [Header("Turret Upgrades")]
    public TurretUpgradeVariables m_turretUpgradeVars;
    [System.Serializable]
    public struct TurretUpgradeVariables
    {
        [Tooltip("The button that upgrades the turret")]
        public UnityEngine.UI.Button m_btn_upgradeButton;
        [Tooltip("The button that pops up when the turret can no longer be upgraded")]
        public UnityEngine.UI.Button m_btn_turretFullyUpgraded;
        [Tooltip("The text of the upgrade button")]
        public UnityEngine.UI.Text m_upgradeText;
        [Tooltip("The text of the upgrade cost button")]
        public UnityEngine.UI.Text m_upgradeCostText;
        [Tooltip("The text of the return cost button")]
        public UnityEngine.UI.Text m_returnCost;
        [Tooltip("The text that appears on the upgrade button, showing the name of the new turret upgrade")]
        public List<TurretUpgradeTexts> m_turretUpgradeTexts;
    }

    [System.Serializable]
    public struct TurretUpgradeTexts
    {
        public string m_upgradeText1, m_upgradeText2;
        public float m_upgradeCost1, m_upgradeCost2;
        public float m_returnCost, m_returnCost2, m_returnCost3;
    }

    private Coroutine m_enableUpgradesCoroutine;
    private TurretUpgrades m_currentTurretUpgrade;
    private CurrencyManager m_currencyManager;
    #endregion

    private void Start()
    {
        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }
        m_currencyManager = CurrencyManager.Instance;
        AddMeToPauseManager(PauseManager.Instance);
    }

    private void Update()
    {
        if (m_isPaused) return;
        CheckScreenTap();

    }




    private void CheckScreenTap()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!m_enablePlayerBuilding)
            {
                RaycastHit hit;

                #region Player Selects Build Spot
                if (Physics.Raycast(m_mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100f, m_buildingLayer))
                {
                    m_currentBuildSpot = hit.transform.GetComponent<BuildSpot>();
                    m_enablePlayerBuilding = true;
                    m_menuTypeOpened = 0;
                    m_checkBuildingCosts = StartCoroutine(EnableTurretBuildings());
                }
                #endregion

                #region Player Selects Turret
                else if (Physics.Raycast(m_mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100f, m_turretsLayer))
                {
                    m_currentTurretUpgrade = hit.transform.GetComponent<TurretUpgrades>();
                    m_enableUpgradesCoroutine = StartCoroutine(EnableTurretUpgrades());
                    m_enablePlayerBuilding = true;
                    m_menuTypeOpened = 1;

                }
                #endregion
            }
        }

    }

    public void CloseMenu()
    {
        m_enablePlayerBuilding = false;
        if (m_menuTypeOpened == 0)
        {
            m_events.m_closeBuildMenuEvent.Invoke();
            if (m_checkBuildingCosts != null)
            {
                StopCoroutine(m_checkBuildingCosts);
            }
        }
        else if (m_menuTypeOpened == 1)
        {
            TurretUpgradeMenuButtonPressed(3);
        }
    }

    public void ChooseNewTurretType(int p_turretType)
    {
        m_events.m_closeBuildMenuEvent.Invoke();
        m_currentBuildSpot.SpawnTurret(p_turretType);
        m_enablePlayerBuilding = false;
        m_currencyManager.SpendCurrency(m_buildingTurretVars[p_turretType].m_turretCost.m_turretUpgradeLevels[0].m_newTurretCost);
    }

    /// <summary>
    /// Sets the building butttons interactivity, based upon the current amount of currency the player has
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableTurretBuildings()
    {
        bool higherThanAllCosts = false;
        foreach (BuildingTurretsVars currentCost in m_buildingTurretVars)
        {
            currentCost.m_upgradeButton.interactable = false;
        }
        m_events.m_openBuildMenuEvent.Invoke();
        while (!higherThanAllCosts)
        {
            higherThanAllCosts = true;
            foreach (BuildingTurretsVars currentCost in m_buildingTurretVars)
            {
                if (currentCost.m_turretCost.m_turretUpgradeLevels[0].m_newTurretCost > m_currencyManager.GetCurrentCurrency())
                {
                    higherThanAllCosts = false;
                    currentCost.m_upgradeButton.interactable = false;
                }
                else
                {
                    currentCost.m_upgradeButton.interactable = true;
                }
            }
            yield return null;
        }
    }


    /// <summary>
    /// Sets the upgrade butttons interactivity, based upon the current amount of currency the player has
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableTurretUpgrades()
    {


        #region Determine which buttons to show
        m_turretUpgradeVars.m_btn_upgradeButton.interactable = false;

        

        m_turretUpgradeVars.m_btn_turretFullyUpgraded.gameObject.SetActive(false);
        m_turretUpgradeVars.m_btn_upgradeButton.gameObject.SetActive(true);

        if (m_currentTurretUpgrade.GetCurrentUpgradeLevel() == 0)
        {
            m_turretUpgradeVars.m_returnCost.text = m_turretUpgradeVars.m_turretUpgradeTexts[m_currentTurretUpgrade.m_towerTypeIndex].m_returnCost.ToString();
            m_turretUpgradeVars.m_upgradeText.text = m_turretUpgradeVars.m_turretUpgradeTexts[m_currentTurretUpgrade.m_towerTypeIndex].m_upgradeText1;
            m_turretUpgradeVars.m_upgradeCostText.text = m_turretUpgradeVars.m_turretUpgradeTexts[m_currentTurretUpgrade.m_towerTypeIndex].m_upgradeCost1.ToString();
        }
        else if (m_currentTurretUpgrade.GetCurrentUpgradeLevel() == 1)
        {
            m_turretUpgradeVars.m_returnCost.text = m_turretUpgradeVars.m_turretUpgradeTexts[m_currentTurretUpgrade.m_towerTypeIndex].m_returnCost2.ToString();
            m_turretUpgradeVars.m_upgradeText.text = m_turretUpgradeVars.m_turretUpgradeTexts[m_currentTurretUpgrade.m_towerTypeIndex].m_upgradeText2;
            m_turretUpgradeVars.m_upgradeCostText.text = m_turretUpgradeVars.m_turretUpgradeTexts[m_currentTurretUpgrade.m_towerTypeIndex].m_upgradeCost2.ToString();
        }
        else
        {
            m_turretUpgradeVars.m_returnCost.text = m_turretUpgradeVars.m_turretUpgradeTexts[m_currentTurretUpgrade.m_towerTypeIndex].m_returnCost3.ToString();
            m_turretUpgradeVars.m_btn_upgradeButton.gameObject.SetActive(false);
            m_turretUpgradeVars.m_btn_turretFullyUpgraded.gameObject.SetActive(true);
        }
        #endregion

        m_events.m_openUpgradeMenuEvent.Invoke();

        float currentCost = m_currentTurretUpgrade.GetCurrentUpgradeCost();
        if (currentCost < Mathf.Infinity)
        {
            while (m_currencyManager.GetCurrentCurrency() < currentCost || !m_currentTurretUpgrade.GetUpgradeState())
            {
                yield return null;
            }
            m_turretUpgradeVars.m_btn_upgradeButton.interactable = true;
        }
    }


    public void TurretUpgradeMenuButtonPressed(int p_buttonType)
    {
        //Turret Upgrade
        if (p_buttonType == 0)
        {
            m_currencyManager.SpendCurrency(m_currentTurretUpgrade.GetCurrentUpgradeCost());
            m_currentTurretUpgrade.UpgradeTurret();

        }

        //Destroy turret
        else if (p_buttonType == 1)
        {
            m_currencyManager.GiveCurrency(m_currentTurretUpgrade.GetCurrentReturnCost());
            m_currentTurretUpgrade.DestroyTurret();
        }
        //p_button == 3 when you just wanna close the menu;

        if (m_enableUpgradesCoroutine != null)
        {
            StopCoroutine(m_enableUpgradesCoroutine);
        }

        m_events.m_closeUpgradeMenuEvent.Invoke();
        m_enablePlayerBuilding = false;
    }


    #region Pausing
    private bool m_isPaused;
    public void SetPauseState(bool p_paused)
    {
        m_isPaused = p_paused;
    }

    public void AddMeToPauseManager(PauseManager p_pauseManager)
    {
        p_pauseManager.AddNewObject(this);
    }
    #endregion
}
