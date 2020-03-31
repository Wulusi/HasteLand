using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelButtonsEvent : UnityEngine.Events.UnityEvent { }
public class LevelButtonsManager : MonoBehaviour
{
    public List<GameObject> m_levelButtons;

    public LayerMask m_levelMask;
    public LevelButtonsEvent m_buttonPressed;

    public float m_levelLoadTime;
    public CanvasGroup m_canvasGroupFade;

    public int m_gameLoadIndex;

    private bool m_pressed;
    private void Update()
    {
        if (m_pressed) return;
        MousePress();
    }

    private void MousePress()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray newRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(newRay, out hit, 1000, m_levelMask))
            {
                if (m_levelButtons.Contains(hit.transform.gameObject))
                {
                    LevelPressed(m_levelButtons.IndexOf(hit.transform.gameObject));
                }
            }
        }
    }

    private void LevelPressed(int p_levelPressed)
    {
        m_buttonPressed.Invoke();
        LevelPicker.Instance.SetLevel(p_levelPressed);
        StartCoroutine(LoadLevel());
        m_pressed = true;
    }

    IEnumerator LoadLevel()
    {
        float timer = 0;
        while(timer < m_levelLoadTime)
        {
            m_canvasGroupFade.alpha = timer / m_levelLoadTime;
            timer += Time.deltaTime;
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(m_gameLoadIndex);
    }

}
