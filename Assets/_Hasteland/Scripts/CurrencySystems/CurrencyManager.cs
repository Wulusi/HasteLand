using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField]
    private Text m_currencyUI;

    public float m_currentCurrency;
    private void Awake()
    {
        Instance = this;
        StartCoroutine(UpdateUI());
    }

    private void Start()
    {
        m_currentCurrency = WaveManager.Instance.GetCurrentCurrency();
    }
    public float GetCurrentCurrency()
    {
        return m_currentCurrency;
    }

    public void SpendCurrency(float p_amountSpent)
    {
        m_currentCurrency -= p_amountSpent;
    }

    public void GiveCurrency(float p_amountEarned)
    {
        m_currentCurrency += p_amountEarned;
    }

    private IEnumerator UpdateUI()
    {
        while (true)
        {
            m_currencyUI.text = m_currentCurrency.ToString();
            yield return null;
        }
    }
}
