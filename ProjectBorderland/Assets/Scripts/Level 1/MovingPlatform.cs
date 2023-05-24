using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    /// <summary>
    /// This is the code of the script for the Moving Platforms.
    /// </summary>

    private PlayerController _playerController;
    [SerializeField] public Transform platform;
    [SerializeField] public Transform startPosition;
    [SerializeField] public Transform endPosition;
    [SerializeField] public float speed = 1.5f;
    private int _direction = 1;
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

    /// <summary>
    /// Decides if we should move towards the startPosition or the endPosition.
    /// </summary>
    /// <returns>
    /// Returns the position we should move towards.
    /// </returns>
    Vector2 currentMovementTarget()
    {
        //Decides if we should move towards the startPosition or the endPosition.
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

    /// <summary>
    /// Check if we should pause the platform movement and assuimg we should, pause the platform movement for 2 seconds.
    /// </summary>
    void HandlePlatform()
    {
        bool isPlayerDashing = _playerController.GetIsPlayerOnDash;

        Vector3 target = currentMovementTarget();

        //Calculate the direction of the current position ot the target position.
        Vector3 direction = (target - platform.position).normalized;

        _velocity = direction * speed;

        //Check if we should pause the platform movement and if we should, pause the platform movement for 2 seconds.
        if (_pauseTimer > 0 || (isPlayerDashing && _pauseTimer <= 0)) 
        {
            if (isPlayerDashing && _pauseTimer <= 0)
            {
                _pauseTimer = 2f;
            }
            else
            {
                _pauseTimer -= Time.deltaTime;
            }
        }
        else
        {
            //Moves the platform
            _lastPosition = transform.position;
            platform.position += _velocity * Time.deltaTime;
            float distance = (target - platform.position).magnitude;

            //Check if we have reached the target position and if we have, swap direction.
            if (distance <= 0.1f)
            {
                _direction *= -1;
            }
        }
    }

    private void OnDrawGizmos()
    {
        //For Map building and debugging.
        if (platform != null && startPosition != null && endPosition != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPosition.position);
            Gizmos.DrawLine(platform.transform.position, endPosition.position);
        }
    }
}