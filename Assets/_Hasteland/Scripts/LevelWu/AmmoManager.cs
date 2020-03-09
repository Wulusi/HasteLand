using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    [SerializeField]
    private Text ammo;

    [SerializeField]
    private Text outOfAmmo;

    public float currentAmmo, maxAmmo, ammoAdd;

    public bool canFire;

    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        currentAmmo = maxAmmo;
        StartCoroutine(CheckAmmo());
    }

    IEnumerator CheckAmmo()
    {
        while (true)
        {
            Ammo();
            yield return null;
        }
    }

    private void Ammo()
    {
        if (currentAmmo > 0)
        {
            canFire = true;
        }
        else if (currentAmmo <= 0)
        {
            canFire = false;
            currentAmmo = 0;
        }
        else
        {
            canFire = false;
        }

        ammo.text = currentAmmo.ToString();
    }

    public void GiveAmmo()
    {
        currentAmmo += ammoAdd;
    }
}
