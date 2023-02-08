using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [Header("Components")]
    Rigidbody2D rb;

    [Header("Movement Settings")]
    [SerializeField] float speed;
    [SerializeField] bool useGetAxisRaw;
    private float horizontalMovementInput;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (useGetAxisRaw)
        {
            horizontalMovementInput = Input.GetAxisRaw("Horizontal");
        }else
        {
            horizontalMovementInput = Input.GetAxis("Horizontal");
        }

        rb.velocity = new Vector2(speed* horizontalMovementInput, 0);
    }
}
