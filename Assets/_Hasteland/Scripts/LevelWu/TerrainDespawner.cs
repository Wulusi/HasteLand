using UnityEngine;

public class TerrainDespawner : MonoBehaviour
{
    //Deactivates GameObjects as they enter this collider
    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Terrain")
        //Destroy(other.gameObject);

        other.gameObject.SetActive(false);
    }
}
