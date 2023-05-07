using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] Vector2 _direction;
    [SerializeField] float _force;

    BoxCollider2D _collisionCollider;

    // Start is called before the first frame update
    void Start()
    {
        if (_collisionCollider == null)
        {
            _collisionCollider = GetComponent<BoxCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        rb.AddForce(_direction * _force, ForceMode2D.Impulse);

        //if (rb.tag == "Player" && !ItemHolder.IsHoldingGlider(rb))
        //{
        //    rb.AddForce(_direction/* * new Vector2(1, 0)*/ * _force, ForceMode2D.Impulse);
        //}
        //else
        //{
        //    rb.AddForce(_direction * _force, ForceMode2D.Impulse);
        //}
    }
}
