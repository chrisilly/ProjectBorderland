using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Crystal : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap defaultMap;

    CompositeCollider2D cc;
    CompositeCollider2D ccD;

    private void Awake()
    {
        cc = tilemap.GetComponent<CompositeCollider2D>();
        ccD = defaultMap.GetComponent<CompositeCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            cc.isTrigger = false;
            this.gameObject.SetActive(false);
            SetAlpha(1,tilemap);
            SetAlpha(0.6f, defaultMap);
            ccD.isTrigger = true;
        }
    }

    public void SetAlpha(float alpha,Tilemap map)
    {
        Color c = map.color;
        c.a = Mathf.Clamp(alpha, 0, 1);
        map.color = c;
    }
}
