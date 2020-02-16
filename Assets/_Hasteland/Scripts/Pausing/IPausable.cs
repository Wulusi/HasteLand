using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPausable 
{
    void SetPauseState(bool p_paused);

    void AddMeToPauseManager(PauseManager p_pauseManager);
}
