using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAdjust : MonoBehaviour
{
    private CinemachineVirtualCamera _vcam;
    //private GameObject _playerController;
    public PlayerController _playerController;

    [Header("Screen Y Adjustment")]
    [SerializeField] float _maxScreenY;
    [SerializeField] float _minScreenY;
    [SerializeField] float _defualtScreenY;
    private float _mScreenYAdjustValue;

    [Header("Lookahead time Adjustment")]
    [SerializeField] float _maxLookaheadTime;
    private float _lookaheadTime;
    private bool _adjustLookahead;

    [Header("Damping Adjustment")]
    [SerializeField] float _defaultDampingX;
    [SerializeField] float _defaultDampingY;
    [SerializeField] float _minDampingX;
    [SerializeField] float _minDampingY;
    private float _adjustDampingX;
    private float _adjustDampingY;

    [SerializeField] bool _enableCameraAdjust;

    private float _horizontalInput;
    private float _verticalInput;

    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _playerController = _playerController.GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (_enableCameraAdjust)
        {
            AdjustUpDown();
            AdjustOnPlayerDash();
        }
    }

    private void AdjustUpDown()
    {
        // Adjust the camera to move slowly up or down to let player see what is above or under, resets the camera on any player action
        if(_playerController.IsInAction())
        {
            _mScreenYAdjustValue = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = _defualtScreenY;
        }
        else if (_verticalInput > 0 && _mScreenYAdjustValue < _maxScreenY && !_playerController.IsInAction())
        {
            _mScreenYAdjustValue = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY += 0.2f * Time.deltaTime;
        }
        else if (_verticalInput < 0 && _mScreenYAdjustValue > _minScreenY && !_playerController.IsInAction())
        {
            _mScreenYAdjustValue = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY -= 0.2f * Time.deltaTime;
        }
    }

    private void AdjustOnPlayerDash()
    {
        // Adjust the camera to follow the player faster when dashing
        if (_playerController.GetIsPlayerOnDash)
        {
            _adjustDampingX = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = _minDampingX;
            _adjustDampingY = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = _minDampingY;
            StartCoroutine(ResetDamping());
        }

        if (_playerController.IsInAction())
        {
            _adjustLookahead = true;
        }

        if (_adjustLookahead)
        {
            _lookaheadTime = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = _maxLookaheadTime;
            StartCoroutine(StopLookahead());
        }
        else
            _lookaheadTime = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime -= 0.1f * Time.deltaTime;
    }

    private IEnumerator StopLookahead()
    {
        // Cinemachine lookahead property
        yield return new WaitForSeconds(0.5f);
        _adjustLookahead = false;
    }

    private IEnumerator ResetDamping()
    {
        // Cinemachine damping property
        yield return new WaitForSeconds(0.3f);
        _adjustDampingX = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = _defaultDampingX;
        _adjustDampingY = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = _defaultDampingY;
    }
}
