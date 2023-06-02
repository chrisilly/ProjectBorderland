using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex==1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                LoadNextLevel();
            }
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }


    IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == ("Player") && SceneManager.GetActiveScene().buildIndex != 5) //current level is everything except level 3 breathe
        {
            LoadNextLevel();
        }

        if(collision.transform.tag == ("Player") && SceneManager.GetActiveScene().buildIndex == 5) //loads level based on amount gathered collectibles
        {
            if (ItemCollector.gatheredCollectibles <= 1)
            {
                SceneManager.LoadScene("Ending1");
                //LoadEnding1();
            }
            else if (ItemCollector.gatheredCollectibles == 2)
            {
                SceneManager.LoadScene("Ending2");
                //LoadEnding2();
            }
            else if (ItemCollector.gatheredCollectibles == 3)
            {
                SceneManager.LoadScene("Ending3");
                //LoadEnding3();
            }
        }



    }
    IEnumerator LoadEnding1()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending1");
    }
    IEnumerator LoadEnding2()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending2");
    }
    IEnumerator LoadEnding3()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending3");
    }
}
