using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    [SerializeField] float sfxDurationTime = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PlayClickSFX();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        PlayClickSFX();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public IEnumerator QuitGame()
    {
        PlayClickSFX();
        yield return new WaitForSeconds(sfxDurationTime);
        Debug.Log("You Quit");
        Application.Quit();
    }

    public void QuitRoutineWrap()
    {
        StartCoroutine(QuitGame());
    }

    public void PlayClickSFX()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
    }
}
