using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PhaseShifter : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] List<GameObject> tilemapList = new List<GameObject>();
    [SerializeField] Tilemap tilemap;

    bool isCrystalActive = true;

    [SerializeField] float inactiveCrystalTimerLimit = 2.5f;
    float inactiveCrystalTimer = 0;

    [SerializeField] AudioSource phaseShiftSFX;

    #endregion

    private void OnTriggerEnter2D(Collider2D collision) //When the Player Character touches a Phase Crystal
    {
        if (collision.tag == "Player")
        {
            phaseShiftSFX.Play();

            foreach (GameObject go in tilemapList)
            {
                SetAlpha(0.75f, go.GetComponent<Tilemap>()); //Changes opacity of colored platforms to 190 out 255
                go.GetComponent<TilemapCollider2D>().enabled = false; //Disables collision with ALL colored platforms.
            }
            isCrystalActive = false; //Activates update method
            tilemap.GetComponent<TilemapCollider2D>().enabled = true; //Enables collision with the platform that is linked with the crystal. E.G. Red crystal is linked with the red platforms.

            GameObject.Find("Phase Indicator").GetComponent<Image>().color = this.gameObject.GetComponent<SpriteRenderer>().color;

            SetAlpha(1, tilemap);//Changes opacity of linked platform to full.
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false; //Hides crystal
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false; //Disables crystal collision

        }
    }

    private void Update() //Crystal reappearing again after player has collided with it.
    {
        if (isCrystalActive == false)
        {
            inactiveCrystalTimer += Time.deltaTime;
            if (inactiveCrystalTimer >= inactiveCrystalTimerLimit)
            {
                isCrystalActive = true;
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                inactiveCrystalTimer = 0;

            }
        }

    }

    public void SetAlpha(float alpha, Tilemap _tilemap) //Set opacity method
    {

        Color colorController = _tilemap.color;
        colorController.a = Mathf.Clamp(alpha, 0, 1);
        _tilemap.color = colorController;

    }

}