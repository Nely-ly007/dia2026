using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioPlayer))]
public class AudioPlayerEditor : Editor
{
    private SerializedProperty myAudioCollectionProperty;
    private SerializedProperty audioIndexProperty;
    private SerializedProperty playOnStartProperty;

    private void OnEnable()
    {
        myAudioCollectionProperty = serializedObject.FindProperty("myAudioCollection");
        audioIndexProperty = serializedObject.FindProperty("audioIndex");
        playOnStartProperty = serializedObject.FindProperty("playOnStart");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(myAudioCollectionProperty);
        EditorGUILayout.PropertyField(audioIndexProperty, new GUIContent("Music Index"));
        EditorGUILayout.PropertyField(playOnStartProperty);

        var audioPlayer = (AudioPlayer)target;
        var collection = audioPlayer.MyAudioCollection;

        if (collection != null)
        {
            EditorGUILayout.HelpBox($"Musicas disponiveis: {collection.Count}", MessageType.Info);
        }

        EditorGUILayout.Space(8);
        DrawRuntimeControls(audioPlayer);

        serializedObject.ApplyModifiedProperties();
    }

    private static void DrawRuntimeControls(AudioPlayer audioPlayer)
    {
        EditorGUILayout.LabelField("Runtime Controls", EditorStyles.boldLabel);

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Entre em Play Mode para usar Play/Pause/Resume/Stop.", MessageType.None);
            return;
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

