using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] IntSO count;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.collectibleSFX);
            Destroy(collision.gameObject);
            count.Value++;
        }
    }

}
