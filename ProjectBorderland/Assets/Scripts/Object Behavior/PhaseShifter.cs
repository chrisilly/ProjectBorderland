using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PhaseShifter : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    CompositeCollider2D compCollider;

    bool isCrystalActive = true;

    [SerializeField] float inactiveCrystalTimerLimit = 2.5f;

    float inactiveCrystalTimer = 0;

    [SerializeField] List<GameObject> tilemapList = new List<GameObject>();

    private void Awake()
    {
        compCollider = tilemap.GetComponent<CompositeCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (GameObject go in tilemapList)
            {
                SetAlpha(0.75f, go.GetComponent<Tilemap>());
                go.GetComponent<TilemapCollider2D>().enabled=false;
            }
            isCrystalActive = false;
            tilemap.GetComponent<TilemapCollider2D>().enabled = true;


            SetAlpha(1, tilemap);
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void Update()
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

    public void SetAlpha(float alpha,Tilemap _tilemap)
    {

        Color colorController = _tilemap.color;
        colorController.a = Mathf.Clamp(alpha, 0, 1);
        _tilemap.color = colorController;

    }

}