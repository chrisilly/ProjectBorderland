using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] public Transform platform;
    [SerializeField] public Transform startPosition;
    [SerializeField] public Transform endPosition;
    [SerializeField] public float speed = 1.5f;
    private int _direction = 1;
    [SerializeField][Tooltip("Makes the platform pause for X seconds once arrving at each end position. ")] 
    private float _goalWaitTime = 0f;
    private float _pauseTimer = 0f;
    private Vector3 _velocity;
    private Vector3 _lastPosition;

    public float PauseTimer { get { return _pauseTimer; } }
    public Vector3 Velocity { get { return _velocity; } }
    public Vector3 LastPosition { get { return _lastPosition; } }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlatform();
    }

    Vector2 currentMovementTarget()
    {
        if (_direction == 1)
        {
            return startPosition.position;
        }
        else
        {
            return endPosition.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerController.SetCurrentMovingPlatform(this);
        } 
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            _playerController.ClearCurrentMovingPlatform();
        }            
    }

    void HandlePlatform() //Freezes the platform for x seconds if the player dashes, otherwise moves the platform.
    {
        bool isPlayerDashing = _playerController.GetIsPlayerOnDash;

        Vector3 target = currentMovementTarget();
        Vector3 direction = (target - platform.position).normalized;
        _velocity = direction * speed;

        if (_pauseTimer > 0 || (isPlayerDashing && _pauseTimer <= 0))
        {
            if (isPlayerDashing && _pauseTimer <= 0)
            {
                _pauseTimer = 3.5f;
            }
            else
            {
                _pauseTimer -= Time.deltaTime;
            }
        }
        else
        {
            _lastPosition = transform.position;
            platform.position += _velocity * Time.deltaTime;
            float distance = (target - platform.position).magnitude;

            if (distance <= 0.1f)
            {
                _direction *= -1;
                _pauseTimer = _goalWaitTime;
            }
        }
    }

    private void OnDrawGizmos() // för debug och map building
    {
        if (platform != null && startPosition != null && endPosition != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPosition.position);
            Gizmos.DrawLine(platform.transform.position, endPosition.position);
        }
    }
}