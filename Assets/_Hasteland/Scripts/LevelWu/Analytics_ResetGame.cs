using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics_ResetGame : MonoBehaviour
{
    public void ResetGame()
    {
        GameHub.TrackEventsContinuous.RestartGame();
    }
}
