using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioPlayer))]
public class AudioPlayerEditor : UnityEditor.Editor
{
    private SerializedProperty _myAudioCollectionProperty;
    private SerializedProperty _audioIndexProperty;
    private SerializedProperty _playOnStartProperty;

    private int _runtimeIndexToPlay;

    private void OnEnable()
    {
        _myAudioCollectionProperty = serializedObject.FindProperty("myAudioCollection");
        _audioIndexProperty = serializedObject.FindProperty("audioIndex");
        if (_audioIndexProperty == null)
        {
            // Backward compatibility for scenes/prefabs that still serialize the old field name.
            _audioIndexProperty = serializedObject.FindProperty("currentIndex");
        }
        _playOnStartProperty = serializedObject.FindProperty("playOnStart");

        var audioPlayer = (AudioPlayer)target;
        _runtimeIndexToPlay = audioPlayer.AudioIndex;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (_myAudioCollectionProperty != null)
        {
            EditorGUILayout.PropertyField(_myAudioCollectionProperty);
        }

        if (_audioIndexProperty != null)
        {
            EditorGUILayout.PropertyField(_audioIndexProperty, new GUIContent("Music Index"));
        }

        if (_playOnStartProperty != null)
        {
            EditorGUILayout.PropertyField(_playOnStartProperty);
        }

        var audioPlayer = (AudioPlayer)target;
        var collection = audioPlayer.myAudioCollection;

        // Guarantee index control even if serialized property lookup fails.
        if (_audioIndexProperty == null)
        {
            var fallbackIndex = EditorGUILayout.IntField("Music Index", audioPlayer.AudioIndex);
            if (fallbackIndex != audioPlayer.AudioIndex)
            {
                Undo.RecordObject(audioPlayer, "Set Music Index");
                audioPlayer.SetIndex(fallbackIndex);
                EditorUtility.SetDirty(audioPlayer);
            }

            EditorGUILayout.HelpBox("Could not find serialized index field (audioIndex/currentIndex). Using fallback index control.", MessageType.Warning);
        }

        if (collection != null)
        {
            EditorGUILayout.HelpBox($"Musicas disponiveis: {collection.Count}", MessageType.Info);
        }

        EditorGUILayout.Space(8);
        DrawRuntimeControls(audioPlayer);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawRuntimeControls(AudioPlayer audioPlayer)
    {
        EditorGUILayout.LabelField("Runtime Controls", EditorStyles.boldLabel);

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Entre em Play Mode para usar os controles Play/Pause/Resume/Stop.", MessageType.None);
            return;
        }

        _runtimeIndexToPlay = EditorGUILayout.IntField("Runtime Index", _runtimeIndexToPlay);
        if (GUILayout.Button("Set Runtime Index"))
        {
            audioPlayer.SetIndex(_runtimeIndexToPlay);
        }

        if (GUILayout.Button("Play Selected Index"))
        {
            audioPlayer.PlaySelected();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Pause"))
        {
            audioPlayer.PauseMusic();
        }

        if (GUILayout.Button("Resume"))
        {
            audioPlayer.ResumeMusic();
        }

        if (GUILayout.Button("Stop"))
        {
            audioPlayer.StopMusic();
        }

        EditorGUILayout.EndHorizontal();
    }
}
