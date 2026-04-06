using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _systemSource;
    private List<AudioSource> _activeSources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _systemSource = gameObject.GetComponent<AudioSource>();
            _activeSources = new List<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Funções de gerenciamento de áudio
    public void PlaySound(AudioClip clip)
    {
        if (_systemSource == null || clip == null)
        {
            return;
        }

        _systemSource.Stop();
        _systemSource.clip = clip;
        _systemSource.Play();
    }

    public void StopSound()
    {
        if (_systemSource == null)
        {
            return;
        }

        _systemSource.Stop();
    }

    public void PauseSound()
    {
        if (_systemSource == null)
        {
            return;
        }

        _systemSource.Pause();
    }

    public void ResumeSound()
    {
        if (_systemSource == null)
        {
            return;
        }

        _systemSource.UnPause();
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
        if (_systemSource == null || clip == null)
        {
            return;
        }

        _systemSource.PlayOneShot(clip);
    }

    // Funções de gerenciamento de audio 3d
    public void PlaySound(AudioClip clip, AudioSource source)
    {
        if (source == null || clip == null)
        {
            return;
        }

        if (!_activeSources.Contains(source))
        {
            _activeSources.Add(source);
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

        if (!_activeSources.Contains(source))
        {
            _activeSources.Add(source);
        }
    }

    public void StopSound(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.Stop();
        _activeSources.Remove(source);
    }

    public void PauseSound(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.Pause();
        _activeSources.Remove(source);
    }

    public void ResumeSound(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.UnPause();
        _activeSources.Remove(source);
    }
}