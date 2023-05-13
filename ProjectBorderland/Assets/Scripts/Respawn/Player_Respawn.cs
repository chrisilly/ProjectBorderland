using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player_Respawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject glider;
    [SerializeField] GameObject fallCheck;
    Vector3 respawnPoint;
    [Header("Phase Tilemaps to disable after player dies")]
    [SerializeField] List<GameObject> phaseTilemapList;
    [SerializeField] List<GameObject> BlackoutPhaseTilemapList;
    [SerializeField] List<GameObject> crystalList;
    [SerializeField] List<GameObject> primaryCrystalList;
    [SerializeField] List<GameObject> gliderObjectList;
        

    private void Awake()
    {
        respawnPoint = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fall Check")
        {
            player.transform.position = respawnPoint;

            // Reset Phase Shifting level design
            ResetTilemaps();
            ResetBlackoutPhaseCrystals();
            foreach(GameObject go in primaryCrystalList)
            {
                go.GetComponent<SpriteRenderer>().enabled = true;
                go.GetComponent<BoxCollider2D>().enabled = true;
            }

            // Reset Glider items
            ResetGliderToPlayer();
        }
        else if (collision.tag == "Check Point")
        {
            respawnPoint = collision.transform.position;
        }
    }

    void Update()
    {
        //SaveActiveGlider();

        if (Input.GetButtonDown("Respawn"))
        {
            player.transform.position = respawnPoint;
            ResetTilemaps();
            ResetBlackoutPhaseCrystals();
            ResetGliderToPlayer();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Hazard")
        {
            player.transform.position = respawnPoint;
            ResetTilemaps();
            ResetGliderToPlayer();
        }
    }
    private void ResetTilemaps()
    {
        foreach (GameObject go in phaseTilemapList)
        {
            SetAlpha(0.75f, go.GetComponent<Tilemap>()); //Changes opacity of colored platforms to 190 out 255
            go.GetComponent<TilemapCollider2D>().enabled = false; //Disables collision with ALL colored platforms.
        }
        foreach (GameObject go in BlackoutPhaseTilemapList)
        {
            SetAlpha(0f, go.GetComponent<Tilemap>()); //Changes opacity of colored platforms to 190 out 255
            go.GetComponent<TilemapCollider2D>().enabled = false; //Disables collision with ALL colored platforms.
        }
    }
    void ResetBlackoutPhaseCrystals()
    {
        foreach (GameObject go in crystalList)
        {
            go.GetComponent<SpriteRenderer>().enabled = false;
            go.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void SetAlpha(float alpha, Tilemap _tilemap) //Set opacity method
    {

        Color colorController = _tilemap.color;
        colorController.a = Mathf.Clamp(alpha, 0, 1);
        _tilemap.color = colorController;

    }

    void ResetGliderToPlayer()
    {
        //foreach (GameObject item in gliderObjectList)
        //{
        //    item.transform.position = player.transform.position;
        //}
        glider.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        glider.transform.position = player.transform.position;
    }

    void SaveActiveGlider()
    {
        ItemHolder itemHolder = player.GetComponent<ItemHolder>();

        if (ItemHolder.IsHoldingGlider(itemHolder) && gliderObjectList.Count == 0)
        {
            gliderObjectList.Add(itemHolder.HeldItem.GetComponent<GameObject>());
        }
    }
}