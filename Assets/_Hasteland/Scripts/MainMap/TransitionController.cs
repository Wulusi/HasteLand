using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TransitionController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeInDurationInSeconds;
    public int nextScene;
    public UnityEvent OnTransitionComplete = new UnityEvent();

    private void Awake()
    {
        transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        canvasGroup = transform.parent.GetChild(1).GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextscene();
        }
    }
    private IEnumerator FadeIn()
    {
        float _alphaFactor = 1 / fadeInDurationInSeconds;
        while (canvasGroup.alpha>0f)
        {
            canvasGroup.alpha -= _alphaFactor*Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = 0f;
        OnTransitionComplete.Invoke();
    }
    public void LoadNextscene()
    {
        StartCoroutine(FadeOut(nextScene));
    }

    public void LoadScreen(int _index)
    {
        StartCoroutine(FadeOut(_index));
    }

    private IEnumerator FadeOut(int _index) // Load Scene
    {
        AsyncOperation loader = SceneManager.LoadSceneAsync(_index);
        while (!loader.isDone)
        {
            float _progress = Mathf.Clamp01(loader.progress*.9f);
            canvasGroup.alpha = _progress;
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = 1f;
    }
}
