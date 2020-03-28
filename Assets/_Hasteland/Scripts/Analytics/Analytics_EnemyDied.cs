using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics_EnemyDied : MonoBehaviour
{
    public void IDied()
    {
        GameHub.TrackEventsContinuous.AddEnemiesKilled();
    }
}
