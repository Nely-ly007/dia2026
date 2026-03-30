using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioCollection myAudioCollection;
    [SerializeField, Min(0)] private int audioIndex;
    [SerializeField] private bool playOnStart = true;

    public int AudioIndex
    {
        get => audioIndex;
        set => audioIndex = Mathf.Max(0, value);
    }

    public AudioCollection MyAudioCollection => myAudioCollection;

    private void Start()
    {
        if (playOnStart)
        {
            PlaySelected();
        }
    }

    public void PlaySelected()
    {
        if (!TryGetSelectedClip(out var clip))
        {
            return;
        }

        AudioManager.Instance.PlaySound(clip);
    }

    public void PlayFromIndex(int index)
    {
        AudioIndex = index;
        PlaySelected();
    }

    public void PauseMusic()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioPlayer: AudioManager.Instance nao encontrado.", this);
            return;
        }

        AudioManager.Instance.PauseSound();
    }

    public void ResumeMusic()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioPlayer: AudioManager.Instance nao encontrado.", this);
            return;
        }

        AudioManager.Instance.ResumeSound();
    }

    public void StopMusic()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioPlayer: AudioManager.Instance nao encontrado.", this);
            return;
        }

        AudioManager.Instance.StopSound();
    }

    private bool TryGetSelectedClip(out AudioClip clip)
    {
        clip = null;

        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioPlayer: AudioManager.Instance nao encontrado.", this);
            return false;
        }

        if (myAudioCollection == null)
        {
            Debug.LogWarning("AudioPlayer: nenhum AudioCollection foi atribuido.", this);
            return false;
        }

        if (!myAudioCollection.TryGetClip(audioIndex, out clip))
        {
            Debug.LogWarning($"AudioPlayer: indice {audioIndex} invalido ou sem AudioClip na colecao.", this);
            return false;
        }

        return true;
    }
}
