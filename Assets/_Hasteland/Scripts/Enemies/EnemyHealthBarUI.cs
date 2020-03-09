using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField]
    protected Camera mainCamera;

    [SerializeField]
    private Health health;

    [SerializeField]
    Image healthBar;

    private float LastHealth;

    #region AutoSerialization
    /// <summary>
    /// Data Getter for all components, runs when in editor mode to reduce overhead
    /// </summary>
#if UNITY_EDITOR

    private void GetRef()
    {
        if (!Application.isEditor)
        {
            return;
        }
        else
        {
            health = GetComponentInParent<Health>();
            //Assuming only one camera exist in game
            mainCamera = FindObjectOfType<Camera>();
        }
    }

    protected virtual void OnValidate()
    {
        GetRef();
    }

#endif
    #endregion

    public void OnEnable()
    {
        mainCamera = FindObjectOfType<Camera>();
        health = GetComponentInParent<Health>();
        StartCoroutine(StartHealthBars());
        LastHealth = health.m_maxHealth;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartHealthBars()
    {
        while (true)
        {
            FaceCamera();
            UpdateUI();
            yield return null;
        }
    }

    private void FaceCamera()
    {
        var LookBack = transform.position + mainCamera.transform.rotation * Vector3.back;
        var LookUp = mainCamera.transform.rotation * Vector3.up;

        transform.LookAt(LookBack, LookUp);
    }

    private void UpdateUI()
    {
        
        var currentHealth = health.m_currentHealth / health.m_maxHealth;

        if (healthBar.fillAmount != currentHealth)
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth, Time.deltaTime * 5f);
        }
    }

}
