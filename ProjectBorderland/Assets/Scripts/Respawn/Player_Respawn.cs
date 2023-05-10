using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player_Respawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject fallCheck;
    Vector3 respawnPoint;
    [Header("Phase Tilemaps to disable after player dies")]
    [SerializeField] List<GameObject> phaseTilemapList;

    private void Awake()
    {
        respawnPoint = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fall Check")
        {
            player.transform.position = respawnPoint;
            ResetTilemaps();
        }
        else if (collision.tag == "Check Point")
        {
            respawnPoint = collision.transform.position;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Respawn"))
        {
            player.transform.position = respawnPoint;
            ResetTilemaps();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Hazard")
        {
            player.transform.position = respawnPoint;
            ResetTilemaps();
        }
    }
    private void ResetTilemaps()
    {
        foreach (GameObject go in phaseTilemapList)
        {
            SetAlpha(0.75f, go.GetComponent<Tilemap>()); //Changes opacity of colored platforms to 190 out 255
            go.GetComponent<TilemapCollider2D>().enabled = false; //Disables collision with ALL colored platforms.
        }
    }

    void SetAlpha(float alpha, Tilemap _tilemap) //Set opacity method
    {

        Color colorController = _tilemap.color;
        colorController.a = Mathf.Clamp(alpha, 0, 1);
        _tilemap.color = colorController;

    }
}