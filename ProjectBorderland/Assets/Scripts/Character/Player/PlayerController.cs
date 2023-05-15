using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(StaminaManager))]
public class PlayerController : SingletonMonobehaviour<PlayerController>
{
    //Level 1 platform variables
    private MovingPlatform currentMovingPlatform;
    private FallingPlatform currentFallingPlatform;


    #region VARIABLES
    [Header("Instance")]
    StaminaManager staminaManager;

    [Header("Components")]
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private TrailRenderer _trailRenderer;

    [Header("Layer Masks")]
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] LayerMask _cornerCorrectLayer;

    [Header("Movement Variable")]
    [SerializeField] float _defaultMovementAcceleration;
    [SerializeField] float _maxMoventSpeed;
    [SerializeField] float _defalutMovementDeceleration;
    private float _movementAcceleration;
    private float _movementDeceleration;
    private float _horizontalMovementInput;
    private float _verticalMovementInput;

    [Header("Jump Variable")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _airDeceleration;
    [SerializeField] float _fallMultiplier;
    [Tooltip("Amount of time holding space to jump higher")][SerializeField] float _holdJumpTime;
    [SerializeField] float _defaultMaxFallingSpeed;
    [SerializeField] float _coyoteTime;
    [SerializeField] float _jumpBufferTimer;
    [SerializeField] Vector2 _defaultPlayerGravity;
    [SerializeField] float _defaultRbGravityScale;
    [SerializeField] bool _enableDoubleJump;
    private Vector2 _playerGravity;
    private float _rbGravityScale;
    private float _maxFallingSpeed;
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    private float _jumpTimeCounter;
    private bool _isJumping;
    private bool _canDoubleJump;
    private bool _decreaseStaminaOnJump;

    [Header("Dash Settings")]
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashTime;
    [SerializeField] float _dashCooldown;
    [SerializeField] float _superDashMultiplier;
    private Vector2 _dashDirection;
    private float _dashMultiplier;
    private bool _isDashing;
    private bool _canDash;
    private bool _hasDashCooldown = true;
    private bool _decreaseStaminaOnDash;

    [Header("Wall Variable")]
    [SerializeField] float _wallJumpMultiplierX;
    [SerializeField] float _wallJumpMultiplierY;
    [SerializeField] float _wallJumpTime;
    [SerializeField] float _climbingSpeed;
    [SerializeField] float _raycastLenght;
    private RaycastHit2D _topHit;
    private float _climbMultiplier;
    private float _wallSlidingSpeed;
    private float _wallJumpCounter;
    private float _wallJumpDirection;
    private bool _isWallSliding;
    private bool _isHoldingWall;
    private bool _isClimbing;
    private bool _decreaseStaminaHangWall;
    private bool _decreaseStaminaOnWallMovement;

    [Header("Corner Correction Variable")]
    [SerializeField] float _topRaycastLength;
    [SerializeField] Vector3 _edgeRaycastOffset;
    [SerializeField] Vector3 _innerRaycastOffset;
    RaycastHit2D _headDetector;
    private bool _canCornerCorrect;

    [Header("Animation Bools")]
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isClimbing;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isIdle;
    [HideInInspector] public bool isFalling;

    public ItemHolder itemHolder;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _trailRenderer = GetComponent<TrailRenderer>();

        staminaManager = GetComponent<StaminaManager>();

        itemHolder = GetComponent<ItemHolder>();
    }

    private void Start()
    {
        // Give a default value so that is able to change the current variable value without losing the default value
        _maxFallingSpeed = _defaultMaxFallingSpeed;
        _movementAcceleration = _defaultMovementAcceleration;
        _movementDeceleration = _defalutMovementDeceleration;
        _playerGravity = _defaultPlayerGravity;
        _rbGravityScale = _defaultRbGravityScale;
    }

    private void FixedUpdate()
    {
        CornerCollision();

        MovePlayer();
        OnWallMovement();
        PlayerFall();

        // Check if player is on Ground or Air
        // Apply deceleration based on where player are and reset/decrease coyote time
        if (IsGrounded())
        {
            ApplyGroundDeceleration();
            _coyoteTimeCounter = _coyoteTime;
            staminaManager.CanGainStamina = true;
            if (staminaManager._staminaIsFullyRefilledOnLanding)
            {
                staminaManager._stamina = staminaManager._maxStamina;
            }
        }
        else
        {
            staminaManager.CanGainStamina = true; // Player not on ground anymore => will be able to gain stamina while landing

            ApplyAirDeceleration();
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (_canCornerCorrect) CornerCorrect(_rb.velocity.y);

        // Jack Handles movement of playing while standing on platform.      
        HandleMovingPlatform();
        HandleFallingPlatform();
    }

    private void Update()
    {
        EventHandler.CallMovement(isWalking, isClimbing, isJumping, isIdle, isFalling);

        _horizontalMovementInput = GetInput().x;
        _verticalMovementInput = GetInput().y;

        // Make sure the player can not keep running into a wall with corner correct while sliding on wall
        if (_isWallSliding && _horizontalMovementInput == transform.localScale.x)
            _horizontalMovementInput = 0;

        FlipPlayer();

        Jump();
        JumpBuffer();

        WallSlider();
        WallJump();

        Dash();

        RunAnimation();
    }

    private void RunAnimation()
    {
        if (_isJumping)
        {
            isWalking = false;
            isIdle = false;
            isJumping = true;
        }
        else
        {
            isJumping = false;
            isWalking = false;
            isIdle = false;
            isFalling = true;
        }

        if (IsGrounded())
        {
            isIdle = true;
            isJumping = false;

            if (Mathf.Abs(_horizontalMovementInput) > 0.01f)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
        }
    }

    #region PLAYER CONTROLLER
    private void MovePlayer()
    {
        _rb.AddForce(new Vector2(_horizontalMovementInput, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rb.velocity.x) > _maxMoventSpeed && !_isDashing)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoventSpeed, _rb.velocity.y);
    }

    private void Jump()
    {
        // NORMAL JUMP
        if (_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f && staminaManager.EnoughStaminaAction)
        {
            staminaManager.CanGainStamina = false;

            _decreaseStaminaOnJump = true;
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _jumpTimeCounter = _holdJumpTime;
            _isJumping = true;
            _jumpBufferCounter = 0f;
        }
        else
        {
            // DOUBLE JUMP
            if (Input.GetButtonDown("Jump") && _canDoubleJump && !IsGrounded() && _coyoteTimeCounter <= 0f && _enableDoubleJump && staminaManager.EnoughStaminaAction && !ItemHolder.IsHoldingItem(itemHolder))
            {
                _decreaseStaminaOnJump = true;
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                _jumpTimeCounter = _holdJumpTime;
                _isJumping = true;
                _canDoubleJump = false;
            }
        }

        // HOLD SPACE TO JUMP HIGHER
        if (Input.GetButton("Jump") && _isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
            _coyoteTimeCounter = 0f; // Make player not able spam Space to do double jump by using coyote time;
        }

        if (IsGrounded())
            _canDoubleJump = true;

        if (HitHead())
            _isJumping = false; // Let player falldown as soon as hit an object above
    }

    private void Dash()
    {
        if (Input.GetButtonDown("Dash") && _canDash && _hasDashCooldown && !ItemHolder.IsHoldingItem(itemHolder))
        {
            _canDash = false;
            _hasDashCooldown = false;

            _isDashing = true;
            _trailRenderer.emitting = true;
            _canDoubleJump = true; // Reset Double Jump on Dash

            // If player are not giving any direction input
            // it will automatically dash forward
            _dashDirection = GetInput();

            if (_dashDirection == Vector2.zero)
            {
                _dashDirection = new Vector2(transform.localScale.x, 0f);
            }

            if (_dashDirection.y > 0f)
            {
                staminaManager.CanGainStamina = false;
            }

            if (staminaManager.EnoughStaminaAction && staminaManager.CanSuperDash)
            {
                _dashMultiplier = _superDashMultiplier;
            }
            else if (staminaManager.EnoughStaminaAction)
            {
                _dashMultiplier = 1f;
            }

            _decreaseStaminaOnDash = true;

            StartCoroutine(StopDashing());
        }

        // Setting Dash Velocity
        if (_isDashing)
            _rb.velocity = _dashDirection.normalized * _dashSpeed * _dashMultiplier;

        // Check if player have enough Stamina to Dash
        if (staminaManager.EnoughStaminaAction)
            _canDash = true;
        else
            _canDash = false;
    }

    /// <summary>
    /// Check when player Stops dashing and set the Dash Cooldown
    /// </summary>
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashTime);
        _trailRenderer.emitting = false;
        _isDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        _hasDashCooldown = true;
    }

    private void WallSlider()
    {
        // Grab the wall
        if (LedgeClimbCheck() && IsOnWall() && Input.GetButton("Grab") && staminaManager.EnoughStaminaAction && !ItemHolder.IsHoldingItem(itemHolder))
        {
            _isHoldingWall = true;
            _isWallSliding = true;
            _canDoubleJump = false;

            _decreaseStaminaHangWall = true;
        }
        // Sliding the wall
        else if (LedgeClimbCheck() && IsOnWall() && !IsGrounded())
        {
            _isHoldingWall = false;
            _isWallSliding = true;
            _canDoubleJump = false;
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
        // Check when to push player up
        else if (!LedgeClimbCheck() && IsOnWall() && !IsGrounded() && Input.GetButton("Grab") && _verticalMovementInput != 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            StartCoroutine(PushFrowardOnEdge());
        }
        else
        {
            _isHoldingWall = false;
            _isWallSliding = false;
        }

        // Check if player have enough stamina to hang on wall
        // if not having enough gonna fall down faster
        if (staminaManager.EnoughStaminaAction)
            _wallSlidingSpeed = 0;
        else
            _wallSlidingSpeed = 10;
    }

    /// <summary>
    /// Pushes Player Up from the edge when climbing up
    /// </summary>
    private IEnumerator PushFrowardOnEdge()
    {
        yield return new WaitForSeconds(0.1f);
        _rb.velocity = new Vector2(transform.localScale.x * 10f, _rb.velocity.y);
    }

    private void OnWallMovement()
    {
        if (_isHoldingWall)
        {
            if (_verticalMovementInput > 0)
            {
                _climbMultiplier = 1f;
                _isClimbing = true;

                _decreaseStaminaOnWallMovement = true;
            }
            else if (_verticalMovementInput < 0)
            {
                _climbMultiplier = 1.5f;
                _isClimbing = true;
            }
            else
            {
                _isClimbing = false;
            }
        }
    }

    private void WallJump()
    {
        if (_isWallSliding)
        {
            _wallJumpDirection = -transform.localScale.x;
            _wallJumpCounter = _wallJumpTime;
        }
        else
        {
            _wallJumpCounter -= Time.deltaTime;
        }

        // Jumping while not holding wall will change jump direction
        if (Input.GetButtonDown("Jump") && _wallJumpCounter > 0f && !_isHoldingWall && staminaManager.EnoughStaminaAction)
        {
            _decreaseStaminaOnJump = true;
            _rb.velocity = new Vector2(_wallJumpDirection * _jumpForce * _wallJumpMultiplierX, _jumpForce * _wallJumpMultiplierY);

            // Change player's facing direction
            if (transform.localScale.x != _wallJumpDirection)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        // Holding wall jumping will jump upwards
        else if (Input.GetButtonDown("Jump") && _isHoldingWall && staminaManager.EnoughStaminaAction)
        {
            _decreaseStaminaOnJump = true;
            _isHoldingWall = false;
            _rb.velocity = new Vector2(0f, _jumpForce * _wallJumpMultiplierY);
        }
    }

    /// <summary>
    /// Will let player jump even pressed jump button slighly too early
    /// </summary>
    private void JumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
            _jumpBufferCounter = _jumpBufferTimer;
        else
            _jumpBufferCounter -= Time.deltaTime;
    }

    #endregion

    #region PHYSIC CHECK
    private void ApplyGroundDeceleration()
    {
        if (_isDashing)
        {
            _rb.drag = 0f;
        }
        else if (Mathf.Abs(_horizontalMovementInput) < 0.4f)
        {
            _rb.drag = _movementDeceleration;
        }
        else if (ChangingDirection()) // Makes player turning faster
        {
            _rb.drag = _movementAcceleration * 2f;
        }
        else
        {
            _rb.drag = 0;
        }
    }

    private void ApplyAirDeceleration()
    {
        if (!_isHoldingWall)
            _rb.drag = _airDeceleration;
        else
            _rb.drag = _airDeceleration * 5;
    }

    /// <summary>
    /// Handle how player are falling,
    /// Changes gravitation based on if player are hanging on the wall
    /// </summary>
    private void PlayerFall()
    {
        bool isHoldingGlider = itemHolder.HeldItem != null && itemHolder.HeldItem.CompareTag("Glider");

        if (!isHoldingGlider)
        {
            // Reset the gravity to default value unless player is holding a Glider item
            _playerGravity = _defaultPlayerGravity;
            _rb.gravityScale = _defaultRbGravityScale;
        }

        if (_rb.velocity.y < 0 && !_isHoldingWall)
        {
            // Player will have a higher gravitation when falling
            _rb.velocity += Vector2.up * _playerGravity.y * _fallMultiplier * Time.deltaTime;

            // Player's falling speed will be adjusted
            // so that player won't lose controll to the characteer
            if (_rb.velocity.y < _maxFallingSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _maxFallingSpeed);
            }
        }
        else if (_isHoldingWall && _isClimbing)
        {
            // Set Player gravity scale to ZERO so player won't fall while hoildiing wall
            _rb.gravityScale = 0f;
            _rb.velocity = new Vector2(transform.localScale.x, _verticalMovementInput * _climbMultiplier) * _climbingSpeed;
        }
        else if (_isHoldingWall)
        {
            _rb.gravityScale = 0f;
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y);
        }
    }

    private void CornerCorrect(float Yvelocity)
    {
        //Push player to the right
        RaycastHit2D hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.left, _topRaycastLength, _cornerCorrectLayer);
        if (hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x + newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
            return;
        }

        //Push player to the left
        hit = Physics2D.Raycast(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.right, _topRaycastLength, _cornerCorrectLayer);
        if (hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength,
                transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x - _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
        }
    }
    #endregion

    #region CONDITION CHECK
    /// <summary>
    /// Check if player is pressing opposite button than facing direction
    /// </summary>
    private bool ChangingDirection()
    {
        return (_rb.velocity.x > 0f && _horizontalMovementInput < 0f) || (_rb.velocity.x < 0f && _horizontalMovementInput > 0f);
    }

    private void FlipPlayer()
    {
        if (_horizontalMovementInput > 0.01f)
            transform.localScale = Vector2.one;

        else if (_horizontalMovementInput < -0.01f)
            transform.localScale = new Vector2(-1f, 1f);
    }

    private bool HitHead()
    {
        _headDetector = Physics2D.BoxCast(_boxCollider.bounds.center, new Vector2(_innerRaycastOffset.x * 2, _boxCollider.bounds.size.y), 0f, Vector2.up, 0.1f, _groundLayer);
        return _headDetector.collider != null;

    }

    private void CornerCollision()
    {
        //Corner Collisions
        _canCornerCorrect = Physics2D.Raycast(transform.position + _edgeRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) &&
                           !Physics2D.Raycast(transform.position + _innerRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) ||
                           Physics2D.Raycast(transform.position - _edgeRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer) &&
                           !Physics2D.Raycast(transform.position - _innerRaycastOffset, Vector2.up, _topRaycastLength, _cornerCorrectLayer);
    }

    private bool IsOnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(new Vector2(_boxCollider.bounds.center.x, _boxCollider.bounds.center.y - transform.localScale.y / 3f),
            new Vector2(_boxCollider.bounds.size.x, transform.localScale.y / 2), 0f, new Vector2(transform.localScale.x, 0), 0.2f, _wallLayer);
        return raycastHit.collider != null;
    }

    // Raycast to check when to push player up from the edge
    private bool LedgeClimbCheck()
    {
        _topHit = Physics2D.Raycast(_boxCollider.bounds.center, new Vector2(transform.localScale.x, 0), _topRaycastLength, _wallLayer);
        return _topHit.collider != null;
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, 0.1f, _groundLayer);
        return raycastHit.collider != null;
    }

    public bool IsInAction()
    {
        if (_isJumping || IsOnWall() || _isDashing || _horizontalMovementInput != 0)
            return true;
        return false;
    }
    #endregion

    #region PROPERTIES
    /// <summary>
    /// Check if player is Dashing
    /// </summary>
    public bool GetIsPlayerOnDash
    {
        get { return _isDashing; }
    }

    /// <summary>
    /// Check if player is colliding with walls
    /// </summary>
    public bool GetIsPlayerOnWall
    {
        get { return IsOnWall(); }
    }

    /// <summary>
    /// Boolean value to check When player are Jumping and decreasing the stamina
    /// </summary>
    public bool IsDecreasingStaminaOnJump
    {
        get { return _decreaseStaminaOnJump; }
        set { _decreaseStaminaOnJump = value; }
    }

    /// <summary>
    /// Boolean value to check When player are Holding Wall and decreasing the stamina
    /// </summary>
    public bool IsDecreasingStaminaHoldWall
    {
        get { return _decreaseStaminaHangWall; }
        set { _decreaseStaminaHangWall = value; }
    }

    /// <summary>
    /// Boolean value to check When player are Climbing the Wall and decreasing the stamina
    /// </summary>
    public bool IsDecreasingStaminaOnWallMovement
    {
        get { return _decreaseStaminaOnWallMovement; }
        set { _decreaseStaminaOnWallMovement = value; }
    }

    /// <summary>
    /// Boolean value to check When player are Dashing and decreasing the stamina
    /// </summary>
    public bool IsDecreasingStaminaOnDash
    {
        get { return _decreaseStaminaOnDash; }
        set { _decreaseStaminaOnDash = value; }
    }
    #endregion

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //Gizmos.DrawCube(new Vector2(_collisionCollider.bounds.center.x, _collisionCollider.bounds.center.y - transform.localScale.y / 3f), new Vector2(_collisionCollider.bounds.size.x, transform.localScale.y/2));

        //Gizmos.DrawWireCube(_collisionCollider.bounds.center, new Vector2(_innerRaycastOffset.x * 2, _collisionCollider.bounds.size.y));

        ////Corner Check
        //Gizmos.DrawLine(transform.position + _edgeRaycastOffset, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        //Gizmos.DrawLine(transform.position - _edgeRaycastOffset, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        //Gizmos.DrawLine(transform.position + _innerRaycastOffset, transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength);
        //Gizmos.DrawLine(transform.position - _innerRaycastOffset, transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength);

        ////Corner Distance Check
        //Gizmos.DrawLine(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength,
        //                transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.left * _topRaycastLength);
        //Gizmos.DrawLine(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength,
        //                transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.right * _topRaycastLength);
    }

    // Used to store the current platform the player is standing on.
    public void SetCurrentMovingPlatform(MovingPlatform platform)
    {
        currentMovingPlatform = platform;
    }

    public void ClearCurrentMovingPlatform()
    {
        currentMovingPlatform = null;
    }

    public void SetCurrentFallingPlatform(FallingPlatform platform)
    {
        currentFallingPlatform = platform;
    }

    public void ClearCurrentFallingPlatform()
    {
        currentFallingPlatform = null;
    }

    public void HandleFallingPlatform()
    {
        //Falling platform
        if (currentFallingPlatform != null && currentFallingPlatform.PauseTimer <= 0)
        {
            transform.position += currentFallingPlatform.Velocity * Time.deltaTime;
        }
    }

    public void HandleMovingPlatform()
    {
        //Moving Platform
        if (currentMovingPlatform != null && currentMovingPlatform.PauseTimer <= 0)
        {
            transform.position += currentMovingPlatform.transform.position - currentMovingPlatform.LastPosition + currentMovingPlatform.Velocity * Time.deltaTime;
        }
    }
}
