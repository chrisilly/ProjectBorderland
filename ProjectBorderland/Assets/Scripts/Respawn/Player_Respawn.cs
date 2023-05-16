using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player_Respawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject fallCheck;
    Vector3 respawnPoint;
    [Header("Phase Tilemaps to disable after player dies")]
    [SerializeField] List<GameObject> phaseTilemapList;
    [SerializeField] List<GameObject> BlackoutPhaseTilemapList;
    [SerializeField] List<GameObject> crystalList;
    [SerializeField] List<GameObject> primaryCrystalList;

    Color defaultPhaseColor;

    private void Awake()
    {
        respawnPoint = player.transform.position;
        defaultPhaseColor = GameObject.Find("Phase Indicator").GetComponent<Image>().color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fall Check")
        {
            player.transform.position = respawnPoint;
            ResetTilemaps();
            ResetBlackoutPhaseCrystals();
            foreach(GameObject go in primaryCrystalList)
            {
                go.GetComponent<SpriteRenderer>().enabled = true;
                go.GetComponent<BoxCollider2D>().enabled = true;
            }
            GameObject.Find("Phase Indicator").GetComponent<Image>().color = defaultPhaseColor;
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
            ResetBlackoutPhaseCrystals();
            GameObject.Find("Phase Indicator").GetComponent<Image>().color = defaultPhaseColor;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Hazard")
        {
            player.transform.position = respawnPoint;
            ResetTilemaps();
            GameObject.Find("Phase Indicator").GetComponent<Image>().color = defaultPhaseColor;
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
}