using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelProgressEvents : UnityEngine.Events.UnityEvent { }
public class LevelProgress : MonoBehaviour, IPausable
{


    public float m_levelTime;

    public LevelProgressEvents m_levelCompleted, m_levelFailed;

    [Header("UI Elements")]
    public Transform m_beginPos;
    public Transform m_endPos, m_progressPos;
    public UnityEngine.UI.Image m_progressBar;

    private void Start()
    {
        AddMeToPauseManager(PauseManager.Instance);
    }

    public void StartLevel()
    {
        StartCoroutine(LevelTimer());
    }
    public void LevelFailed()
    {
        StopAllCoroutines();
        m_levelFailed.Invoke();
    }
    private IEnumerator LevelTimer()
    {
        float timer = 0;
        while (timer < m_levelTime)
        {
            if (!m_isPaused)
            {
                timer += Time.deltaTime;
            }
            UpdateUI(timer / m_levelTime);
            yield return null;
        }

        m_levelCompleted.Invoke();
    }

    private void UpdateUI(float p_progress)
    {
        m_progressPos.transform.position = Vector3.Lerp(m_beginPos.position, m_endPos.position, p_progress);
        m_progressBar.fillAmount = p_progress;
    }


    #region Pause
    private bool m_isPaused;
    public void AddMeToPauseManager(PauseManager p_pauseManager)
    {
        p_pauseManager.AddNewObject(this);
    }

    public void SetPauseState(bool p_paused)
    {
        m_isPaused = p_paused;
    }
    #endregion
}
