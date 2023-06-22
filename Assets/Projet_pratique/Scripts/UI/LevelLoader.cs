using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private AudioClip AudioClip;
    [SerializeField] private AudioClip VictoryClip;
    [SerializeField] private float transitionTime = 1f;


    public void PlayButton()
    {
        LoadNextLevel();
        MusicManager.Instance.ChangeMusic(AudioClip);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void PlayAgainButton()
    {
        // index est pour le main menu
        MusicManager.Instance.ChangeMusic(AudioClip);
        StartCoroutine(LoadLevel(0));

    }
    public void Vitory()
    {
        // index est pour le victory Scene
        MusicManager.Instance.ChangeMusic(VictoryClip);
        StartCoroutine(LoadLevel(3));
    }
    IEnumerator LoadLevel(int levelindex)
    {
        transition.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelindex);
    }
}
