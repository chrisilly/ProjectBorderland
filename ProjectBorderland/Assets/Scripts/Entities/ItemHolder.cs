using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [HideInInspector] Rigidbody2D _holder;
    [SerializeField] float _holdDistance = 0.65f;
    [SerializeField] LayerMask _holdableItemLayer;

    private Rigidbody2D _heldItem;
    [SerializeField] Vector3 _holdOffset = new Vector3 (0f, 1f, 0f);

    private float _heldItemGravityScale;
    private float _holderGravityScale;

    public Rigidbody2D HeldItem
    {
        get { return _heldItem; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (_holder == null)
        {
            _holder = GetComponent<Rigidbody2D>();
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

        if (_heldItem != null)
        {
            _heldItem.transform.position = _holder.transform.TransformPoint(_holdOffset);
        }

        if(_heldItem != null && _heldItem.tag == "Glider")
        {
            _holder.gravityScale = _heldItemGravityScale;
        }
    }

    private void GrabItem()
    {
        if (_heldItem == null)
        {
            // Try to grab an item
            Collider2D[] overlappingItems = Physics2D.OverlapCircleAll(transform.position, _holdDistance, _holdableItemLayer);

            if (overlappingItems.Length > 0)
            {
                // Assume the first item found is the item to grab
                _heldItem = overlappingItems[0].GetComponent<Rigidbody2D>();
                _heldItem.transform.SetParent(_holder.transform);
                _heldItem.transform.localPosition = Vector3.zero;
                _heldItem.velocity = Vector2.zero;

                // Save the object's default physics values
                _heldItemGravityScale = _heldItem.gravityScale;

                // Disable item physics when held
                _heldItem.gravityScale = 0;
                _heldItem.isKinematic = true;
            }
        }
    }

    private void ReleaseItem()
    {
        if(_heldItem != null)
        {
            // Release the held item
            _heldItem.transform.SetParent(null);

            // Enable physics and restore physics values when released
            _heldItem.gravityScale = _heldItemGravityScale;
            _heldItem.isKinematic = false;

            _heldItem.velocity = _holder.velocity;

            _heldItem = null;
        }
    }
}
