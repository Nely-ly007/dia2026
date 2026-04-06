using UnityEngine;
using UnityEngine.Serialization;

public class AudioPlayer : MonoBehaviour
{
    public AudioCollection myAudioCollection;

    [SerializeField, FormerlySerializedAs("currentIndex")]
    private int audioIndex;

    [SerializeField]
    private bool playOnStart = true;

    public int AudioIndex => audioIndex;

    public bool PlayOnStart => playOnStart;

    private void Start()
    {
        if (playOnStart)
        {
            PlaySelected();
        }
    }

    public void Play()
    {
        PlaySelected();
    }

    public void Pause()
    {
        PauseMusic();
    }

    public void Resume()
    {
        ResumeMusic();
    }

    public void Stop()
    {
        StopMusic();
    }

    public void PlaySelected()
    {
        if (!TryGetSelectedClip(out var clip) || AudioManager.Instance == null)
        {
            return;
        }

        AudioManager.Instance.PlaySound(clip);
    }

    public void PauseMusic()
    {
        if (AudioManager.Instance == null)
        {
            return;
        }

        AudioManager.Instance.PauseSound();
    }

    public void ResumeMusic()
    {
        if (AudioManager.Instance == null)
        {
            return;
        }

        AudioManager.Instance.ResumeSound();
    }

    public void StopMusic()
    {
        if (AudioManager.Instance == null)
        {
            return;
        }

        AudioManager.Instance.StopSound();
    }

    public void SetIndex(int index)
    {
        if (myAudioCollection == null || myAudioCollection.Count <= 0)
        {
            audioIndex = 0;
            return;
        }

        audioIndex = Mathf.Clamp(index, 0, myAudioCollection.Count - 1);
    }

    public void PlayNext()
    {
        if (myAudioCollection == null || myAudioCollection.Count <= 0)
        {
            return;
        }

        audioIndex = (audioIndex + 1) % myAudioCollection.Count;
        PlaySelected();
    }

    public void PlayPrevious()
    {
        if (myAudioCollection == null || myAudioCollection.Count <= 0)
        {
            return;
        }

        audioIndex = (audioIndex - 1 + myAudioCollection.Count) % myAudioCollection.Count;
        PlaySelected();
    }

    private bool TryGetSelectedClip(out AudioClip clip)
    {
        clip = null;

        if (myAudioCollection == null)
        {
            return false;
        }

        SetIndex(audioIndex);
        return myAudioCollection.TryGetClip(audioIndex, out clip);
    }
}