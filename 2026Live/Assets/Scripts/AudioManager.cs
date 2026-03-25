using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    private AudioSource systemSource;
    private List<AudioSource> activeSources;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Funções de gerenciamento de áudio
    public void PlaySound(AudioClip clip)
    {
        systemSource.clip = clip;
        systemSource.Play();
        systemSource.Stop();
    }

    public void StopSound(AudioClip clip)
    {
        systemSource.Stop();
    }
    public void PauseSound(AudioClip clip)
    {
        systemSource.Pause();
    }

    public void ResumeSound(AudioClip clip)
    {
        systemSource.UnPause();
    }
    public void PlayOneShot(AudioClip clip)
    {
        systemSource.PlayOneShot(clip);
    }
    
    // Funções de gerenciamento de audio 3d
    public void PlaySound(AudioClip clip, AudioSource source)
    {
        if (!activeSources.Contains(source))activeSources.Add(source);
        source.Stop();
        source.clip = clip;
        source.Play();
    }

    public void PlayOneShot(AudioSource source)
    {
        activeSources.Add(source);
    }

    public void StopSound(AudioSource source)
    {
        source.Stop();
        activeSources.Remove(source);
    }

    public void PauseSound(AudioSource source)
    {
        source.Pause();
        activeSources.Remove(source);
    }

    public void ResumeSound(AudioSource source)
    {
        source.UnPause();
        activeSources.Remove(source);
    }
}