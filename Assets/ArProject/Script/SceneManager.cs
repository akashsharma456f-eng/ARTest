using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void LoadNextScene(string sceneName) 
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }
}
