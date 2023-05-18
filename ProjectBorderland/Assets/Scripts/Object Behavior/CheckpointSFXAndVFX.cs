using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSFXAndVFX : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            GetComponent<ParticleSystem>().Play();
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
