using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    [Tooltip("Amount of time holding space to jump higher")] [SerializeField] float jumpTime;
    [SerializeField] float maxFallingspeed;
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBufferTimer;
    [SerializeField] bool enableDoubleJump; // BETTER TO TURN IT OFF WHEN DOING MOVEMENT TESTS (OFF BY DEFAULT)
    private float jumpBufferCounter;
    private float coyoteTimeCounter;
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
        PlayerFall();

        if (IsGrounded())
        {
            ApplyGroundDeceleration();
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            ApplyAirDeceleration();
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void Update()
    {
        horizontalMovementInput = GetInput().x;

        JumpBuffer();  
        Jump();
    }

    #region PLAYER MOVEMENT
    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(horizontalMovementInput, 0f) * movementAcceleration);

        if (Mathf.Abs(rb.velocity.x) > maxMovementSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMovementSpeed, rb.velocity.y);
        }
    }
    private void Jump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = jumpTime;
            isJumping = true;  
            jumpBufferCounter = 0f;
        }
        else
        {
            // DOUBLE JUMP
            if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump && !IsGrounded() && coyoteTimeCounter <= 0f && enableDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter = jumpTime;
                isJumping = true;
                canDoubleJump = false;
                Debug.Log("DOUBLE JUMPING!");
            }
        }

        // HOLD SPACE TO JUMP HIGHER
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            coyoteTimeCounter = 0f; // So that player can not spam Space to do double jump with coyote time;
        }

        if (IsGrounded())
            canDoubleJump = true;

        if (HitHead())
            isJumping = false;
    }
    private void JumpBuffer()
    {
        // Player can still jump even jump input is slightly too early
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTimer;
        else
            jumpBufferCounter -= Time.deltaTime;
    }
    #endregion

    #region PHYSICS CHECKS
    private void PlayerFall()
    {
        if (rb.velocity.y < 0)
        {
            // Player will have a higher gravitation when falling
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;

            // Player's falling speed will be adjusted
            // so that player won't lose controll to the characteer
            if (rb.velocity.y < maxFallingspeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxFallingspeed);
            }
        }
    }
    private void ApplyGroundDeceleration()
    {
        //Deceleration for player when ON GROUND
        if (Mathf.Abs(horizontalMovementInput) < 0.4f)
        {
            rb.drag = movementDeceleration;
        }
        else if (ChangingDirection()) // Makes player turning faster
        {
            rb.drag = movementAcceleration * 1.4f;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void ApplyAirDeceleration()
    {
        //Deceleration for player when ON AIR
        rb.drag = airDeceleration;
    }
    #endregion

    #region CONDITION CHECKS
    private bool ChangingDirection()
    {
        return (rb.velocity.x > 0f && horizontalMovementInput < 0f) || (rb.velocity.x < 0f && horizontalMovementInput > 0f);
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool HitHead()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    #endregion

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
    //}
}
