using UnityEngine;

public class TerrainDespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }
}
