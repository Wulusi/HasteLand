using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class TrackEventsContinuous : MonoBehaviour
{
    public int
        numOfTimesPlayed,
        numOfBulletsFired,
        numOfEnemiesKilled,
        numOfTotalUpgrades,
        totalAmmoObtained,
        totalCurrency,
        totalDamageToTruck;

    public float sessionTime;

    public bool timerActive;

    // Start is called before the first frame update
    void Start()
    {
        CheckSingeleton();
        DontDestroyOnLoad(this.gameObject);
        RestartGame();
    }

    private void CheckSingeleton()
    {
        if (FindObjectsOfType<TrackEventsContinuous>().Length > 1)
        {
            print("destroying Tracker copy");
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (timerActive)
        {
            sessionTime += Time.deltaTime;
        }
    }

    public void AddEnemiesKilled()
    {
        print("Enemy Killed");
        numOfEnemiesKilled++;
    }

    public void RestartGame()
    {
        AddSession();
        ResetStats();
        timerActive = true;
    }

    public void AddSession()
    {
        numOfTimesPlayed++;
    }

    public void ResetStats()
    {
        print("Reset");
        numOfEnemiesKilled = 0;
        numOfTotalUpgrades = 0;
        numOfBulletsFired = 0;
        totalAmmoObtained = 0;
        totalCurrency = 0;
        totalDamageToTruck = 0;
        sessionTime = 0;
    }

    public void ReportPlaySessionData()
    {
        //Analytics for reporting how many enemies killed, 
        //number of times played,
        //total number of upgrades obtained
        Analytics.CustomEvent("Play_Session_Stats", new Dictionary<string, object>
        {
            {"play_session_number", numOfTimesPlayed},
            {"number_of_enemies_killed", numOfEnemiesKilled},
            {"number_of_total_upgrades", numOfTotalUpgrades},
            {"number_of_bullets_fired", numOfBulletsFired},
            {"total_ammo_obtained", totalAmmoObtained},
            {"total_currency_obtained", totalCurrency},
            {"total_Damage_To_Truck", totalDamageToTruck},
            {"total_game_duration", sessionTime}
        });

        Debug.Log("Data reported");
    }
}
