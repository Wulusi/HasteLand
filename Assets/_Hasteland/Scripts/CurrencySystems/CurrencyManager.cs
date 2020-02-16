using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public float m_currentCurrency;
    private void Awake()
    {
        Instance = this;
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
}
