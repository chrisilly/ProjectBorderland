using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingLoader : MonoBehaviour
{
    [SerializeField] IntSO collectableCount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if(collectableCount.Value <= 1)
            {
                SceneManager.LoadScene("Ending1");
            }
            else if(collectableCount.Value == 2)
            {
                SceneManager.LoadScene("Ending2");
            }
            else if(collectableCount.Value == 3)
            {
                SceneManager.LoadScene("Ending3");
            }
        }
    }
}
