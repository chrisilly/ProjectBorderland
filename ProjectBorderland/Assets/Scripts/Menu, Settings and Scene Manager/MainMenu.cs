using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] float sfxDurationTime = 1f;
    //[SerializeField] IntSO intSO;
    private void Start()
    {
        //intSO.Value = 0; //Collectible count is reset to 0 when game is started
    }

    public IEnumerator PlayGame()
    {
        PlayClickSFX();
        transition.SetTrigger("Start"); //Start level transition animation
        yield return new WaitForSeconds(sfxDurationTime); //wait for animation to finish before loading next level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //loads next scene in build order
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
