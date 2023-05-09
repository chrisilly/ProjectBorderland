using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private float fallDelay = 0.25f;
    [SerializeField][Tooltip("Increases the falling speed(fall speed)")] private float gravityScale = 10f;
    private bool _isFalling = false;
    private float _fallTime = 0f;
    private float _fallSpeed = 0f;
    private Rigidbody2D _ridigbody;
    private float _pauseTimer = 0f;
    private bool _isCountingDown = false;
    private Vector3 _lastPosition;
    private Vector3 _velocity;

    public Vector3 Velocity { get { return _velocity; } }
    public float PauseTimer { get { return _pauseTimer; } }

    // Start is called before the first frame update
    void Start()
    {
        _ridigbody = GetComponentInChildren<Rigidbody2D>();
        _ridigbody.gravityScale = 0f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>();
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
        _playerController.SetCurrentFallingPlatform(this);

        if (collision.gameObject.CompareTag("Player") && !_isFalling)
        {
            StartCoroutine(Fall());
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        _playerController.ClearCurrentFallingPlatform();
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        _isFalling = true;

        float timeElapsed = 0f;
        float startGravity = _ridigbody.gravityScale;

        while (timeElapsed < fallDelay)
        {
            float time = timeElapsed / fallDelay;
            _ridigbody.gravityScale = Mathf.Lerp(startGravity, gravityScale, time);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        _ridigbody.gravityScale = gravityScale;
    }

    void HandlePlatformPauseTimer() //Used to pause the platform movement when the player dashes.
    {
        bool isPlayerDashing = _playerController.GetIsPlayerOnDash;

        if (isPlayerDashing)
        {
            _pauseTimer = 2f;
            _isCountingDown = true;
        }

        if (_isCountingDown)
        {
            _pauseTimer -= Time.deltaTime;
            if (_pauseTimer <= 0f)
            {
                _isCountingDown = false;
            }
        }
    }

    void HandleFall()
    {
        if (_isFalling)
        {
            if (_pauseTimer > 0f) //Pauses the fall
            {
                _ridigbody.velocity = Vector2.zero;
            }
            else // Contiunes the fall.
            {
                _fallTime += Time.deltaTime;
                _fallSpeed = Mathf.Lerp(0, gravityScale, _fallTime / fallDelay);
                _ridigbody.transform.Translate(Vector2.down * _fallSpeed * Time.deltaTime);

                Debug.Log("fallspeed" + _fallSpeed);
            }
        }
    }

    void GetVelocity()
    {
        _velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
    }
}