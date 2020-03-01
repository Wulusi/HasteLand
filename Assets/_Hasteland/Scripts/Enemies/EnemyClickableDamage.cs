using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables enemies to take damage if clicked
/// </summary>
public class EnemyClickableDamage : MonoBehaviour
{
    [SerializeField]
    private Health health;
    // Start is called before the first frame update

    public float damageAmt;

    private void OnMouseUp()
    {
        if (GameHub.AmmoManager.canFire)
        {
            health.TakeDamage(damageAmt);
            GameHub.AmmoManager.currentAmmo--;
        }
    }

    #region AutoSerialization
#if UNITY_EDITOR

    private void GetRef()
    {
        if (!Application.isEditor)
        {
            return;
        }
        else
        {
            health = GetComponent<Health>();
        }
    }

    protected virtual void OnValidate()
    {
        GetRef();
    }

#endif
    #endregion
}
