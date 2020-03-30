using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public int m_nextLevel;

    public float m_loadSceneTime;
    private bool m_startedScene;

    public CanvasGroup m_canvasGroup;

    public void Update()
    {
        if (m_startedScene) return;
        if (Input.GetMouseButtonDown(0))
        {
            m_startedScene = true;
            StartCoroutine(StartLoading());
        }
    }

    private IEnumerator StartLoading()
    {
        float timer = 0;
        while(timer < m_loadSceneTime)
        {
            timer += Time.deltaTime;
            yield return null;
            m_canvasGroup.alpha = timer / m_loadSceneTime;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(m_nextLevel);
    }
}
