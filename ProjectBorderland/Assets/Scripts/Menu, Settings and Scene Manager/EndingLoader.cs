using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class EndingLoader : MonoBehaviour
{
    //[SerializeField] IntSO collectableCount;

    [SerializeField] Animator transition;

    [SerializeField] float transitionTime = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (ItemCollector.gatheredCollectibles <= 1)
            {
                //SceneManager.LoadScene("Ending1");
                LoadEnding1();
            }
            else if (ItemCollector.gatheredCollectibles == 2)
            {
                //SceneManager.LoadScene("Ending2");
                LoadEnding2();
            }
            else if (ItemCollector.gatheredCollectibles == 3)
            {
                //SceneManager.LoadScene("Ending3");
                LoadEnding3();
            }
        }
    }

    IEnumerator /*coRoutine*/LoadEnding1()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending1");
    }
    IEnumerator /*coRoutine*/LoadEnding2()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending2");
    }
    IEnumerator /*coRoutine*/LoadEnding3()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending3");
    }
}
