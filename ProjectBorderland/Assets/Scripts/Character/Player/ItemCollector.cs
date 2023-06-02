using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] public static int gatheredCollectibles = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.collectibleSFX);
            Destroy(collision.gameObject);
            gatheredCollectibles++;
        }
    }

}
