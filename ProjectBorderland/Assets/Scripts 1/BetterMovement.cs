using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    [Header("Layer Masks")]
    [SerializeField] LayerMask groundLayer;

    [Header("Movement Settings")]
    [SerializeField] float movementAcceleration;
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float movementDeceleration;
    private float horizontalMovementInput;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce;
    [SerializeField] float airDeceleration;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;
    private bool canDoubleJump;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        MoveCharacter();

        if (IsGrounded())
        {
            ApplyGroundDeceleration();
        }else
        {
            ApplyAirDeceleration();
        }
    }

    private void Update()
    {
        horizontalMovementInput = GetInput().x;
        Jump();

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(horizontalMovementInput, 0f) * movementAcceleration);

        if (Mathf.Abs(rb.velocity.x) > maxMovementSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMovementSpeed, rb.velocity.y);
        }
    }

    private void ApplyGroundDeceleration()
    {
        if (Mathf.Abs(horizontalMovementInput) < 0.4f)
        {
            rb.drag = movementDeceleration;
        }
        else if (ChangingDirection())
        {
            rb.drag = movementDeceleration * 1.5f;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void ApplyAirDeceleration()
    {
        rb.drag = airDeceleration;
    }

    private bool ChangingDirection()
    {
        return (rb.velocity.x > 0 && horizontalMovementInput < 0f || rb.velocity.x < 0 && horizontalMovementInput > 0f);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = jumpTime;
            isJumping = true;
        }else
        {
            if(Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter = jumpTime;
                isJumping = true;
                canDoubleJump = false;
            }
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
            isJumping = false;

        if (IsGrounded())
            canDoubleJump = true;

        if (HitHead())
            isJumping = false;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool HitHead()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
    //}
}
