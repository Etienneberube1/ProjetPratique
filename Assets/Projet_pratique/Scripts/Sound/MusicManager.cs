using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance => instance;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float duration = 2f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        StartCoroutine(ChangeMusicCoroutine(newClip));
    }

    private IEnumerator ChangeMusicCoroutine(AudioClip newClip)
    {
        // Graduellement baisser le volume de la musique actuelle
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / duration;
            yield return null;
        }

        // Changer la musique
        audioSource.clip = newClip;
        audioSource.Play();

        // Graduellement augmenter le volume de la nouvelle musique
        while (audioSource.volume < 0.1f)
        {
            audioSource.volume += Time.deltaTime / duration ;
            yield return null;
        }
    }
}
