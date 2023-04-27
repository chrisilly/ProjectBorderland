using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Respawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject fallCheck;
    Vector3 respawnPoint;
    
    private void Awake()
    {
        respawnPoint = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fall Check")
        {
            player.transform.position = respawnPoint;
        }
        else if (collision.tag == "Check Point")
        {
            respawnPoint = collision.transform.position;
        }
    }
}