using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private int count;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            Destroy(collision.gameObject);
            count++;
        }
    }

    public int Count
    {
        get
        {
            return count;
        }
    }

}
