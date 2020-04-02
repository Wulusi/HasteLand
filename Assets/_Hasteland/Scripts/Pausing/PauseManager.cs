using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public List<GameObject> m_pausableObjectsInScene;
    private List<IPausable> m_allPausableObjects = new List<IPausable>();
    public static PauseManager Instance;

    private void Awake()
    {
        Instance = this;
        foreach(GameObject pauseMe in m_pausableObjectsInScene)
        {
            IPausable pause = pauseMe.GetComponent<IPausable>();
            if (pauseMe != null)
            {
                AddNewObject(pause);
            }
        }
    }

    public void SetPauseState(bool p_isPaused)
    {
        foreach(IPausable pauseMe in m_allPausableObjects)
        {
            pauseMe.SetPauseState(p_isPaused);
        }
    }

    public void AddNewObject(IPausable p_newPausable)
    {
        m_allPausableObjects.Add(p_newPausable);
    }
}
