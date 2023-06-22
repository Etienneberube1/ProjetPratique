using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    private List<AudioSource> m_Pool = new List<AudioSource>();
    public AudioSource GetAvailableObjectInPool()
    {
        foreach (AudioSource CurrentAudio in m_Pool)
        {
            if (!CurrentAudio.isPlaying)
            {
                return CurrentAudio;
            }
        }
        GameObject NewGameOjbect = new GameObject();
        AudioSource NewAudioSource = NewGameOjbect.AddComponent<AudioSource>();
        m_Pool.Add(NewAudioSource);
        return NewAudioSource;
    }
}
