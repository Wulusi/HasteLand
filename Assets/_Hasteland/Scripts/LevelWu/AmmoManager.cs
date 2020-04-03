using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GunHitEvent : UnityEngine.Events.UnityEvent { }

public class AmmoManager : MonoBehaviour
{
    [SerializeField]
    private Text ammo;

    [SerializeField]
    private Text outOfAmmo;

    public float currentAmmo, maxAmmo, ammoAdd, shotDamage, rayLength;

    public bool canFire;

    private int layer_mask;

    public GameObject hitParticle;

    public PoolManager objectPooler;

    [SerializeField]
    GunHitEvent gunHitEvent;

    [SerializeField]
    GunHitEvent gunHitEvent2;

    public LayerMask m_nonHitLayer;
    public UnityEngine.EventSystems.EventSystem m_eventSystem;


    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        currentAmmo = maxAmmo;
        StartCoroutine(CheckAmmo());

        objectPooler = PoolManager.Instance;

        layer_mask = (1 << LayerMask.NameToLayer("GroundEnemy")) | (1 << LayerMask.NameToLayer("AirEnemy"));
    }

    IEnumerator CheckAmmo()
    {
        while (true)
        {
            Ammo();
            CheckRay();
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

    public void CheckRay()
    {
        if (Input.GetMouseButtonUp(0) && canFire)
        {
            if (m_eventSystem.IsPointerOverGameObject()) return;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, m_nonHitLayer))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
                {
                    //Debug.Log("ray hit " + hit.collider.name);

                    var hitObj = hit.collider.GetComponent<Health>();

                    hitObj.TakeDamage(shotDamage);

                    objectPooler.SpawnFromPool
                        (hitParticle.name, hit.point - ray.direction.normalized * rayLength, Quaternion.identity).
                        GetComponentInChildren<ParticleSystem>().Play();

                    gunHitEvent.Invoke();
                }
                else
                {
                    gunHitEvent2.Invoke();
                }
                currentAmmo--;
            }
        }
    }
}
