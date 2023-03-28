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
    [SerializeField] bool enableCameraAdjust;
    private float _mScreenYAdjustValue;

    private float _horizontalInput;
    private float _verticalInput;

    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _playerController = FindObjectOfType<PlayerController>();

    }

    private void FixedUpdate()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        AdjustUpDown();
    }

    private void AdjustUpDown()
    {
        _mScreenYAdjustValue = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY;

        if(enableCameraAdjust)
        {
            if (_verticalInput > 0 && _horizontalInput == 0 && _mScreenYAdjustValue < _maxScreenY && !_playerController.GetIsPlayerOnWall)
            {
                _mScreenYAdjustValue = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY += 0.3f * Time.deltaTime;
            }
            else if (_verticalInput < 0 && _horizontalInput == 0 && _mScreenYAdjustValue > _minScreenY && !_playerController.GetIsPlayerOnWall)
            {
                _mScreenYAdjustValue = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY -= 0.3f * Time.deltaTime;
            }
            else if (_verticalInput == 0)
            {
                _mScreenYAdjustValue = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = _defualtScreenY;
            }
        }
    }
}
