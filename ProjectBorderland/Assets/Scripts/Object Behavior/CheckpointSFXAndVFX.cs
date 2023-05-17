using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSFXAndVFX : MonoBehaviour
{
    [SerializeField] AudioSource checkpointSFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            checkpointSFX.Play();
            //Destroy(gameObject);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
