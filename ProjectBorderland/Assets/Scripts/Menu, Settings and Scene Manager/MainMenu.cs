using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource clickSFX;
    [SerializeField] float sfxDurationTime = 1f;
    public IEnumerator PlayGame()
    {
        PlayClickSFX();
        yield return new WaitForSeconds(sfxDurationTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator QuitGame()
    {
        PlayClickSFX();
        yield return new WaitForSeconds(sfxDurationTime);
        Debug.Log("You Quit");
        Application.Quit();
    }

    public void PlayClickSFX()
    {
        clickSFX.Play();
    }

    public void PlayRoutineWrap()
    {
        StartCoroutine(PlayGame());
    }

    public void QuitRoutineWrap()
    {
        StartCoroutine (QuitGame());
    }
}
