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
    private Vector3 _lastPosition;
    private Vector3 _velocity;
    private bool _isPaused = false;

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
            _isPaused = false; //resettar flaggan när platformen faller
            StartCoroutine(Fall());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _playerController.ClearCurrentFallingPlatform();
        _isPaused = false;
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

    void HandlePlatformPauseTimer() //pausar bara om platformen faller och inte redan är pausad
    {
        bool isPlayerDashing = _playerController.GetIsPlayerOnDash;

        if (isPlayerDashing && !_isPaused && _isFalling)
        {
            _pauseTimer = 2f;
            _isPaused = true;
        }

        if (_isPaused)
        {
            _pauseTimer -= Time.deltaTime;
            if (_pauseTimer <= 0f)
            {
                _isPaused = false;
            }
        }
    }

    void HandleFall() //platformen faller bara om det inte är pausad
    {
        if (_isFalling && !_isPaused) 
        {
            if (_pauseTimer > 0f) //pausar
            {
                _ridigbody.velocity = Vector2.zero;
            }
            else // fortsätter.
            {
                _fallTime += Time.deltaTime;
                _fallSpeed = Mathf.Lerp(0, gravityScale, _fallTime / fallDelay);
                _ridigbody.transform.Translate(Vector2.down * _fallSpeed * Time.deltaTime);
            }
        }
    }

    void GetVelocity()
    {
        _velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
    }
}