using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByInt(int p_sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(p_sceneIndex);
    }

    public void ReloadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
