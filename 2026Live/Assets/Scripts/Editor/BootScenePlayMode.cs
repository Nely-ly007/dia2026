#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public static class BootScenePlayMode
    {
        private const string BootScenePath = "Assets/Scenes/_boot.unity";
        private static bool _sSwitchingToBootScene;

        static BootScenePlayMode()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode || _sSwitchingToBootScene)
            {
                return;
            }

            if (SceneManager.GetActiveScene().path == BootScenePath)
            {
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorApplication.isPlaying = false;
                return;
            }

            _sSwitchingToBootScene = true;
            EditorSceneManager.OpenScene(BootScenePath);
            EditorApplication.delayCall += ResumePlayFromBootScene;
        }

        private static void ResumePlayFromBootScene()
        {
            EditorApplication.delayCall -= ResumePlayFromBootScene;
            _sSwitchingToBootScene = false;

            if (!EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = true;
            }
        }
    }
}
#endif


