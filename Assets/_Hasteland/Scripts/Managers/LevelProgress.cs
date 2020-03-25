using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelProgressEvents : UnityEngine.Events.UnityEvent { }
public class LevelProgress : MonoBehaviour, IPausable
{
    public float m_levelTime;
    public float m_startLevelTime;

    public LevelEvents m_levelEvents;
    [System.Serializable]
    public struct LevelEvents
    {
        public LevelProgressEvents m_levelStarted, m_levelCompleted, m_levelFailed;
    }

    [Header("Countdown Canvas Elements")]
    public CanvasGroup m_countdownCanvasGroup;
    public UnityEngine.UI.Text m_countdownText;
    public float m_fadeTime;

    [Header("Progress UI Elements")]
    public Transform m_beginPos;
    public Transform m_endPos, m_progressPos;
    public UnityEngine.UI.Image m_progressBar;

    private bool m_died;

    private void Start()
    {
        AddMeToPauseManager(PauseManager.Instance);
        StartCoroutine(StartLevelTimer());
    }

    public void StartLevel()
    {
        m_levelEvents.m_levelStarted.Invoke();
        StartCoroutine(LevelTimer());
    }
    public void LevelFailed()
    {
        StopAllCoroutines();
        m_levelEvents.m_levelFailed.Invoke();
        m_died = true;
    }
    public void LevelCompleted()
    {
        m_levelEvents.m_levelCompleted.Invoke();
    }
    private IEnumerator LevelTimer()
    {
        float timer = 0;
        while (timer < m_levelTime)
        {
            if (!m_isPaused && !m_died)
            {
                timer += Time.deltaTime;
            }
            UpdateUI(timer / m_levelTime);
            yield return null;
        }
        LevelCompleted();
    }

    private IEnumerator StartLevelTimer()
    {
        float timer = 0;
        float timerCountdown = 3;
        Coroutine fadeCoroutine = StartCoroutine(FadeCavasGroup(timerCountdown));
        UpdateUI(0);
        while (timer < m_startLevelTime)
        {
            if (timer >= 4 - timerCountdown)
            {
                timerCountdown -= 1;
                if(fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(FadeCavasGroup(timerCountdown));
            }
            yield return null;
            timer += Time.deltaTime;
        }
        StartLevel();
    }

    private IEnumerator FadeCavasGroup(float p_time)
    {
        float timer = 0;
        m_countdownText.text = ((int)p_time).ToString();
        while (timer < m_fadeTime)
        {
            yield return null;
            timer += Time.deltaTime;
            m_countdownCanvasGroup.alpha = 1 - (timer / m_fadeTime);
        }
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
