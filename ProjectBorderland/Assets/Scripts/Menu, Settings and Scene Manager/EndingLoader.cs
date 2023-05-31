using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class EndingLoader : MonoBehaviour
{
    [SerializeField] IntSO collectableCount;

    [SerializeField] Animator transition;

    [SerializeField] float transitionTime = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if(collectableCount.Value <= 1)
            {
                //SceneManager.LoadScene("Ending1");
                LoadEnding1();
            }
            else if(collectableCount.Value == 2)
            {
                //SceneManager.LoadScene("Ending2");
                LoadEnding2();
            }
            else if(collectableCount.Value == 3)
            {
                //SceneManager.LoadScene("Ending3");
                LoadEnding3();
            }
        }
    }

    IEnumerator LoadEnding1()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending 1");
    }
    IEnumerator LoadEnding2()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending 2");
    }
    IEnumerator LoadEnding3()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Ending 3");
    }
}
