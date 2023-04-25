using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PhaseShifter : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    CompositeCollider2D compCollider;

    private void Awake()
    {
        compCollider = tilemap.GetComponent<CompositeCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            compCollider.isTrigger = false;
            this.gameObject.SetActive(false);
            SetAlpha(255);
        }
    }

    public void SetAlpha(float alpha)
    {
        Color c = tilemap.color;
        c.a = Mathf.Clamp(alpha, 0, 1);
        tilemap.color = c;
    }
}