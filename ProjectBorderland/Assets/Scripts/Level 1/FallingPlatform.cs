using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    /// <summary>
    /// This is the code of the script for the Falling Platforms.
    /// </summary>

    private PlayerController _playerController;
    [SerializeField] private float fallDelay = 0.25f;
    [SerializeField][Tooltip("Increases the falling speed(fall speed)")] private float gravityScale = 10f;
    private bool _isFalling = false;
    private float _fallTime = 0f;
    private float _fallSpeed = 0f;
    private Rigidbody2D _ridigbody;
    private float _pauseTimer = 0f;
    private Vector3 _lastPosition;
    private Vector3 _velocity;
    private bool _isPaused = false;
    private Vector3 _startPosition;
    private float _resetDelay;
    private float _startPosOffset = 7f;

    public Vector3 Velocity { get { return _velocity; } }
    public float PauseTimer { get { return _pauseTimer; } }

    // Start is called before the first frame update
    void Start()
    {
        _ridigbody = GetComponentInChildren<Rigidbody2D>();
        _ridigbody.gravityScale = 0f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>();
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlatformPauseTimer();
        HandleFall();
        GetVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Set the current falling platform in the player controller.
        _playerController.SetCurrentFallingPlatform(this);

        if (collision.gameObject.CompareTag("Player") && !_isFalling)
        {
            //Check so that the platform is not paused.
            _isPaused = false;
            StartCoroutine(Fall());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _playerController.SetCurrentFallingPlatform(this);

        if (collision.gameObject.CompareTag("Player") && !_isFalling)
        {
            _isPaused = false;
            _resetDelay= 0f;
            StartCoroutine(Fall());
        }
    }

    //Clears the platform in the playercontroller and resets the delay and pause.
    private void OnCollisionExit2D(Collision2D collision)
    {
        _playerController.ClearCurrentFallingPlatform();
        _isPaused = false;
        _resetDelay = 2f;
        //StopCoroutine(ResetPlatform());
    }

    /// <summary>
    /// Coroutine for making the platform fall that scales over a time period.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fall()
    {
        //Wait for the fall delay before starting the fall.
        yield return new WaitForSeconds(fallDelay);

        _isFalling = true;

        //Keeps track of the time elapsed during the fall.
        float timeElapsed = 0f;

        //Stores the initial gravity value.
        float startGravity = _ridigbody.gravityScale;

        while (timeElapsed < fallDelay)
        {
            float time = timeElapsed / fallDelay;

            //Adjust the gravity scale from the initial value to the targeted value based on the time.
            _ridigbody.gravityScale = Mathf.Lerp(startGravity, gravityScale, time);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //Checks so gravity scale is set to the target value after the fall duration.
        _ridigbody.gravityScale = gravityScale;
    }

    void HandlePlatformPauseTimer() 
    {
        //Check if the player is dashing
        bool isPlayerDashing = _playerController.GetIsPlayerOnDash;

        //Pauses the platform if the player dashes, assuming the platform is falling and isn't already paused.
        if (isPlayerDashing && !_isPaused && _isFalling)
        {
            _pauseTimer = 2f;
            _isPaused = true;
        }

        if (_isPaused)
        {
            _pauseTimer -= Time.deltaTime;

            //Set the platform to no longer be paused.
            if (_pauseTimer <= 0f)
            {
                _isPaused = false;
            }
        }
    }

    //Handles the behavior of the falling platform (if not paused)
    void HandleFall()
    {
        //Check if the platform is falling and not paused.
        if (_isFalling && !_isPaused)
        {
            //Check if the pause timer is greater than 0 to determine if falling should be paused.
            if (_pauseTimer > 0f)
            {
                _ridigbody.velocity = Vector2.zero;
            }
            else
            {
                _fallTime += Time.deltaTime;

                //Calculates the fall speed and makes the platform fall based on the fallspeed
                _fallSpeed = Mathf.Lerp(0, gravityScale, _fallTime / fallDelay); 
                _ridigbody.transform.Translate(Vector2.down * _fallSpeed * Time.deltaTime);

                //Reset the platform if the platform has fallen a set distance from startPosition.
                if (_ridigbody.transform.position.y < _startPosition.y - _startPosOffset)
                {
                    StartCoroutine(ResetPlatform()); 
                }
            }
        }
    }

    public void GetVelocity()
    {
        //Calculates the velocity.
        _velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
    }

    //Resets the platform to startPosition after a short delay.
    private IEnumerator ResetPlatform() 
    {
        yield return new WaitForSeconds(_resetDelay);
        _ridigbody.velocity = Vector2.zero;
        _ridigbody.gravityScale = 0f;
        transform.position = _startPosition;
        _isFalling = false;
    }
}