using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] float sfxDurationTime = 1f;
    [SerializeField] IntSO intSO;
    private void Start()
    {
        intSO.Value = 0;
    }

    public IEnumerator PlayGame()
    {
        PlayClickSFX();
        transition.SetTrigger("Start");
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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.clickSFX);
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
