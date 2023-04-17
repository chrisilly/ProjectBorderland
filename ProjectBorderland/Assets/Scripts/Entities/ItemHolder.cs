using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public float holdDistance = 0.65f;
    [HideInInspector] private Rigidbody2D holder;
    public LayerMask holdableItemLayer;

    private Rigidbody2D heldItem;
    public Vector3 holdOffset = new Vector3 (0f, 1f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (holder == null)
        {
            holder = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Grab"))
        {
            GrabItem();
        }
        else if (Input.GetButtonUp("Grab"))
        {
            ReleaseItem();
        }

        if (heldItem != null)
        {
            heldItem.transform.position = holder.transform.TransformPoint(holdOffset);
        }
    }

    private void GrabItem()
    {
        if (heldItem == null)
        {
            // Try to grab an item
            Collider2D[] overlappingItems = Physics2D.OverlapCircleAll(transform.position, holdDistance, holdableItemLayer);

            if (overlappingItems.Length > 0)
            {
                // Assume the first item found is the item to grab
                heldItem = overlappingItems[0].GetComponent<Rigidbody2D>();
                heldItem.transform.SetParent(holder.transform);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.velocity = Vector2.zero;

                // Disable physics when held
                heldItem.gravityScale = 0;
                heldItem.isKinematic = true;
            }
        }
    }

    private void ReleaseItem()
    {
        if(heldItem != null)
        {
            // Release the held item
            heldItem.transform.SetParent(null);

            // Enable physics when released
            heldItem.gravityScale = 1;
            heldItem.isKinematic = false;

            heldItem.velocity = holder.velocity;

            heldItem = null;
        }
    }

}
