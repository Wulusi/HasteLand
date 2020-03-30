using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics_ResetGame : MonoBehaviour
{
    public void ReportData()
    {
        GameHub.TrackEventsContinuous.timerActive = false;
        GameHub.TrackEventsContinuous.ReportPlaySessionData();
    }


    public void ResetGame()
    {
        GameHub.TrackEventsContinuous.timerActive = false;
        GameHub.TrackEventsContinuous.RestartGame();
    }
}
