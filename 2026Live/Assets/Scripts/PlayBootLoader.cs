using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class PlayBootLoader
{
    private const string BootSceneName = "_boot";
    private const string MainSceneName = "MenuPrincipal";
    private const float BootDelaySeconds = 0.5f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void LoadMainSceneAfterBoot()
    {
        if (SceneManager.GetActiveScene().name != BootSceneName)
        {
            return;
        }

        var loaderObject = new GameObject(nameof(PlayBootLoader));
        Object.DontDestroyOnLoad(loaderObject);
        loaderObject.AddComponent<BootSceneDelayLoader>().Begin(MainSceneName, BootDelaySeconds);
    }

    private sealed class BootSceneDelayLoader : MonoBehaviour
    {
        private string _sceneName;
        private float _delaySeconds;

        public void Begin(string sceneName, float delaySeconds)
        {
            _sceneName = sceneName;
            _delaySeconds = delaySeconds;
            StartCoroutine(LoadAfterDelay());
        }

        private IEnumerator LoadAfterDelay()
        {
            yield return new WaitForSeconds(_delaySeconds);
            if (SceneManager.GetActiveScene().name != _sceneName)
            {
                SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
            }
        }
    }
}
