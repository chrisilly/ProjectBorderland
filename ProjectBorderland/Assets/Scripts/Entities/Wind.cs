using Ink.Parsed;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //rb.velocity = initialVelocity;

        //ApplyWindForce(other);
        //Rigidbody rb = other.GetComponent<Rigidbody>();
        //Vector3 initialVelocity = rb.velocity;
        //rb.velocity = new Vector2(0, 0);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gliderSFX);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ApplyWindForce(other);

        //if (rb.tag == "Player" && !ItemHolder.IsHoldingGlider(rb))
        //{
        //    rb.AddForce(_direction/* * new Vector2(1, 0)*/ * _force, ForceMode2D.Impulse);
        //}
        //else
        //{
        //    rb.AddForce(_direction * _force, ForceMode2D.Impulse);
        //}
    }

    private void ApplyWindForce(Collider2D target)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

        // Apply different force to player if they are holding a glider
        //if(rb.tag == "Player" && ItemHolder.IsHoldingGlider(rb))
        //{
        //    rb.AddForce(new Vector2(_direction.x, 0) * _force, ForceMode2D.Force);
        //}

        rb.AddForce(_direction * _force, ForceMode2D.Force);
    }
}
