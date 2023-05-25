using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending_Loader : MonoBehaviour
{
    ItemCollector itemCollector = new ItemCollector();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if(itemCollector.Count <= 1)
            {
                SceneManager.LoadScene("Ending1");
            }
            else if(itemCollector.Count == 2)
            {
                SceneManager.LoadScene("Ending2");
            }
        }
    }
}
