using System.Collections.Generic;
using UnityEngine;

public class AudioCollection : MonoBehaviour
{
    public List<AudioClip> AudioClipsCollection;

    public int Count => AudioClipsCollection != null ? AudioClipsCollection.Count : 0;

    public bool TryGetClip(int index, out AudioClip clip)
    {
        clip = null;

        if (AudioClipsCollection == null)
        {
            return false;
        }

        if (index < 0 || index >= AudioClipsCollection.Count)
        {
            return false;
        }

        clip = AudioClipsCollection[index];
        return clip != null;
    }

    public AudioClip this[int i]
    {
        get
        {
            TryGetClip(i, out var clip);
            return clip;
        }
    }
}