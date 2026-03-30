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
            systemSource = gameObject.GetComponent<AudioSource>();
            activeSources = new List<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Funções de gerenciamento de áudio
    public void PlaySound(AudioClip clip)
    {
        if (systemSource == null || clip == null)
        {
            return;
        }

        systemSource.Stop();
        systemSource.clip = clip;
        systemSource.Play();
    }

    public void StopSound()
    {
        if (systemSource == null)
        {
            return;
        }

        systemSource.Stop();
    }

    public void PauseSound()
    {
        if (systemSource == null)
        {
            return;
        }

        systemSource.Pause();
    }

    public void ResumeSound()
    {
        if (systemSource == null)
        {
            return;
        }

        systemSource.UnPause();
    }

    // Compatibilidade com chamadas antigas
    public void StopSound(AudioClip clip)
    {
        StopSound();
    }

    public void PauseSound(AudioClip clip)
    {
        PauseSound();
    }

    public void ResumeSound(AudioClip clip)
    {
        ResumeSound();
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (systemSource == null || clip == null)
        {
            return;
        }

        systemSource.PlayOneShot(clip);
    }

    // Funções de gerenciamento de audio 3d
    public void PlaySound(AudioClip clip, AudioSource source)
    {
        if (source == null || clip == null)
        {
            return;
        }

        if (!activeSources.Contains(source))
        {
            activeSources.Add(source);
        }

        source.Stop();
        source.clip = clip;
        source.Play();
    }

    public void PlayOneShot(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        if (!activeSources.Contains(source))
        {
            activeSources.Add(source);
        }
    }

    public void StopSound(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.Stop();
        activeSources.Remove(source);
    }

    public void PauseSound(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.Pause();
        activeSources.Remove(source);
    }

    public void ResumeSound(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.UnPause();
        activeSources.Remove(source);
    }
}