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

    [SerializeField] bool isCrystalActive = true;

    public int layerIndex;

    [SerializeField] float inactiveCrystalTimerLimit = 2.5f;

    float inactiveCrystalTimer = 0;

    [SerializeField] List<int> inactiveLayerIndexes = new List<int>();

    private void Awake()
    {
        compCollider = tilemap.GetComponent<CompositeCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isCrystalActive = false;
            Physics2D.IgnoreLayerCollision(0, layerIndex, false);
            foreach (int index in inactiveLayerIndexes)
            {
                Physics2D.IgnoreLayerCollision(0, index, true);
                List<GameObject> tempPlatformList = FindAllPlatformObjectsInLayer(index);
                foreach(GameObject platform in tempPlatformList) 
                {
                    SetAlpha(0.75f, platform.GetComponent<Tilemap>());
                }
                
            }

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

    public List<GameObject> FindAllPlatformObjectsInLayer(int layerIndex)
    {
        List<GameObject> platformGameObjectlist = new List<GameObject>();
        platformGameObjectlist = GameObject.FindGameObjectsWithTag("Platform").ToList();
        for (int i = 0; platformGameObjectlist.Count < i; i++)
        {
            if (platformGameObjectlist[i].layer != layerIndex)
            {
                platformGameObjectlist.RemoveAt(i);
                i--;
            }
        }
        return platformGameObjectlist;
    }
}