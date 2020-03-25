using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class TrackEventsContinuous : MonoBehaviour
{
    public int numOfTimesPlayed, numOfBulletsFired, numOfEnemiesKilled, numOfTotalUpgrades;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        numOfTimesPlayed = 1;
    }

    public void AddEnemiesKilled()
    {
        numOfEnemiesKilled++;
    }

    public void AddBulletsFired()
    {
        numOfBulletsFired++;
    }

    public void RestartGame()
    {
        ReportPlaySessionData();
        AddSession();
        ResetStats();
    }

    public void AddSession()
    {
        numOfTimesPlayed++;
    }

    public void ResetStats()
    {
        numOfEnemiesKilled = 0;
        numOfTotalUpgrades = 0;
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
            {"number_of_bullets_fired", numOfBulletsFired},
            {"number_of_total_upgrades", numOfTotalUpgrades},
        });
    }
}
