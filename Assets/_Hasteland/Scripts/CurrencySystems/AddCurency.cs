using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script should go on anything that can give the player currency
/// IE. Enemies, Pickups, etc.
/// </summary>
public class AddCurency : MonoBehaviour
{
    public float m_amountToGive;

    private CurrencyManager m_currencyManager;
    private void Start()
    {
        m_currencyManager = CurrencyManager.Instance;
    }
    public void GivePlayerCurrency()
    {
        m_currencyManager.GiveCurrency(m_amountToGive);
    }
}
